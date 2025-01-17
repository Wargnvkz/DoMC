using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCModuleControl.Commands;
using static DoMCLib.Classes.Module.LCB.LCBModule;

/// <summary>
/// Управление получением данных из платы и передача данных в плату
/// </summary>
namespace DoMCLib.Classes
{
    public abstract class WaitingCommandBase : AbstractCommandBase
    {
        protected bool IsWaitingCompletedSuccessfuly = false;
        public WaitingCommandBase(IMainController mainController, AbstractModuleBase module, Type? InputType, Type? OutputType) : base(mainController, module, InputType, OutputType)
        {
            if (MakeDecisionIsCommandCompleteFunc == null) throw new ArgumentNullException(nameof(MakeDecisionIsCommandCompleteFunc));
        }

        public override object? Wait(int timeoutInSeconds)
        {
            if (HasStopedAlready()) return null;
            bool NoNeedToWaitMore = false;
            try
            {
                Controller.GetObserver().NotificationReceivers += NotificationReceived;
                if (HasNotBeenRunningYet())
                    ExecuteCommand();
                var start = DateTime.Now;
                while (!NoNeedToWaitMore && (DateTime.Now - start).TotalSeconds < timeoutInSeconds && !CancelationTokenSourceToCancelCommandExecution.IsCancellationRequested && !IsError)
                {
                    NoNeedToWaitMore = MakeDecisionIsCommandCompleteFunc();
                    if (NoNeedToWaitMore)
                    {
                        Stop();
                        break;
                    }
                    Task.Delay(10).Wait();
                }
            }
            finally
            {
                Controller.GetObserver().NotificationReceivers -= NotificationReceived;
            }
            PrepareOutputData();
            if (NoNeedToWaitMore)
                return OutputData;
            else
                return null;
        }

        protected abstract void NotificationReceived(string NotificationName, object? data);

        protected abstract bool MakeDecisionIsCommandCompleteFunc();
        protected abstract void PrepareOutputData();

    }

    public abstract class SimpleWaitingCommandBase : WaitingCommandBase
    {
        protected string Operation;
        protected string EventName;
        public SimpleWaitingCommandBase(IMainController mainController, AbstractModuleBase module, Type? InputType, Type? OutputType, string operation, string eventName) : base(mainController, module, InputType, OutputType)
        {
            Operation = operation;
            EventName = eventName;
        }

        protected override bool MakeDecisionIsCommandCompleteFunc()
        {
            return IsWaitingCompletedSuccessfuly;
        }

        protected override void NotificationReceived(string NotificationName, object? data)
        {
            if (ObserverExtention.GetEventName(Module, Operation, EventName) == NotificationName)
            {
                IsWaitingCompletedSuccessfuly = true;
            }
        }

        protected override void PrepareOutputData()
        {

        }
    }

}
