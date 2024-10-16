using DoMCModuleControl;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Modules;

namespace DoMCLib.Classes.Module.RDPB
{
    public partial class RDPBModule
    {
        public class StopCommand : AbstractCommandBase
        {
            public StopCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, null) { }

            protected override void Executing() => ((RDPBModule)Module).Stop();

        }

    }
}
