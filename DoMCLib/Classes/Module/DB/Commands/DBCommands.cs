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
    public class StartCommand : AbstractCommandBase, IExecuteCommandAsync
    {
        public StartCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => ((DBModule)Module).Start();
        public new async Task ExecuteCommandAsync()
        {
            await base.ExecuteCommandAsync();
        }
    }
    public class StopCommand : AbstractCommandBase, IExecuteCommandAsync
    {
        public StopCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => ((DBModule)Module).Stop();
        public new async Task ExecuteCommandAsync()
        {
            await base.ExecuteCommandAsync();
        }
    }
    public class EnqueueCycleDateCommand : GenericCommandBase<CycleImagesCCD, bool>
    {
        public EnqueueCycleDateCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing()
        {
            try
            {
                ((DBModule)Module).EnqueueCycleDate(InputData);
                SetOutput(true);
            }
            catch (Exception ex)
            {
                SetOutput(false);
            }

        }

    }
    public class EnqueueBoxDateCommand : GenericCommandBase<Box, bool>
    {
        public EnqueueBoxDateCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing()
        {
            try
            {
                ((DBModule)Module).EnqueueBoxDate((Box)InputData);
                SetOutput(true);

            }
            catch (Exception ex)
            {
                SetOutput(false);

            }
        }

    }

}
