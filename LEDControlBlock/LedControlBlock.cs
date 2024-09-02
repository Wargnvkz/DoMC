using DoMCLib.Classes;
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
using DoMCLib.Classes.Old_App_Classes;
using DoMCLib.Classes.Model.LCB;

namespace LedControlBlock
{
    // Изначально это был другой класс который я пытаюсь переделать на ModuleBase
    public class LedControlBlock : ModuleBase
    {
        UdpClient udp;
        int portFrom = 1556;
        int portTo = 1555;
        byte[] buffer;
        IPEndPoint Broadcast;
        IPEndPoint Localaddress;
        IPEndPoint Any;

        TimeSpan LCBTimeout = new TimeSpan(0, 0, 30);

        //int LEDLoadConfigStatus = 0;

        static byte startbyte = 0xff;
        IMainController MainController;
        Observer CurrentObserver;
        ApplicationContext CurrentContext;

        Log WorkingLog;

        public LedControlBlock(IMainController mainController) : base(mainController)
        {
            MainController = mainController;
            Broadcast = new IPEndPoint(IPAddress.Broadcast, portTo);
            Localaddress = new IPEndPoint(IPAddress.Any, portFrom);
            Any = new IPEndPoint(IPAddress.Any, 0);
        }

        public override void Initialize()
        {

        }

        public override void Shutdown()
        {
            throw new NotImplementedException();
        }

        public override void Start()
        {
            WorkingLog = new Log(Log.LogModules.LCB);
        }

        public override void Stop()
        {
            try
            {
                udp?.Close();
            }
            catch { }
            udp = null;
            WorkingLog?.StopLog();
        }

        // Вызов команд снаружи модуля
        private void InterfaceDataExchange_OnCommand(DoMCInterfaceDataExchange sender, ModuleCommand command)
        {
            switch (command)
            {
                case ModuleCommand.InitLCB:
                    {
                        InterfaceDataExchange.InitLCBStatus = 1;
                    }
                    break;
                //1
                case ModuleCommand.SetLCBCurrentRequest:
                    {
                        var cmd = new LEDBlockCommand();
                        cmd.Command = (int)LEDCommandType.SetLEDCurrentRequest;
                        cmd.Data = new byte[1];
                        sender.LEDStatus.LEDCurrent = sender.Configuration.LCBSettings.LEDCurrent;
                        cmd.Data[0] = (byte)(sender.LEDStatus.LEDCurrent / 10);
                        sender.LEDOutCommands.Enqueue(cmd);
                    }
                    break;
                //2
                case ModuleCommand.GetLCBCurrentRequest:
                    {
                        var cmd = new LEDBlockCommand();
                        cmd.Command = (int)LEDCommandType.GetLEDCurrentRequest;
                        cmd.Data = new byte[0];
                        sender.LEDOutCommands.Enqueue(cmd);
                    }
                    break;
                //5
                case ModuleCommand.SetLCBMovementParametersRequest:
                    {
                        sender.LEDStatus.PreformLength = (int)(sender.Configuration.LCBSettings.PreformLength);
                        sender.LEDStatus.DelayLength = (int)(sender.Configuration.LCBSettings.DelayLength);
                        var lmp = new LEDMovementParameters();
                        lmp.PreformLengthImpulses = (int)(sender.LEDStatus.PreformLength);
                        lmp.DelayLengthImpulses = (int)(sender.LEDStatus.DelayLength);

                        var cmd = new LEDBlockCommand();
                        cmd.Command = (int)LEDCommandType.SetLCBMovementParametersRequest;
                        cmd.Data = lmp.ToBytes();
                        sender.LEDOutCommands.Enqueue(cmd);
                    }
                    break;
                //6
                case ModuleCommand.GetLCBMovementParametersRequest:
                    {
                        var cmd = new LEDBlockCommand();
                        cmd.Command = (int)LEDCommandType.GetLCBMovementParametersRequest;
                        cmd.Data = new byte[0];
                        sender.LEDOutCommands.Enqueue(cmd);
                    }
                    break;
                //7
                case ModuleCommand.SetLCBEquipmentStatusRequest:
                    {
                        var les = new LEDEquimpentStatus();
                        les.Inputs = sender.LEDStatus.Inputs;
                        les.Outputs = sender.LEDStatus.Outputs;
                        les.Magnets = sender.LEDStatus.Magnets;
                        les.Valve = sender.LEDStatus.Valve;
                        les.LEDStatuses = sender.LEDStatus.LEDStatuses;

                        var cmd = new LEDBlockCommand();
                        cmd.Command = (int)LEDCommandType.SetLCBEquipmentStatusRequest;
                        cmd.Data = les.ToBytesFull();
                        sender.LEDOutCommands.Enqueue(cmd);
                    }
                    break;
                //8
                case ModuleCommand.GetLCBEquipmentStatusRequest:
                    {
                        var cmd = new LEDBlockCommand();
                        cmd.Command = (int)LEDCommandType.GetLCBEquipmentStatusRequest;
                        cmd.Data = new byte[0];
                        sender.LEDOutCommands.Enqueue(cmd);
                    }
                    break;
                //9
                case ModuleCommand.SetLCBWorkModeRequest:
                    {
                        var cmd = new LEDBlockCommand();
                        cmd.Command = (int)LEDCommandType.SetLCBWorkModeRequest;
                        cmd.Data = new byte[1];
                        cmd.Data[0] = (byte)1;
                        sender.LEDOutCommands.Enqueue(cmd);
                    }
                    break;
                case ModuleCommand.SetLCBNonWorkModeRequest:
                    {
                        var cmd = new LEDBlockCommand();
                        cmd.Command = (int)LEDCommandType.SetLCBWorkModeRequest;
                        cmd.Data = new byte[1];
                        cmd.Data[0] = (byte)0;
                        sender.LEDOutCommands.Enqueue(cmd);
                    }
                    break;
                //10
                case ModuleCommand.GetLCBMaxPositionRequest:
                    {
                        var cmd = new LEDBlockCommand();
                        cmd.Command = (int)LEDCommandType.GetLCBMaxHorizontalStrokeRequest;
                        cmd.Data = new byte[0];
                        sender.LEDOutCommands.Enqueue(cmd);
                    }
                    break;
                //11
                case ModuleCommand.GetLCBCurrentPositionRequest:
                    {
                        var cmd = new LEDBlockCommand();
                        cmd.Command = (int)LEDCommandType.GetLCBCurrentHorizontalStrokeRequest;
                        cmd.Data = new byte[0];
                        sender.LEDOutCommands.Enqueue(cmd);
                    }
                    break;
                case ModuleCommand.LCBUDPReconnect:
                    {

                    }
                    break;
                case ModuleCommand.StopModuleWork:
                case ModuleCommand.LCBStop:
                    {
                        if (udp != null)
                        {
                            InterfaceDataExchange.InitLCBStatus = 0;
                            try
                            {
                                udp.Close();
                            }
                            catch { }
                            udp = null;
                        }
                    }
                    break;
            }
        }

        // OnRead, OnWrite, OnCalculate выполняются в отдельном потоке заданном снаружи класса
        public bool OnRead(ref DataExchangeKernel.ACS_Core.Memory data, double dt)
        {
            LCBTimeout = new TimeSpan(0, 0, 0, 0, InterfaceDataExchange.Configuration.Timeouts.WaitForLCBCardAnswerTimeout);
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
                            InterfaceDataExchange.LEDStatus.UDPReceived = DateTime.Now;
                        }
                        catch (Exception ex)
                        {
                            InterfaceDataExchange.Errors.UDPError = true;
                            throw ex;
                        }

                        try
                        {
                            var bl = buffer.Length;
                            Array.Resize(ref buffer, bl + tmp.Length);
                            Array.Copy(tmp, 0, buffer, bl, tmp.Length);
                            InterfaceDataExchange.Errors.LCBMemoryError = false;
                        }
                        catch (Exception ex)
                        {
                            InterfaceDataExchange.Errors.LCBMemoryError = true;
                            throw ex;
                        }
                    }
                }
            }
            return true;

        }
        public bool OnWrite(ref DataExchangeKernel.ACS_Core.Memory data, double dt)
        {
            //if (guidFound)
            {

                if (InterfaceDataExchange.InitLCBStatus == 1)
                {
                    if (udp == null)
                    {
                        //udp = new UdpClient(new IPEndPoint(IPAddress.Any, port));
                        try
                        {

                            udp = new UdpClient();
                            Localaddress = new IPEndPoint(IPAddress.Any, portFrom);
                            udp.Client.Bind(Localaddress);

                            udp.EnableBroadcast = true;
                            udp.AllowNatTraversal(false);
                            InterfaceDataExchange.Errors.UDPError = false;
                            InterfaceDataExchange.LEDStatus.LastCommandSent = DateTime.Now;
                            InterfaceDataExchange.LEDStatus.LastCommandResponseReceived = DateTime.Now;
                            InterfaceDataExchange.Errors.UDPReceivesTrash = false;
                        }
                        catch (Exception ex)
                        {
                            InterfaceDataExchange.Errors.UDPError = true;
                            throw ex;
                        }
                    }
                    //udp.LoadConfigurationCommand(Broadcast);
                    buffer = new byte[0];
                    InterfaceDataExchange.LEDOutCommands = new ConcurrentQueue<LEDBlockCommand>();
                    /*var configcmd = new LEDBlockCommand();
                    configcmd.Command = 1;
                    configcmd.Data = new byte[] { 70 };
                    InterfaceDataExchange.LEDOutCommands.Enqueue(configcmd);*/
                    InterfaceDataExchange.InitLCBStatus = 2;
                }


                LEDBlockCommand lbc = null;
                do
                {
                    if (InterfaceDataExchange.LEDOutCommands != null)
                    {
                        if (InterfaceDataExchange.LEDOutCommands.TryDequeue(out lbc))
                        {
                            InterfaceDataExchange.LEDStatus.LastCommandSent = DateTime.Now;
                            InterfaceDataExchange.LEDStatus.NumberOfLastCommandReceived = 0;
                            InterfaceDataExchange.LEDStatus.LastCommandResponseReceived = DateTime.MinValue;
                            InterfaceDataExchange.LEDStatus.NumberOfLastCommandSent = lbc.Command;
                            var bytes = lbc.ToBytes();
                            WorkingLog.Add($"-> БУС: <{Log.ByteArrayToHexString(bytes)}>");
                            try
                            {
                                udp?.Send(bytes, bytes.Length, Broadcast);
                            }
                            catch (Exception ex)
                            {
                                InterfaceDataExchange.Errors.UDPError = true;
                                throw ex;
                            }
                        }
                    }
                } while (lbc != null);
            }
            return true;

        }



        public bool OnCalculate(ref DataExchangeKernel.ACS_Core.Memory data, double dt)
        {
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
                                InterfaceDataExchange.LEDStatus.LastCommandReceivedStatusIsOK = isOk;
                            }
                            break;
                        /*case 0x02:
                            break;*/
                        case (int)LEDCommandType.GetLEDCurrentResponse://0x82:
                            {
                                var current = cmd.Data[0] * 10;
                                InterfaceDataExchange.LEDStatus.LEDCurrent = current;
                            }
                            break;
                        /*case 0x03:
                            break;*/
                        case (int)LEDCommandType.LEDSynchrosignalResponse://0x83:
                            {
                                InterfaceDataExchange.LEDStatus.TimePreviousSyncSignalGot = InterfaceDataExchange.LEDStatus.TimeSyncSignalGot;
                                InterfaceDataExchange.LEDStatus.TimeSyncSignalGot = DateTime.Now;
                                WorkingLog.Add("Получен синхросигнал");
                                MainController.Observer.Notify("LEDSynchrosignal", null);
                            }
                            break;
                        /*case 0x04:
                            break;*/
                        case (int)LEDCommandType.LEDStatusResponse://0x84:
                            {
                                var les = LEDEquimpentStatus.FromBytes(cmd.Data);
                                InterfaceDataExchange.LEDStatus.TimeLEDStatusGot = DateTime.Now;
                                InterfaceDataExchange.LEDStatus.LEDStatuses = les.LEDStatuses;
                                WorkingLog.Add($"Получен статус светодиодов - {String.Join("", les.LEDStatuses.Select(i => i ? 1 : 0))}");
                            }
                            break;
                        /*case 0x05:
                            break;*/
                        case (int)LEDCommandType.SetLCBMovementParametersResponse://0x85:
                            {
                                var isOk = cmd.Data[0] == 1;
                                InterfaceDataExchange.LEDStatus.LastCommandReceivedStatusIsOK = isOk;
                            }
                            break;
                        /*case 0x06:
                            break;*/
                        case (int)LEDCommandType.GetLCBMovementParametersResponse://0x86:
                            {
                                var lmp = LEDMovementParameters.FromBytes(cmd.Data);
                                InterfaceDataExchange.LEDStatus.PreformLength = (int)(lmp.PreformLengthImpulses);
                                InterfaceDataExchange.LEDStatus.DelayLength = (int)(lmp.DelayLengthImpulses);
                                InterfaceDataExchange.LEDStatus.LastMovementParametersReceived = DateTime.Now;

                            }
                            break;
                        /*case 0x07:
                            break;*/
                        case (int)LEDCommandType.SetLCBEquipmentStatusResponse://0x87:
                            {
                                var isOk = cmd.Data[0] == 1;
                                InterfaceDataExchange.LEDStatus.LastCommandReceivedStatusIsOK = isOk;
                            }
                            break;
                        /*case 0x08:
                            break;*/
                        case (int)LEDCommandType.GetLCBEquipmentStatusResponse://0x88:
                            {
                                var les = LEDEquimpentStatus.FromBytes(cmd.Data);
                                if (InterfaceDataExchange.IsWorkingModeStarted)
                                {
                                    InterfaceDataExchange.LEDStatus.LEDStatuses = les.LEDStatuses;
                                }
                                InterfaceDataExchange.LEDStatus.TimeLEDStatusGot = DateTime.Now;
                                InterfaceDataExchange.LEDStatus.InOutStatusGot = DateTime.Now;
                                InterfaceDataExchange.LEDStatus.LEDStatuses = les.LEDStatuses;
                                InterfaceDataExchange.LEDStatus.Inputs = les.Inputs;
                                InterfaceDataExchange.LEDStatus.Outputs = les.Outputs;
                                InterfaceDataExchange.LEDStatus.Magnets = les.Magnets;
                                InterfaceDataExchange.LEDStatus.Valve = les.Valve;
                            }
                            break;
                        /*case 0x09:
                            break;*/
                        case (int)LEDCommandType.SetLCBWorkModeResponse://0x89:
                            {
                                var isOk = cmd.Data[0] == 1;
                                InterfaceDataExchange.LEDStatus.LastCommandReceivedStatusIsOK = isOk;
                            }
                            break;
                        case (int)LEDCommandType.GetLCBMaxHorizontalStrokeResponse://0x8a:
                            {
                                var maxStrokeImpulses = BitConverter.ToUInt16(cmd.Data, 0);
                                InterfaceDataExchange.LEDStatus.MaximumHorizontalStroke = maxStrokeImpulses;
                            }
                            break;
                        case (int)LEDCommandType.GetLCBCurrentHorizontalStrokeResponse: //0x8b
                            {
                                var curStrokeImpulses = BitConverter.ToUInt16(cmd.Data, 0);
                                InterfaceDataExchange.LEDStatus.CurrentHorizontalStroke = curStrokeImpulses;
                            }
                            break;
                    }
                    InterfaceDataExchange.LEDStatus.NumberOfLastCommandReceived = cmd.Command;
                    InterfaceDataExchange.LEDStatus.LastCommandResponseReceived = DateTime.Now;
                    InterfaceDataExchange.Errors.UDPReceivesTrash = false;
                    /*InterfaceDataExchange.CurrentCycleLED = new CycleImagesLED();
                    InterfaceDataExchange.CurrentCycleLED.CycleDateTime = DateTime.Now;
                    InterfaceDataExchange.CurrentCycleLED.IsStatusesReady = false;*/
                }
            }
            if (InterfaceDataExchange.IsWorkingModeStarted)
            {
                // Если мы не получаем статусы и синхроимпульс, то
                if (DateTime.Now - InterfaceDataExchange.LEDStatus.TimeSyncSignalGot > LCBTimeout &&
                    DateTime.Now - InterfaceDataExchange.LEDStatus.TimeLEDStatusGot > LCBTimeout)

                {
                    // БУС не отвечает
                    InterfaceDataExchange.Errors.LCBDoesNotRespond = true;
                    // а если пакеты все же приходят
                    if (InterfaceDataExchange.LEDStatus.UDPReceived - InterfaceDataExchange.LEDStatus.TimeSyncSignalGot < LCBTimeout)
                    {
                        // то говорим, что приходит мусор
                        InterfaceDataExchange.Errors.UDPReceivesTrash = true;
                    }
                }
                else
                {
                    InterfaceDataExchange.Errors.LCBDoesNotRespond = false;
                    InterfaceDataExchange.Errors.UDPReceivesTrash = false;
                }
                // Не получены статусы светодиодов от БУС. v1.Если после синхроимпульса уже есть данные от ПЗС, а статусов нет, то они не передавались.
                // Реализовано: Если статус получен после текущего или прошлого синхроимпульса, то все ок.
                if (InterfaceDataExchange.LEDStatus.TimeSyncSignalGot < InterfaceDataExchange.LEDStatus.TimeLEDStatusGot || InterfaceDataExchange.LEDStatus.TimePreviousSyncSignalGot < InterfaceDataExchange.LEDStatus.TimeSyncSignalGot)
                {
                    InterfaceDataExchange.Errors.LEDStatusGettingError = false;
                }
                else
                {
                    InterfaceDataExchange.Errors.LEDStatusGettingError = true;
                }
            }
            return true;

        }

        public void InterfaceParameters() { }

        public void Dispose()
        {
        }


    }


}
