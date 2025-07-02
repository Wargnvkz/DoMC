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
using System.ComponentModel;

namespace DoMCLib.Classes.Module.API.Commands
{
    [Description("Запуск модуля обмена данными REST API в работу")]
    public class StartRESTAPIServerCommand : GenericCommandBase<bool>
    {
        public StartRESTAPIServerCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => await ((APIModule)Module).StartServer();
    }
    [Description("Остановка работы модуля обмена данными REST API")]
    public class StopRESTAPIServerCommand : GenericCommandBase<bool>
    {
        public StopRESTAPIServerCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => await ((APIModule)Module).StopServer();
    }

}
