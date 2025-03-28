using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes
{
    public class TcpSocketDevice : ISocketDevice
    {
        private TcpClient _tcpClient;
        private NetworkStream _networkStream;

        public async Task ConnectAsync(IPEndPoint ipEndpoint, int timeoutMilliseconds, CancellationToken cancellationToken)
        {
            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                cts.CancelAfter(timeoutMilliseconds);  // Таймаут подключения
                _tcpClient = new TcpClient();

                try
                {
                    await _tcpClient.ConnectAsync(ipEndpoint).WaitAsync(cts.Token); // Установка таймаута подключения
                    //_tcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                    _networkStream = _tcpClient.GetStream();
                    //_networkStream.ReadTimeout = 100;  // Таймаут чтения, если понадобится
                }
                catch (OperationCanceledException)
                {
                    throw new TimeoutException($"Подключение к {ipEndpoint} не выполнено за {timeoutMilliseconds} мс.");
                }
                /*try
                {
                    // Ускорение подтверждения получения пакета. Платы ждут подтверждения ровно 200 мс и перепосылают пакет, а windows обычно отвечат позже
                    int SIO_TCP_SET_ACK_FREQUENCY = unchecked((int)0x98000017);
                    var outputArray = new byte[128];
                    var bytesInOutputArray = _tcpClient.Client.IOControl(SIO_TCP_SET_ACK_FREQUENCY, BitConverter.GetBytes(1), outputArray);
                }
                catch { }*/
            }
        }

        public async Task<int> ReadAsync(byte[] buffer, int offset, int size, CancellationToken cancellationToken)
        {
            return await _networkStream.ReadAsync(buffer, offset, size, cancellationToken);
        }

        public async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            await _networkStream.WriteAsync(buffer, offset, count, cancellationToken);
        }

        public void Close()
        {
            _networkStream?.Close();
            _tcpClient?.Close();
        }

        public void SetReadTimeout(int msTimeout)
        {
            if (_networkStream != null)
                _networkStream.ReadTimeout = msTimeout;
        }

        public int AvailableBytes()
        {
            return _networkStream.Socket.Available;
        }

        public bool CanRead()
        {
            return _networkStream.CanRead;
        }

        public bool CanWrite()
        {
            return _networkStream.CanWrite;
        }
/*        public int Read(byte[] buffer, int offset, int size)
        {
            return _networkStream.Read(buffer, offset, size);
        }*/
    }

}
