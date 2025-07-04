using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoMCModuleControl.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoMCModuleControl.Modules;
using DoMCTestingTools.ClassesForTests;

namespace DoMCModuleControl.Commands.Tests
{
    [TestClass()]
    public class AbstractCommandBaseTests
    {
        [TestMethod()]
        public void HasNotBeenRunningYetTest()
        {
            var mainController=new MainController(new NoFileSystem());
            var testCmd = new TestCommand(mainController, new TestModule(mainController), null, null);
            Assert.IsTrue(testCmd.HasNotBeenRunningYet());

        }

    }
    public class TestCommand : AbstractCommandBase,IExecuteCommandAsync
    {
        public TestCommand(IMainController mainController, AbstractModuleBase module, Type? inputType = null, Type? outputType = null) : base(mainController, module, inputType, outputType)
        {
        }

        protected override async Task Executing()
        {
            throw new NotImplementedException();
        }
        public new async Task ExecuteCommandAsync()
        {
            await base.ExecuteCommandAsync();
        }
    }
}