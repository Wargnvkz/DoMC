using DoMCModuleControl;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Modules;

namespace DoMCLib.Classes.Model.RDPB
{
    public partial class RDPBModule
    {
        /*

        RDPBOn,
        RDPBOff,
        RDPBGetParameters,
        RDPBSetCoolingBlockQuantity,
        RDPBSendManualCommand,
         */
        public class LoadConfigurationCommand : CommandBase
        {
            public LoadConfigurationCommand(IMainController mainController, ModuleBase module) : base(mainController, module, typeof(RemoveDefectedPreformBlockConfig), null) { }
            protected override void Executing() => ((RDPBModule)Module).config = (Configuration.RemoveDefectedPreformBlockConfig)InputData;
        }

        public class StartCommand : CommandBase
        {
            public StartCommand(IMainController mainController, ModuleBase module) : base(mainController, module, null, null) { }
            protected override void Executing() => ((RDPBModule)Module).Start();

        }
        public class StopCommand : CommandBase
        {
            public StopCommand(IMainController mainController, ModuleBase module) : base(mainController, module, null, null) { }

            protected override void Executing() => ((RDPBModule)Module).Stop();

        }

        public class SendSetIsOkCommand : CommandBase
        {
            public SendSetIsOkCommand(IMainController mainController, ModuleBase module) : base(mainController, module, null, null) { }

            protected override void Executing() => ((RDPBModule)Module).Send(RDPBCommandType.SetIsOK);

        }

        public class SendSetIsBadCommand : CommandBase
        {
            public SendSetIsBadCommand(IMainController mainController, ModuleBase module) : base(mainController, module, null, null) { }

            protected override void Executing() => ((RDPBModule)Module).Send(RDPBCommandType.SetIsBad);

        }
        public class TurnOnCommand : CommandBase
        {
            public TurnOnCommand(IMainController mainController, ModuleBase module) : base(mainController, module, null, null) { }

            protected override void Executing() => ((RDPBModule)Module).Send(RDPBCommandType.On);

        }
        public class TurnOffCommand : CommandBase
        {
            public TurnOffCommand(IMainController mainController, ModuleBase module) : base(mainController, module, null, null) { }

            protected override void Executing() => ((RDPBModule)Module).Send(RDPBCommandType.Off);

        }
        public class MakeBlockSendWorkingStateCommand : CommandBase
        {
            public MakeBlockSendWorkingStateCommand(IMainController mainController, ModuleBase module) : base(mainController, module, null, null) { }

            protected override void Executing() => ((RDPBModule)Module).Send(RDPBCommandType.MakeBlockSendWorkingState);

        }
        public class SetCoolingBlockQuantityCommand : CommandBase
        {
            public SetCoolingBlockQuantityCommand(IMainController mainController, ModuleBase module) : base(mainController, module, typeof(int), null) { }

            protected override void Executing() => ((RDPBModule)Module).Send(RDPBCommandType.SetCoolingBlocks, (int)(InputData ?? 4));

        }
        public class SendManualCommand : CommandBase
        {
            public SendManualCommand(IMainController mainController, ModuleBase module) : base(mainController, module, typeof(string), null) { }

            protected override void Executing() => ((RDPBModule)Module).SendManualCommandProc((string)(InputData ?? String.Empty));

        }

    }
}
