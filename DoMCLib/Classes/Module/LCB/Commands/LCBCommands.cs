using DoMCModuleControl.Modules;
using DoMCModuleControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoMCModuleControl.Commands;
using Microsoft.AspNetCore.Components.Forms;

namespace DoMCLib.Classes.Module.LCB.Commands
{
    public class LCBStartCommand : AbstractCommandBase
    {
        public LCBStartCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, null) { }
        protected override async Task Executing() => ((LCBModule)Module).Start();
    }
    public class LCBStopCommand : AbstractCommandBase
    {
        public LCBStopCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, null) { }
        protected override async Task Executing() => ((LCBModule)Module).Stop();
    }

    public class SetLCBCurrentCommand : GenericCommandBase<int, bool>
    {
        public SetLCBCurrentCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => SetOutput(await ((LCBModule)Module).SetLCBCurrent(InputData));
    }

    public class GetLCBCurrentCommand : GenericCommandBase<int>
    {
        public GetLCBCurrentCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => SetOutput(await ((LCBModule)Module).GetLCBCurrent());
    }

    public class SetLCBMovementParametersCommand : GenericCommandBase<(int PreformLength, int DelayLength), bool>
    {
        public SetLCBMovementParametersCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => SetOutput(await ((LCBModule)Module).SetLCBMovementParameters(InputData.PreformLength, InputData.DelayLength));
    }

    public class GetLCBMovementParametersCommand : GenericCommandBase<LEDMovementParameters>
    {
        public GetLCBMovementParametersCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => SetOutput(await ((LCBModule)Module).GetLCBMovementParameters());
    }
    public class SetLCBEquipmentStatusCommand : GenericCommandBase<LEDMovementParameters, bool>
    {
        public SetLCBEquipmentStatusCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => SetOutput(await ((LCBModule)Module).SetLCBEquipmentStatus(InputData));
    }
    public class GetLCBEquipmentStatusCommand : GenericCommandBase<LEDEquimpentStatus>
    {
        public GetLCBEquipmentStatusCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => SetOutput(await ((LCBModule)Module).GetLCBEquipmentStatus());
    }
    public class SetLCBWorkModeCommand : GenericCommandBase
    {
        public SetLCBWorkModeCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => await ((LCBModule)Module).SetLCBWorkMode();
    }
    public class SetLCBNonWorkModeCommand : GenericCommandBase
    {
        public SetLCBNonWorkModeCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => await ((LCBModule)Module).SetLCBNonWorkMode();
    }
    public class GetLCBMaxPositionCommand : GenericCommandBase<int>
    {
        public GetLCBMaxPositionCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => SetOutput(await ((LCBModule)Module).GetLCBMaxPosition());
    }
    public class GetLCBCurrentPositionCommand : GenericCommandBase<int>
    {
        public GetLCBCurrentPositionCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module) { }
        protected override async Task Executing() => SetOutput(await ((LCBModule)Module).GetLCBCurrentPosition());
    }

}
