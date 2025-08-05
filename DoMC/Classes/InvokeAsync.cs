using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMC.Classes
{
    public static class ControlExtensions
    {
        public static Task InvokeAsync(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                var tcs = new TaskCompletionSource();
                control.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        action();
                        tcs.SetResult();
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }
                }));
                return tcs.Task;
            }
            else
            {
                action();
                return Task.CompletedTask;
            }
        }
        public static Task InvokeAsync(this Control control, Func<Task> asyncAction)
        {
            if (control.InvokeRequired)
            {
                var tcs = new TaskCompletionSource();

                control.BeginInvoke(new Action(async () =>
                {
                    try
                    {
                        await asyncAction();
                        tcs.SetResult();
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }
                }));

                return tcs.Task;
            }
            else
            {
                return asyncAction();
            }
        }
        public static Task<TResult> InvokeAsync<TResult>(this Control control, Func<Task<TResult>> asyncAction)
        {
            if (control.InvokeRequired)
            {
                var tcs = new TaskCompletionSource<TResult>();

                control.BeginInvoke(new Action(async () =>
                {
                    try
                    {
                        TResult result = await asyncAction();
                        tcs.SetResult(result);
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }
                }));

                return tcs.Task;
            }
            else
            {
                return asyncAction();
            }
        }
    }
}
