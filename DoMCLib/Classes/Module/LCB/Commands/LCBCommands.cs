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
    public class LCBStartCommand : SimpleWaitingCommandBase
    {
        public LCBStartCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, null, LCBModule.Operations.Started.ToString(), LCBModule.EventType.Success.ToString()) { }
        protected override void Executing() => ((LCBModule)Module).Start();
    }
    public class LCBStopCommand : AbstractCommandBase
    {
        public LCBStopCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, null) { }
        protected override void Executing() => ((LCBModule)Module).Stop();
    }


    public class SetLCBCurrentCommand : WaitingCommandBase
    {
        private bool AnswerReceived = false;
        public bool IsSuccessful { get; private set; }

        public SetLCBCurrentCommand(IMainController mainController, AbstractModuleBase module)
            : base(mainController, module, typeof(int), typeof(bool)) { }

        protected override void Executing()
        {
            var module = (LCBModule)Module;
            var current = (int)InputData!;
            module.SetLCBCurrent(current);
        }

        protected override void NotificationReceived(string notificationName, object? data)
        {
            if (notificationName.Contains(LEDCommandType.SetLEDCurrentResponse.ToString()))
            {
                AnswerReceived = true;
                IsSuccessful = (bool)data!;
            }
        }

        protected override bool MakeDecisionIsCommandCompleteFunc()
        {
            return AnswerReceived;
        }

        protected override void PrepareOutputData()
        {
            OutputData = IsSuccessful;
        }
    }

    public class GetLCBCurrentCommand : WaitingCommandBase
    {
        private bool AnswerReceived = false;
        public int Current { get; private set; }

        public GetLCBCurrentCommand(IMainController mainController, AbstractModuleBase module)
            : base(mainController, module, null, typeof(int)) { }

        protected override void Executing()
        {
            var module = (LCBModule)Module;
            module.GetLCBCurrent();
        }

        protected override void NotificationReceived(string notificationName, object? data)
        {
            if (notificationName.Contains(LEDCommandType.GetLEDCurrentResponse.ToString()))
            {
                AnswerReceived = true;
                Current = (int)data!;
            }
        }

        protected override bool MakeDecisionIsCommandCompleteFunc()
        {
            return AnswerReceived;
        }

        protected override void PrepareOutputData()
        {
            OutputData = Current;
        }
    }

    public class SetLCBMovementParametersCommand : WaitingCommandBase
    {
        private bool AnswerReceived = false;
        public bool IsSuccessful { get; private set; }

        public SetLCBMovementParametersCommand(IMainController mainController, AbstractModuleBase module)
            : base(mainController, module, typeof((int PreformLength, int DelayLength)), typeof(bool)) { }

        protected override void Executing()
        {
            var module = (LCBModule)Module;
            var input = ((int PreformLength, int DelayLength))InputData!;
            module.SetLCBMovementParameters(input.PreformLength, input.DelayLength);
        }

        protected override void NotificationReceived(string notificationName, object? data)
        {
            if (notificationName.Contains(LEDCommandType.SetLCBMovementParametersResponse.ToString()))
            {
                AnswerReceived = true;
                IsSuccessful = (bool)data!;
            }
        }

        protected override bool MakeDecisionIsCommandCompleteFunc()
        {
            return AnswerReceived;
        }

        protected override void PrepareOutputData()
        {
            OutputData = IsSuccessful;
        }
    }

    public class GetLCBMovementParametersCommand : WaitingCommandBase
    {
        private bool AnswerReceived = false;
        public LEDMovementParameters Parameters { get; private set; }

        public GetLCBMovementParametersCommand(IMainController mainController, AbstractModuleBase module)
            : base(mainController, module, null, typeof(LEDMovementParameters)) { }

        protected override void Executing()
        {
            var module = (LCBModule)Module;
            module.GetLCBMovementParameters();
        }

        protected override void NotificationReceived(string notificationName, object? data)
        {
            if (notificationName.Contains(LEDCommandType.GetLCBMovementParametersResponse.ToString()))
            {
                AnswerReceived = true;
                Parameters = (LEDMovementParameters)data!;
            }
        }

        protected override bool MakeDecisionIsCommandCompleteFunc()
        {
            return AnswerReceived;
        }

        protected override void PrepareOutputData()
        {
            OutputData = Parameters;
        }
    }

    public class SetLCBEquipmentStatusCommand : WaitingCommandBase
    {
        private bool AnswerReceived = false;
        public bool IsSuccessful { get; private set; }

        public SetLCBEquipmentStatusCommand(IMainController mainController, AbstractModuleBase module)
            : base(mainController, module, typeof(LEDEquimpentStatus), typeof(bool)) { }

        protected override void Executing()
        {
            var module = (LCBModule)Module;
            var status = (LEDEquimpentStatus)InputData!;
            module.SetLCBEquipmentStatus(status);
        }

        protected override void NotificationReceived(string notificationName, object? data)
        {
            if (notificationName.Contains(LEDCommandType.SetLCBEquipmentStatusResponse.ToString()))
            {
                AnswerReceived = true;
                IsSuccessful = (bool)data!;
            }
        }

        protected override bool MakeDecisionIsCommandCompleteFunc()
        {
            return AnswerReceived;
        }

        protected override void PrepareOutputData()
        {
            OutputData = IsSuccessful;
        }
    }

    public class GetLCBEquipmentStatusCommand : WaitingCommandBase
    {
        private bool AnswerReceived = false;
        public LEDEquimpentStatus EquipmentStatus { get; private set; }

        public GetLCBEquipmentStatusCommand(IMainController mainController, AbstractModuleBase module)
            : base(mainController, module, null, typeof(LEDEquimpentStatus)) { }

        protected override void Executing()
        {
            var module = (LCBModule)Module;
            module.GetLCBEquipmentStatus();
        }

        protected override void NotificationReceived(string notificationName, object? data)
        {
            if (notificationName.Contains(LEDCommandType.GetLCBEquipmentStatusResponse.ToString()))
            {
                AnswerReceived = true;
                EquipmentStatus = (LEDEquimpentStatus)data!;
            }
        }

        protected override bool MakeDecisionIsCommandCompleteFunc()
        {
            return AnswerReceived;
        }

        protected override void PrepareOutputData()
        {
            OutputData = EquipmentStatus;
        }
    }

    public class SetLCBWorkModeCommand : WaitingCommandBase
    {
        private bool AnswerReceived = false;
        public bool IsSuccessful { get; private set; }

        public SetLCBWorkModeCommand(IMainController mainController, AbstractModuleBase module)
            : base(mainController, module, null, typeof(bool)) { }

        protected override void Executing()
        {
            var module = (LCBModule)Module;
            module.SetLCBWorkMode();
        }

        protected override void NotificationReceived(string notificationName, object? data)
        {
            if (notificationName.Contains(LEDCommandType.SetLCBWorkModeResponse.ToString()))
            {
                AnswerReceived = true;
                IsSuccessful = (bool)data!;
            }
        }

        protected override bool MakeDecisionIsCommandCompleteFunc()
        {
            return AnswerReceived;
        }

        protected override void PrepareOutputData()
        {
            OutputData = IsSuccessful;
        }
    }

    public class SetLCBNonWorkModeCommand : WaitingCommandBase
    {
        private bool AnswerReceived = false;
        public bool IsSuccessful { get; private set; }

        public SetLCBNonWorkModeCommand(IMainController mainController, AbstractModuleBase module)
            : base(mainController, module, null, typeof(bool)) { }

        protected override void Executing()
        {
            var module = (LCBModule)Module;
            module.SetLCBNonWorkMode();
        }

        protected override void NotificationReceived(string notificationName, object? data)
        {
            if (notificationName.Contains(LEDCommandType.SetLCBWorkModeResponse.ToString()))
            {
                AnswerReceived = true;
                IsSuccessful = (bool)data!;
            }
        }

        protected override bool MakeDecisionIsCommandCompleteFunc()
        {
            return AnswerReceived;
        }

        protected override void PrepareOutputData()
        {
            OutputData = IsSuccessful;
        }
    }

    public class GetLCBMaxPositionCommand : WaitingCommandBase
    {
        private bool AnswerReceived = false;
        public int MaxPosition { get; private set; }

        public GetLCBMaxPositionCommand(IMainController mainController, AbstractModuleBase module)
            : base(mainController, module, null, typeof(int)) { }

        protected override void Executing()
        {
            var module = (LCBModule)Module;
            module.GetLCBMaxPosition();
        }

        protected override void NotificationReceived(string notificationName, object? data)
        {
            if (notificationName.Contains(LEDCommandType.GetLCBMaxHorizontalStrokeResponse.ToString()))
            {
                AnswerReceived = true;
                MaxPosition = (int)data!;
            }
        }

        protected override bool MakeDecisionIsCommandCompleteFunc()
        {
            return AnswerReceived;
        }

        protected override void PrepareOutputData()
        {
            OutputData = MaxPosition;
        }
    }

    public class GetLCBCurrentPositionCommand : WaitingCommandBase
    {
        private bool AnswerReceived = false;
        public int CurrentPosition { get; private set; }

        public GetLCBCurrentPositionCommand(IMainController mainController, AbstractModuleBase module)
            : base(mainController, module, null, typeof(int)) { }

        protected override void Executing()
        {
            var module = (LCBModule)Module;
            module.GetLCBCurrentPosition();
        }

        protected override void NotificationReceived(string notificationName, object? data)
        {
            if (notificationName.Contains(LEDCommandType.GetLCBCurrentHorizontalStrokeResponse.ToString()))
            {
                AnswerReceived = true;
                CurrentPosition = (int)data!;
            }
        }

        protected override bool MakeDecisionIsCommandCompleteFunc()
        {
            return AnswerReceived;
        }

        protected override void PrepareOutputData()
        {
            OutputData = CurrentPosition;
        }
    }

}
