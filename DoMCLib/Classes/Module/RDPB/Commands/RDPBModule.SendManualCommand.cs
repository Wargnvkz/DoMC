using DoMCModuleControl;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Modules;

namespace DoMCLib.Classes.Module.RDPB
{
    public partial class RDPBModule
    {
        public class SendManualCommand : AbstractCommandBase
        {
            public SendManualCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof(string), null) { }

            protected override void Executing() => ((RDPBModule)Module).SendManualCommandProc((string)(InputData ?? String.Empty));

        }

    }
}
