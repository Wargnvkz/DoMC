using DoMCModuleControl;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Modules;

namespace DoMCLib.Classes.Module.ArchiveDB
{
    public partial class ArchiveDBModule
    {
        public class StartCommand : CommandBase
        {
            public StartCommand(IMainController mainController, ModuleBase module) : base(mainController, module, null, null) { }
            protected override void Executing() => ((ArchiveDBModule)Module).Start();
        }
       
    }
}
