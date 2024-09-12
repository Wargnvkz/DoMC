using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCModuleControl.Commands;

namespace DoMCLib.Classes.Module.LCB
{
    public partial class LCBModule
    {
        public class GetLCBMovementParametersCommand : CommandBase
        {
            public GetLCBMovementParametersCommand(IMainController mainController, ModuleBase module) : base(mainController, module, null, null) { }
            protected override void Executing() => ((LCBModule)Module).GetLCBMovementParameters();
        }


    }

    
}
