using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace DoMCModuleControl.Logging
{
    public class BaseSystemLogger : IBaseLogger
    {
        public void AddMessage(string Module, string Message)
        {
            //TODO: Запись логов в системные логи, в случае если ошибка, которую в обычные логи невозможно записать. Например, нет места на диске для логов.
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
            }
        }

        public void RegisterExternalLogger(ILogger logger)
        {

        }
    }
}
