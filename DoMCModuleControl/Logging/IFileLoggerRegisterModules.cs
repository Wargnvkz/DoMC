using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl.Logging
{
    public interface IFileLoggerRegisterModules
    {
        /// <summary>
        /// Registered a module
        /// </summary>
        /// <returns></returns>
        public void RegisterModule(string ModuleName);

        /// <summary>
        /// Get a list of registered modules
        /// </summary>
        /// <returns></returns>
        public List<(string ModuleName, string LogPath, string LastLogFile)> GetRegisteredModules();

        /// <summary>
        /// Get a log file of module
        /// </summary>
        /// <returns></returns>
        public string GetLogFileOfModule(string ModuleName);

    }
}
