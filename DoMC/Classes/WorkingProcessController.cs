using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMC.Classes
{
    public class PollingController
    {
        private Control _control;
        private readonly Func<Task<bool>> _onStart;
        private readonly Func<CancellationToken, Task<WorkingStatus>> _pollingFunc;
        private readonly Func<WorkingStatus, Task> _onStop;

        private CancellationTokenSource _cts;
        private Task _pollingTask;

        public bool IsRunning => _pollingTask != null && !_pollingTask.IsCompleted;

        public PollingController(
            Control control,
            Func<Task<bool>> onStart,
            Func<CancellationToken, Task<WorkingStatus>> pollingFunc,
            Func<WorkingStatus, Task> onStop)
        {
            _control = control;
            _onStart = onStart;
            _pollingFunc = pollingFunc;
            _onStop = onStop;
        }

        public async Task<bool> StartAsync()
        {
            if (IsRunning)
                return false;
            if (!await _control.InvokeAsync(_onStart)) return false;
            _cts = new CancellationTokenSource();

            _pollingTask = Task.Run(async () =>
            {
                WorkingStatus status = WorkingStatus.Completed;

                try
                {
                    status = await _pollingFunc(_cts.Token);
                }
                catch (OperationCanceledException)
                {
                    status = WorkingStatus.Canceled;
                }
                catch (Exception ex)
                {
                    // Можно логировать
                    status = WorkingStatus.Error;
                }

                if (_onStop != null)
                {
                    if (_control.InvokeRequired)
                    {
                        await _control.InvokeAsync(() => _onStop(status));
                    }
                    else
                    {
                        await _onStop(status);
                    }
                }
            });

            await Task.Yield();
            return true;
        }

        public async Task StopAsync()
        {
            if (!IsRunning)
                return;

            _cts.Cancel();
            await _pollingTask;
            _pollingTask = null;
            _cts.Dispose();
            _cts = null;
        }
    }

    public enum WorkingStatus
    {
        Completed,
        Canceled,
        Error,
    }
}
