#pragma warning disable IDE0290
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl.Logging
{
    public class Logger : ILogger
    {
        public LoggerLevel MaxLogginLevel { get; private set; }
        public string ModuleName { get; private set; }
        public IBaseLogger BaseLogger { get; private set; }
        public Logger(string moduleName, IBaseLogger baseLogger)
        {
            ModuleName = moduleName;
            BaseLogger = baseLogger;
            MaxLogginLevel = LoggerLevel.Important;
        }

        public void Add(LoggerLevel level, string Message)
        {
            if (level <= MaxLogginLevel)
            {
                BaseLogger.AddMessage(ModuleName, Message);
            }
        }

        public void Add(LoggerLevel level, string Message, Exception ex)
        {
            if (level <= MaxLogginLevel)
            {
                BaseLogger.AddMessage(ModuleName, $"{Message} {ex.Message} {ex.StackTrace ?? ""}");
            }
        }

        public void SetMaxLogginLevel(LoggerLevel level)
        {
            MaxLogginLevel = level;
        }
    }
}
