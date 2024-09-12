using System.Net.NetworkInformation;
using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCModuleControl.Commands;

namespace DoMCLib.Classes.Module.LCB
{
    public partial class LCBModule
    {
        public class StartForInterfaceCommand : CommandBase
        {
            public StartForInterfaceCommand(IMainController mainController, ModuleBase module) : base(mainController, module, typeof(NetworkInterface), null) { }
            protected override void Executing() => ((LCBModule)Module).StartForInterface((NetworkInterface)InputData);
        }


    }

    

}
