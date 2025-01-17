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

        [TestMethod()]
        public void HasStopedAlreadyTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void WasCompletedSuccessfullyTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AbstractCommandBaseTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void NotificationProcedureTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ExecuteCommandTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ExecuteCommandTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ExecuteCommandAsyncTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ExecuteCommandAsyncTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetInputDataTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetOutputDataTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void WaitTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void WaitTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CancelTest()
        {
            Assert.Fail();
        }
    }
    public class TestCommand : AbstractCommandBase
    {
        public TestCommand(IMainController mainController, AbstractModuleBase module, Type? inputType = null, Type? outputType = null) : base(mainController, module, inputType, outputType)
        {
        }

        protected override void Executing()
        {
            throw new NotImplementedException();
        }
    }
}