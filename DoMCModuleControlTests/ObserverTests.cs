using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoMCModuleControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoMCModuleControl.Logging;
using DoMCModuleControlTests.External;

namespace DoMCModuleControl.Tests
{
    [TestClass()]
    public class ObserverTests
    {
        [TestMethod()]
        public void ObserverTest()
        {
            var fileSystem = new FileSystemForTests();
            var baseLogger = new BaseFilesLogger(fileSystem);
            var logger = new Logger("Test", baseLogger);
            logger.SetMaxLogginLevel(LoggerLevel.FullDetailedInformation);
            var observer = new Observer(logger);

            bool wasNotified = false;  // Флаг для фиксации вызова события
            string expectedEventName = "TestEvent";
            object? expectedEventData = null;
            int counter = 0;
            observer.NotificationReceivers += (eventName, eventData) =>
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
            double timeoutInSeconds = 1;
            while (counter == 0 && (DateTime.Now - start).TotalSeconds < timeoutInSeconds)
            {
                Task.Delay(10).Wait();
            }
            // Assert
            Assert.AreEqual(1, counter);
            logger.Flush();
            var files = fileSystem.GetFiles(null);
            Assert.AreNotEqual(files.Length, 0);
            var logText = String.Join("\r\n\r\n", files.Select(file => fileSystem.GetStreamReader(file).ReadToEnd()));
            Assert.AreNotEqual(logText.Trim().Length, 0);
        }
    }
}