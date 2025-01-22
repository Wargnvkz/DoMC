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
using DoMCLib.Classes.Module.ArchiveDB;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using DoMCModuleControl.Logging;
using System.Runtime.CompilerServices;
using DoMCLib.Tools;
using System.Threading;

namespace DoMCLib.Classes.Module.LCB
{
    // Изначально это был другой класс который я пытаюсь переделать на AbstractModuleBase
    public partial class LCBModule : AbstractModuleBase
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
        //IMainController MainController;
        Observer CurrentObserver;

        ILogger WorkingLog;
        ConcurrentQueue<LEDBlockCommand> LEDOutCommands = new ConcurrentQueue<LEDBlockCommand>();
        //LCBSettings CurrentSettings;
        //LEDDataExchangeStatus Status=new LEDDataExchangeStatus();
        LCBWorkingParameters CurrentWorkingParameters = new LCBWorkingParameters();
        Task task;
        CancellationTokenSource cancellationTokenSource;

        public LCBModule(IMainController mainController) : base(mainController)
        {
            //MainController = mainController;
            Broadcast = new IPEndPoint(IPAddress.Broadcast, portTo);
            Localaddress = new IPEndPoint(IPAddress.Any, portFromComputer);
            Any = new IPEndPoint(IPAddress.Any, 0);
            WorkingLog = mainController.GetLogger(this.GetType().Name);
            CurrentObserver = mainController.GetObserver();
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
            cancellationTokenSource = new CancellationTokenSource();
            task = new Task(Process);
            task.Start();
        }
        /*public void StartForInterface(NetworkInterface ni)
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
        public void StartForIPAddress(IPAddress ipAddress)
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
            cancellationTokenSource = new CancellationTokenSource();
            task = new Task(Process);
            task.Start();
        }*/

        public void Stop()
        {
            try
            {
                cancellationTokenSource.Cancel();
                udp?.Close();
            }
            catch { }
            udp = null;
        }


        // Вызов команд снаружи модуля
        /*public void SetLCBParameters(LCBSettings settings)
        {
            CurrentSettings = settings;
        }*/
        public void SetWorkingParameters(LCBWorkingParameters parameters)
        {
            CurrentWorkingParameters = parameters.Clone();
            CurrentObserver.Notify(this, Operations.ModuleParametersSet.ToString(), EventType.Success.ToString(), null);
        }
        public void SetLCBCurrent(int current)
        {
            var cmd = new LEDBlockCommand();
            cmd.Command = (int)LEDCommandType.SetLEDCurrentRequest;
            cmd.Data = new byte[1];
            cmd.Data[0] = (byte)(current / 10);
            LEDOutCommands.Enqueue(cmd);
        }
        public void GetLCBCurrent()
        {
            var cmd = new LEDBlockCommand();
            cmd.Command = (int)LEDCommandType.GetLEDCurrentRequest;
            cmd.Data = new byte[0];
            LEDOutCommands.Enqueue(cmd);
        }
        public void SetLCBMovementParameters(int PreformLength, int DelayLength)
        {
            var lmp = new LEDMovementParameters();
            lmp.PreformLengthImpulses = PreformLength;
            lmp.DelayLengthImpulses = DelayLength;

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
        public void SetLCBEquipmentStatus(LEDEquimpentStatus newStatus)
        {
            var les = new LEDEquimpentStatus();
            les.Inputs = newStatus.Inputs;
            les.Outputs = newStatus.Outputs;
            les.Magnets = newStatus.Magnets;
            les.Valve = newStatus.Valve;
            les.LEDStatuses = newStatus.LEDStatuses;

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
        private void Send()
        {
            while (LEDOutCommands.TryDequeue(out LEDBlockCommand lbc))
            {
                var bytes = lbc.ToBytes();
                WorkingLog?.Add(LoggerLevel.FullDetailedInformation, $"-> БУС: <{ArrayTools.ByteArrayToHexString(bytes)}>");
                try
                {
                    udp?.Send(bytes, bytes.Length, Broadcast);
                    CurrentObserver.Notify(this, Operations.SendCommand.ToString(), EventType.Success.ToString(), bytes);
                }
                catch (Exception ex)
                {
                    WorkingLog?.Add(LoggerLevel.Critical, "Не могу отправить пакет. ", ex);
                    CurrentObserver.Notify(this, Operations.SendCommand.ToString(), EventType.Error.ToString(), ex);
                    throw ex;
                }
            }
        }



        // OnRead, OnWrite, OnCalculate выполняются в отдельном потоке заданном снаружи класса
        private void Process()
        {
            CurrentObserver.Notify(this, Operations.Started.ToString(), EventType.Success.ToString(), null);
            LCBTimeout = new TimeSpan(0, 0, 0, 0, CurrentWorkingParameters.WaitForLCBCardAnswerTimeoutInSeconds);
            var buffer = new byte[0];
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                Send();

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
                            CurrentObserver.Notify(this, Operations.ReadUDP.ToString(), EventType.Success.ToString(), null);
                        }
                        catch (Exception ex)
                        {
                            WorkingLog.Add(LoggerLevel.Critical, $"Не удалость прочитать из сокета UDP, хотя в нем {len} байт", ex);
                            CurrentObserver.Notify(this, Operations.ReadUDP.ToString(), EventType.Error.ToString(), ex);
                            throw ex;
                        }

                        try
                        {
                            var bl = buffer.Length;
                            Array.Resize(ref buffer, bl + tmp.Length);
                            Array.Copy(tmp, 0, buffer, bl, tmp.Length);
                            CurrentObserver.Notify(this, Operations.AddingRecievedDataToBuffer.ToString(), EventType.Success.ToString(), null);
                        }
                        catch (Exception ex)
                        {
                            WorkingLog.Add(LoggerLevel.Critical, $"Не удалость добавить данные из UDP сокета в буффер обработки", ex);
                            CurrentObserver.Notify(this, Operations.AddingRecievedDataToBuffer.ToString(), EventType.Error.ToString(), ex);
                        }
                    }
                }

                lock (buffer)
                {
                    var cmd = LEDBlockCommand.FromByteArray(ref buffer, startbyte);
                    if (cmd != null)
                    {
                        WorkingLog.Add(LoggerLevel.FullDetailedInformation, $"БУС ->: <{ArrayTools.ByteArrayToHexString(cmd.ToBytes())}>");

                        switch (cmd.Command)
                        {
                            /*case 0x01:
                                break;*/
                            case (int)LEDCommandType.SetLEDCurrentResponse://0x81:
                                {
                                    var isOk = cmd.Data[0] == 1;
                                    CurrentObserver.Notify(this, LEDCommandType.SetLEDCurrentResponse.ToString(), EventType.Received.ToString(), isOk);
                                }
                                break;
                            /*case 0x02:
                                break;*/
                            case (int)LEDCommandType.GetLEDCurrentResponse://0x82:
                                {
                                    var current = cmd.Data[0] * 10;
                                    CurrentObserver.Notify(this, LEDCommandType.GetLEDCurrentResponse.ToString(), EventType.Received.ToString(), current);
                                }
                                break;
                            /*case 0x03:
                                break;*/
                            case (int)LEDCommandType.LEDSynchrosignalResponse://0x83:
                                {
                                    WorkingLog.Add(LoggerLevel.Information, "Получен синхросигнал");
                                    CurrentObserver.Notify(this, LEDCommandType.LEDSynchrosignalResponse.ToString(), EventType.Received.ToString(), DateTime.Now);
                                }
                                break;
                            /*case 0x04:
                                break;*/
                            case (int)LEDCommandType.LEDStatusResponse://0x84:
                                {
                                    var les = LEDEquimpentStatus.FromBytes(cmd.Data);
                                    WorkingLog.Add(LoggerLevel.Information, $"Получен статус светодиодов - {string.Join("", les.LEDStatuses.Select(i => i ? 1 : 0))}");
                                    CurrentObserver.Notify(this, LEDCommandType.LEDStatusResponse.ToString(), EventType.Received.ToString(), les);
                                }
                                break;
                            /*case 0x05:
                                break;*/
                            case (int)LEDCommandType.SetLCBMovementParametersResponse://0x85:
                                {
                                    var isOk = cmd.Data[0] == 1;
                                    CurrentObserver.Notify(this, LEDCommandType.SetLCBMovementParametersResponse.ToString(), EventType.Received.ToString(), isOk);
                                }
                                break;
                            /*case 0x06:
                                break;*/
                            case (int)LEDCommandType.GetLCBMovementParametersResponse://0x86:
                                {
                                    var lmp = LEDMovementParameters.FromBytes(cmd.Data);
                                    CurrentObserver.Notify(this, LEDCommandType.GetLCBMovementParametersResponse.ToString(), EventType.Received.ToString(), lmp);

                                }
                                break;
                            /*case 0x07:
                                break;*/
                            case (int)LEDCommandType.SetLCBEquipmentStatusResponse://0x87:
                                {
                                    var isOk = cmd.Data[0] == 1;
                                    CurrentObserver.Notify(this, LEDCommandType.SetLCBEquipmentStatusResponse.ToString(), EventType.Received.ToString(), isOk);
                                }
                                break;
                            /*case 0x08:
                                break;*/
                            case (int)LEDCommandType.GetLCBEquipmentStatusResponse://0x88:
                                {
                                    var les = LEDEquimpentStatus.FromBytes(cmd.Data);
                                    CurrentObserver.Notify(this, LEDCommandType.GetLCBEquipmentStatusResponse.ToString(), EventType.Received.ToString(), les);
                                }
                                break;
                            /*case 0x09:
                                break;*/
                            case (int)LEDCommandType.SetLCBWorkModeResponse://0x89:
                                {
                                    var isOk = cmd.Data[0] == 1;
                                    CurrentObserver.Notify(this, LEDCommandType.SetLCBWorkModeResponse.ToString(), EventType.Received.ToString(), isOk);
                                }
                                break;
                            case (int)LEDCommandType.GetLCBMaxHorizontalStrokeResponse://0x8a:
                                {
                                    var maxStrokeImpulses = (int)BitConverter.ToUInt16(cmd.Data, 0);
                                    CurrentObserver.Notify(this, LEDCommandType.GetLCBMaxHorizontalStrokeResponse.ToString(), EventType.Received.ToString(), maxStrokeImpulses);
                                }
                                break;
                            case (int)LEDCommandType.GetLCBCurrentHorizontalStrokeResponse: //0x8b
                                {
                                    var curStrokeImpulses = (int)BitConverter.ToUInt16(cmd.Data, 0);
                                    CurrentObserver.Notify(this, LEDCommandType.GetLCBCurrentHorizontalStrokeResponse.ToString(), EventType.Received.ToString(), curStrokeImpulses);
                                }
                                break;
                        }
                    }
                }
                Task.Delay(10).Wait();
            }
            CurrentObserver.Notify(this, Operations.Stopped.ToString(), EventType.Success.ToString(), null);
        }

        public enum Operations
        {
            Started,
            Stopped,
            ParametersSet,
            SendCommand,
            ReadUDP,
            AddingRecievedDataToBuffer,
            ModuleParametersSet
        }

        public enum EventType
        {
            Success,
            Error,
            Received
        }



        public class SetLCBCurrentCommand : WaitingCommandBase
        {
            private bool AnswerReceived = false;
            public bool IsSuccessful { get; private set; }

            public SetLCBCurrentCommand(IMainController mainController, AbstractModuleBase module)
                : base(mainController, module, typeof(int), typeof(bool)) { }

            protected override void Executing()
            {
                var module = (LCBModule)Module;
                var current = (int)InputData!;
                module.SetLCBCurrent(current);
            }

            protected override void NotificationReceived(string notificationName, object? data)
            {
                if (notificationName.Contains(LEDCommandType.SetLEDCurrentResponse.ToString()))
                {
                    AnswerReceived = true;
                    IsSuccessful = (bool)data!;
                }
            }

            protected override bool MakeDecisionIsCommandCompleteFunc()
            {
                return AnswerReceived;
            }

            protected override void PrepareOutputData()
            {
                OutputData = IsSuccessful;
            }
        }

        public class GetLCBCurrentCommand : WaitingCommandBase
        {
            private bool AnswerReceived = false;
            public int Current { get; private set; }

            public GetLCBCurrentCommand(IMainController mainController, AbstractModuleBase module)
                : base(mainController, module, null, typeof(int)) { }

            protected override void Executing()
            {
                var module = (LCBModule)Module;
                module.GetLCBCurrent();
            }

            protected override void NotificationReceived(string notificationName, object? data)
            {
                if (notificationName.Contains(LEDCommandType.GetLEDCurrentResponse.ToString()))
                {
                    AnswerReceived = true;
                    Current = (int)data!;
                }
            }

            protected override bool MakeDecisionIsCommandCompleteFunc()
            {
                return AnswerReceived;
            }

            protected override void PrepareOutputData()
            {
                OutputData = Current;
            }
        }

        public class SetLCBMovementParametersCommand : WaitingCommandBase
        {
            private bool AnswerReceived = false;
            public bool IsSuccessful { get; private set; }

            public SetLCBMovementParametersCommand(IMainController mainController, AbstractModuleBase module)
                : base(mainController, module, typeof((int PreformLength, int DelayLength)), typeof(bool)) { }

            protected override void Executing()
            {
                var module = (LCBModule)Module;
                var input = ((int PreformLength, int DelayLength))InputData!;
                module.SetLCBMovementParameters(input.PreformLength, input.DelayLength);
            }

            protected override void NotificationReceived(string notificationName, object? data)
            {
                if (notificationName.Contains(LEDCommandType.SetLCBMovementParametersResponse.ToString()))
                {
                    AnswerReceived = true;
                    IsSuccessful = (bool)data!;
                }
            }

            protected override bool MakeDecisionIsCommandCompleteFunc()
            {
                return AnswerReceived;
            }

            protected override void PrepareOutputData()
            {
                OutputData = IsSuccessful;
            }
        }

        public class GetLCBMovementParametersCommand : WaitingCommandBase
        {
            private bool AnswerReceived = false;
            public LEDMovementParameters Parameters { get; private set; }

            public GetLCBMovementParametersCommand(IMainController mainController, AbstractModuleBase module)
                : base(mainController, module, null, typeof(LEDMovementParameters)) { }

            protected override void Executing()
            {
                var module = (LCBModule)Module;
                module.GetLCBMovementParameters();
            }

            protected override void NotificationReceived(string notificationName, object? data)
            {
                if (notificationName.Contains(LEDCommandType.GetLCBMovementParametersResponse.ToString()))
                {
                    AnswerReceived = true;
                    Parameters = (LEDMovementParameters)data!;
                }
            }

            protected override bool MakeDecisionIsCommandCompleteFunc()
            {
                return AnswerReceived;
            }

            protected override void PrepareOutputData()
            {
                OutputData = Parameters;
            }
        }

        public class SetLCBEquipmentStatusCommand : WaitingCommandBase
        {
            private bool AnswerReceived = false;
            public bool IsSuccessful { get; private set; }

            public SetLCBEquipmentStatusCommand(IMainController mainController, AbstractModuleBase module)
                : base(mainController, module, typeof(LEDEquimpentStatus), typeof(bool)) { }

            protected override void Executing()
            {
                var module = (LCBModule)Module;
                var status = (LEDEquimpentStatus)InputData!;
                module.SetLCBEquipmentStatus(status);
            }

            protected override void NotificationReceived(string notificationName, object? data)
            {
                if (notificationName.Contains(LEDCommandType.SetLCBEquipmentStatusResponse.ToString()))
                {
                    AnswerReceived = true;
                    IsSuccessful = (bool)data!;
                }
            }

            protected override bool MakeDecisionIsCommandCompleteFunc()
            {
                return AnswerReceived;
            }

            protected override void PrepareOutputData()
            {
                OutputData = IsSuccessful;
            }
        }

        public class GetLCBEquipmentStatusCommand : WaitingCommandBase
        {
            private bool AnswerReceived = false;
            public LEDEquimpentStatus EquipmentStatus { get; private set; }

            public GetLCBEquipmentStatusCommand(IMainController mainController, AbstractModuleBase module)
                : base(mainController, module, null, typeof(LEDEquimpentStatus)) { }

            protected override void Executing()
            {
                var module = (LCBModule)Module;
                module.GetLCBEquipmentStatus();
            }

            protected override void NotificationReceived(string notificationName, object? data)
            {
                if (notificationName.Contains(LEDCommandType.GetLCBEquipmentStatusResponse.ToString()))
                {
                    AnswerReceived = true;
                    EquipmentStatus = (LEDEquimpentStatus)data!;
                }
            }

            protected override bool MakeDecisionIsCommandCompleteFunc()
            {
                return AnswerReceived;
            }

            protected override void PrepareOutputData()
            {
                OutputData = EquipmentStatus;
            }
        }

        public class SetLCBWorkModeCommand : WaitingCommandBase
        {
            private bool AnswerReceived = false;
            public bool IsSuccessful { get; private set; }

            public SetLCBWorkModeCommand(IMainController mainController, AbstractModuleBase module)
                : base(mainController, module, null, typeof(bool)) { }

            protected override void Executing()
            {
                var module = (LCBModule)Module;
                module.SetLCBWorkMode();
            }

            protected override void NotificationReceived(string notificationName, object? data)
            {
                if (notificationName.Contains(LEDCommandType.SetLCBWorkModeResponse.ToString()))
                {
                    AnswerReceived = true;
                    IsSuccessful = (bool)data!;
                }
            }

            protected override bool MakeDecisionIsCommandCompleteFunc()
            {
                return AnswerReceived;
            }

            protected override void PrepareOutputData()
            {
                OutputData = IsSuccessful;
            }
        }

        public class SetLCBNonWorkModeCommand : WaitingCommandBase
        {
            private bool AnswerReceived = false;
            public bool IsSuccessful { get; private set; }

            public SetLCBNonWorkModeCommand(IMainController mainController, AbstractModuleBase module)
                : base(mainController, module, null, typeof(bool)) { }

            protected override void Executing()
            {
                var module = (LCBModule)Module;
                module.SetLCBNonWorkMode();
            }

            protected override void NotificationReceived(string notificationName, object? data)
            {
                if (notificationName.Contains(LEDCommandType.SetLCBWorkModeResponse.ToString()))
                {
                    AnswerReceived = true;
                    IsSuccessful = (bool)data!;
                }
            }

            protected override bool MakeDecisionIsCommandCompleteFunc()
            {
                return AnswerReceived;
            }

            protected override void PrepareOutputData()
            {
                OutputData = IsSuccessful;
            }
        }

        public class GetLCBMaxPositionCommand : WaitingCommandBase
        {
            private bool AnswerReceived = false;
            public int MaxPosition { get; private set; }

            public GetLCBMaxPositionCommand(IMainController mainController, AbstractModuleBase module)
                : base(mainController, module, null, typeof(int)) { }

            protected override void Executing()
            {
                var module = (LCBModule)Module;
                module.GetLCBMaxPosition();
            }

            protected override void NotificationReceived(string notificationName, object? data)
            {
                if (notificationName.Contains(LEDCommandType.GetLCBMaxHorizontalStrokeResponse.ToString()))
                {
                    AnswerReceived = true;
                    MaxPosition = (int)data!;
                }
            }

            protected override bool MakeDecisionIsCommandCompleteFunc()
            {
                return AnswerReceived;
            }

            protected override void PrepareOutputData()
            {
                OutputData = MaxPosition;
            }
        }

        public class GetLCBCurrentPositionCommand : WaitingCommandBase
        {
            private bool AnswerReceived = false;
            public int CurrentPosition { get; private set; }

            public GetLCBCurrentPositionCommand(IMainController mainController, AbstractModuleBase module)
                : base(mainController, module, null, typeof(int)) { }

            protected override void Executing()
            {
                var module = (LCBModule)Module;
                module.GetLCBCurrentPosition();
            }

            protected override void NotificationReceived(string notificationName, object? data)
            {
                if (notificationName.Contains(LEDCommandType.GetLCBCurrentHorizontalStrokeResponse.ToString()))
                {
                    AnswerReceived = true;
                    CurrentPosition = (int)data!;
                }
            }

            protected override bool MakeDecisionIsCommandCompleteFunc()
            {
                return AnswerReceived;
            }

            protected override void PrepareOutputData()
            {
                OutputData = CurrentPosition;
            }
        }
    }
}
