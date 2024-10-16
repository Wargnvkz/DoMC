using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCModuleControl.Commands;

namespace DoMCLib.Classes.Module.LCB
{
    public partial class LCBModule
    {
        public class SetLCBEquipmentStatusCommand : AbstractCommandBase
        {
            public SetLCBEquipmentStatusCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, null) { }
            protected override void Executing() => ((LCBModule)Module).SetLCBEquipmentStatus();
        }


    }

    

}
