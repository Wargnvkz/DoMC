using DoMCModuleControl;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Modules;

namespace DoMCLib.Classes.Model.RDPB
{
    public partial class RDPBModule
    {
        public class SendManualCommand : CommandBase
        {
            public SendManualCommand(IMainController mainController, ModuleBase module) : base(mainController, module, typeof(string), null) { }

            protected override void Executing() => ((RDPBModule)Module).SendManualCommandProc((string)(InputData ?? String.Empty));

        }

    }
}
