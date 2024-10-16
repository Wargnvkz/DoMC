using DoMCLib.Classes.Module.RDPB.Classes;
using DoMCModuleControl;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Modules;

namespace DoMCLib.Classes.Module.RDPB
{
    public partial class RDPBModule
    {
        public class SendSetIsBadCommand : AbstractCommandBase
        {
            public SendSetIsBadCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, null) { }

            protected override void Executing() => ((RDPBModule)Module).Send(RDPBCommandType.SetIsBad);

        }

    }
}
