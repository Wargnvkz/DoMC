using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl
{
    public interface ILogger
    {
        public void Add(LoggerLevel level, string Message, bool ShowDateTime = true);
        public void Add(LoggerLevel level, string Message, Exception ex);
        public void SetMaxLogginLevel(LoggerLevel level);
    }

    public enum LoggerLevel
    {
        Error = 0,
        Warning = 1,
        Information = 2,
        Total = 4
    }
}
