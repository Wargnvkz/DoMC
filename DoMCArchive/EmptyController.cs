using DoMCModuleControl;
using DoMCModuleControl.External;
using DoMCModuleControl.Logging;
using DoMCModuleControl.Modules;
using DoMCModuleControl.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCArchive
{
    public class EmptyController : IMainController
    {
        BaseFilesLogger baselogger;
        IFileSystem fileSystem;
        public EmptyController()
        {
            fileSystem = new DoMCModuleControl.External.FileSystem();
            baselogger = new BaseFilesLogger(fileSystem);
        }
        public Type? LastCommand { get; set; }

        public ILogger GetLogger(string ModuleName)
        {
            return new Logger(ModuleName, baselogger);
        }

        public IMainUserInterface GetMainUserInterface()
        {
            return null;
        }

        public AbstractModuleBase GetModule(Type ModuleType)
        {
            return null;
        }

        public AbstractModuleBase GetModule(string ModuleName)
        {
            return null;
        }

        public Observer GetObserver()
        {
            return new Observer(GetLogger("События"));
        }
    }
}
