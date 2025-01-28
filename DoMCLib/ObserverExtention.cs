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
        public static void Notify(this Observer observer, AbstractModuleBase module, string Operation, string eventType, object? data)
        {
            observer.Notify(GetEventName(module, Operation, eventType), data);
        }
        public static string GetEventName(AbstractModuleBase module, string Operation, string eventType)
        {
            return $"{module?.GetType().Name ?? ""}.{Operation}.{eventType}";
        }
    }
}
