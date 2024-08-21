using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Factories.ProcessState
{
    public class ProcessStateFactory
    {
        public ProcessState CreateProcessState() => new ProcessState();
    }
}
