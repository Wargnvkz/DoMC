using DoMCModuleControl;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Modules;

namespace DoMCLib.Classes.Module.RDPB
{
    public partial class RDPBModule
    {
        public class SetCoolingBlockQuantityCommand : CommandBase
        {
            public SetCoolingBlockQuantityCommand(IMainController mainController, ModuleBase module) : base(mainController, module, typeof(int), null) { }

            protected override void Executing() => ((RDPBModule)Module).Send(RDPBCommandType.SetCoolingBlocks, (int)(InputData ?? 4));

        }

    }
}
