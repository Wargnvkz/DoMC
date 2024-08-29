#pragma warning disable IDE0290
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl.Configuration
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class CommandModuleTypeAttribute : Attribute
    {
        public Type ModuleType { get; }

        public CommandModuleTypeAttribute(Type moduleType)
        {
            ModuleType = moduleType;
        }
    }

}
