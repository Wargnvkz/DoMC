using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCModuleControl.Commands;

namespace DoMCLib.Classes.Module.LCB
{
    public partial class LCBModule
    {
        public class LCBStartCommand : SimpleWaitingCommandBase
        {
            public LCBStartCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, null, LCBModule.Operations.Started.ToString(), LCBModule.EventType.Success.ToString()) { }
            protected override void Executing() => ((LCBModule)Module).Start();
        }
    }
}
