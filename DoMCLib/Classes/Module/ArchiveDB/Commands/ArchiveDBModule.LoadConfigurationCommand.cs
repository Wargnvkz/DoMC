using DoMCModuleControl;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Modules;

namespace DoMCLib.Classes.Module.ArchiveDB
{
    public partial class ArchiveDBModule
    {
        public class SetConfigurationCommand : AbstractCommandBase
        {
            public SetConfigurationCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof(ArchiveDBConfiguration), null) { }
            protected override void Executing() => ((ArchiveDBModule)Module).SetConfiguration((ArchiveDBConfiguration)InputData);
        }
       
    }
}
