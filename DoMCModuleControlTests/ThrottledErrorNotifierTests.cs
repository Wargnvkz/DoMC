using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoMCModuleControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoMCModuleControl.Logging;
using System.Threading;
using DoMCTestingTools.ClassesForTests;

namespace DoMCModuleControl.Tests
{
    [TestClass()]
    public class ThrottledErrorNotifierTests
    {
        [TestMethod()]
        public void ThrottledErrorNotifierTest()
        {
            var ModuleName = LoggerTestTool.GetLoggerTestModuleName();
            var fileSystem = new FileSystemForTests();
            var baseLogger = new BaseFilesLogger(fileSystem);
            var logger = new Logger(ModuleName, baseLogger);
            logger.SetMaxLogginLevel(LoggerLevel.FullDetailedInformation);
            var observer = new Observer(logger);
            new ThrottledErrorNotifier(observer, 10, 10);
            new ThrottledErrorNotifier(observer, -1, -1);
            Assert.ThrowsException<ArgumentNullException>(() => new ThrottledErrorNotifier(null, -1, -1));
        }

        [TestMethod()]
        public void SendErrorTest()
        {
            var ModuleName = LoggerTestTool.GetLoggerTestModuleName();
            int throttleTime = 4;
            int throttleCounter = 10;
            var fileSystem = new FileSystemForTests();
            var baseLogger = new BaseFilesLogger(fileSystem);
            var logger = new Logger(ModuleName, baseLogger);
            logger.SetMaxLogginLevel(LoggerLevel.FullDetailedInformation);
            var observer = new Observer(logger);
            var throttledErrorNotifier = new ThrottledErrorNotifier(observer, throttleTime, throttleCounter);

            string expectedEventName = "TestEvent";
            Exception expectedEventData = new AccessViolationException();
            int counter = 0;
            observer.NotificationReceivers += async (eventName, eventData) =>
            {
                // Проверяем, что событие вызвано с правильными аргументами
                if (eventName == expectedEventName && eventData == expectedEventData)
                {
                    counter++;
                }
            };
            var start = DateTime.Now;
            throttledErrorNotifier.SendError(expectedEventName, expectedEventData);
            throttledErrorNotifier.SendError(expectedEventName, expectedEventData);
            double timeoutInSeconds = 30;
            while (counter == 0 && (DateTime.Now - start).TotalSeconds < timeoutInSeconds)
            {
                Task.Delay(10).Wait();
            }
            Assert.AreEqual(1, counter);
            int runCounter = 0;
            while ((DateTime.Now - start).TotalSeconds < throttleTime)
            {
                if (runCounter < throttleCounter - 1)
                {
                    throttledErrorNotifier.SendError(expectedEventName, expectedEventData);
                }
                runCounter++;
            }
            Assert.AreEqual(1, counter);
            throttledErrorNotifier.SendError(expectedEventName, expectedEventData);
            start = DateTime.Now;
            while (counter == 1 && (DateTime.Now - start).TotalSeconds < timeoutInSeconds)
            {
                Task.Delay(10).Wait();
            }
            Assert.AreEqual(2, counter);
            throttledErrorNotifier.SendError(expectedEventName, expectedEventData);
            start = DateTime.Now;
            while (counter == 2 && (DateTime.Now - start).TotalSeconds < timeoutInSeconds)
            {
                Task.Delay(10).Wait();
            }
            Assert.AreEqual(2, counter);

        }
    }
}