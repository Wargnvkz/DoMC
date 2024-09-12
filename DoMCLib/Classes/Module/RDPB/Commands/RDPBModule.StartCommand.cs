using DoMCModuleControl;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Modules;

namespace DoMCLib.Classes.Module.RDPB
{
    public partial class RDPBModule
    {
        public class StartCommand : CommandBase
        {
            public StartCommand(IMainController mainController, ModuleBase module) : base(mainController, module, null, null) { }
            protected override void Executing() => ((RDPBModule)Module).Start();

        }

    }
}
