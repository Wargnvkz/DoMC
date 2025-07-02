using DoMCModuleControl.Modules;
using DoMCModuleControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoMCModuleControl.Commands;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel;

namespace DoMCLib.Classes.Module.LCB.Commands
{
    [Description("Запуск модуля БУС в работу")]
    public class LCBStartCommand : AbstractCommandBase, IExecuteCommandAsync
    {
        public LCBStartCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, null) { }
        protected override async Task Executing() => ((LCBModule)Module).Start();

        public new async Task ExecuteCommandAsync()
        {
            await base.ExecuteCommandAsync();
        }
    }
    [Description("Остановка работы модуля БУС")]
    public class LCBStopCommand : AbstractCommandBase, IExecuteCommandAsync
    {
        public LCBStopCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, null) { }
        protected override async Task Executing() => ((LCBModule)Module).Stop();
        public new async Task ExecuteCommandAsync()
        {
            await base.ExecuteCommandAsync();
        }
    }

    [Description("Установка значения тока светодиодов БУС")]
    public class SetLCBCurrentCommand : GenericCommandBase<int, bool>
    {
        public SetLCBCurrentCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => SetOutput(await ((LCBModule)Module).SetLCBCurrent(InputData));
    }

    [Description("Получение значения тока светодиодов БУС")]
    public class GetLCBCurrentCommand : GenericCommandBase<int>
    {
        public GetLCBCurrentCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => SetOutput(await ((LCBModule)Module).GetLCBCurrent());
    }

    [Description("Установка параметров движения БУС")]
    public class SetLCBMovementParametersCommand : GenericCommandBase<(int PreformLength, int DelayLength), bool>
    {
        public SetLCBMovementParametersCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => SetOutput(await ((LCBModule)Module).SetLCBMovementParameters(InputData.PreformLength, InputData.DelayLength));
    }

    [Description("Получение параметров движения БУС")]
    public class GetLCBMovementParametersCommand : GenericCommandBase<LEDMovementParameters>
    {
        public GetLCBMovementParametersCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => SetOutput(await ((LCBModule)Module).GetLCBMovementParameters());
    }
    [Description("Установка состояния выходов БУС")]
    public class SetLCBEquipmentStatusCommand : GenericCommandBase<LEDEquipmentStatus, bool>
    {
        public SetLCBEquipmentStatusCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => SetOutput(await ((LCBModule)Module).SetLCBEquipmentStatus(InputData));
    }
    [Description("Получение состояния выходов БУС")]
    public class GetLCBEquipmentStatusCommand : GenericCommandBase<LEDEquipmentStatus>
    {
        public GetLCBEquipmentStatusCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => SetOutput(await ((LCBModule)Module).GetLCBEquipmentStatus());
    }
    [Description("Установка рабочего режима БУС")]
    public class SetLCBWorkModeCommand : GenericCommandBase<bool>
    {
        public SetLCBWorkModeCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => SetOutput(await ((LCBModule)Module).SetLCBWorkMode());
    }
    [Description("Отключение рабочего режима БУС")]
    public class SetLCBNonWorkModeCommand : GenericCommandBase<bool>
    {
        public SetLCBNonWorkModeCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => SetOutput(await ((LCBModule)Module).SetLCBNonWorkMode());
    }
    [Description("Получение максимального положения с датчика положения БУС")]
    public class GetLCBMaxPositionCommand : GenericCommandBase<int>
    {
        public GetLCBMaxPositionCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => SetOutput(await ((LCBModule)Module).GetLCBMaxPosition());
    }
    [Description("Получение текущей позиции с датчика положения БУС")]
    public class GetLCBCurrentPositionCommand : GenericCommandBase<int>
    {
        public GetLCBCurrentPositionCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => SetOutput(await ((LCBModule)Module).GetLCBCurrentPosition());
    }

}
