using DoMCModuleControl;
using DoMCModuleControl.Logging;
using DoMCModuleControl.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes.Model.RDPB
{
    /// <summary>
    /// Observer of MainController:
    /// "RDPBModule.StatusChanged" - RDPBStatus
    /// </summary>
    public partial class RDPBModule : ModuleBase
    {
        public Configuration.RemoveDefectedPreformBlockConfig config;
        private IMainController mainController;
        TcpClient client;
        IPEndPoint remoteIP;
        private byte[] buffer;
        public bool IsConnected;
        private int MachineNumber = 1;
        private RDPBStatus CurrentStatus;

        private TimeSpan Timeout = new TimeSpan(0, 0, 5);

        private Thread ProcessDataThread;
        public bool IsStarted { get; private set; }

        private ILogger WorkingLog;
        public RDPBModule(IMainController MainController) : base(MainController)
        {
            mainController = MainController;
            WorkingLog = mainController.GetLogger(this.GetType().Name);
            WorkingLog.SetMaxLogginLevel(LoggerLevel.FullDetailedInformation);
        }

        public void Start()
        {
            if (IsStarted) return;
            MakeConnected();
            IsStarted = true;
            ProcessDataThread = new Thread(RDPBProcessDataThreadProc);
            ProcessDataThread.Start();
            WorkingLog.Add(LoggerLevel.Critical, "Модуль запущен");
        }
        public void Stop()
        {
            if (!IsStarted) return;
            try { ProcessDataThread.Abort(); } catch { }
            try { client?.Close(); } catch { }
            IsStarted = false;
            WorkingLog.Add(LoggerLevel.Critical, "Модуль остановлен");
        }

        private bool DoNeedToRestart()
        {
            if (client?.Connected ?? false)
            {
                // таймаут если сообщение послано, ответ не получен и если оно послано давно
                //var isTimeouted = CurrentStatus.LastSentTime != DateTime.MinValue && !CurrentStatus.ResponseGot && (DateTime.Now - CurrentStatus.LastSentTime) < Timeout;
                //var isTimeouted = CurrentStatus.LastSentTime != 0 && !CurrentStatus.ResponseGot && (CurrentStatus.timer- CurrentStatus.LastSentTime) < Timeout;
                var isTimeouted = CurrentStatus.IsTimeout;
                if (isTimeouted) // timeout времени бракер не отвечает
                {
                    return true;
                }
                return false;
            }
            else return true;
        }

        private void MakeConnected()
        {
            if (DoNeedToRestart())
            {
                if (client?.Connected ?? false)
                {
                    client?.Close();
                    client = null;

                }
                client = new TcpClient();
                client.Connect(remoteIP);
            }

        }

        public void Send(RDPBCommandType Command, int CoolingBlocks = 0)
        {
            if (client.Connected)
            {
                WorkingLog.Add(LoggerLevel.FullDetailedInformation, $"Команда модулю бракера: <{Command.ToString()}>");
                CurrentStatus.SetTimeLastSent();
                //CurrentStatus.LastSentTime = Timer.ElapsedTicks;//DateTime.Now;
                RDPBStatus stat = new RDPBStatus();
                stat.CommandType = Command;
                stat.MachineNumber = MachineNumber;
                stat.CoolingBlocksQuantity = CoolingBlocks;
                CurrentStatus.SentCommandType = Command;
                CurrentStatus.SetTimeCommandSent();
                //CurrentStatus.TimeCommandSent = Timer.ElapsedTicks;//DateTime.Now;
                var str = stat.ToString();
                WorkingLog.Add(LoggerLevel.FullDetailedInformation, $"Команда бракеру: <{str.Trim()}>");
                var bytes = Encoding.ASCII.GetBytes(str);
                var ns = client.GetStream();
                ns.Write(bytes, 0, bytes.Length);
            }
        }

        public void SendManualCommandProc(string cmd)
        {
            var crc = RDPBStatus.CalcLRC(cmd);
            var rescmd = cmd.Trim() + " " + crc + "\r\n";
            WorkingLog.Add(LoggerLevel.FullDetailedInformation, $"Команда бракеру: <{rescmd.Trim()}>");
            var bytes = Encoding.ASCII.GetBytes(rescmd);
            var ns = client.GetStream();
            ns.Write(bytes, 0, bytes.Length);
        }

        public void GetData()
        {
            ReadNetwork();
            ProcessBuffer();
        }

        private void ReadNetwork()
        {
            if (client.Connected)
            {
                if (buffer == null) buffer = new byte[0];
                var ns = client.GetStream();
                while (ns.CanRead && ns.DataAvailable)
                {
                    ns.ReadTimeout = 1;
                    byte[] tempreadbuff = new byte[1024];
                    var read = ns.Read(tempreadbuff, 0, 1024);
                    if (read > 0)
                    {
                        var tempnextbuffer = new byte[buffer.Length + read];
                        Array.Copy(buffer, 0, tempnextbuffer, 0, buffer.Length);
                        Array.Copy(tempreadbuff, 0, tempnextbuffer, buffer.Length, read);
                        buffer = tempnextbuffer;
                        //CurrentStatus.TimeLastReceive = DateTime.Now;
                        CurrentStatus.SetTimeLastReceived();
                    }
                }
            }

        }

        private void ProcessBuffer()
        {
            if (buffer.Length > 0)
            {
                do
                {
                    var StartIndex = Array.IndexOf<byte>(buffer, 0x4E);
                    var StopIndex = Array.IndexOf<byte>(buffer, 0x0A, StartIndex + 1);
                    var NextStartIndex = Array.IndexOf<byte>(buffer, 0x4E, StartIndex + 1);
                    if (StartIndex == -1 || (StopIndex == -1 && NextStartIndex != -1))
                    {
                        break;
                    }

                    if (NextStartIndex != -1 && NextStartIndex < StopIndex)
                    {
                        var nbl = buffer.Length - NextStartIndex;
                        Array.Copy(buffer, NextStartIndex, buffer, 0, nbl);
                        Array.Resize(ref buffer, nbl);
                        continue;
                    }

                    int endmessage;
                    if (StopIndex == -1)
                    {
                        if (NextStartIndex == -1) break;
                        endmessage = NextStartIndex;
                    }
                    else
                    {
                        endmessage = StopIndex + 1;
                    }
                    var msglen = endmessage - StartIndex;
                    var msgarr = new byte[msglen];
                    Array.Copy(buffer, StartIndex, msgarr, 0, msglen);
                    var newbufferlength = buffer.Length - endmessage;
                    Array.Copy(buffer, endmessage, buffer, 0, newbufferlength);
                    Array.Resize(ref buffer, newbufferlength);
                    if (CurrentStatus == null) CurrentStatus = new RDPBStatus();
                    var AnswerString = Encoding.ASCII.GetString(msgarr);
                    WorkingLog.Add(LoggerLevel.FullDetailedInformation, $"Получено от бракера: <{AnswerString.Trim()}>");
                    var result = CurrentStatus.ChangeFromString(AnswerString);
                    mainController.GetObserver().Notify(this, "StatusChanged", result.ToString(), CurrentStatus);
                }
                while (buffer.Length > 0);
            }
        }

        private void RDPBProcessDataThreadProc()
        {
            while (IsStarted)
            {
                try
                {
                    if (DoNeedToRestart())
                        Start();
                    GetData();
                }
                catch (Exception ex)
                {
                    WorkingLog?.Add(LoggerLevel.Critical, "", ex);
                }
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
