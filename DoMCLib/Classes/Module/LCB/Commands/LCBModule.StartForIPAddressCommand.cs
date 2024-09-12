using System.Net;
using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCModuleControl.Commands;

namespace DoMCLib.Classes.Module.LCB
{
    public partial class LCBModule
    {
        public class StartForIPAddressCommand : CommandBase
        {
            public StartForIPAddressCommand(IMainController mainController, ModuleBase module) : base(mainController, module, typeof(IPAddress), null) { }
            protected override void Executing() => ((LCBModule)Module).StartForIPAddress((IPAddress)InputData);
        }


    }

   

}
