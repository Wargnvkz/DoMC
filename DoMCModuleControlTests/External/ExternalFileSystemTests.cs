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
    public class ExternalFileSystemTests
    {
        [TestMethod()]
        public void CreateDirectoryTest()
        {
            var nfs = new ExternalFileSystem();
            nfs.CreateDirectory("1");
            Assert.ThrowsException<ArgumentException>(() => nfs.CreateDirectory(""));
            Assert.ThrowsException<ArgumentNullException>(() => nfs.CreateDirectory(null));
        }

        [TestMethod()]
        public void DeleteDirectoryTest()
        {
            var nfs = new ExternalFileSystem();
            nfs.DeleteDirectory("", true);
            nfs.DeleteDirectory("", false);
            nfs.DeleteDirectory(null, true);
            nfs.DeleteDirectory(null, false);
        }

        [TestMethod()]
        public void DeleteFileTest()
        {
            var nfs = new ExternalFileSystem();
            nfs.DeleteFile("");
            nfs.DeleteFile(null);
        }

        [TestMethod()]
        public void GetDirectoryNameTest()
        {
            var nfs = new ExternalFileSystem();
            Assert.AreEqual("c:\\", nfs.GetDirectoryName("c:\\test.file"));
        }

        [TestMethod()]
        public void GetFilesTest()
        {
            var nfs = new ExternalFileSystem();
            nfs.GetFiles("C:\\");
            nfs.GetFiles(null);
        }

        [TestMethod()]
        public void GetStreamReaderTest()
        {
            var nfs = new ExternalFileSystem();
            nfs.GetStreamReader("C:\\");
            nfs.GetStreamReader(null);
        }

        [TestMethod()]
        public void GetStreamWriterTest()
        {
            var nfs = new ExternalFileSystem();
            nfs.GetStreamWriter("C:\\", true);
            nfs.GetStreamWriter(null, false);
        }

        [TestMethod()]
        public void IsDirectoryExistsTest()
        {
            var nfs = new ExternalFileSystem();
            nfs.IsDirectoryExists("C:\\");
            nfs.IsDirectoryExists(null);
        }

        [TestMethod()]
        public void IsFileExistsTest()
        {
            var nfs = new ExternalFileSystem();
            nfs.IsFileExists("C:\\");
            nfs.IsFileExists(null);
        }

        [TestMethod()]
        public void PathCombineTest()
        {
            var nfs = new ExternalFileSystem();
            nfs.PathCombine("1", "2", "3");
            Assert.ThrowsException<ArgumentNullException>(() => nfs.PathCombine("1", null, "2"));
        }
    }
}