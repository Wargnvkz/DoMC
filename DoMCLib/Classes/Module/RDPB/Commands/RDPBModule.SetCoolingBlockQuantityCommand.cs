using DoMCLib.Classes.Module.RDPB.Classes;
using DoMCModuleControl;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Modules;

namespace DoMCLib.Classes.Module.RDPB
{
    public partial class RDPBModule
    {
        public class SetCoolingBlockQuantityCommand : AbstractCommandBase
        {
            public SetCoolingBlockQuantityCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof(int), null) { }

            protected override void Executing() => ((RDPBModule)Module).Send(RDPBCommandType.SetCoolingBlocks, (int)(InputData ?? 4));

        }

    }
}
