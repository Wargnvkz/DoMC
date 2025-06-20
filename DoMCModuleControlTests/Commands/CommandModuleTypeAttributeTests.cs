using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoMCModuleControl.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoMCModuleControl.Modules;
using DoMCModuleControl.External;
using DoMCTestingTools.ClassesForTests;

namespace DoMCModuleControl.Commands.Tests
{
    [TestClass()]
    public class CommandModuleTypeAttributeTests
    {
        [TestMethod()]
        public void CommandModuleTypeAttributeTest()
        {
            new CommandModuleTypeAttribute(typeof(TestModule));
            new CommandModuleTypeAttribute(null);
            var controller = new MainController(new NoFileSystem());
            controller.CreateUserInterface(typeof(TestUserInterface));
            var module = new TestModule(controller);

            var cmd = new TestCommand(controller, module);
            var nullcmd = new NullTestCommand(controller, module);
            cmd.GetType().GetCustomAttributes(false);
            nullcmd.GetType().GetCustomAttributes(false);
        }



        [CommandModuleType(typeof(TestModule))]
        internal class TestCommand : AbstractCommandBase
        {
            public TestCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, null)
            {
            }

            protected override async Task Executing()
            {

            }
        }
        [CommandModuleType(typeof(TestModule))]
        internal class NullTestCommand : AbstractCommandBase
        {
            public NullTestCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, null)
            {
            }

            protected override async Task Executing()
            {

            }
        }
    }
}