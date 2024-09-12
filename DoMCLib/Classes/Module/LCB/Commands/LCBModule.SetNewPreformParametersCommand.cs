using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCModuleControl.Commands;

namespace DoMCLib.Classes.Module.LCB
{
    public partial class LCBModule
    {
        public class SetNewPreformParametersCommand : CommandBase
        {
            public SetNewPreformParametersCommand(IMainController mainController, ModuleBase module) : base(mainController, module, typeof(LCBSettings), null) { }
            protected override void Executing() => ((LCBModule)Module).SetNewPreformParameters((LCBSettings)InputData);
        }


    }

   

}
