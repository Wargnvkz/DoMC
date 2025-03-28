#pragma warning disable IDE0290
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl.Logging
{
    public class Logger : ILogger, IDisposable
    {
        public LoggerLevel MaxLogginLevel { get; private set; }
        public string ModuleName { get; private set; }
        public IBaseLogger BaseLogger { get; private set; }
        public Logger(string moduleName, IBaseLogger baseLogger)
        {
            if (moduleName == null) throw new ArgumentNullException(nameof(moduleName));
            if (baseLogger == null) throw new ArgumentNullException(nameof(baseLogger));
            ModuleName = moduleName;
            BaseLogger = baseLogger;
            MaxLogginLevel = LoggerLevel.FullDetailedInformation;
        }

        public void Add(LoggerLevel level, string Message)
        {
            if (level <= MaxLogginLevel)
            {
                BaseLogger?.AddMessage(ModuleName, Message);
            }
        }

        public void Add(LoggerLevel level, string Message, Exception ex)
        {
            if (ex == null) throw new ArgumentNullException(nameof(ex));
            if (level <= MaxLogginLevel)
            {
                BaseLogger?.AddMessage(ModuleName, $"{Message} {ex.Message} {ex.StackTrace ?? ""}");
            }
        }

        public void SetMaxLogginLevel(LoggerLevel level)
        {
            MaxLogginLevel = level;
        }

        public void Flush()
        {
            lock (this)
            {
                BaseLogger?.Flush(ModuleName);
            }
        }

        public void Dispose()
        {
            BaseLogger = null;
        }
    }
}
