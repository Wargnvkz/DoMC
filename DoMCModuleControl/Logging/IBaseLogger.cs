using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl.Logging
{
    public interface IBaseLogger
    {
        public void AddMessage(string Module, string Message);
        public void RegisterExternalLogger(ILogger logger);

    }
}
