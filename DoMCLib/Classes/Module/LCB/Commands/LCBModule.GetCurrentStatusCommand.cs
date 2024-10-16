using System.Net.NetworkInformation;
using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCModuleControl.Commands;

namespace DoMCLib.Classes.Module.LCB
{
    public partial class LCBModule
    {
        public class GetCurrentStatusCommand : AbstractCommandBase
        {
            public GetCurrentStatusCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof(NetworkInterface), typeof(LEDDataExchangeStatus)) { }
            protected override void Executing()
            {
                OutputData = ((LCBModule)Module).GetLEDDataExchangeStatus();
            }

        }

    }

}
