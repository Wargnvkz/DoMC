using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoMCModuleControl.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoMCTestingTools.ClassesForTests;


namespace DoMCModuleControl.Logging.Tests
{
    [TestClass()]
    public class BaseSystemLoggerTests
    {
        [TestMethod()]
        public void AddMessageTest()
        {
            var fileSystem = new FileSystemForTests();
            var baseSystemLogger = new BaseSystemLogger(fileSystem);
            baseSystemLogger.AddMessage("Test", "Test");
            var files = fileSystem.GetFiles(null);
            Assert.AreEqual(1, files.Length);
            Assert.AreEqual(1, fileSystem.GetStreamReader(files[0]).ReadToEnd().Trim().Split("\r\n").Length);
        }

        [TestMethod()]
        public void RegisterExternalLoggerTest()
        {
            var baseSystemLogger = new BaseSystemLogger(new ErrorFileSystem());
            var externallogger = new ExternalTestLogger();
            baseSystemLogger.RegisterExternalLogger(externallogger);
            baseSystemLogger.AddMessage("Test", "Test");
            Assert.AreNotEqual(null, externallogger.LastMessage);
            Assert.AreEqual(LoggerLevel.Critical, externallogger.LastMessageLevel);

        }
    }
}