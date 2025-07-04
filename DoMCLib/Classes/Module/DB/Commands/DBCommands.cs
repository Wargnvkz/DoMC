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
using System.ComponentModel;

namespace DoMCLib.Classes.Module.DB.Commands
{
    [Description("Установка конфигурации базы данных")]
    public class SetConfigurationCommand : GenericCommandBase<string, bool>
    {
        public SetConfigurationCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing()
        {
            try
            {
                ((DBModule)Module).SetConfiguration(InputData);
                SetOutput(true);
            }
            catch
            {
                SetOutput(false);
            }
        }
    }
    [Description("Запуск модуля базы данных в работу")]
    public class StartCommand : AbstractCommandBase, IExecuteCommandAsync
    {
        public StartCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => ((DBModule)Module).Start();
        public new async Task ExecuteCommandAsync()
        {
            await base.ExecuteCommandAsync();
        }
    }
    [Description("Остановка работы модуля базы данных")]
    public class StopCommand : AbstractCommandBase, IExecuteCommandAsync
    {
        public StopCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => ((DBModule)Module).Stop();
        public new async Task ExecuteCommandAsync()
        {
            await base.ExecuteCommandAsync();
        }
    }
    [Description("Постановка данных о текущем съеме в очередь записи базы данных")]
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
    [Description("Постановка данных о текущем коробе в очередь записи базы данных")]
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
