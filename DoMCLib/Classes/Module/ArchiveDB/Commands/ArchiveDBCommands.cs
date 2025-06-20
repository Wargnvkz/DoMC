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
    public class SetConfigurationCommand : GenericCommandBase<ArchiveDBConfiguration, bool>
    {
        public SetConfigurationCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing()
        {
            await ((ArchiveDBModule)Module).SetConfigurationAsync(InputData);
            SetOutput(true);
        }

    }
    public class StartCommand : AbstractCommandBase
    {
        public StartCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, null) { }
        protected override async Task Executing() => await ((ArchiveDBModule)Module).StartAsync();
    }
    public class StopCommand : AbstractCommandBase
    {
        public StopCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, null) { }
        protected override async Task Executing() => await ((ArchiveDBModule)Module).StopAsync();
    }
    public class GetWorkingStatusCommand : GenericCommandBase<bool>
    {
        public GetWorkingStatusCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => SetOutput(((ArchiveDBModule)Module).IsStarted);
    }
}
