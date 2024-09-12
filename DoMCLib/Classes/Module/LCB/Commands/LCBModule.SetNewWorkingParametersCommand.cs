using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCModuleControl.Commands;

namespace DoMCLib.Classes.Module.LCB
{
    public partial class LCBModule
    {
        public class SetNewWorkingParametersCommand : CommandBase
        {
            public SetNewWorkingParametersCommand(IMainController mainController, ModuleBase module) : base(mainController, module, typeof(LCBWorkingParameters), null) { }
            protected override void Executing() => ((LCBModule)Module).SetNewWorkingParameters((LCBWorkingParameters)InputData);
        }


    }

    

}
