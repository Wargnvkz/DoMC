using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes
{

    public interface ISocketDevice
    {
        public Task ConnectAsync(IPEndPoint ipEndpoint, int timeoutMilliseconds, CancellationToken cancellationToken);
        Task<int> ReadAsync(byte[] buffer, int offset, int size, CancellationToken cancellationToken);
        Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken);
        void SetReadTimeout(int msTimeout);
        void Close();
        int AvailableBytes();
        bool CanRead();
        bool CanWrite();
        //int Read(byte[] ReadBuffer, int offset, int size);
    }

}
