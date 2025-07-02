using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl.Logging
{
    public interface IBaseLogger
    {
        /// <summary>
        /// Write message to logging system where messages is divided by modules
        /// </summary>
        /// <param name="Module">Module name</param>
        /// <param name="Message">Message</param>
        public void AddMessage(string Module, string Message);
        /// <summary>
        /// Add logger that will be used when this logger can write message to storage properly (e.g. if disk is full or storage is unaccessable)
        /// </summary>
        /// <param name="logger"></param>
        public void RegisterExternalLogger(ILogger logger);
        /// <summary>
        /// Flush buffered module messages into a storage
        /// </summary>
        /// <param name="Module"></param>
        public void Flush(string Module);


    }
}
