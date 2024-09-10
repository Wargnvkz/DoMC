using DoMCModuleControl;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Modules;

namespace DoMCLib.Classes.Model.RDPB
{
    public partial class RDPBModule
    {
        public class MakeBlockSendWorkingStateCommand : CommandBase
        {
            public MakeBlockSendWorkingStateCommand(IMainController mainController, ModuleBase module) : base(mainController, module, null, null) { }

            protected override void Executing() => ((RDPBModule)Module).Send(RDPBCommandType.MakeBlockSendWorkingState);

        }

    }
}
