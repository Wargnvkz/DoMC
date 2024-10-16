using DoMCModuleControl;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Modules;

namespace DoMCLib.Classes.Module.ArchiveDB
{
    public partial class ArchiveDBModule
    {
        public class StopCommand : AbstractCommandBase
        {
            public StopCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, null) { }
            protected override void Executing() => ((ArchiveDBModule)Module).Stop();
        }

    }
}
