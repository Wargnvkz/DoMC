using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCModuleControl.Commands;

namespace DoMCLib.Classes.Module.LCB
{
    public partial class LCBModule
    {
        public class ConfigurationSetWorkingParametersCommand : CommandBase
        {
            public ConfigurationSetWorkingParametersCommand(IMainController mainController, ModuleBase module) : base(mainController, module, typeof(LCBWorkingParameters), null) { }
            protected override void Executing() => ((LCBModule)Module).SetWorkingParameters((LCBWorkingParameters)InputData);
        }


    }

    

}
