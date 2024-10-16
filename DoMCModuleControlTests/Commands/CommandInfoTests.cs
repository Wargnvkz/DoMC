using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoMCModuleControl.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoMCModuleControlTests.ClassesForTests;
using static DoMCModuleControl.Commands.Tests.CommandModuleTypeAttributeTests;

namespace DoMCModuleControl.Commands.Tests
{
    [TestClass()]
    public class CommandInfoTests
    {
        [TestMethod()]
        public void CommandInfoTest()
        {
            var controller = new MainController(typeof(TestUserInterface), new NoFileSystem());
            var module = new TestModule(controller);

            var cmd = new TestCommand(controller, module);
            var nullcmd = new NullTestCommand(controller, module);
            cmd.GetType().GetCustomAttributes(false);
            nullcmd.GetType().GetCustomAttributes(false);
            new CommandInfo("test", null, null, typeof(TestModule.TestCommand), module);
            new CommandInfo(null, null, null, typeof(TestModule.TestCommand), module);
            new CommandInfo(null, typeof(int), null, typeof(TestModule.TestCommand), module);
            new CommandInfo(null, null, typeof(int), typeof(TestModule.TestCommand), module);
            new CommandInfo(null, typeof(int), typeof(int), typeof(TestModule.TestCommand), module);
            Assert.ThrowsException<ArgumentNullException>(() => new CommandInfo(null, null, null, null, module));
            Assert.ThrowsException<ArgumentNullException>(() => new CommandInfo(null, null, null, typeof(TestModule.TestCommand), null));
            Assert.ThrowsException<ArgumentNullException>(() => new CommandInfo("test", null, null, null, module));
            Assert.ThrowsException<ArgumentNullException>(() => new CommandInfo("test", null, null, typeof(TestModule.TestCommand), null));
            Assert.ThrowsException<ArgumentNullException>(() => new CommandInfo("test", null, null, null, null));
            Assert.ThrowsException<ArgumentNullException>(() => new CommandInfo(null, null, null, null, null));
        }

        [TestMethod()]
        public void CloneTest()
        {
            var controller = new MainController(typeof(TestUserInterface), new NoFileSystem());
            var module = new TestModule(controller);

            var cmd = new TestCommand(controller, module);
            var nullcmd = new NullTestCommand(controller, module);
            cmd.GetType().GetCustomAttributes(false);
            nullcmd.GetType().GetCustomAttributes(false);
            var ci = new CommandInfo("test", null, null, typeof(TestModule.TestCommand), module);
            var clone = ci.Clone();
            Assert.IsNotNull(clone);
            Assert.AreEqual(ci.CommandName, clone.CommandName);
            Assert.AreEqual(ci.InputType, clone.InputType);
            Assert.AreEqual(ci.OutputType, clone.OutputType);
            Assert.AreEqual(ci.CommandClass, clone.CommandClass);
            Assert.AreEqual(ci.Module, clone.Module);
        }

    }
}