using System.Net.NetworkInformation;
using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCModuleControl.Commands;

namespace DoMCLib.Classes.Module.LCB
{
    public partial class LCBModule
    {
        public class GetCurrentStatusCommand : CommandBase
        {
            public GetCurrentStatusCommand(IMainController mainController, ModuleBase module) : base(mainController, module, typeof(NetworkInterface), typeof(LEDDataExchangeStatus)) { }
            protected override void Executing()
            {
                OutputData = ((LCBModule)Module).GetLEDDataExchangeStatus();
            }

        }

    }

}
