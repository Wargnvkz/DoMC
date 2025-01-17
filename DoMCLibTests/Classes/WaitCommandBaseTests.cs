using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoMCLib.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoMCModuleControl;
using DoMCModuleControl.Modules;
using DoMCTestingTools.ClassesForTests;

namespace DoMCLib.Classes.Tests
{
    [TestClass()]
    public class WaitCommandBaseTests
    {
        [TestMethod()]
        public void WaitCommandBaseTest()
        {
            var filesystem = new NoFileSystem();
            var mainController = new MainController(filesystem);
            mainController.CreateUserInterface(typeof(TestUserInterface));
            var module = new TestModule(mainController);
            new TestWaitingCommand(mainController, module);
        }

        [TestMethod()]
        public void WaitTest()
        {
            var filesystem = new NoFileSystem();
            var mainController = new MainController(filesystem);
            mainController.CreateUserInterface(typeof(TestUserInterface));
            var module = new TestModule(mainController);
            var command = new TestWaitingCommand(mainController, module);
            var result = command.Wait(1);
            Assert.AreEqual(true, result);
        }

    }

    public class TestWaitingCommand : WaitingCommandBase
    {
        string NotificationName = "WaitingCommandTest";
        int? DataGot;
        bool NotificationRecieved = false;
        public TestWaitingCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, typeof(bool))
        {

        }

        protected override void Executing()
        {
            Controller.GetObserver().Notify(NotificationName, 1);
        }

        protected override bool MakeDecisionIsCommandCompleteFunc()
        {
            return NotificationRecieved;
        }

        protected override void NotificationReceived(string NotificationName, object? data)
        {
            if (NotificationName == this.NotificationName)
            {
                if (data is int idata)
                {
                    DataGot = idata;
                    NotificationRecieved = true;
                }
            }
        }

        protected override void PrepareOutputData()
        {
            OutputData = DataGot.HasValue ? DataGot == 1 : false;
        }
    }
}