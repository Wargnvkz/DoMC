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
using System.ComponentModel;
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
    [Description("Бракёр")]
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
        private SemaphoreSlim _reconnectLock = new(1, 1);
        int _timeoutCounter = 0;

        public RDPBModule(IMainController MainController) : base(MainController)
        {
            mainController = MainController;
            WorkingLog = mainController.GetLogger(this.GetType().GetDescriptionOrName());
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
            try
            {
                cancellationTokenSource?.Cancel();
            }
            catch { }
            try { TCPClientCommandConnection?.Close(); } catch { }
            IsStarted = false;
            WorkingLog.Add(LoggerLevel.Critical, "Модуль остановлен");
        }

        //Use HandleConnectionLost(string reason) instead
        private async Task ReconnectLoop()
        {
            WorkingLog.Add(LoggerLevel.Critical, $"Попытка переподключения");
            WorkingLog.Add(LoggerLevel.Critical, $"Отключаем соединение");
            TCPClientCommandConnection.Close();

            for (int i = 1; i <= 5; i++)
            {
                try
                {
                    WorkingLog.Add(LoggerLevel.Critical, $"Попытка переподключения {i}");

                    await TCPClientCommandConnection.ConnectAsync(remoteIP, (int)(CurrentStatus.RDPBTimeoutInns / 10000), cancellationTokenSource.Token);

                    WorkingLog.Add(LoggerLevel.Critical, "Соединение восстановлено");

                    _timeoutCounter = 0;
                    return;
                }
                catch (Exception ex)
                {
                    WorkingLog.Add(LoggerLevel.Critical, "Ошибка переподключения", ex);
                    await Task.Delay(2000);
                }
            }

            WorkingLog.Add(LoggerLevel.Critical, "Не удалось восстановить соединение");
        }
        private async Task HandleConnectionLost(string reason)
        {
            _timeoutCounter++;
            WorkingLog.Add(LoggerLevel.Critical, $"Потеря соединения: {reason}");

            // 1. сброс pending
            _pendingCommandController.SetException(
                new Exception("Соединение потеряно"));

            // 2. закрытие
            try { TCPClientCommandConnection?.Close(); } catch { }



            // 3. реконнект
            await _reconnectLock.WaitAsync();
            try
            {
                if (_timeoutCounter == 0) return; // уже восстановились

                await ReconnectLoop();
            }
            finally
            {
                _reconnectLock.Release();
            }

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
                return await _pendingCommandController.AsyncCommandAsync(Token, null, async () =>
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
                    try
                    {
                        await TCPClientCommandConnection.WriteAsync(bytes, 0, bytes.Length, cancellationTokenSource.Token);
                    }
                    catch (Exception ex)
                    {
                        WorkingLog.Add(LoggerLevel.Critical, "Ошибка передачи данных бракеру: ", ex);
                        await HandleConnectionLost(ex.Message);
                        throw;
                    }
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

                // асинхронно читаем
                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                    cancellationTokenSource.Token); // глобальный

                linkedCts.CancelAfter(TimeSpan.FromSeconds(30)); // локальный таймаут

                read = await TCPClientCommandConnection.ReadAsync(tempreadbuff, 0, tempreadbuff.Length, linkedCts.Token);


                if (read > 0)
                {
                    AddToBuffer(tempreadbuff, 0, read);
                    CurrentStatus.SetTimeLastReceived();
                }

            }
            catch (Exception ex)
            {
                WorkingLog.Add(LoggerLevel.Critical, "Ошибка при чтении данных от бракёра. ", ex);
                throw;
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

                        var success = _pendingCommandController.TrySetResult(0, CurrentStatus);
                        if (!success)
                        {
                            WorkingLog.Add(LoggerLevel.Critical, "Не удалось установить результат команды");
                        }
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
                catch (OperationCanceledException)
                {
                    if (cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        // это Stop()
                        WorkingLog?.Add(LoggerLevel.Information, "Остановка модуля");
                    }
                    else
                    {
                        // это таймаут
                        WorkingLog?.Add(LoggerLevel.FullDetailedInformation, "Таймаут ожидания данных");

                        await HandleConnectionLost("Таймаут ожидания данных");
                    }

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
