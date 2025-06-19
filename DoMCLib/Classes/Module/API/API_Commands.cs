using DoMCLib.Classes.Module.RDPB;
using DoMCLib.Configuration;
using DoMCModuleControl.Modules;
using DoMCModuleControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoMCModuleControl.Commands;
using DoMCLib.Classes.Module.LCB;

namespace DoMCLib.Classes.Module.API.Commands
{
    public class StartRESTAPIServerCommand : GenericCommandBase<bool>
    {
        public StartRESTAPIServerCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => await ((APIModule)Module).StartServer();
    }
    public class StopRESTAPIServerCommand : GenericCommandBase<bool>
    {
        public StopRESTAPIServerCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => await ((APIModule)Module).StopServer();
    }

}
