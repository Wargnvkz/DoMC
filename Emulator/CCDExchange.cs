using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Emulator
{
    internal class CCDExchange
    {

    }
    /*public class TCPCCDCardServer
    {
        public TcpListener Server;
        public TcpListener[] ImageServer;
        List<(Socket, Thread)> ServerSocketConnected = new List<(Socket, Thread)>();
        List<(Socket, Thread)> ImageServerSocketConnected = new List<(Socket, Thread)>();
        bool IsRunning = false;
        Thread ServerThread;
        Thread[] ImageServerThread;
        short[][,] SocketImages;
        System.Threading.Timer timer;
        public void Start()
        {
            Server = new TcpListener(200);
            Server.Start();
            ServerThread = new Thread(CommandServerThreadProc);
            ServerThread.Start(Server);

            ImageServer = new TcpListener[8];
            ImageServerThread = new Thread[8];
            for (int i = 0; i < ImageServer.Length; i++)
            {
                ImageServer[i] = new TcpListener(i + 100);
                ImageServer[i].Start();
                ImageServerThread[i] = new Thread(ImageSocketServerThreadProc);
                ImageServerThread[i].Start(ImageServer[i]);
            }
            SocketImages = new short[8][,];
            timer = new System.Threading.Timer(ClearDisconnectedSocket, null, 0, 30000);
            IsRunning = true;
        }
        public void Stop()
        {
            IsRunning = false;
            Server.Stop();
        }

        public void CommandServerThreadProc(object oCommandServer)
        {
            var Server = (TcpListener)oCommandServer;
            while (IsRunning)
            {
                try
                {
                    var socket = Server.AcceptSocket();
                    if (socket != null)
                    {
                        AddCommandClientSocket(socket);
                    }
                }
                catch (Exception ex)
                {

                }
            }

        }
        private void CommandSocketThread(object oSocket)
        {
            var socket = (Socket)oSocket;
            while (IsRunning)
            {
                //получение данных в буфер, разбор команд по шаблону
                // Структура команды по байтам: <гнездо, если есть или 1, или 0; 1 байт> <команда; 1 байт> <Данные; N байт> <crc>
                // реакция и ответ
                // команды 1-запрос на чтение, 9-запрос на отправку прочитанных изображений, 4-установка параметров чтения, b-установка параметров чтения, 5- тип чтения
            }
        }

        private void ImageSocketServerThreadProc(object oImageServerData)
        {
            var imageServerData = ((TcpListener, int))oImageServerData;
            var imageServer = imageServerData.Item1;
            var ImageSocketNum = imageServerData.Item2;
            while (IsRunning)
            {
                try
                {
                    var socket = imageServer.AcceptSocket();
                    if (socket != null)
                    {
                        AddImageClientSocket(socket, ImageSocketNum);
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
        private void ImageSocketThread(object oSocket)
        {
            var socketData = ((Socket, int))oSocket;
            var socket = socketData.Item1;
            var ImageSocketNum = socketData.Item2;
            if (SocketImages[ImageSocketNum] != null)
            {
                socket.Send(SocketImages[ImageSocketNum].Cast<byte>().ToArray());
                SocketImages[ImageSocketNum] = null;
            }
            else
            {
                //Ошибка, нет данных, на командном порту возвращаем сообщение об ошибке
                // Структура команды: <гнездо, если есть или 1, или 0> <команда> <Данные> <crc>
                // гнездо ImageSocketNum, команда 9, данные 1 - ошибка. нет данных
                SendCommand(new byte[] { (byte)ImageSocketNum, 09, 00, 01 });
            }
            socket.Close();
        }
        private void SendCommand(byte[] data)
        {
            var crc = ~(byte)data.Sum(d => d);
            var forSend = new byte[data.Length + 1];
            Array.Copy(data, forSend, data.Length);
            forSend[forSend.Length - 1] = (byte)crc;
            ServerSocketConnected.ForEach(s => s.Item1.Send(forSend));
        }

        private void ClearDisconnectedSocket(object? state)
        {
            ServerSocketConnected.Where(s => !s.Item1.Connected).ToList().ForEach(s => { ServerSocketConnected.Remove(s); s.Item2.Abort(); });
            ImageServerSocketConnected.Where(s => !s.Item1.Connected).ToList().ForEach(s => { ImageServerSocketConnected.Remove(s); s.Item2.Abort(); });

        }
        private void AddCommandClientSocket(Socket CommandSocket)
        {
            var th = new Thread(CommandSocketThread);
            ServerSocketConnected.Add((CommandSocket, th));
            th.Start(CommandSocket);
        }
        private void AddImageClientSocket(Socket CommandSocket, int ImageSocketNum)
        {
            var th = new Thread(ImageSocketThread);
            ImageServerSocketConnected.Add((CommandSocket, th));
            th.Start((CommandSocket, ImageSocketNum));
        }
    }
    */


    public class TCPCCDCardServer
    {
        private TcpListener Server;
        private TcpListener[] ImageServers;

        private ConcurrentDictionary<Socket, Task> CommandClients = new();
        private ConcurrentDictionary<Socket, Task> ImageClients = new();

        private CancellationTokenSource cts;
        private CancellationTokenSource ImageCts;
        private Task ServerAcceptTask;
        private Task[] ImageAcceptTasks;
        private short[][,] SocketImages;
        private System.Threading.Timer timer;
        volatile bool ImageDataIsReady;
        public int CardNumber { get; private set; }
        public bool IsCardWorkingProperly { get; set; }
        public bool IsDataReading { get; private set; }
        private CancellationTokenSource StopWaitingForSynchrosignal = null;
        private readonly Action<string> log;
        public bool isStarted { get; private set; }
        public TCPCCDCardServer(int CardNumber, Action<string> log = null)
        {
            if (CardNumber > 12) CardNumber = 13;
            this.CardNumber = CardNumber;
            this.log = log ?? (_ => { });
        }

        private IPAddress GetIP()
        {
            var ip = $"192.168.255.{CardNumber + 100}";
            var ipAddr = IPAddress.Parse(ip);
            return ipAddr;
        }

        public void Start()
        {
            ThreadPool.SetMinThreads(100, 100);
            cts = new CancellationTokenSource();
            SocketImages = new short[8][,];

            var ipAddr = GetIP();

            Server = new TcpListener(ipAddr, 200);
            Server.Start();
            ServerAcceptTask = Task.Factory.StartNew(() => AcceptLoop(Server, HandleCommandClient, CommandClients, cts.Token), TaskCreationOptions.LongRunning);


            timer = new System.Threading.Timer(ClearDisconnectedSockets, null, 0, 30000);
            isStarted = true;

        }

        public void Stop()
        {
            cts.Cancel();

            Server.Stop();
            StopImageThreads();
            isStarted = false;

        }

        private void StartImageThreads()
        {
            ImageCts = new CancellationTokenSource();
            ImageServers = new TcpListener[8];
            ImageAcceptTasks = new Task[8];
            for (int i = 0; i < 8; i++)
            {
                var ipAddr = GetIP();
                ImageServers[i] = new TcpListener(ipAddr, i + 100);
                ImageServers[i].Start();
                int socketNum = i; // Capture for closure
                ImageAcceptTasks[socketNum] = Task.Factory.StartNew(() => AcceptLoop(ImageServers[socketNum], s => HandleImageClient(s, socketNum), ImageClients, ImageCts.Token), TaskCreationOptions.LongRunning);
            }

        }
        private void StopImageThreads()
        {
            if (ImageCts == null || ImageCts.IsCancellationRequested) return;
            ImageCts.Cancel();

            foreach (var server in ImageServers)
                server.Stop();

            Task.WaitAll(ImageAcceptTasks);
            ServerAcceptTask.Wait();

            foreach (var task in CommandClients.Values.Concat(ImageClients.Values))
                task.Wait();
            ImageCts.Dispose();
            ImageCts = null;
        }

        public void RaiseSynchrosignal()
        {
            if (StopWaitingForSynchrosignal != null)
            {
                StopWaitingForSynchrosignal.Cancel();
            }
        }

        private async Task AcceptLoop(TcpListener listener, Func<Socket, Task> clientHandler, ConcurrentDictionary<Socket, Task> registry, CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var client = await listener.AcceptSocketAsync(token);
                    var task = Task.Factory.StartNew(async () =>
                    {
                        log($"Card {CardNumber}: Клиент {(client?.RemoteEndPoint as IPEndPoint)?.Address.ToString() ?? ""} подключился");
                        await clientHandler(client);
                        registry.TryRemove(client, out _);
                    }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default).Unwrap();

                    registry.TryAdd(client, task);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                log($"Card {CardNumber}: Accept error: {ex}");
            }
        }

        private async Task HandleCommandClient(Socket socket)
        {
            var Fullbuffer = new byte[0];
            var buffer = new byte[1024];
            try
            {
                while (socket.Connected)
                {
                    var bytesRead = await socket.ReceiveAsync(buffer, SocketFlags.None);
                    if (bytesRead > 0)
                    {
                        log($"Card {CardNumber}: Получено: <{Encoding.ASCII.GetString(buffer, 0, bytesRead)}>");
                        var LastBufferSize = Fullbuffer.Length;
                        Array.Resize(ref Fullbuffer, Fullbuffer.Length + bytesRead);
                        Array.Copy(buffer, 0, Fullbuffer, LastBufferSize, bytesRead);
                        var commands = Packet2Response(ref Fullbuffer, out List<Tuple<byte[], Tuple<byte, bool>>> cmdlog);
                        for (int i = 0; i < cmdlog.Count; i++)
                        {
                            log($"Плата {CardNumber}: (CRC:{(cmdlog[i].Item2.Item2 ? "+" : $"- (0x{cmdlog[i].Item2.Item1:X2})")}): " + $"<{String.Join(", ", cmdlog[i].Item1.Select(i => $"0x{i:X2}"))}");
                        }
                        for (int cmdN = 0; cmdN < commands.Count; cmdN++)
                        {
                            switch (commands[cmdN][1])
                            {
                                case 1:
                                    ImageDataIsReady = false;
                                    if (!IsDataReading)
                                    {
                                        StartReading(2000, 3000);
                                    }
                                    else
                                    {
                                        SendCommand(new byte[] { 00, 01, 01, 10 });
                                        ClearImageData();
                                    }
                                    break;
                                case 4:
                                    SendCommand(new byte[] { 0, 04, 00 });
                                    break;
                                case 5:
                                    var pars = commands[cmdN][2];
                                    if ((pars & 1) == 1)
                                    {
                                        if (!IsDataReading)
                                        {
                                            StartReading(8000, 15000, true);
                                        }
                                        else
                                        {
                                            SendCommand(new byte[] { 00, 01, 01, 10 });
                                            ClearImageData();
                                        }
                                    }
                                    else
                                    {
                                        SendCommand(new byte[] { 0, 05, (byte)(ImageDataIsReady ? 1 : 0) });
                                    }
                                    break;
                                case 7:
                                    SendCommand(new byte[] { 0, 07, (byte)(ImageDataIsReady ? 1 : 0) });
                                    break;
                                case 9:
                                    StartImageThreads();
                                    break;
                                case 11:
                                    SendCommand(new byte[] { 0, 11, 00 });
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log($"Card {CardNumber}: Command client error: {ex}");
            }
            finally
            {
                socket.Close();
            }
        }

        private void StartReading(int DelayFromMs, int DelayToMs, bool waitForSynchrosignal = false)
        {
            if (!waitForSynchrosignal)
            {
                _ = Task.Run(async () =>
                {
                    var rnd = new Random();
                    await Task.Delay(rnd.Next(DelayToMs - DelayFromMs) + DelayFromMs);
                    FillImageData(CardNumber);
                    SendCommand(new byte[] { 00, 01, 00, 10 });
                    ImageDataIsReady = true;
                });
            }
            else
            {
                if (StopWaitingForSynchrosignal != null) return;
                StopWaitingForSynchrosignal = new CancellationTokenSource();
                _ = Task.Run(async () =>
                {
                    try
                    {
                        try
                        {
                            await Task.Delay(60000, StopWaitingForSynchrosignal.Token);
                        }
                        catch (OperationCanceledException)
                        {
                            log?.Invoke($"Card {CardNumber}: Synchrosignal");
                        }
                        FillImageData(CardNumber);
                        SendCommand(new byte[] { 00, 01, 00, 10 });
                        ImageDataIsReady = true;
                    }
                    finally
                    {
                        StopWaitingForSynchrosignal?.Dispose();
                        StopWaitingForSynchrosignal = null;
                    }
                });
            }
        }

        private async Task HandleImageClient(Socket socket, int socketNum)
        {
            try
            {
                if (SocketImages[socketNum] != null)
                {
                    var data = SocketImages[socketNum].Cast<short>().SelectMany(BitConverter.GetBytes).ToArray();
                    await socket.SendAsync(data, SocketFlags.None);
                    SocketImages[socketNum] = null;
                }
                else
                {
                    SendCommand(new byte[] { (byte)socketNum, 0x09, 0x00, 0x01 });
                }
            }
            catch (Exception ex)
            {
                log($"Card {CardNumber}: Image client error: {ex}");
            }
            finally
            {
                socket.Close();
            }
        }

        private void SendCommand(byte[] data)
        {
            var crc = (byte)(~data.Sum(x => x));
            var full = new byte[data.Length + 1];
            Array.Copy(data, full, data.Length);
            full[^1] = crc;


            var _2Send = Command2Packet(full);

            foreach (var client in CommandClients.Keys)
            {
                if (client.Connected)
                {
                    var rom = new ReadOnlyMemory<byte>(_2Send);
                    client.SendAsync(rom);
                }
            }
        }

        private void ClearDisconnectedSockets(object? state)
        {
            foreach (var kv in CommandClients)
            {
                if (!kv.Key.Connected)
                    CommandClients.TryRemove(kv.Key, out _);
            }

            foreach (var kv in ImageClients)
            {
                if (!kv.Key.Connected)
                    ImageClients.TryRemove(kv.Key, out _);
            }
        }

        private void FillImageData(int seed)
        {
            var rnd = new Random(seed + 1);
            for (int i = 0; i < SocketImages.Length; i++)
            {
                SocketImages[i] = new short[512, 512];
                for (int x = 0; x < 512; x++)
                {
                    var max = 5000 + rnd.NextDouble() * 5000;
                    for (int y = 0; y < 512; y++)
                    {
                        var v = Math.Exp(Math.Pow((x - 256d) / 256d * 10, 2));
                        v = v * max * 10;
                        if (v > max) v = max;
                        v += (rnd.NextDouble() * 2 - 1) * 200;
                        SocketImages[i][y, x] = (short)v;
                    }
                }
            }
        }
        private void ClearImageData()
        {
            for (int i = 0; i < SocketImages.Length; i++)
            {
                SocketImages[i] = null;
            }
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
    }

}
