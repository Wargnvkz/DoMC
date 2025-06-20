using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoMCModuleControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoMCModuleControl.Logging;
using DoMCTestingTools.ClassesForTests;

namespace DoMCModuleControlTests
{
    [TestClass()]
    public class ObserverTests
    {
        [TestMethod()]
        public void ObserverTest()
        {
            var ModuleName = LoggerTestTool.GetLoggerTestModuleName();
            var fileSystem = new FileSystemForTests();
            var baseLogger = new BaseFilesLogger(fileSystem);
            var logger = new Logger(ModuleName, baseLogger);
            logger.SetMaxLogginLevel(LoggerLevel.FullDetailedInformation);
            var observer = new Observer(logger);
            bool wasNotified = false;
            string expectedEventName = "TestEvent";
            object? expectedEventData = null;
            int counter = 0;
            observer.NotificationReceivers += async (eventName, eventData) =>
            {
                // Проверяем, что событие вызвано с правильными аргументами
                if (eventName == expectedEventName && eventData == expectedEventData)
                {
                    wasNotified = true;
                }
                counter++;
            };
            // Act
            observer.Notify(expectedEventName, expectedEventData);
            var start = DateTime.Now;
            double timeoutInSeconds = 30;
            while (counter == 0 && (DateTime.Now - start).TotalSeconds < timeoutInSeconds)
            {
                Task.Delay(10).Wait();
            }
            // Assert
            Assert.AreEqual(1, counter);
            Assert.IsTrue(wasNotified);
            logger.Flush();

        }
    }
}