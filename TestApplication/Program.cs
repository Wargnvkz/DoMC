using DoMCModuleControl;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Modules;
using System;
using System.Net;
using System.Runtime.InteropServices;

namespace TestApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var c = new cmd();
            var ct = c.GetType();
            var attrs=ct.GetCustomAttributes(false);
            if (attrs.Length > 0)
            {
                if (attrs[0] is CommandModuleTypeAttribute attr)
                {
                    Console.WriteLine(attr.ModuleType.Name);
                }
            }
        }
    }
    [CommandModuleType(null)]
    public class cmd
    {

    }
    [CommandModuleType(typeof(TestModule))]
    public class cmd1
    {

    }
    public class TestModule : AbstractModuleBase
    {
        public TestModule(IMainController MainController) : base(MainController)
        {
        }
    }
}
