using DoMCModuleControl;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Modules;

namespace DoMCLib.Classes.Module.RDPB
{
    public partial class RDPBModule
    {

        public class LoadConfigurationCommand : CommandBase
        {
            public LoadConfigurationCommand(IMainController mainController, ModuleBase module) : base(mainController, module, typeof(RemoveDefectedPreformBlockConfig), null) { }
            protected override void Executing() => ((RDPBModule)Module).config = (Configuration.RemoveDefectedPreformBlockConfig)InputData;
        }

    }
}
