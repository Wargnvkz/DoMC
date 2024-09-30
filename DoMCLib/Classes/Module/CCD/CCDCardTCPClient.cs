using DoMCLib.Classes.Configuration.CCD;
using DoMCLib.Classes.Module.CCD.CCDCardDataExchangeCommandClasses;
using DoMCLib.Classes.Module.LCB;
using DoMCLib.Tools;
using DoMCModuleControl;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Logging;
using DoMCModuleControl.Modules;
using System;
using System.Collections.Generic;
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
    public class CCDCardTCPClient
    {
        public int PortCommand = 200;
        public int PortData = 100;
        public int CardNumber { get; private set; }
        int SocketNumber = 8;
        //public SocketWorkStatus[] SocketsStatuses { get; private set; }
        //SocketReadingParameters[] SocketConfigurations;
        private CancellationTokenSource receiveCancellationTokenSource { get; set; }
        private Task ReceiveTask { get; set; }

        private TcpClient TCPClientCommandConnection;
        private TcpClient DoMCCardTCPClientDataConnection;
        private DateTime LastReceiveAt;
        private DateTime LastSendAt;
        public int ReconnectTimeoutInSeconds = 30;
        //private bool TerminateThread = false;
        //private Thread ReceiveThread;
        public bool IsStarted { get; private set; } = false;
        byte[] ReadBuffer = new byte[0];
        //public long TotalReadBytes;
        //public int ReadDataTimeoutSeconds = 30;
        public int ConnectionTimeoutInMilliseconds = 2000;
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

        /// <summary>
        /// </summary>
        /// <param name="DoMCCardNumber">Номер платы. от 1 до 12</param>
        public CCDCardTCPClient(int DoMCCardNumber, IMainController controller)
        {
            IsCardNumberUsed = true;
            if (CardNumber < 1 && CardNumber > 12) throw new ArgumentOutOfRangeException("Номер платы должен быть от 1 до 12");
            CardNumber = DoMCCardNumber;
            //SocketsStatuses = new SocketWorkStatus[8];
            //for (int i = 0; i < 8; i++) SocketsStatuses[i] = new SocketWorkStatus();

            Controller = controller;
            WorkingLog = Controller.GetLogger(this.GetType().Name);
            //WorkingLogSocketStatus = Controller.GetLogger("SocketStatus");
        }

        /*public void SetSocketConfiguration(SocketReadingParameters[] Configurations)
        {
            SocketConfigurations = new SocketReadingParameters[SocketNumber];
            for (int i = 0; i < Configurations.Length; i++)
            {
                SocketConfigurations[i] = SocketConfigurations[i].Clone();
            }
        }*/

        private void Connect()
        {
            try
            {
                if ((DateTime.Now - LastReceiveAt).TotalSeconds > ReconnectTimeoutInSeconds || (LastReceiveAt - LastSendAt).TotalSeconds > ReconnectTimeoutInSeconds)
                {
                    if (TCPClientCommandConnection != null)
                    {
                        try
                        {
                            IsConnected = false;
                            TCPClientCommandConnection.Close();
                        }
                        catch { }
                        TCPClientCommandConnection = null;
                    }
                }
                TCPClientCommandConnection = new TcpClient();
                var target = GetServerCommandIPAddress();
                WorkingLog.Add(LoggerLevel.Information, $"Плата: {CardNumber}. Установка соединения с {target.Address.ToString()}:{target.Port}");
                var connectionResult = TCPClientCommandConnection.BeginConnect(target.Address, target.Port, null, null);
                var connectSucceeded = connectionResult.AsyncWaitHandle.WaitOne(ConnectionTimeoutInMilliseconds);
                if (!connectSucceeded)
                {
                    throw new SocketException(10060);
                }
                TCPClientCommandConnection.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                //TCPClientCommandConnection.Connect(target);

                try
                {
                    // Ускорение подтверждения получения пакета. Платы ждут подтверждения ровно 200 мс и перепосылают пакет, а windows обычно отвечат позже
                    int SIO_TCP_SET_ACK_FREQUENCY = unchecked((int)0x98000017);
                    var outputArray = new byte[128];
                    var bytesInOutputArray = TCPClientCommandConnection.Client.IOControl(SIO_TCP_SET_ACK_FREQUENCY, BitConverter.GetBytes(1), outputArray);
                }
                catch { }
                IsConnected = true;
                WorkingLog.Add(LoggerLevel.Information, $"Плата: {CardNumber}. Соединение установлено.");
                //WorkingLog.Add(LoggerLevel.Information,$"Плата: {CardNumber}. ");

            }
            catch (Exception ex)
            {

                WorkingLog.Add(LoggerLevel.Information, $"Плата: {CardNumber}. Соединение не удалось установить. " + ex.Message);

            }
        }

        /// <summary>
        /// Запускает чтение из порта данных. Должно выполняться вместе с командой получения изображений (9)
        /// </summary>
        /// <param name="Socket"></param>
        /// <param name="msTimeout"></param>
        /// <returns>Завершено ли чтение</returns>
        public bool GetImageDataFromSocketSync(int Socket, int msTimeout, CancellationTokenSource cancellationTokenSource, out SocketReadData socketReadData)
        {
            socketReadData = new SocketReadData();
            try
            {
                {
                    List<Tuple<int, long>> StepToTick = new List<Tuple<int, long>>();
                    var sw = new Stopwatch();
                    sw.Start();

                    StepToTick.Add(new Tuple<int, long>(0, sw.ElapsedTicks));

                    var ip = GetServerIPAddressSocketData(Socket);
                    WorkingLog.Add(LoggerLevel.Information, $"Плата: {CardNumber}. Начало чтения гнезда {Socket}. Адрес: {ip.ToString()}");
                    DoMCCardTCPClientDataConnection = new TcpClient();

                    StepToTick.Add(new Tuple<int, long>(1, sw.ElapsedTicks));

                    DoMCCardTCPClientDataConnection.Connect(ip);

                    StepToTick.Add(new Tuple<int, long>(2, sw.ElapsedTicks));

                    WorkingLog.Add(LoggerLevel.Information, $"Плата: {CardNumber}. Гнездо: {Socket}. Соединение открыто.");
                    DoMCCardTCPClientDataConnection.ReceiveBufferSize = 1048576;

                    StepToTick.Add(new Tuple<int, long>(3, sw.ElapsedTicks));

                    var ns = DoMCCardTCPClientDataConnection.GetStream();

                    socketReadData.ImageDataRead = 0;
                    socketReadData.ImageData = new byte[SocketWorkStatus.ProperImageSizeInBytes];
                    ns.ReadTimeout = 100;
                    var buffer = new byte[4096];
                    var startedAt = DateTime.Now;
                    var FirstRead = true;
                    var PreciseTimer = new Stopwatch();

                    StepToTick.Add(new Tuple<int, long>(4, sw.ElapsedTicks));
                    do
                    {
                        try
                        {
                            StepToTick.Add(new Tuple<int, long>(5, sw.ElapsedTicks));
                            var read = ns.Read(buffer, 0, buffer.Length);
                            if (FirstRead)
                            {
                                StepToTick.Add(new Tuple<int, long>(6, sw.ElapsedTicks));
                                FirstRead = false;
                                PreciseTimer.Start();
                                WorkingLog.Add(LoggerLevel.Information, $"Плата: {CardNumber}. Гнездо: {Socket}. Получен первый пакет.");
                            }
                            StepToTick.Add(new Tuple<int, long>(7, sw.ElapsedTicks));
                            if (socketReadData.ImageDataRead + read > SocketWorkStatus.ProperImageSizeInBytes)
                                read = SocketWorkStatus.ProperImageSizeInBytes - socketReadData.ImageDataRead;
                            StepToTick.Add(new Tuple<int, long>(8, sw.ElapsedTicks));
                            Array.Copy(buffer, 0, socketReadData.ImageData, socketReadData.ImageDataRead, read);
                            StepToTick.Add(new Tuple<int, long>(9, sw.ElapsedTicks));
                            socketReadData.ImageDataRead += read;
                            StepToTick.Add(new Tuple<int, long>(10, sw.ElapsedTicks));
                            //file.Write(buffer, 0, read);
                        }
                        catch (IOException ex)
                        {
                        }
                        StepToTick.Add(new Tuple<int, long>(11, sw.ElapsedTicks));
                    } while (socketReadData.ImageDataRead < SocketWorkStatus.ProperImageSizeInBytes && (DateTime.Now - startedAt).TotalMilliseconds < msTimeout && !cancellationTokenSource.Token.IsCancellationRequested);
                    StepToTick.Add(new Tuple<int, long>(12, sw.ElapsedTicks));
                    //socketReadData.ImageDataRead = ns.Read(socketReadData.ImageData, 0, SocketWorkStatus.ProperImageSizeInBytes);
                    sw.Stop();

                    if (cancellationTokenSource.Token.IsCancellationRequested)
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
                        StringBuilder sb = new StringBuilder();
                        for (int i = 1; i < StepToTick.Count; i++)
                            if (StepToTick[i].Item2 - StepToTick[i - 1].Item2 > 1000000)
                            {
                                sb.Append($"{StepToTick[i - 1].Item1}:{StepToTick[i - 1].Item2};");
                                sb.Append($"{StepToTick[i].Item1}:{StepToTick[i].Item2};");
                            }
                        var stepsTimings = sb.ToString();
                        WorkingLog.Add(LoggerLevel.FullDetailedInformation, $"Плата: {CardNumber}. Гнездо: {Socket}. Времена выполнения шагов (шаг:мс): {stepsTimings}");
                        throw new Exception($"Прочитано {socketReadData.ImageDataRead} байт");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                WorkingLog.Add(LoggerLevel.Information, $"Плата: {CardNumber}. Гнездо: {Socket}. Чтение гнезда завершено с ошибкой. {ex.Message}");
                return false;
            }
            finally
            {
                DoMCCardTCPClientDataConnection.Close();
                WorkingLog.Add(LoggerLevel.Information, $"Плата: {CardNumber}. Гнездо: {Socket}. Соединение закрыто.");
            }
        }

        /*private bool NeedReconnect()
        {
            return !(IsAnyCommandCompleteForAllSockets() || GetLastCommandRunningTime() / 1000 < ReconnectTimeoutInSeconds);
            //return DoMCLib.Tools.TCPClientTools.CheckConnection(TCPClientCommandConnection);
            //return (DateTime.Now - LastReceiveAt).TotalSeconds > ReconnectTimeoutInSeconds || (LastReceiveAt - LastSendAt).TotalSeconds > ReconnectTimeoutInSeconds;
        }*/
        private void Disconnect()
        {
            try
            {
                if (IsConnected) WorkingLog.Add(LoggerLevel.Information, $"Плата: {CardNumber}. Соединение закрыто.");
                IsConnected = false;
                TCPClientCommandConnection?.Close();
            }
            catch { }
        }

        public void Start()
        {
            if (IsStarted) return;
            IsStarted = true;
            Connect();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            ReceiveTask = new Task(ReceiveThreadProc);
            ReceiveTask.Start();
            Controller.GetObserver().Notify($"{this.GetType().Name}.Module.Start", null);
        }

        public void Stop()
        {
            IsStarted = false;
            Disconnect();
            Controller.GetObserver().Notify($"{this.GetType().Name}.Module.Stop", null);
        }

        public bool Send(byte[] data)
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
                return WritePacketToTCPSocket(packet);
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
        private void Send(byte Socket, byte[] data)
        {
            if (!IsStarted) throw new SocketException((int)SocketError.NotConnected);
            data[0] = Socket;
            Send(data);
        }
        private bool WritePacketToTCPSocket(byte[] packet)
        {
            if (!IsConnected) return false;
            lock (TCPClientCommandConnection)
            {
                if (TCPClientCommandConnection != null)
                {
                    var ns = TCPClientCommandConnection.GetStream();
                    ns.Write(packet, 0, packet.Length);
                    return true;
                }
            }
            return false;
        }

        private void ReceiveThreadProc()
        {
            //Connect();
            while (!receiveCancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    //if (NeedReconnect())
                    //    Connect();
                    lock (TCPClientCommandConnection)
                    {
                        var ns = TCPClientCommandConnection.GetStream();
                        ns.ReadTimeout = 1;
                        var tmpBuffer = new byte[2048];
                        int read;
                        do
                        {
                            try
                            {
                                read = ns.Read(tmpBuffer, 0, tmpBuffer.Length);
                            }
                            catch (Exception ex) { read = 0; }
                            if (read > 0)
                            {
                                AddToBuffer(tmpBuffer, 0, read);
                            }
                        } while (read > 0);
                    }
                }
                catch (Exception ex)
                {

                }
                try
                {
                    ProcessBuffer();
                }
                catch (Exception ex)
                {
                    WorkingLog.Add(LoggerLevel.Information, "Исключение при обработке буфера: ", ex);
                }
                Thread.Sleep(10);
            }
            Disconnect();
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

        public void ProcessBuffer()
        {
            ParallelOptions po = new ParallelOptions() { MaxDegreeOfParallelism = 4 };
            var packets = Packet2Response(ref ReadBuffer, out List<Tuple<byte[], Tuple<byte, bool>>> log);


            for (int i = 0; i < log.Count; i++)
            {
                WorkingLog.Add(LoggerLevel.FullDetailedInformation, $"{CardToComputer()} (CRC:{(log[i].Item2.Item2 ? "+" : $"- (0x{log[i].Item2.Item1:X2})")}): " + log[i].Item1);
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
                    WorkingLog.Add(LoggerLevel.Information, $"Плата: {CardNumber}. В {LastReceiveAt:dd-MM-yyyy HH\\:mm\\:ss.fffff} получен ответ на команду {cmd} для гнезда {packet[0]}.");
                    var CardAnswerResult = new CCDCardAnswerResults() { CardNumber = CardNumber };

                    switch (cmd)
                    {
                        case 1:
                            {
                                byte result = packet[2];
                                var dtime = (packet[3] + packet[4] * 256) * 10;
                                CardAnswerResult.ReadingSocketsResult = result;
                                CardAnswerResult.ReadingSocketsTime = dtime;

                                Controller.GetObserver().Notify($"{this.GetType().Name}.ReadingSocketsResult.ResponseReadSockets", CardAnswerResult);
                            }
                            break;
                        case 4:
                            {
                                Controller.GetObserver().Notify($"{this.GetType().Name}.ReadingSocketsResult.ResponseSetSocketsExpositionParameters", CardAnswerResult);

                            }
                            break;
                        case 5:
                            {
                                Controller.GetObserver().Notify($"{this.GetType().Name}.ReadingSocketsResult.ResponseSetConfiguration", CardAnswerResult);

                            }
                            break;

                        case 9:
                            {

                                Controller.GetObserver().Notify($"{this.GetType().Name}.ReadingSocketsResult.ResponseGetSocketsImages", CardAnswerResult);

                            }
                            break;
                        case 11:
                            {

                                Controller.GetObserver().Notify($"{this.GetType().Name}.ReadingSocketsResult.ResponseSetReadingParametersConfiguration", CardAnswerResult);

                            }
                            break;
                        default:
                            break;
                    }

                }

            });
        }

        private IPEndPoint GetServerCommandIPAddress()
        {
            IPEndPoint ip;
            if (IsCardNumberUsed)
            {
                ip = new IPEndPoint(new IPAddress(new byte[] { 192, 168, 255, (byte)(100 + CardNumber) }), PortCommand);
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
                ip = new IPEndPoint(new IPAddress(new byte[] { 192, 168, 255, (byte)(100 + CardNumber) }), PortData + Socket);
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

        public void SendCommandSetSocketsExpositionParameters(SocketParameters socketParameters)
        {
            if (!IsStarted) throw new SocketException((int)SocketError.NotConnected);

            if (socketParameters == null || !socketParameters.IsSocketReading) return;
            var cfg = socketParameters.ReadingParameters.GetFrameExpositionConfiguration();
            Send(0, BinaryConverter.ToBytes(cfg));

        }
        /// <summary>
        /// Command = 11, Загрузка основной конфигурации в гнезда по списку
        /// </summary>
        public void SendCommandSetSocketReadingParameters(SocketParameters socketParameters)
        {
            if (!IsStarted) throw new SocketException((int)SocketError.NotConnected);

            if (socketParameters == null || !socketParameters.IsSocketReading) return;
            var cfg = socketParameters.ReadingParameters.GetReadingParametersConfiguration();
            Send(0, BinaryConverter.ToBytes(cfg));

        }


        /// <summary>
        /// Command = 1, читать изображения с датчиков
        /// </summary>

        public void SendCommandReadAllSockets()
        {
            if (!IsStarted) throw new SocketException((int)SocketError.NotConnected);

            var cfg = new CCDCardReadRequest1();
            cfg.Address = 0;
            Send(BinaryConverter.ToBytes(cfg));

        }
        /// <summary>
        /// Command = 1, читать изображение одного гнезда
        /// </summary>

        public void SendCommandReadSocket(byte SocketNum)
        {
            if (!IsStarted) throw new SocketException((int)SocketError.NotConnected);

            var cfg = new CCDCardReadRequest1();
            cfg.Address = 0;
            Send(SocketNum, BinaryConverter.ToBytes(cfg));
        }

        /// <summary>
        /// Command = 5, установка параметров чтения или запуск чтения по внешнему сигналу
        /// </summary>

        public void SendCommandSetSocketReadingParameters(bool AnswerWithImage, bool ExternalStart, bool FastRead, bool ResetReady = true)
        {
            if (!IsStarted) throw new SocketException((int)SocketError.NotConnected);

            var cfg = CCDCardConfigRequest5.GetConfiguration(ResetReady, AnswerWithImage, ExternalStart, FastRead);
            cfg.Address = 0;
            Send(BinaryConverter.ToBytes(cfg));

        }
        /// <summary>
        /// Command = 5, запуск по внешнему старту
        /// </summary>
        public void SendCommandReadSeveralSocketsExternal(bool FastRead)
        {
            if (!IsStarted) throw new SocketException((int)SocketError.NotConnected);

            var cfg = CCDCardConfigRequest5.GetConfiguration(true, false, true, FastRead);
            cfg.Address = 0;
            Send(BinaryConverter.ToBytes(cfg));

        }

        /// <summary>
        /// Command = 9, получить прочитанные изображения по всем гнездам
        /// </summary>

        public void SendCommandGetAllSocketImages()
        {

            if (!IsStarted) throw new SocketException((int)SocketError.NotConnected);

            var cfg = new CCDCardArrayRequest9();
            cfg.Address = 8;
            Send(BinaryConverter.ToBytes(cfg));

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