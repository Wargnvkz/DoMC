using DoMCModuleControl.Commands;
using DoMCModuleControl.Modules;
using DoMCModuleControl;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes.Module.ArchiveDB.Commands
{
    public class SetConfigurationCommand : AbstractCommandBase
    {
        public SetConfigurationCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof(ArchiveDBConfiguration), null) { }
        protected override async Task Executing() => ((ArchiveDBModule)Module).SetConfigurationAsync((ArchiveDBConfiguration)InputData);
    }
    public class StartCommand : AbstractCommandBase
    {
        public StartCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, null) { }
        protected override async Task Executing() => ((ArchiveDBModule)Module).StartAsync();
    }
    public class StopCommand : AbstractCommandBase
    {
        public StopCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, null) { }
        protected override async Task Executing() => ((ArchiveDBModule)Module).StopAsync();
    }
    public class GetWorkingStatusCommand : AbstractCommandBase
    {
        public GetWorkingStatusCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, typeof(bool)) { }
        protected override async Task Executing() => OutputData = ((ArchiveDBModule)Module).IsStarted;
    }
}
