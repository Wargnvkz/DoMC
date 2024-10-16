using System.Net;
using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCModuleControl.Commands;

namespace DoMCLib.Classes.Module.LCB
{
    public partial class LCBModule
    {
        public class StartForIPAddressCommand : AbstractCommandBase
        {
            public StartForIPAddressCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof(IPAddress), null) { }
            protected override void Executing() => ((LCBModule)Module).StartForIPAddress((IPAddress)InputData);
        }


    }

   

}
