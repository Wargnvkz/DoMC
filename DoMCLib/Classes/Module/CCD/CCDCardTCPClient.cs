using DoMCLib.Classes.Configuration.CCD;
using DoMCLib.Classes.Module.CCD.CCDCardDataExchangeCommandClasses;
using DoMCLib.Exceptions;
using DoMCLib.Tools;
using DoMCModuleControl;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Logging;
using DoMCModuleControl.Modules;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace DoMCLib.Classes.Module.CCD
{
    /// <summary>
    /// Работа с одной платой по протоколу TCP/IP
    /// </summary>
    [Description("Платы ПЗС")]
    public class CCDCardTCPClient
    {
        private static string BaseIPv4Address = "192.168.255.0";
        public int PortCommand = 200;
        public int PortData = 100;
        public int CardNumber { get; private set; }
        int SocketNumber = 8;
        //public SocketWorkStatus[] SocketsStatuses { get; private set; }
        //SocketReadingParameters[] SocketConfigurations;
        //private CancellationTokenSource receiveCancellationTokenSource { get; set; }
        private Task ReceiveTask { get; set; }

        ISocketDevice TCPClientCommandConnection;
        Type SocketDeviceType;
        //ISocketDevice TCPClientImageDataConnection;
        //private TcpClient TCPClientCommandConnection;
        //private TcpClient TCPClientImageDataConnection;
        private DateTime LastReceiveAt;
        private DateTime LastSendAt;
        public int ReconnectTimeoutInSeconds = 30;
        //private bool TerminateThread = false;
        //private Thread ReceiveThread;
        public bool IsStarted { get; private set; } = false;
        byte[] ReadBuffer = new byte[0];
        //public long TotalReadBytes;
        //public int ReadDataTimeoutSeconds = 30;
        public int ConnectionTimeoutInMilliseconds = 4000;
        public bool IsConnected { get; private set; }
        private bool IsCardNumberUsed = false;
        public string IPAddr { get; private set; }
        //public DateTime LastExceptionDateTime;
        //public Exception LastException;
        //public string LastExceptionPlace;
        //public int LastReadingTime = 0;

        //public bool IsLogginOn = true;
        //public bool IsLogginSocketStatusOn = true;
        private ILogger WorkingLog;
        //private ILogger WorkingLogSocketStatus;
        private IMainController Controller;
        private Task ImagesReadAllSocketsThread = null;
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();


        private PendingCommandController<CCDCardAnswerResults>? _pendingCommandController = new PendingCommandController<CCDCardAnswerResults>();
        private PendingCommandController<(SocketReadData, bool)[]> _pendingCommandsImageReceiveController = new PendingCommandController<(SocketReadData, bool)[]>();

        /// <summary>
        /// </summary>
        /// <param name="DoMCCardNumber">Номер платы. от 1 до 12</param>
        public CCDCardTCPClient(int DoMCCardNumber, Type socketDeviceType, IMainController controller)
        {
            SocketDeviceType = socketDeviceType;
            if (!socketDeviceType.IsAssignableTo(typeof(ISocketDevice))) throw new ArgumentException($"{nameof(socketDeviceType)} должен реализовывать ISocketDevice");
            TCPClientCommandConnection = (ISocketDevice)Activator.CreateInstance(socketDeviceType);


            IsCardNumberUsed = true;
            if (CardNumber < 1 && CardNumber > 12) throw new ArgumentOutOfRangeException("Номер платы должен быть от 1 до 12");
            CardNumber = DoMCCardNumber;
            //SocketsStatuses = new SocketWorkStatus[8];
            //for (int rc = 0; rc < 8; rc++) SocketsStatuses[rc] = new SocketWorkStatus();

            Controller = controller;
            WorkingLog = Controller.GetLogger(this.GetType().GetDescriptionOrName());
            //WorkingLogSocketStatus = Controller.GetLogger("SocketStatus");
        }

        /*public void SetSocketConfiguration(SocketReadingParameters[] Configurations)
        {
            SocketConfigurations = new SocketReadingParameters[SocketNumber];
            for (int rc = 0; rc < Configurations.Length; rc++)
            {
                SocketConfigurations[rc] = SocketConfigurations[rc].Clone();
            }
        }*/

        private async Task Connect(CancellationToken cancellationToken)
        {
            try
            {
                if ((DateTime.Now - LastReceiveAt).TotalSeconds > ReconnectTimeoutInSeconds || (LastReceiveAt - LastSendAt).TotalSeconds > ReconnectTimeoutInSeconds)
                {
                    try
                    {
                        IsConnected = false;
                        TCPClientCommandConnection.Close();
                    }
                    catch { }
                }
                var target = GetServerCommandIPAddress();
                WorkingLog.Add(LoggerLevel.Information, $"Плата: {CardNumber}. Установка соединения с {target.Address.ToString()}:{target.Port}");
                await TCPClientCommandConnection.ConnectAsync(target, ConnectionTimeoutInMilliseconds, cancellationToken);
                IsConnected = true;
                WorkingLog.Add(LoggerLevel.Information, $"Плата: {CardNumber}. Соединение установлено.");

            }
            catch (Exception ex)
            {

                WorkingLog.Add(LoggerLevel.Information, $"Плата: {CardNumber}. Соединение не удалось установить. " + ex.Message);
                throw;

            }
        }


        public bool GetImageDataFromSocketAsync(int Socket, int msTimeout, CancellationToken cancellationToken, out SocketReadData socketReadData)
        {
            int N = 0;
            socketReadData = new SocketReadData();
            var TCPClientImageDataConnection = (ISocketDevice)Activator.CreateInstance(SocketDeviceType);
            if (TCPClientImageDataConnection == null) throw new ArgumentException("");
            try
            {
                var ip = GetServerIPAddressSocketData(Socket);
                N = 1;
                WorkingLog.Add(LoggerLevel.Information, $"Плата: {CardNumber}. Начало чтения гнезда {Socket}. Адрес: {ip}");
                N = 2;

                TCPClientImageDataConnection.ConnectAsync(ip, msTimeout, cancellationToken).Wait();
                N = 3;
                WorkingLog.Add(LoggerLevel.Information, $"Подключение к {ip} успешно установлено.");
                N = 4;


                //var ns = TCPClientImageDataConnection.GetStream();

                socketReadData.ImageDataRead = 0;
                socketReadData.ImageData = new byte[SocketWorkStatus.ProperImageSizeInBytes];
                //ns.ReadTimeout = 100;
                var buffer = new byte[4096];
                var startedAt = DateTime.Now;
                var FirstRead = true;
                var PreciseTimer = new Stopwatch();
                N = 5;

                do
                {
                    N = 6;
                    if (TCPClientImageDataConnection.AvailableBytes() > 0)
                    {
                        N = 7;
                        var readTask = TCPClientImageDataConnection.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                        N = 8;
                        int read = 0;
                        if (readTask.Wait(msTimeout, cancellationToken))
                        {
                            N = 9;
                            read = readTask.Result;
                        }
                        //Если прочитано больше нужного размера, то берем только то, что вначале
                        if (socketReadData.ImageDataRead + read > SocketWorkStatus.ProperImageSizeInBytes)
                            read = SocketWorkStatus.ProperImageSizeInBytes - socketReadData.ImageDataRead;
                        Array.Copy(buffer, 0, socketReadData.ImageData, socketReadData.ImageDataRead, read);
                        socketReadData.ImageDataRead += read;
                        N = 10;

                        if (FirstRead && read > 0)
                        {
                            FirstRead = false;
                            PreciseTimer.Start();
                            WorkingLog.Add(LoggerLevel.FullDetailedInformation, $"Плата: {CardNumber}. Гнездо: {Socket}. Получен первый пакет.");
                        }
                    }
                    N = 11;

                } while (socketReadData.ImageDataRead < SocketWorkStatus.ProperImageSizeInBytes &&
                         (DateTime.Now - startedAt).TotalSeconds < msTimeout &&
                         !cancellationToken.IsCancellationRequested);

                if (cancellationToken.IsCancellationRequested)
                {
                    WorkingLog.Add(LoggerLevel.Information, $"Плата: {CardNumber}. Гнездо: {Socket}. Нет изображения гнезда");
                }
                if (FirstRead)
                {
                    WorkingLog.Add(LoggerLevel.Information, $"Плата: {CardNumber}. Гнездо: {Socket}. Чтение гнезда завершено с ошибкой.");
                }
                else
                {
                    WorkingLog.Add(LoggerLevel.Information, $"Плата: {CardNumber}. Гнездо: {Socket}. Чтение гнезда завершено. Время чтения: {socketReadData.ImageTicksRead / 10000d} мс. Прочитано {socketReadData.ImageDataRead} байт");
                }
                socketReadData.ImageTicksRead = PreciseTimer.ElapsedTicks;
                if (socketReadData.ImageDataRead != SocketWorkStatus.ProperImageSizeInBytes)
                {
                    throw new Exception($"Прочитано {socketReadData.ImageDataRead} байт из необходимых {SocketWorkStatus.ProperImageSizeInBytes}");
                }

                return socketReadData.ImageDataRead == SocketWorkStatus.ProperImageSizeInBytes;
            }
            catch (Exception ex)
            {
                WorkingLog.Add(LoggerLevel.Critical, $"Плата: {CardNumber}. Гнездо: {Socket}. Чтение гнезда завершено с ошибкой. {ex.Message}");
                Controller.GetObserver().Notify($"{this.GetType().Name}.GetImageDataFromSocketAsync.Error", (CardNumber, Socket, socketReadData.ImageDataRead));
                return false;
            }
            finally
            {
                try
                {
                    TCPClientImageDataConnection.Close();
                }
                catch { }
                WorkingLog.Add(LoggerLevel.Information, $"Плата: {CardNumber}. Гнездо: {Socket}. Соединение закрыто.");
            }
        }

        private async Task<(SocketReadData, bool)[]> GetImageDataFromAllSocketsAsync(int msTimeout, CancellationToken cancellationToken)
        {
            var result = new (SocketReadData, bool)[8];
            for (int socket = 0; socket < 8; socket++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                var (readData, success) = await Task.Run(() =>
                {
                    var ok = GetImageDataFromSocketAsync(socket, msTimeout, cancellationToken, out var data);
                    return (data, ok);
                });

                result[socket] = (readData, success);
            }

            return result;
        }

        public async Task<(SocketReadData, bool)[]> GetAllImagesDataAsync(int msTimeout, CancellationToken cancellationToken)
        {
            //TODO:
            // 1.послать запрос на получение изображений, как SendCommandGetAllSocketImagesAsync
            // 2. Запустить чтение изображений, как GetImageDataFromAllSocketsAsync
            // 3. Дождаться либо отмены, либо таймаута, либо завершения
            return await _pendingCommandsImageReceiveController.AsyncCommand(
                cancellationToken,
                (rc) => false,
                () => GetImageDataFromAllSocketsAsync(msTimeout, cancellationToken),
                () =>
            {
                WorkingLog.Add(LoggerLevel.Information, $"Отправляется запрос на получение изображений платы {CardNumber}");
                var cfg = new CCDCardArrayRequest9();
                cfg.Address = 8;
                Send(BinaryConverter.ToBytes(cfg), cancellationToken);
            }, msTimeout
                );
        }

        private async Task Disconnect()
        {
            try
            {
                if (IsConnected) WorkingLog.Add(LoggerLevel.Information, $"Плата: {CardNumber}. Соединение закрыто.");
                IsConnected = false;
                TCPClientCommandConnection?.Close();
            }
            catch { }
        }

        public async Task Start()
        {
            if (IsStarted) return;
            cancellationTokenSource = new CancellationTokenSource();
            await Connect(cancellationTokenSource.Token);

            ReceiveTask = Task.Run(() => ReceiveThreadProc(cancellationTokenSource.Token));
            Controller.GetObserver().Notify($"{this.GetType().Name}.Module.Start", new CCDCardAnswerResults() { CardNumber = CardNumber });
            IsStarted = true;
        }

        public async Task Stop()
        {
            IsStarted = false;
            cancellationTokenSource?.Cancel();
            await Disconnect();
            Controller.GetObserver().Notify($"{this.GetType().Name}.Module.StopAsync", new CCDCardAnswerResults() { CardNumber = CardNumber });
        }

        public async Task<bool> TestConntectivity()
        {
            try
            {
                var cts = new CancellationTokenSource();
                await Connect(cts.Token);
                if (IsConnected)
                    await Disconnect();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Send(byte[] data, CancellationToken cancellationToken)
        {
            if (!IsStarted) throw new SocketException((int)SocketError.NotConnected);
            //WriteSocketStatusLog("Статус гнезд при посылке данных в плату");
            try
            {
                WorkingLog.Add(LoggerLevel.Information, $"{ComputerToCard()}: " + ArrayTools.ByteArrayToHexString(data));
            }
            catch { }
            var packet = Command2Packet(data);// NetworkStreamConverter.Data2Packet(data);
            try
            {
                return WritePacketToTCPSocket(packet, cancellationToken);
            }
            catch (InvalidOperationException ex)
            {

                try
                {
                    WorkingLog.Add(LoggerLevel.Information, $"{ComputerToCard()}: {ex.Message}");
                }
                catch { }

                IsConnected = false;
                return false;
            }
        }
        private void Send(byte Socket, byte[] data, CancellationToken cancellationToken)
        {
            if (!IsStarted) throw new SocketException((int)SocketError.NotConnected);
            data[0] = Socket;
            Send(data, cancellationToken);
        }
        private bool WritePacketToTCPSocket(byte[] packet, CancellationToken cancellationToken)
        {
            if (!IsConnected) return false;
            try
            {
                lock (TCPClientCommandConnection)
                {
                    TCPClientCommandConnection.WriteAsync(packet, 0, packet.Length, cancellationToken);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private async Task ReceiveThreadProc(CancellationToken token)
        {
            try
            {
                var buffer = new byte[2048];

                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        int read = 0;

                        // ждём, пока появятся байты
                        if (TCPClientCommandConnection.AvailableBytes() > 0)
                        {
                            // асинхронно читаем
                            read = await TCPClientCommandConnection.ReadAsync(buffer, 0, buffer.Length, token);
                        }

                        if (read > 0)
                        {
                            AddToBuffer(buffer, 0, read);

                            // пробуем обработать сразу
                            try
                            {
                                if (SizeOfReadBuffer() > 0)
                                    ProcessBuffer();
                            }
                            catch (Exception ex)
                            {
                                WorkingLog.Add(LoggerLevel.Critical, $"Исключение при обработке буфера: {ex.Message}", ex);
                            }
                        }
                        else
                        {
                            // Ничего не прочитали — делаем короткую паузу, чтобы не крутить процессор
                            await Task.Delay(10, token);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        // всё ок — корректная отмена
                        break;
                    }
                    catch (Exception ex)
                    {
                        WorkingLog.Add(LoggerLevel.Critical, $"Исключение в ReceiveThreadProc (внутри): {ex.Message}", ex);
                        await Task.Delay(100, token); // дожидаемся перед следующим кругом, чтобы не спамить
                    }
                }
            }
            catch (Exception ex)
            {
                WorkingLog.Add(LoggerLevel.Critical, $"Ошибка в ReceiveThreadProc: {ex.Message}", ex);
            }

            // отключаемся в самом конце
            try { await Disconnect(); } catch { }
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
        private int SizeOfReadBuffer()
        {
            lock (ReadBuffer)
            {
                return ReadBuffer.Length;
            }
        }

        public void ProcessBuffer()
        {
            ParallelOptions po = new ParallelOptions() { MaxDegreeOfParallelism = 4 };
            var packets = Packet2Response(ref ReadBuffer, out List<Tuple<byte[], Tuple<byte, bool>>> log);


            for (int i = 0; i < log.Count; i++)
            {
                WorkingLog.Add(LoggerLevel.FullDetailedInformation, $"{CardToComputer()} (CRC:{(log[i].Item2.Item2 ? "+" : $"- (0x{log[i].Item2.Item1:X2})")}): " + $"<{String.Join(", ", log[i].Item1.Select(i => $"0x{i:X2}"))}");
            }


            Parallel.ForEach(packets, po, packet => //foreach(var packet in packets)
            {
                if (packet != null)
                {

                    //WriteSocketStatusLog("Статус гнезд перед обработкой полученного пакета");


                    var addr = (byte)(packet[0] & 7);
                    int socketnum = addr;

                    //var socket = SocketsStatuses;
                    var cmd = packet[1];
                    //socket.LastCommandReceivingFromSocketTime = LastReceiveAt;
                    //socket.LastCommandReceivedFromSocket = cmd;
                    WorkingLog.Add(LoggerLevel.Information, $"Плата: {CardNumber}. В {DateTime.Now:dd-MM-yyyy HH\\:mm\\:ss.fffff} получен ответ на команду {cmd} для гнезда {packet[0]}.");
                    var CardAnswerResult = new CCDCardAnswerResults() { CardNumber = CardNumber };

                    switch (cmd)
                    {
                        case 1:
                            {
                                byte result = packet[2];
                                var dtime = (packet[3] + packet[4] * 256) * 10;
                                CardAnswerResult.ReadingSocketsResult = result;
                                CardAnswerResult.ReadingSocketsTime = dtime;


                                _pendingCommandController?.TrySetResult(1, CardAnswerResult);


                                Controller.GetObserver().Notify($"{this.GetType().Name}.ReadingSocketsResult.ResponseReadSockets", CardAnswerResult);
                            }
                            break;
                        case 4:
                            {
                                _pendingCommandController?.TrySetResult(4, CardAnswerResult);
                                Controller.GetObserver().Notify($"{this.GetType().Name}.ReadingSocketsResult.ResponseSetSocketsExpositionParameters", CardAnswerResult);

                            }
                            break;
                        case 5:
                            {
                                _pendingCommandController?.TrySetResult(5, CardAnswerResult);
                                Controller.GetObserver().Notify($"{this.GetType().Name}.ReadingSocketsResult.ResponseSetConfiguration", CardAnswerResult);

                            }
                            break;

                        case 9:
                            {
                                _pendingCommandsImageReceiveController?.SetCanceled();
                                _pendingCommandController?.SetCanceled();
                                Controller.GetObserver().Notify($"{this.GetType().Name}.ReadingSocketsResult.ResponseGetSocketsImages", CardAnswerResult);

                            }
                            break;
                        case 11:
                            {
                                _pendingCommandController?.TrySetResult(11, CardAnswerResult);
                                Controller.GetObserver().Notify($"{this.GetType().Name}.ReadingSocketsResult.ResponseSetReadingParametersConfiguration", CardAnswerResult);

                            }
                            break;
                        default:
                            _pendingCommandController?.SetCanceled();
                            break;
                    }

                }

            });
        }

        public static string GetCardIPAddress(int CardNumber)
        {
            return new IPAddress(IPForCardNumber(CardNumber)).ToString();
        }
        private static byte[] IPForCardNumber(int cardNumber)
        {
            var baseIp = IPAddress.Parse(BaseIPv4Address);
            IPEndPoint ip;
            var baseIpArr = baseIp.GetAddressBytes();
            baseIpArr[3] = (byte)(100 + cardNumber);
            return baseIpArr;
        }
        private IPEndPoint GetServerCommandIPAddress()
        {
            IPEndPoint ip;
            if (IsCardNumberUsed)
            {
                ip = new IPEndPoint(new IPAddress(IPForCardNumber(CardNumber)), PortCommand);
            }
            else
            {
                ip = new IPEndPoint(IPAddress.Parse(IPAddr), PortCommand);
            }
            return ip;
        }

        private IPEndPoint GetServerIPAddressSocketData(int Socket)
        {
            IPEndPoint ip;
            if (IsCardNumberUsed)
            {
                ip = new IPEndPoint(new IPAddress(IPForCardNumber(CardNumber)), PortData + Socket);
            }
            else
            {
                ip = new IPEndPoint(IPAddress.Parse(IPAddr), PortData + Socket);
            }
            return ip;
        }

        public static byte[] Command2Packet(byte[] b)
        {
            //var crc = (byte)(0x100 - (((byte)b.Sum(bb => bb)) & 0xff));
            var res = string.Join("", b.Select(bb => bb.ToString("X2")));
            var crc = (byte)(0x100 - ((byte)res.Sum(bb => bb) & 0xff));
            var resStr = $":{res}{crc:X2}\r\n";
            var resb = Encoding.ASCII.GetBytes(resStr);
            return resb;
        }
        public static List<byte[]> Packet2Response(ref byte[] buffer, out List<Tuple<byte[], Tuple<byte, bool>>> log)
        {
            List<byte[]> packets = new List<byte[]>();
            int TotalEndIndex = -1;
            int endIndex = 0;
            do
            {
                var startIndex = Array.FindIndex(buffer, endIndex, e => e == ':');
                if (startIndex < 0) break;
                endIndex = Array.FindIndex(buffer, startIndex + 1, e => e == '\r');
                if (endIndex < 0 || buffer.Length == endIndex + 1 || buffer[endIndex + 1] != '\n') break;

                var arr = new byte[endIndex + 2 - startIndex];
                Array.Copy(buffer, startIndex, arr, 0, arr.Length);
                packets.Add(arr);
                TotalEndIndex = endIndex;
            } while (endIndex >= 0);
            if (TotalEndIndex > 0)
            {
                var newb = new byte[buffer.Length - TotalEndIndex - 2];
                Array.Copy(buffer, TotalEndIndex + 2, newb, 0, newb.Length);
                buffer = newb;
            }
            var resultPackets = new List<byte[]>();
            log = new List<Tuple<byte[], Tuple<byte, bool>>>();
            foreach (var packet in packets)
            {
                if (packet.Length >= 5 && (packet.Length - 5 & 1) == 0 && packet[0] == ':')
                {
                    var crc = 0;
                    var resultpacket = new byte[(packet.Length - 5) / 2];
                    for (int i = 0; i < resultpacket.Length; i++)
                    {
                        crc += packet[1 + i * 2] + packet[1 + i * 2 + 1];
                        resultpacket[i] = Hex2Byte(packet[1 + i * 2], packet[1 + i * 2 + 1]);
                    }
                    crc = 0x100 - (crc & 0xff);
                    var packetCRC = Hex2Byte(packet[packet.Length - 4], packet[packet.Length - 3]);
                    resultPackets.Add(resultpacket);
                    if (packetCRC == crc)
                    {
                        log.Add(new Tuple<byte[], Tuple<byte, bool>>(resultpacket, new Tuple<byte, bool>(packetCRC, true)));
                    }
                    else
                    {
                        log.Add(new Tuple<byte[], Tuple<byte, bool>>(resultpacket, new Tuple<byte, bool>(packetCRC, false)));
                    }
                }
            }
            return resultPackets;
        }

        private static bool CheckCRC(byte[] b, byte crc)
        {
            return (b.Sum(e => e) + crc & 0xff) == 0;
        }
        private static byte Hex2Byte(byte hiASCII, byte loASCII)
        {
            hiASCII -= 0x30; if (hiASCII >= 10) hiASCII -= 0x07;
            loASCII -= 0x30; if (loASCII >= 10) loASCII -= 0x07;
            byte res = (byte)(hiASCII << 4 | loASCII);
            return res;
        }

        #region Requests



        /// <summary>
        /// Command = 4, Загрузка конфигурации чтения в гнезда по списку
        /// </summary>

        public async Task<CCDCardAnswerResults> SendCommandSetSocketsExpositionParametersAsync(SocketParameters socketParameters, int msTimeout, CancellationToken cancellationToken)
        {
            if (!IsStarted) throw new SocketException((int)SocketError.NotConnected);

            if (socketParameters == null || socketParameters.ReadingParameters == null || socketParameters.ReadingParameters.Exposition == 0 || socketParameters.ReadingParameters.FrameDuration == 0) throw new DoMCSocketParametersNotSetException(CardNumber, 0);
            return await _pendingCommandController.AsyncCommand(cancellationToken, (rc) => rc == 4, () =>
            {
                var cfg = socketParameters.ReadingParameters.GetFrameExpositionConfiguration();
                Send(1, BinaryConverter.ToBytes(cfg), cancellationToken);
            }, msTimeout);
        }
        /// <summary>
        /// Command = 11, Загрузка основной конфигурации в гнезда по списку
        /// </summary>
        public async Task<CCDCardAnswerResults> SendCommandSetSocketReadingParametersAsync(SocketParameters socketParameters, int msTimeout, CancellationToken cancellationToken)
        {
            if (!IsStarted) throw new SocketException((int)SocketError.NotConnected);

            if (socketParameters == null || socketParameters.ReadingParameters == null || socketParameters.ReadingParameters.Exposition == 0 || socketParameters.ReadingParameters.FrameDuration == 0) throw new DoMCSocketParametersNotSetException(CardNumber, 0);
            return await _pendingCommandController.AsyncCommand(cancellationToken, (rc) => rc == 11, () =>
            {
                var cfg = socketParameters.ReadingParameters.GetReadingParametersConfiguration();
                Send(1, BinaryConverter.ToBytes(cfg), cancellationToken);
            }, msTimeout);
        }


        /// <summary>
        /// Command = 1, читать изображения с датчиков
        /// </summary>

        public async Task<CCDCardAnswerResults> SendCommandReadAllSocketsAsync(int msTimeout, CancellationToken cancellationToken)
        {
            if (!IsStarted) throw new SocketException((int)SocketError.NotConnected);
            return await _pendingCommandController.AsyncCommand(cancellationToken, (rc) => rc == 1, () =>
            {
                var cfg = new CCDCardReadRequest1();
                cfg.Address = 1;
                Send(BinaryConverter.ToBytes(cfg), cancellationToken);
            }, msTimeout);

        }

        /// <summary>
        /// Command = 1, читать изображение одного гнезда
        /// </summary>
        public async Task<CCDCardAnswerResults> SendCommandReadSocketAsync(byte socketNum, int msTimeout, CancellationToken token)
        {
            if (!IsStarted) throw new SocketException((int)SocketError.NotConnected);
            return await _pendingCommandController.AsyncCommand(token, (rc) => rc == 1, () =>
            {
                // отправляем команду
                var cfg = new CCDCardReadRequest1 { Address = 1 };
                Send(socketNum, BinaryConverter.ToBytes(cfg), token);
            }, msTimeout);

        }


        /*
                public void SendCommandReadSocket(byte SocketNum, CancellationToken cancellationToken)
                {
                    if (!IsStarted) throw new SocketException((int)SocketError.NotConnected);

                    var cfg = new CCDCardReadRequest1();
                    cfg.Address = 0;
                    Send(SocketNum, BinaryConverter.ToBytes(cfg), cancellationToken);
                }
        */
        /// <summary>
        /// Command = 5, установка параметров чтения или запуск чтения по внешнему сигналу
        /// </summary>

        public async Task<CCDCardAnswerResults> SendCommandSetSocketReadingStateAsync(CancellationToken cancellationToken, int msTimeout, bool AnswerWithImage, bool ExternalStart, bool FastRead, bool ResetReady = true)
        {
            if (!IsStarted) throw new SocketException((int)SocketError.NotConnected);
            return await _pendingCommandController.AsyncCommand(cancellationToken, (rc) => !ExternalStart ? rc == 5 : rc == 1, () =>
            {
                var cfg = CCDCardConfigRequest5.GetConfiguration(ResetReady, AnswerWithImage, ExternalStart, FastRead);
                cfg.Address = 1;
                Send(BinaryConverter.ToBytes(cfg), cancellationToken);
            }, msTimeout);
        }
        /// <summary>
        /// Command = 5, запуск по внешнему старту
        /// </summary>
        public async Task<CCDCardAnswerResults> SendCommandReadSeveralSocketsExternalAsync(int msTimeout, CancellationToken cancellationToken)
        {
            if (!IsStarted) throw new SocketException((int)SocketError.NotConnected);
            return await _pendingCommandController.AsyncCommand(cancellationToken, (rc) => rc == 1, () =>
            {
                var cfg = CCDCardConfigRequest5.GetConfiguration(true, false, true, true);
                cfg.Address = 1;
                Send(BinaryConverter.ToBytes(cfg), cancellationToken);
            }, msTimeout);
        }

        /// <summary>
        /// Command = 9, получить прочитанные изображения по всем гнездам
        /// </summary>

        public async Task<CCDCardAnswerResults> SendCommandGetAllSocketImagesAsync(int msTimeout, CancellationToken cancellationToken)
        {

            if (!IsStarted) throw new SocketException((int)SocketError.NotConnected);
            return await _pendingCommandController.AsyncCommand(cancellationToken, (rc) => rc == 9, () =>
            {
                WorkingLog.Add(LoggerLevel.Information, $"Отправляется запрос на получение изображений платы {CardNumber}");
                var cfg = new CCDCardArrayRequest9();
                cfg.Address = 8;
                Send(BinaryConverter.ToBytes(cfg), cancellationToken);
            }, msTimeout);
        }

        /// <summary>
        /// Command = 9, получить прочитанное изображение по одному гнезду
        /// </summary>

        public async Task<CCDCardAnswerResults> SendCommandGetSocketImageAsync(int socket, int msTimeout, CancellationToken cancellationToken)
        {

            if (!IsStarted) throw new SocketException((int)SocketError.NotConnected);
            return await _pendingCommandController.AsyncCommand(cancellationToken, (rc) => rc == 9, () =>
            {
                var cfg = new CCDCardArrayRequest9();
                cfg.Address = (byte)socket;
                Send(BinaryConverter.ToBytes(cfg), cancellationToken);
            }, msTimeout);
        }
        #endregion

        #region Logging
        private string ComputerToCard()
        {
            return $"Комп -> {CardNumber}";
        }
        private string CardToComputer()
        {
            return $"{CardNumber} -> Комп";
        }
        #endregion

    }
}