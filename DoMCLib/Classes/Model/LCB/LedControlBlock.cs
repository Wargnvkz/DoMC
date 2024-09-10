using DoMCLib;
using DoMCLib.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using DoMCModuleControl.Modules;
using DoMCModuleControl;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DoMCModuleControl.Configuration;
using DoMCLib.Classes.Model.LCB;
using DoMCLib.Classes.Model.ArchiveDB;
using DoMCModuleControl.Commands;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using DoMCModuleControl.Logging;
using System.Runtime.CompilerServices;
using DoMCLib.Tools;
using DoMCLib.Classes;

namespace LedControlBlock
{
    // Изначально это был другой класс который я пытаюсь переделать на ModuleBase
    public class LedControlBlock : ModuleBase
    {
        UdpClient udp;
        int portFromComputer = 1556;
        int portTo = 1555;
        IPEndPoint Broadcast;
        IPEndPoint Localaddress;
        IPEndPoint Any;

        TimeSpan LCBTimeout = new TimeSpan(0, 0, 30);

        //int LEDLoadConfigStatus = 0;

        static byte startbyte = 0xff;
        IMainController MainController;
        Observer CurrentObserver;

        ILogger WorkingLog;
        byte[] buffer = new byte[0];
        ConcurrentQueue<LEDBlockCommand> LEDOutCommands = new ConcurrentQueue<LEDBlockCommand>();
        LCBSettings CurrentSettings;
        LEDDataExchangeStatus Status;
        LCBWorkingParameters CurrentWorkingParameters;

        public LedControlBlock(IMainController mainController) : base(mainController)
        {
            MainController = mainController;
            Broadcast = new IPEndPoint(IPAddress.Broadcast, portTo);
            Localaddress = new IPEndPoint(IPAddress.Any, portFromComputer);
            Any = new IPEndPoint(IPAddress.Any, 0);
            WorkingLog = mainController.GetLogger(this.GetType().Name);
        }

        public void Start()
        {
            Broadcast = new IPEndPoint(IPAddress.Broadcast, portTo);
            Localaddress = new IPEndPoint(IPAddress.Any, portFromComputer);
            Any = new IPEndPoint(IPAddress.Any, 0);

            udp = new UdpClient();
            Localaddress = new IPEndPoint(IPAddress.Any, portFromComputer);
            udp.Client.Bind(Localaddress);

            udp.EnableBroadcast = true;
            udp.AllowNatTraversal(false);
        }
        public void StartForInterface(NetworkInterface ni)
        {
            Broadcast = new IPEndPoint(NetworkTools.GetBroadcastAddress(ni), portTo);
            Localaddress = new IPEndPoint(IPAddress.Any, portFromComputer);
            Any = new IPEndPoint(IPAddress.Any, 0);

            udp = new UdpClient();
            Localaddress = new IPEndPoint(IPAddress.Any, portFromComputer);
            udp.Client.Bind(Localaddress);

            udp.EnableBroadcast = true;
            udp.AllowNatTraversal(false);
        }
        public void StartForInterface(IPAddress ipAddress)
        {
            Broadcast = new IPEndPoint(NetworkTools.GetBroadcastAddress(ipAddress), portFromComputer);
            //Broadcast = new IPEndPoint(IPAddress.Broadcast, portTo);
            Localaddress = new IPEndPoint(IPAddress.Any, portFromComputer);
            Any = new IPEndPoint(IPAddress.Any, 0);

            udp = new UdpClient();
            Localaddress = new IPEndPoint(IPAddress.Any, portFromComputer);
            udp.Client.Bind(Localaddress);

            udp.EnableBroadcast = true;
            udp.AllowNatTraversal(false);
        }

        public void Stop()
        {
            try
            {
                udp?.Close();
            }
            catch { }
            udp = null;
        }


        // Вызов команд снаружи модуля
        public void SetNewPreformParameters(LCBSettings settings)
        {
            CurrentSettings = settings;
        }
        public void SetNewWorkingParameters(LCBWorkingParameters parameters)
        {
            CurrentWorkingParameters = parameters;
        }
        public void SetLCBCurrent()
        {
            var cmd = new LEDBlockCommand();
            cmd.Command = (int)LEDCommandType.SetLEDCurrentRequest;
            cmd.Data = new byte[1];
            cmd.Data[0] = (byte)(CurrentSettings.LEDCurrent / 10);
            LEDOutCommands.Enqueue(cmd);
        }

        public void GetLCBCurrent()
        {
            var cmd = new LEDBlockCommand();
            cmd.Command = (int)LEDCommandType.GetLEDCurrentRequest;
            cmd.Data = new byte[0];
            LEDOutCommands.Enqueue(cmd);
        }

        public void SetLCBMovementParameters()
        {
            Status.PreformLength = (int)(CurrentSettings.PreformLength);
            Status.DelayLength = (int)(CurrentSettings.DelayLength);
            var lmp = new LEDMovementParameters();
            lmp.PreformLengthImpulses = (int)(Status.PreformLength);
            lmp.DelayLengthImpulses = (int)(Status.DelayLength);

            var cmd = new LEDBlockCommand();
            cmd.Command = (int)LEDCommandType.SetLCBMovementParametersRequest;
            cmd.Data = lmp.ToBytes();
            LEDOutCommands.Enqueue(cmd);
        }

        public void GetLCBMovementParameters()
        {
            var cmd = new LEDBlockCommand();
            cmd.Command = (int)LEDCommandType.GetLCBMovementParametersRequest;
            cmd.Data = new byte[0];
            LEDOutCommands.Enqueue(cmd);
        }
        public void SetLCBEquipmentStatus()
        {
            var les = new LEDEquimpentStatus();
            les.Inputs = Status.Inputs;
            les.Outputs = Status.Outputs;
            les.Magnets = Status.Magnets;
            les.Valve = Status.Valve;
            les.LEDStatuses = Status.LEDStatuses;

            var cmd = new LEDBlockCommand();
            cmd.Command = (int)LEDCommandType.SetLCBEquipmentStatusRequest;
            cmd.Data = les.ToBytesFull();
            LEDOutCommands.Enqueue(cmd);
        }
        public void GetLCBEquipmentStatus()
        {
            var cmd = new LEDBlockCommand();
            cmd.Command = (int)LEDCommandType.GetLCBEquipmentStatusRequest;
            cmd.Data = new byte[0];
            LEDOutCommands.Enqueue(cmd);
        }
        public void SetLCBWorkMode()
        {
            var cmd = new LEDBlockCommand();
            cmd.Command = (int)LEDCommandType.SetLCBWorkModeRequest;
            cmd.Data = new byte[1];
            cmd.Data[0] = (byte)1;
            LEDOutCommands.Enqueue(cmd);
        }
        public void SetLCBNonWorkMode()
        {
            var cmd = new LEDBlockCommand();
            cmd.Command = (int)LEDCommandType.SetLCBWorkModeRequest;
            cmd.Data = new byte[1];
            cmd.Data[0] = (byte)0;
            LEDOutCommands.Enqueue(cmd);
        }
        public void GetLCBMaxPosition()
        {
            var cmd = new LEDBlockCommand();
            cmd.Command = (int)LEDCommandType.GetLCBMaxHorizontalStrokeRequest;
            cmd.Data = new byte[0];
            LEDOutCommands.Enqueue(cmd);
        }
        public void GetLCBCurrentPosition()
        {
            var cmd = new LEDBlockCommand();
            cmd.Command = (int)LEDCommandType.GetLCBCurrentHorizontalStrokeRequest;
            cmd.Data = new byte[0];
            LEDOutCommands.Enqueue(cmd);
        }
        public void Send()
        {
            while (LEDOutCommands.TryDequeue(out LEDBlockCommand lbc))
            {
                var bytes = lbc.ToBytes();
                WorkingLog?.Add(LoggerLevel.FullDetailedInformation, $"-> БУС: <{ArrayTools.ByteArrayToHexString(bytes)}>");
                try
                {
                    udp?.Send(bytes, bytes.Length, Broadcast);
                    CurrentObserver.Notify(this, "SendCommand", "Success", bytes);
                }
                catch (Exception ex)
                {
                    WorkingLog?.Add(LoggerLevel.Critical, "Не могу отправить пакет. ", ex);
                    CurrentObserver.Notify(this, "SendCommand", "Error", ex);
                    throw ex;
                }
            }
        }



        // OnRead, OnWrite, OnCalculate выполняются в отдельном потоке заданном снаружи класса
        public void Process()
        {
            LCBTimeout = new TimeSpan(0, 0, 0, 0, CurrentWorkingParameters.WaitForLCBCardAnswerTimeoutInSeconds);
            if (udp != null /*&& udp.Client != null && udp.Client.Connected*/)
            {
                var len = udp.Available;
                if (len > 0)
                {
                    lock (buffer)
                    {
                        byte[] tmp;
                        try
                        {
                            IPEndPoint drIP = Any;
                            tmp = udp.Receive(ref drIP);
                            Status.UDPReceived = DateTime.Now;
                            CurrentObserver.Notify(this, "ReadUDP", "Success", null);
                        }
                        catch (Exception ex)
                        {
                            WorkingLog.Add(LoggerLevel.Critical, $"Не удалость прочитать из сокета UDP, хотя в нем {len} байт", ex);
                            CurrentObserver.Notify(this, "ReadUDP", "Error", ex);
                            throw ex;
                        }

                        try
                        {
                            var bl = buffer.Length;
                            Array.Resize(ref buffer, bl + tmp.Length);
                            Array.Copy(tmp, 0, buffer, bl, tmp.Length);
                            CurrentObserver.Notify(this, "AddingRecievedDataToBuffer", "Success", null);
                        }
                        catch (Exception ex)
                        {
                            WorkingLog.Add(LoggerLevel.Critical, $"Не удалость прочитать из сокета UDP, хотя в нем {len} байт", ex);
                            CurrentObserver.Notify(this, "ReadUDP", "Error", ex);
                            throw ex;
                        }
                    }
                }
            }

            if ((buffer?.Length ?? 0) > 0)
            {
                lock (buffer)
                {
                    var cmd = LEDBlockCommand.FromByteArray(ref buffer, startbyte);
                    if (cmd == null) return true;
                    WorkingLog.Add($"БУС ->: <{Log.ByteArrayToHexString(cmd.ToBytes())}>");

                    switch (cmd.Command)
                    {
                        /*case 0x01:
                            break;*/
                        case (int)LEDCommandType.SetLEDCurrentResponse://0x81:
                            {
                                var isOk = cmd.Data[0] == 1;
                                Status.LastCommandReceivedStatusIsOK = isOk;
                            }
                            break;
                        /*case 0x02:
                            break;*/
                        case (int)LEDCommandType.GetLEDCurrentResponse://0x82:
                            {
                                var current = cmd.Data[0] * 10;
                                Status.LEDCurrent = current;
                            }
                            break;
                        /*case 0x03:
                            break;*/
                        case (int)LEDCommandType.LEDSynchrosignalResponse://0x83:
                            {
                                Status.TimePreviousSyncSignalGot = Status.TimeSyncSignalGot;
                                Status.TimeSyncSignalGot = DateTime.Now;
                                WorkingLog.Add(LoggerLevel.Information, "Получен синхросигнал");
                                CurrentObserver.Notify("LEDSynchrosignal", null);
                            }
                            break;
                        /*case 0x04:
                            break;*/
                        case (int)LEDCommandType.LEDStatusResponse://0x84:
                            {
                                var les = LEDEquimpentStatus.FromBytes(cmd.Data);
                                Status.TimeLEDStatusGot = DateTime.Now;
                                Status.LEDStatuses = les.LEDStatuses;
                                WorkingLog.Add(LoggerLevel.Information, $"Получен статус светодиодов - {string.Join("", les.LEDStatuses.Select(i => i ? 1 : 0))}");
                            }
                            break;
                        /*case 0x05:
                            break;*/
                        case (int)LEDCommandType.SetLCBMovementParametersResponse://0x85:
                            {
                                var isOk = cmd.Data[0] == 1;
                                Status.LastCommandReceivedStatusIsOK = isOk;
                            }
                            break;
                        /*case 0x06:
                            break;*/
                        case (int)LEDCommandType.GetLCBMovementParametersResponse://0x86:
                            {
                                var lmp = LEDMovementParameters.FromBytes(cmd.Data);
                                Status.PreformLength = (int)(lmp.PreformLengthImpulses);
                                Status.DelayLength = (int)(lmp.DelayLengthImpulses);
                                Status.LastMovementParametersReceived = DateTime.Now;

                            }
                            break;
                        /*case 0x07:
                            break;*/
                        case (int)LEDCommandType.SetLCBEquipmentStatusResponse://0x87:
                            {
                                var isOk = cmd.Data[0] == 1;
                                Status.LastCommandReceivedStatusIsOK = isOk;
                            }
                            break;
                        /*case 0x08:
                            break;*/
                        case (int)LEDCommandType.GetLCBEquipmentStatusResponse://0x88:
                            {
                                var les = LEDEquimpentStatus.FromBytes(cmd.Data);
                                Status.LEDStatuses = les.LEDStatuses;
                                Status.TimeLEDStatusGot = DateTime.Now;
                                Status.InOutStatusGot = DateTime.Now;
                                Status.LEDStatuses = les.LEDStatuses;
                                Status.Inputs = les.Inputs;
                                Status.Outputs = les.Outputs;
                                Status.Magnets = les.Magnets;
                                Status.Valve = les.Valve;
                            }
                            break;
                        /*case 0x09:
                            break;*/
                        case (int)LEDCommandType.SetLCBWorkModeResponse://0x89:
                            {
                                var isOk = cmd.Data[0] == 1;
                                Status.LastCommandReceivedStatusIsOK = isOk;
                            }
                            break;
                        case (int)LEDCommandType.GetLCBMaxHorizontalStrokeResponse://0x8a:
                            {
                                var maxStrokeImpulses = BitConverter.ToUInt16(cmd.Data, 0);
                                Status.MaximumHorizontalStroke = maxStrokeImpulses;
                            }
                            break;
                        case (int)LEDCommandType.GetLCBCurrentHorizontalStrokeResponse: //0x8b
                            {
                                var curStrokeImpulses = BitConverter.ToUInt16(cmd.Data, 0);
                                Status.CurrentHorizontalStroke = curStrokeImpulses;
                            }
                            break;
                    }
                    Status.NumberOfLastCommandReceived = cmd.Command;
                    Status.LastCommandResponseReceived = DateTime.Now;

                }
            }


        }

        public void InterfaceParameters() { }

        public void Dispose()
        {
        }


        public class SetLCBIPCommand : CommandBase
        {
            public SetLCBIPCommand(IMainController mainController, ModuleBase module) : base(mainController, module, typeof(string), null) { }
            protected override void Executing() => ((LCBModule)Module).SetIP((string)InputData);
        }

        public class SetConfigurattionCommand : CommandBase
        {
            public SetConfigurattionCommand(IMainController mainController, ModuleBase module) : base(mainController, module, typeof(LCBSettings), null) { }
            protected override void Executing() => ((LCBModule)Module).SetConfiguration((LCBSettings)InputData);
        }

        public class ConnectCommand : CommandBase
        {
            public ConnectCommand(IMainController mainController, ModuleBase module) : base(mainController, module, typeof(), null) { }
            protected override void Executing() => (()Module).(ArchiveDBConfiguration) InputData;
        }
        public class Command : CommandBase
        {
            public Command(IMainController mainController, ModuleBase module) : base(mainController, module, typeof(), null) { }
            protected override void Executing() => (()Module).(ArchiveDBConfiguration) InputData;
        }
        public class Command : CommandBase
        {
            public Command(IMainController mainController, ModuleBase module) : base(mainController, module, typeof(), null) { }
            protected override void Executing() => (()Module).(ArchiveDBConfiguration) InputData;
        }

        public enum Operations
        {

        }
        public enum EventType
        {

        }
    }

    /*
    SetLCBCurrentRequest,

        GetLCBCurrentRequest,

        SetLCBMovementParametersRequest,

        GetLCBMovementParametersRequest,

        SetLCBEquipmentStatusRequest,

        GetLCBEquipmentStatusRequest,

        SetLCBWorkModeRequest,

        SetLCBNonWorkModeRequest,

        GetLCBMaxPositionRequest,

        GetLCBCurrentPositionRequest,
        LCBUDPReconnect,
        LCBStop,*/

}
