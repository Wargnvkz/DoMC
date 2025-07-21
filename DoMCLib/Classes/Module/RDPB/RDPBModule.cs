using DoMCLib.Classes.Configuration;
using DoMCLib.Classes.Module.RDPB.Classes;
using DoMCLib.Configuration;
using DoMCModuleControl;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Logging;
using DoMCModuleControl.Modules;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DoMCLib.Classes.Module.RDPB
{
    /// <summary>
    /// Observer of MainController:
    /// "RDPBModule.StatusChanged" - RDPBStatus
    /// </summary>
    public partial class RDPBModule : AbstractModuleBase
    {
        private DoMCLib.Classes.Configuration.RemoveDefectedPreformBlockConfig RDPBConfig;
        private IMainController mainController;
        TcpSocketDevice TCPClientCommandConnection = new TcpSocketDevice();
        IPEndPoint remoteIP;
        private byte[] ReadBuffer;
        public bool IsConnected;
        private int MachineNumber = 1;
        private RDPBStatus CurrentStatus = new RDPBStatus();

        private TimeSpan Timeout = new TimeSpan(0, 0, 5);

        //private Thread ProcessDataThread;
        public bool IsStarted { get; private set; }

        private ILogger WorkingLog;

        Task task;
        CancellationTokenSource cancellationTokenSource;
        PendingCommandController<RDPBStatus> _pendingCommandController = new PendingCommandController<RDPBStatus>();

        public RDPBModule(IMainController MainController) : base(MainController)
        {
            mainController = MainController;
            WorkingLog = mainController.GetLogger(this.GetType().Name);
            WorkingLog.SetMaxLogginLevel(LoggerLevel.FullDetailedInformation);
        }
        public async Task SetConfig(ApplicationConfiguration config)
        {
            var wasConnected = IsConnected;
            if (wasConnected) await Stop();
            RDPBConfig = config.HardwareSettings.RemoveDefectedPreformBlockConfig;
            remoteIP = new IPEndPoint(IPAddress.Parse(RDPBConfig.IP), RDPBConfig.Port);
            CurrentStatus.SetTimeout(config.HardwareSettings.Timeouts.WaitForRDPBCardAnswerTimeoutInSeconds * 1000);
            MachineNumber = config.HardwareSettings.RemoveDefectedPreformBlockConfig.MachineNumber;
            if (wasConnected) await Start();
        }

        public async Task Start()
        {
            if (IsStarted) return;

            cancellationTokenSource = new CancellationTokenSource();
            await TCPClientCommandConnection.ConnectAsync(remoteIP, (int)(CurrentStatus.RDPBTimeoutInns / 10000), cancellationTokenSource.Token);

            task = Task.Run(RDPBProcessDataThreadProc);

            WorkingLog.Add(LoggerLevel.Critical, "Модуль запущен");
        }
        public async Task Stop()
        {
            cancellationTokenSource?.Cancel();
            try { TCPClientCommandConnection?.Close(); } catch { }
            IsStarted = false;
            WorkingLog.Add(LoggerLevel.Critical, "Модуль остановлен");
        }

        /*private bool DoConnectionNeedToRestart()
        {
            if (TCPClientCommandConnection?.Connected ?? false)
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
        }*/

        public async Task<RDPBStatus> Send(RDPBCommandType Command, CancellationToken Token, int CoolingBlocks = 0)
        {
            if (IsStarted)
            {
                return await _pendingCommandController.AsyncCommand(Token, null, async () =>
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
                    await TCPClientCommandConnection.WriteAsync(bytes, 0, bytes.Length, cancellationTokenSource.Token);
                });
            }
            else
            {
                WorkingLog.Add(LoggerLevel.FullDetailedInformation, $"Нет подключения к бракёру");
                throw new DoMCNotConnectedException();
            }

        }

        public async Task SendManualCommandProc(string cmd)
        {
            var crc = RDPBStatus.CalcLRC(cmd);
            var rescmd = cmd.Trim() + " " + crc + "\r\n";
            WorkingLog.Add(LoggerLevel.FullDetailedInformation, $"Команда бракеру: <{rescmd.Trim()}>");
            var bytes = Encoding.ASCII.GetBytes(rescmd);
            await TCPClientCommandConnection.WriteAsync(bytes, 0, bytes.Length, cancellationTokenSource.Token);
        }


        private async Task ReadNetwork()
        {
            try
            {
                if (ReadBuffer == null) ReadBuffer = new byte[0];

                byte[] tempreadbuff = new byte[1024];
                int read = 0;

                // ждём, пока появятся байты
                if (TCPClientCommandConnection.AvailableBytes() > 0)
                {
                    // асинхронно читаем
                    read = await TCPClientCommandConnection.ReadAsync(tempreadbuff, 0, tempreadbuff.Length, cancellationTokenSource.Token);
                }

                if (read > 0)
                {
                    AddToBuffer(tempreadbuff, 0, read);
                    CurrentStatus.SetTimeLastReceived();
                }

            }
            catch (Exception ex)
            {
                WorkingLog.Add(LoggerLevel.Critical, "Ошибка при чтении данных от бракёра. ", ex);
            }

        }
        private void AddToBuffer(byte[] data, int start, int length)
        {
            lock (ReadBuffer)
            {
                var oldSize = ReadBuffer.Length;
                Array.Resize(ref ReadBuffer, oldSize + length);
                Array.Copy(data, start, ReadBuffer, oldSize, length);
            }
        }

        private void ProcessBuffer()
        {
            try
            {
                if (ReadBuffer.Length > 0)
                {
                    do
                    {
                        var StartIndex = Array.IndexOf<byte>(ReadBuffer, 0x4E);
                        var StopIndex = Array.IndexOf<byte>(ReadBuffer, 0x0A, StartIndex + 1);
                        var NextStartIndex = Array.IndexOf<byte>(ReadBuffer, 0x4E, StartIndex + 1);
                        if (StartIndex == -1 || (StopIndex == -1 && NextStartIndex != -1))
                        {
                            break;
                        }

                        if (NextStartIndex != -1 && NextStartIndex < StopIndex)
                        {
                            var nbl = ReadBuffer.Length - NextStartIndex;
                            Array.Copy(ReadBuffer, NextStartIndex, ReadBuffer, 0, nbl);
                            Array.Resize(ref ReadBuffer, nbl);
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
                        Array.Copy(ReadBuffer, StartIndex, msgarr, 0, msglen);
                        var newbufferlength = ReadBuffer.Length - endmessage;
                        Array.Copy(ReadBuffer, endmessage, ReadBuffer, 0, newbufferlength);
                        Array.Resize(ref ReadBuffer, newbufferlength);
                        if (CurrentStatus == null) CurrentStatus = new RDPBStatus();
                        var AnswerString = Encoding.ASCII.GetString(msgarr);
                        WorkingLog.Add(LoggerLevel.FullDetailedInformation, $"Получено от бракера: <{AnswerString.Trim()}>");
                        var result = CurrentStatus.ChangeFromString(AnswerString);
                        mainController.GetObserver().Notify(this, CurrentStatus.CommandType.ToString(), result.ToString(), CurrentStatus);
                        WorkingLog.Add(LoggerLevel.FullDetailedInformation, $"Получена команда от бракёра: {CurrentStatus.CommandType}");

                        _pendingCommandController.TrySetResult(0, CurrentStatus);
                    }
                    while (ReadBuffer.Length > 0);
                }
            }
            catch (Exception ex)
            {
                WorkingLog.Add(LoggerLevel.Critical, "Ошибка при обработке данных от бракёра. ", ex);
            }
        }

        private async Task RDPBProcessDataThreadProc()
        {
            WorkingLog?.Add(LoggerLevel.Critical, "Запуск потока обработки модуля бракера");
            IsStarted = true;
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {

                try
                {
                    await ReadNetwork();
                    ProcessBuffer();
                }
                catch (Exception ex)
                {
                    WorkingLog?.Add(LoggerLevel.Critical, "Ошибка обработки данных бракера. ", ex);
                }
            }
            WorkingLog?.Add(LoggerLevel.Critical, "Остановка потока обработки модуля бракера.");
            IsStarted = false;
        }

        public void Dispose()
        {
            Stop().FireAndForgetWithResult(null, null);
        }
    }
}
