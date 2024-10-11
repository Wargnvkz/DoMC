using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCModuleControl.Commands;

/// <summary>
/// Управление получением данных из платы и передача данных в плату
/// </summary>
namespace DoMCLib.Classes
{
    public abstract class WaitCommandBase : CommandBase
    {
        public WaitCommandBase(IMainController mainController, ModuleBase module, Type? InputType, Type? OutputType) : base(mainController, module, InputType, OutputType)
        {
            if (MakeDecisionIsCommandCompleteFunc == null) throw new ArgumentNullException(nameof(MakeDecisionIsCommandCompleteFunc));

        }

        public override object? Wait(int timeoutInSeconds)
        {
            if (IsCompleteSuccessfully || IsError) return null;
            bool NoNeedToWaitMore = false;
            try
            {
                Controller.GetObserver().NotificationReceivers += NotificationReceived;
                if (!IsRunning && !IsCompleteSuccessfully && !IsError)
                    ExecuteCommand();
                var start = DateTime.Now;
                while (!IsCompleteSuccessfully && !IsError && (DateTime.Now - start).TotalSeconds < timeoutInSeconds)
                {
                    NoNeedToWaitMore = MakeDecisionIsCommandCompleteFunc();
                    if (NoNeedToWaitMore)
                        break;
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

}
