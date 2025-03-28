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
        public void RegisterCommandTest()
        {
            var controller = MainController.Create(null);
            Assert.IsNotNull(controller);
            var module = new TestModule(controller);
            controller.RegisterModule(module);
            controller.RegisterCommand(new CommandInfo("TestCommand", typeof(TestInputData), typeof(TestOutputData), typeof(TestModule.TestCommand), module));
            Assert.ThrowsException<ArgumentNullException>(() => controller.RegisterCommand(null));
            Assert.ThrowsException<ArgumentNullException>(() => controller.RegisterCommand(new CommandInfo(null, null, null, null, null)));
            Assert.ThrowsException<ArgumentNullException>(() => controller.RegisterCommand(new CommandInfo("TestNull", null, null, null, null)));
            Assert.ThrowsException<ArgumentNullException>(() => controller.RegisterCommand(new CommandInfo("TestNull", null, null, typeof(TestModule.TestCommand), null)));
            controller.RegisterCommand(new CommandInfo("TestNull", null, null, typeof(TestModule.TestCommand), module));
            Assert.ThrowsException<ArgumentNullException>(() => controller.RegisterCommand(new CommandInfo("TestNull", null, null, null, module)));
            Assert.ThrowsException<ArgumentNullException>(() => controller.RegisterCommand(new CommandInfo("TestNull", typeof(TestInputData), null, null, module)));
            Assert.ThrowsException<ArgumentNullException>(() => controller.RegisterCommand(new CommandInfo("TestNull", null, typeof(TestOutputData), null, module)));
            Assert.ThrowsException<ArgumentNullException>(() => controller.RegisterCommand(new CommandInfo("TestNull", typeof(TestInputData), typeof(TestOutputData), null, module)));
            Assert.ThrowsException<ArgumentNullException>(() => controller.RegisterCommand(new CommandInfo("TestNull", typeof(TestInputData), null, typeof(TestModule.TestCommand), null)));
            Assert.ThrowsException<ArgumentNullException>(() => controller.RegisterCommand(new CommandInfo("TestNull", null, typeof(TestOutputData), typeof(TestModule.TestCommand), null)));
            Assert.ThrowsException<ArgumentNullException>(() => controller.RegisterCommand(new CommandInfo("TestNull", typeof(TestInputData), typeof(TestOutputData), typeof(TestModule.TestCommand), null)));
            var commands = controller.GetRegisteredCommandList();
            Assert.IsNotNull(commands.Find(c => c.CommandName == "TestCommand"));
        }

        [TestMethod()]
        public void RegisterAllCommandsTest()
        {
            var controller = MainController.Create(null);
            Assert.IsNotNull(controller);
            controller.RegisterAllCommands();
            var commands = controller.GetRegisteredCommandList();
            Assert.IsNotNull(commands.Find(c => c.CommandName == "TestModule.TestCommand"));
        }

        [TestMethod()]
        public void GetRegisteredCommandListTest()
        {
            var controller = MainController.Create(null);
            Assert.IsNotNull(controller);
            var module = new TestModule(controller);
            controller.RegisterModule(module);
            controller.RegisterCommand(new CommandInfo("TestCommand", typeof(TestInputData), typeof(TestOutputData), typeof(TestModule.TestCommand), module));
            var commands = controller.GetRegisteredCommandList();
            Assert.IsNotNull(commands.Find(c => c.CommandName == "TestCommand"));
        }

        [TestMethod("CreateCommandInstance(string commandName)")]
        public void CreateCommandTest()
        {
            var controller = MainController.Create(null);
            Assert.IsNotNull(controller);
            var module = new TestModule(controller);
            controller.RegisterModule(module);
            controller.RegisterCommand(new CommandInfo("TestCommand", typeof(TestInputData), typeof(TestOutputData), typeof(TestModule.TestCommand), module));
            var command = controller.CreateCommandInstance("TestCommand");
            Assert.IsNotNull(command);
            Assert.ThrowsException<ArgumentNullException>(() => controller.CreateCommandInstance((string)null));
        }

        [TestMethod("CreateCommandInstance(Type commandType)")]
        public void CreateCommandTest1()
        {
            var controller = MainController.Create(null);
            Assert.IsNotNull(controller);
            var module = new TestModule(controller);
            controller.RegisterModule(module);
            controller.RegisterCommand(new CommandInfo("TestCommand", typeof(TestInputData), typeof(TestOutputData), typeof(TestModule.TestCommand), module));
            var command = controller.CreateCommandInstance(typeof(TestModule.TestCommand));
            Assert.IsNotNull(command);
            Assert.ThrowsException<ArgumentNullException>(() => controller.CreateCommandInstance((Type)null));
        }

        [TestMethod("CreateCommandInstance(Type commandType, Type moduleType)")]
        public void CreateCommandTest2()
        {
            var controller = MainController.Create(null);
            Assert.IsNotNull(controller);
            var module = new TestModule(controller);
            controller.RegisterModule(module);
            controller.RegisterCommand(new CommandInfo("TestCommand", typeof(TestInputData), typeof(TestOutputData), typeof(TestModule.TestCommand), module));
            var command = controller.CreateCommandInstance(typeof(TestModule.TestCommand), module.GetType());
            Assert.IsNotNull(command);
            Assert.ThrowsException<ArgumentNullException>(() => controller.CreateCommandInstance(null, module.GetType()));
            Assert.ThrowsException<ArgumentNullException>(() => controller.CreateCommandInstance(typeof(TestModule.TestCommand), null));
            Assert.ThrowsException<ArgumentNullException>(() => controller.CreateCommandInstance(null, null));
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