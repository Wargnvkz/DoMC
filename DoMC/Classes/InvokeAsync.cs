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
                var tcs = new TaskCompletionSource<bool>();
                control.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        action();
                        tcs.SetResult(true);
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
    }
}
