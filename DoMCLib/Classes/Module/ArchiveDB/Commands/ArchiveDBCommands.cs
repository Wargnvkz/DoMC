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
using DoMCLib.DB;

namespace DoMCLib.Classes.Module.ArchiveDB.Commands
{
    [Description("Конфигурирование архива базы данных")]
    public class SetConfigurationCommand : GenericCommandBase<ArchiveDBConfiguration, bool>
    {
        public SetConfigurationCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing()
        {
            await ((ArchiveDBModule)Module).SetConfigurationAsync(InputData);
            SetOutput(true);
        }

    }
    [Description("Запуск в работу модуля архива базы данных")]
    public class StartCommand : AbstractCommandBase, IExecuteCommandAsync
    {
        public StartCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, null) { }
        protected override async Task Executing() => await ((ArchiveDBModule)Module).StartAsync();
        public new async Task ExecuteCommandAsync()
        {
            await base.ExecuteCommandAsync();
        }
    }
    [Description("Остановка работы модуля архива базы данных")]
    public class StopCommand : AbstractCommandBase, IExecuteCommandAsync
    {
        public StopCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, null) { }
        protected override async Task Executing() => await ((ArchiveDBModule)Module).StopAsync();
        public new async Task ExecuteCommandAsync()
        {
            await base.ExecuteCommandAsync();
        }
    }
    [Description("Получение текущенго состояния архива базы данных")]
    public class GetWorkingStatusCommand : GenericCommandBase<bool>
    {
        public GetWorkingStatusCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => SetOutput(((ArchiveDBModule)Module).IsStarted);
    }

    public class GetBoxFromCommand : GenericCommandBase<DateTime, List<Box>>
    {
        public GetBoxFromCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing()
        {
            var boxes = ((ArchiveDBModule)Module).GetBoxes(InputData);
            var result = boxes.Select(b => b.Convert()).ToList();
            SetOutput(result);
        }

    }
}
