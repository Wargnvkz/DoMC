using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl.Factories.ProcessState
{
    public abstract class ProcessStateFactory
    {
        public abstract ProcessState CreateProcessState();
    }
}
