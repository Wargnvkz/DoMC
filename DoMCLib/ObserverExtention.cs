using DoMCModuleControl;
using DoMCModuleControl.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib
{
    public static class ObserverExtention
    {
        public static void Notify(this Observer observer, ModuleBase module, string Operation, string eventType, object? data)
        {
            observer.Notify($"{module}.{Operation}.{eventType}", data);
        }
    }
}
