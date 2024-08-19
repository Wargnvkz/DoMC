using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl
{
    public interface IModule : IDisposable
    {
        public void Initialize();
        public void Shutdown();
        public void Start();
        public void Stop();
        public ModuleStatus GetStatus();
    }
}
