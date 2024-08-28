using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl.Configuration
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class CommandModuleAttribute : Attribute
    {
        public Type ModuleType { get; }

        public CommandModuleAttribute(Type moduleType)
        {
            ModuleType = moduleType;
        }
    }

}
