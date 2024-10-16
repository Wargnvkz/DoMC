using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoMCModuleControl.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl.External.Tests
{
    [TestClass()]
    public class FileSystemTests
    {
        [TestMethod()]
        public void CreateDirectoryTest()
        {
            FileSystem fileSystem = new FileSystem();
            var tmp = Path.GetTempFileName();
            if (File.Exists(tmp)) File.Delete(tmp);
            var di = fileSystem.CreateDirectory(tmp);
            Assert.IsTrue(di.Exists);
            Assert.IsTrue(Directory.Exists(tmp));
            Directory.Delete(tmp);
        }

        [TestMethod()]
        public void DeleteDirectoryTest()
        {
            FileSystem fileSystem = new FileSystem();
            var tmp = Path.GetTempFileName();
            if (File.Exists(tmp)) File.Delete(tmp);
            var di = fileSystem.CreateDirectory(tmp);
            Assert.IsTrue(di.Exists);
            Assert.IsTrue(Directory.Exists(tmp));
            fileSystem.DeleteDirectory(tmp, false);
            fileSystem.CreateDirectory(tmp);
            fileSystem.CreateDirectory(Path.Combine(tmp, "1"));
            fileSystem.DeleteDirectory(tmp, true);
            if (Directory.Exists(tmp))
            {
                Directory.Delete(tmp);
                Assert.Fail("Directory hasn't been deleted");
            }
        }

        [TestMethod()]
        public void DeleteFileTest()
        {
            FileSystem fileSystem = new FileSystem();
            var tmp = Path.GetTempFileName();
            if (!File.Exists(tmp))
            {
                File.Create(tmp).Close();
            }
            if (File.Exists(tmp)) fileSystem.DeleteFile(tmp);
            if (File.Exists(tmp))
            {
                File.Delete(tmp);
                Assert.Fail("File hasn't been deleted");
            }
        }

        [TestMethod()]
        public void GetFilesTest()
        {
            FileSystem fileSystem = new FileSystem();
            var systemRoot = Environment.GetEnvironmentVariable("SystemRoot");
            var files = fileSystem.GetFiles(Environment.GetEnvironmentVariable("SystemRoot") ?? Environment.GetEnvironmentVariable("windir") ?? "C:\\Windows");
            Assert.IsNotNull(files);
            Assert.IsTrue(files.Where(f => f.ToLower().Contains("system.ini")).Count() > 0);
            Assert.ThrowsException<ArgumentNullException>(() => fileSystem.GetFiles(null));

        }

        [TestMethod()]
        public void GetStreamReaderTest()
        {
            FileSystem fileSystem = new FileSystem();
            var tmp = Path.GetTempFileName();
            try
            {
                if (!File.Exists(tmp))
                {
                    File.Create(tmp).Close();
                }
                using (var sr = fileSystem.GetStreamReader(tmp))
                {
                    var text = sr.ReadToEnd();
                    Assert.IsNotNull(text);
                    Assert.AreEqual(0, text.Length);
                }
                Assert.ThrowsException<ArgumentNullException>(() => fileSystem.GetStreamReader(null));
            }
            finally
            {
                if (File.Exists(tmp))
                {
                    File.Delete(tmp);
                }
            }
        }

        [TestMethod()]
        public void GetStreamWriterTest()
        {
            string text = "Test";
            FileSystem fileSystem = new FileSystem();
            var tmp = Path.GetTempFileName();
            try
            {
                if (!File.Exists(tmp))
                {
                    File.Create(tmp).Close();
                }
                using (var sw = fileSystem.GetStreamWriter(tmp, true))
                {
                    sw.Write(text);
                }
                using (var sr = new StreamReader(tmp, true))
                {
                    var fileText = sr.ReadToEnd();
                    Assert.IsTrue(fileText == text);
                }
                Assert.ThrowsException<ArgumentNullException>(() => fileSystem.GetStreamWriter(null, false));
            }
            finally
            {
                if (File.Exists(tmp))
                {
                    File.Delete(tmp);
                }
            }
        }

        [TestMethod()]
        public void IsDirectoryExistsTest()
        {
            FileSystem fileSystem = new FileSystem();
            var tmp = Path.GetTempFileName();
            if (File.Exists(tmp)) File.Delete(tmp);
            var di = fileSystem.CreateDirectory(tmp);
            Assert.IsTrue(di.Exists);
            Assert.IsTrue(Directory.Exists(tmp));
            Assert.IsTrue(fileSystem.IsDirectoryExists(tmp));
            fileSystem.IsDirectoryExists(null);
            if (Directory.Exists(tmp))
            {
                Directory.Delete(tmp);
            }
        }

        [TestMethod()]
        public void IsFileExistsTest()
        {
            FileSystem fileSystem = new FileSystem();
            var tmp = Path.GetTempFileName();
            if (!File.Exists(tmp))
            {
                File.Create(tmp).Close();
            }
            if (File.Exists(tmp))
            {
                if (!fileSystem.IsFileExists(tmp))
                {
                    File.Delete(tmp);
                    Assert.Fail("IsFileExists doen't work properly");
                }
            }
        }
    }
}