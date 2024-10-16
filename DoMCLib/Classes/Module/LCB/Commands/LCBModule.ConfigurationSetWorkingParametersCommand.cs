using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCModuleControl.Commands;

namespace DoMCLib.Classes.Module.LCB
{
    public partial class LCBModule
    {
        public class ConfigurationSetWorkingParametersCommand : AbstractCommandBase
        {
            public ConfigurationSetWorkingParametersCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof(LCBWorkingParameters), null) { }
            protected override void Executing() => ((LCBModule)Module).SetWorkingParameters((LCBWorkingParameters)InputData);
        }


    }

    

}
