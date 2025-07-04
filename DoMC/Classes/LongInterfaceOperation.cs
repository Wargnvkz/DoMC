using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DoMC.Classes
{
    public class LongInterfaceOperation
    {
        public bool IsRunning { get; private set; } = false;
        public bool Failed { get; private set; } = false;
        public async Task StartOperation(Task cmd, Action OnSuccess, Action<Exception> OnError, Action Finally)
        {
            IsRunning = true;
            Failed = false;
            try
            {
                await cmd;
                OnSuccess?.Invoke();
            }
            catch (Exception e)
            {
                Failed = true;
                OnError?.Invoke(e);
            }
            finally { IsRunning = false; Finally?.Invoke(); }

        }
    }
}
