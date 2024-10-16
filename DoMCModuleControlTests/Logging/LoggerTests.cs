using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoMCModuleControl.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoMCModuleControlTests.External;

namespace DoMCModuleControl.Logging.Tests
{
    [TestClass()]
    public class LoggerTests
    {
        [TestMethod()]
        public void LoggerTest()
        {
            var fileSystem = new FileSystemForTests();
            var baseLogger = new BaseFilesLogger(fileSystem);
            new Logger("Test", baseLogger);
            Assert.ThrowsException<ArgumentNullException>(() => new Logger(null, baseLogger));
            Assert.ThrowsException<ArgumentNullException>(() => new Logger("Test", null));
            Assert.ThrowsException<ArgumentNullException>(() => new Logger(null, null));
        }

        [TestMethod("public void Add(LoggerLevel level, string Message)")]
        public void AddTest()
        {
            var fileSystem = new FileSystemForTests();
            var baseLogger = new BaseFilesLogger(fileSystem);
            var logger = new Logger("Test", baseLogger);
            logger.SetMaxLogginLevel(LoggerLevel.FullDetailedInformation);
            logger.Add(LoggerLevel.FullDetailedInformation, "TestMessage");
            logger.Flush();
            var files = fileSystem.GetFiles(null);
            Assert.AreEqual(1, files.Length);
            Assert.AreEqual(1, fileSystem.GetStreamReader(files[0]).ReadToEnd().Trim().Split("\r\n").Length);
            logger.Add(LoggerLevel.FullDetailedInformation, null);
            logger.Flush();
            Assert.AreEqual(1, fileSystem.GetStreamReader(files[0]).ReadToEnd().Trim().Split("\r\n").Length);
            logger.Add((LoggerLevel)int.MaxValue, "TestMessage");
            logger.Flush();
            Assert.AreEqual(1, fileSystem.GetStreamReader(files[0]).ReadToEnd().Trim().Split("\r\n").Length);

        }

        [TestMethod("public void Add(LoggerLevel level, string Message, Exception ex)")]
        public void AddWithExceptionTest()
        {
            var fileSystem = new FileSystemForTests();
            var baseLogger = new BaseFilesLogger(fileSystem);
            var logger = new Logger("Test", baseLogger);
            logger.SetMaxLogginLevel(LoggerLevel.FullDetailedInformation);
            logger.Add(LoggerLevel.FullDetailedInformation, "TestMessage", new Exception());
            logger.Flush();
            var files = fileSystem.GetFiles(null);
            Assert.AreEqual(1, files.Length);
            Assert.AreEqual(1, fileSystem.GetStreamReader(files[0]).ReadToEnd().Trim().Split("\r\n").Length);
            logger.Add(LoggerLevel.FullDetailedInformation, null, new Exception());
            logger.Flush();
            Assert.AreEqual(2, fileSystem.GetStreamReader(files[0]).ReadToEnd().Trim().Split("\r\n").Length);
            Assert.ThrowsException<ArgumentNullException>(() => logger.Add(LoggerLevel.FullDetailedInformation, null, null));
            logger.Flush();
            Assert.AreEqual(2, fileSystem.GetStreamReader(files[0]).ReadToEnd().Trim().Split("\r\n").Length);
            logger.Add((LoggerLevel)int.MaxValue, "TestMessage", new Exception());
            logger.Flush();
            Assert.AreEqual(2, fileSystem.GetStreamReader(files[0]).ReadToEnd().Trim().Split("\r\n").Length);
        }

        [TestMethod()]
        public void SetMaxLogginLevelTest()
        {
            var fileSystem = new FileSystemForTests();
            var baseLogger = new BaseFilesLogger(fileSystem);
            var logger = new Logger("Test", baseLogger);
            logger.SetMaxLogginLevel(LoggerLevel.FullDetailedInformation);
            logger.SetMaxLogginLevel((LoggerLevel)int.MaxValue);
        }
    }
}