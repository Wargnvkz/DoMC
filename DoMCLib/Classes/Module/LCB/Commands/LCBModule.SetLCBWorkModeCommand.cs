using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCModuleControl.Commands;

namespace DoMCLib.Classes.Module.LCB
{
    public partial class LCBModule
    {
        public class SetLCBWorkModeCommand : AbstractCommandBase
        {
            public SetLCBWorkModeCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, null) { }
            protected override void Executing() => ((LCBModule)Module).SetLCBWorkMode();
        }


    }

    

}
