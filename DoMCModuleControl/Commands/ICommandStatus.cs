using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl.Commands
{
    public interface ICommandStatus
    {
        bool IsRunning { get; }
        bool IsCompleteSuccessfully { get; }
        bool IsCanceled { get; }
        bool IsError { get; }
        object? OutputData { get; }
        Exception? Error { get; }
    }
}
