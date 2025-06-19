using DoMCLib.Classes.Configuration.CCD;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Modules;
using DoMCModuleControl;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes.Module.DB.Commands
{

    public class SetConfigurationCommand : GenericCommandBase<string>
    {
        public SetConfigurationCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => ((DBModule)Module).SetConfiguration((string)InputData);
    }
    public class StartCommand : AbstractCommandBase
    {
        public StartCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => ((DBModule)Module).Start();
    }
    public class StopCommand : AbstractCommandBase
    {
        public StopCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => ((DBModule)Module).Stop();
    }
    public class EnqueueCycleDateCommand : AbstractCommandBase
    {
        public EnqueueCycleDateCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => ((DBModule)Module).EnqueueCycleDate((CycleImagesCCD)InputData);
    }
    public class EnqueueBoxDateCommand : AbstractCommandBase
    {
        public EnqueueBoxDateCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => ((DBModule)Module).EnqueueBoxDate((Box)InputData);
    }

}
