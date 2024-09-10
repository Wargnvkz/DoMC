using DoMCModuleControl;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Modules;

namespace DoMCLib.Classes.Model.ArchiveDB
{
    public partial class ArchiveDBModule
    {
        public class StopCommand : CommandBase
        {
            public StopCommand(IMainController mainController, ModuleBase module) : base(mainController, module, typeof(), null) { }
            protected override void Executing() => ((ArchiveDBModule)Module).Stop();
        }
       
    }
}
