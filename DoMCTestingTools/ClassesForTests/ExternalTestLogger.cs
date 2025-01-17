using DoMCModuleControl.Logging;

namespace DoMCTestingTools.ClassesForTests
{
    public class ExternalTestLogger : ILogger
    {
        public string? LastMessage;
        public LoggerLevel LastMessageLevel;
        public Exception? LastException;
        public void Add(LoggerLevel level, string Message)
        {
            LastMessage = Message;
            LastMessageLevel = level;
        }

        public void Add(LoggerLevel level, string Message, Exception exception)
        {
            LastMessage = Message;
            LastMessageLevel = level;
            LastException = exception;
        }

        public void Flush()
        {

        }

        public void SetMaxLogginLevel(LoggerLevel level)
        {

        }
    }
}