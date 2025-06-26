using DoMCLib.Classes.Configuration;
using DoMCLib.Classes.Module.RDPB.Classes;
using DoMCLib.Configuration;
using DoMCModuleControl;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Modules;

namespace DoMCLib.Classes.Module.RDPB.Commands
{
    public class SendSetIsOkCommand : GenericCommandBase<RDPBStatus>
    {
        public SendSetIsOkCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => await ((RDPBModule)Module).Send(RDPBCommandType.SetIsOK, CancelationTokenSourceToCancelCommandExecution.Token);
    }

    public class SendConfigurationToModuleCommand : GenericCommandBase<ApplicationConfiguration, bool>
    {
        public SendConfigurationToModuleCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing()
        {
            await ((RDPBModule)Module).SetConfig(InputData);
            SetOutput(true);
        }
    }

    public class GetParametersCommand : GenericCommandBase<RDPBStatus>
    {
        public GetParametersCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => await ((RDPBModule)Module).Send(RDPBCommandType.GetParameters, CancelationTokenSourceToCancelCommandExecution.Token);
    }

    public class SendManualCommand : GenericCommandBase<string, bool>
    {
        public SendManualCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => ((RDPBModule)Module).SendManualCommandProc(InputData);
    }

    public class SendSetIsBadCommand : GenericCommandBase<RDPBStatus>
    {
        public SendSetIsBadCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => await ((RDPBModule)Module).Send(RDPBCommandType.SetIsBad, CancelationTokenSourceToCancelCommandExecution.Token);
    }

    public class SetCoolingBlockQuantityCommand : GenericCommandBase<int, RDPBStatus>
    {
        public SetCoolingBlockQuantityCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => await ((RDPBModule)Module).Send(RDPBCommandType.SetCoolingBlocks, CancelationTokenSourceToCancelCommandExecution.Token, InputData);
    }

    public class RDPBStartCommand : GenericCommandBase
    {
        public RDPBStartCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => await ((RDPBModule)Module).Start();
    }

    public class RDPBStopCommand : GenericCommandBase
    {
        public RDPBStopCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => await ((RDPBModule)Module).Stop();
    }

    public class TurnOffCommand : GenericCommandBase<RDPBStatus>
    {
        public TurnOffCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => SetOutput(await ((RDPBModule)Module).Send(RDPBCommandType.Off, CancelationTokenSourceToCancelCommandExecution.Token));
    }

    public class TurnOnCommand : GenericCommandBase<RDPBStatus>
    {
        public TurnOnCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => SetOutput(await ((RDPBModule)Module).Send(RDPBCommandType.On, CancelationTokenSourceToCancelCommandExecution.Token));
    }


}
