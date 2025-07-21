using DoMCLib.Classes.Configuration;
using DoMCLib.Classes.Module.RDPB.Classes;
using DoMCLib.Configuration;
using DoMCModuleControl;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Modules;
using System.ComponentModel;

namespace DoMCLib.Classes.Module.RDPB.Commands
{
    [Description("Отправка информации бракёру, что съем хороший")]
    public class SendSetIsOkCommand : GenericCommandBase<RDPBStatus>
    {
        public SendSetIsOkCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => SetOutput(await ((RDPBModule)Module).Send(RDPBCommandType.SetIsOK, CancelationTokenSourceToCancelCommandExecution.Token));
    }

    [Description("Установка параметров подключения к бракёру")]
    public class SendConfigurationToModuleCommand : GenericCommandBase<ApplicationConfiguration, bool>
    {
        public SendConfigurationToModuleCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing()
        {
            await ((RDPBModule)Module).SetConfig(InputData);
            SetOutput(true);
        }
    }
    [Description("Получение параметров бракёра")]
    public class GetParametersCommand : GenericCommandBase<RDPBStatus>
    {
        public GetParametersCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => SetOutput(await((RDPBModule)Module).Send(RDPBCommandType.GetParameters, CancelationTokenSourceToCancelCommandExecution.Token));
    }
    [Description("Отправка команды в бракёр вручную")]
    public class SendManualCommand : GenericCommandBase<string, bool>
    {
        public SendManualCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing()
        {
            await ((RDPBModule)Module).SendManualCommandProc(InputData);
            SetOutput(true);
        }
    }
    [Description("Отправка информации бракёру, что съем плохой")]
    public class SendSetIsBadCommand : GenericCommandBase<RDPBStatus>
    {
        public SendSetIsBadCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => SetOutput(await((RDPBModule)Module).Send(RDPBCommandType.SetIsBad, CancelationTokenSourceToCancelCommandExecution.Token));
    }
    [Description("Установка количества охлаждающих блоков")]
    public class SetCoolingBlockQuantityCommand : GenericCommandBase<int, RDPBStatus>
    {
        public SetCoolingBlockQuantityCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => SetOutput(await((RDPBModule)Module).Send(RDPBCommandType.SetCoolingBlocks, CancelationTokenSourceToCancelCommandExecution.Token, InputData));
    }
    [Description("Запуск модуля бракёра в работу")]
    public class RDPBStartCommand : GenericCommandBase
    {
        public RDPBStartCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => await ((RDPBModule)Module).Start();
    }
    [Description("Остановка роботы модуля бракёра")]
    public class RDPBStopCommand : GenericCommandBase
    {
        public RDPBStopCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => await ((RDPBModule)Module).Stop();
    }
    [Description("Выключение бракёра")]
    public class TurnOffCommand : GenericCommandBase<RDPBStatus>
    {
        public TurnOffCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => SetOutput(await ((RDPBModule)Module).Send(RDPBCommandType.Off, CancelationTokenSourceToCancelCommandExecution.Token));
    }
    [Description("Включение бракёра")]
    public class TurnOnCommand : GenericCommandBase<RDPBStatus>
    {
        public TurnOnCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => SetOutput(await ((RDPBModule)Module).Send(RDPBCommandType.On, CancelationTokenSourceToCancelCommandExecution.Token));
    }


}
