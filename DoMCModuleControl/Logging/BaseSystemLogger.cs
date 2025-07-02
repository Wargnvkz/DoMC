using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using DoMCModuleControl.External;
using System.Collections.Concurrent;

namespace DoMCModuleControl.Logging
{
    public class BaseSystemLogger : IBaseLogger
    {
        IFileSystem FileSystem;
        ILogger? ExternalLogger;
        public BaseSystemLogger(IFileSystem fileSystem)
        {
            FileSystem = fileSystem;
        }
        public void AddMessage(string Module, string Message)
        {
            try
            {
                using (var sw = FileSystem.GetStreamWriter(Module, true))
                {
                    sw.WriteLine(Message);
                }
            }
            catch (Exception ex)
            {
                ExternalLogger?.Add(LoggerLevel.Critical, $"Ошибка при логировании", ex);
            }
        }

        public void Flush(string Module)
        {

        }

        public void RegisterExternalLogger(ILogger logger)
        {
            ExternalLogger = logger;
        }

    }
}
