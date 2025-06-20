using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoMCModuleControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoMCModuleControl.Commands;
using System.Diagnostics.CodeAnalysis;
using DoMCTestingTools.ClassesForTests;

namespace DoMCModuleControl.Tests
{
    [TestClass()]
    public class MainControllerTests
    {
        [TestMethod()]
        public void MainControllerTest()
        {
            var controller = new MainController();
            controller.CreateUserInterface(typeof(TestUserInterface));
            Assert.IsNotNull(controller);
            Assert.IsNotNull(controller.GetLogger("Test"));
            Assert.IsNotNull(controller.GetObserver());
            Assert.IsNotNull(controller.GetMainUserInterface());
            Assert.ThrowsException<ArgumentNullException>(() => new MainController(null));
        }

        [TestMethod()]
        public void CreateTest()
        {
            var controller = MainController.Create(null);
            Assert.IsNotNull(controller);
            Assert.IsNotNull(controller.GetLogger("Test"));
            Assert.IsNotNull(controller.GetObserver());
            Assert.IsNotNull(controller.GetMainUserInterface());
        }

        [TestMethod()]
        public void RegisterModuleTest()
        {
            var controller = MainController.Create(null);
            Assert.IsNotNull(controller);
            var module = new TestModule(controller);
            controller.RegisterModule(module);
            Assert.IsNotNull(controller.GetModule(typeof(TestModule)) as TestModule);
        }

        [TestMethod()]
        public void LoadDLLModulesTest()
        {
            var controller = MainController.LoadDLLModulesAndRegisterCommands();
            Assert.IsNotNull(controller);
            Assert.IsNotNull(controller.GetModule(typeof(TestModule)) as TestModule);
        }

        [TestMethod()]
        public void FindAndRegisterAllModulesTest()
        {
            var controller = MainController.LoadDLLModulesAndRegisterCommands();
            Assert.IsNotNull(controller);
            Assert.IsNotNull(controller.GetModule(typeof(TestModule)) as TestModule);
        }

       

        [TestMethod()]
        public void GetObserverTest()
        {
            var controller = new MainController();
            controller.CreateUserInterface(typeof(TestUserInterface));
            Assert.IsNotNull(controller);
            Assert.IsNotNull(controller.GetObserver());
        }

        [TestMethod()]
        public void GetLoggerTest()
        {
            var controller = new MainController();
            controller.CreateUserInterface(typeof(TestUserInterface));
            Assert.IsNotNull(controller);
            Assert.IsNotNull(controller.GetLogger("Test"));
        }

        [TestMethod()]
        public void GetMainUserInterfaceTest()
        {
            var controller = new MainController();
            controller.CreateUserInterface(typeof(TestUserInterface));
            Assert.IsNotNull(controller);
            Assert.IsNotNull(controller.GetMainUserInterface());
        }

        [TestMethod("GetModule(string ModuleName)")]
        public void GetModuleTest()
        {
            var controller = MainController.Create(null);
            Assert.IsNotNull(controller);
            var module = new TestModule(controller);
            controller.RegisterModule(module);
            Assert.IsNotNull(controller.GetModule(module.GetType().Name) as TestModule);
            Assert.IsNull(controller.GetModule((string)null));
        }

        [TestMethod("GetModule(Type ModuleType)")]
        public void GetModuleTest1()
        {
            var controller = MainController.Create(null);
            Assert.IsNotNull(controller);
            var module = new TestModule(controller);
            controller.RegisterModule(module);
            Assert.IsNotNull(controller.GetModule(module.GetType()) as TestModule);
            Assert.IsNull(controller.GetModule((Type)null));
        }
    }
}