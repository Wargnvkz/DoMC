using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoMCModuleControl.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoMCModuleControl.External.Tests;
using DoMCModuleControlTests.External;
using DoMCModuleControlTests.ClassesForTests;

namespace DoMCModuleControl.Logging.Tests
{
    [TestClass()]
    public class BaseFilesLoggerTests
    {
        [TestMethod()]
        public void BaseFilesLoggerTest()
        {
            var fileSystem = new FileSystemForTests();
            new BaseFilesLogger(fileSystem);
            Assert.ThrowsException<ArgumentNullException>(() => new BaseFilesLogger(null));
        }

        [TestMethod()]
        public void AddMessageTest()
        {
            var fileSystem = new FileSystemForTests();
            var baseLogger = new BaseFilesLogger(fileSystem);
            var ModuleName = "TestModule";
            var MessageText = "Text Message";

            baseLogger.AddMessage(ModuleName, MessageText);
            baseLogger.Flush(ModuleName);
            var files = fileSystem.GetFiles(null);
            Assert.AreEqual(1, files.Length);
            var filename = files[0];
            using (var sr = fileSystem.GetStreamReader(filename))
            {
                var text = sr.ReadToEnd().Trim();
                var lines = text.Split("\r\n");
                Assert.AreEqual(1, lines.Length);
                Assert.IsTrue(lines[0].Contains(MessageText));
            }
        }

        [TestMethod()]
        public void DisposeTest()
        {
            var fileSystem = new FileSystemForTests();
            var baseLogger = new BaseFilesLogger(fileSystem);
            baseLogger.Dispose();
            baseLogger.Dispose();
        }

        [TestMethod()]
        public void RegisterExternalLoggerTest()
        {
            var ModuleName = "Test";
            var Message = "TestMessage";
            var baseFileLogger = new BaseFilesLogger(new ErrorFileSystem());
            baseFileLogger.RegisterExternalLogger(null);
            baseFileLogger.AddMessage(ModuleName, Message);
            baseFileLogger.Flush(ModuleName);
            var externallogger = new ExternalTestLogger();
            baseFileLogger.RegisterExternalLogger(externallogger);
            baseFileLogger.AddMessage(ModuleName, Message);
            baseFileLogger.Flush(ModuleName);
            Assert.AreNotEqual(null, externallogger.LastMessage);
            Assert.AreEqual(LoggerLevel.Critical, externallogger.LastMessageLevel);
        }
    }
}