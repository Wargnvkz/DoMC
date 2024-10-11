using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes.Module.LCB
{

    public interface ISocketDevice
    {
        public Task ConnectAsync(IPEndPoint ipEndpoint, int timeoutMilliseconds, CancellationToken cancellationToken);
        Task<int> ReadAsync(byte[] buffer, int offset, int size, CancellationToken cancellationToken);
        Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken);
        void SetReadTimeout(int msTimeout);
        void Close();
    }

}
