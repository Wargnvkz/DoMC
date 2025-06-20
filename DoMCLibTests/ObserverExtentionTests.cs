using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoMCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoMCModuleControl.Logging;
using DoMCModuleControl;
using DoMCTestingTools.ClassesForTests;
using static DoMCLib.Classes.Module.LCB.LCBModule;

namespace DoMCLib.Tests
{
    [TestClass()]
    public class ObserverExtentionTests
    {
        [TestMethod()]
        public void NotifyTest()
        {
            var ModuleName = LoggerTestTool.GetLoggerTestModuleName();
            var fileSystem = new FileSystemForTests();
            var controller = new MainController(new NoFileSystem());
            controller.CreateUserInterface(typeof(TestUserInterface));

            var module = new TestModule(controller);

            bool wasNotified = false;  // Флаг для фиксации вызова события
            var ModuleObserverName = module.GetType().Name;
            var OperationName = "TestOperation";
            string expectedEventName = "TestEvent";
            object? expectedEventData = new object();
            int counter = 0;
            controller.GetObserver().NotificationReceivers += async (eventName, eventData) =>
            {
                // Проверяем, что событие вызвано с правильными аргументами
                if (eventName == $"{ModuleObserverName}.{OperationName}.{expectedEventName}" && eventData == expectedEventData)
                {
                    wasNotified = true;
                }
                counter++;
            };
            // Act
            controller.GetObserver().Notify(module, OperationName, expectedEventName, expectedEventData);
            var start = DateTime.Now;
            double timeoutInSeconds = 30;
            while (counter == 0 && (DateTime.Now - start).TotalSeconds < timeoutInSeconds)
            {
                Task.Delay(10).Wait();
            }
            // Assert
            Assert.AreEqual(1, counter);
        }
    }
}