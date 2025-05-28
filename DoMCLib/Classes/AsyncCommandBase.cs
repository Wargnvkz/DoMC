using DoMCModuleControl.Commands;
using DoMCModuleControl.Modules;
using DoMCModuleControl;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes
{
    public abstract class AsyncCommandBase : AbstractCommandBase
    {
        private TaskCompletionSource<object?> _completionSource;
        protected CancellationToken Token;

        public AsyncCommandBase(IMainController controller, AbstractModuleBase module, Type? inputType, Type? outputType)
            : base(controller, module, inputType, outputType)
        { }

        public async Task<object?> ExecuteAsync(object? inputData, CancellationToken cancellationToken)
        {
            InputData = inputData;
            Token = cancellationToken;
            _completionSource = new TaskCompletionSource<object?>(TaskCreationOptions.RunContinuationsAsynchronously);

            Controller.GetObserver().NotificationReceivers += NotificationReceived;

            try
            {
                ExecuteCommand(); // Запускает выполнение, как раньше
                using (cancellationToken.Register(() => _completionSource.TrySetCanceled()))
                {
                    return await _completionSource.Task;
                }
            }
            finally
            {
                Controller.GetObserver().NotificationReceivers -= NotificationReceived;
            }
        }

        protected void CompleteCommand()
        {
            PrepareOutputData();
            _completionSource.TrySetResult(OutputData);
        }

        protected void FailCommand(Exception ex)
        {
            _completionSource.TrySetException(ex);
        }

        protected abstract void NotificationReceived(string NotificationName, object? data);
        protected abstract void PrepareOutputData();
    }

}
