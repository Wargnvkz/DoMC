using DoMCLib.Classes.Configuration;
using DoMCModuleControl;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Modules;

namespace DoMCLib.Classes.Module.RDPB
{
    public partial class RDPBModule
    {

        public class LoadConfigurationCommand : AbstractCommandBase
        {
            public LoadConfigurationCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof(RemoveDefectedPreformBlockConfig), null) { }
            protected override void Executing() => ((RDPBModule)Module).config = (DoMCLib.Classes.Configuration.RemoveDefectedPreformBlockConfig)InputData;
        }

    }
}
