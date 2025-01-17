using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoMCLib.Classes.Configuration.CCD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoMCLib.Configuration;
using DoMCTestingTools.ClassesForTests;

namespace DoMCLib.Classes.Configuration.CCD.Tests
{
    [TestClass()]
    public class CycleImagesCCDTests
    {
        [TestMethod()]
        public void SetLEDStatusesTest()
        {
            var ciCCD = new CycleImagesCCD();
            var rnd = new Random();
            var ledqnt = CycleImagesCCD.DefaultLEDQnt;
            bool[] newLedStatuses = new bool[ledqnt];
            for (int i = 0; i < ledqnt; i++)
            {
                newLedStatuses[i] = rnd.NextDouble() >= 0.5;
            }
            var datetime = DateTime.Now.AddSeconds((rnd.NextDouble() - 0.5) * 1000);
            ciCCD.SetLEDStatuses(newLedStatuses, datetime);
            Assert.AreEqual(datetime, ciCCD.TimeLCBSyncSignalGot);
            Assert.AreNotSame(newLedStatuses, ciCCD.LEDStatuses);
            CollectionAssert.AreEqual(newLedStatuses, ciCCD.LEDStatuses);
            Assert.ThrowsException<ArgumentNullException>(() => ciCCD.SetLEDStatuses(null, DateTime.MinValue));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ciCCD.SetLEDStatuses(newLedStatuses, DateTime.MinValue));

        }

        [TestMethod()]
        public void SetImageProcessParametersTest()
        {
            var ciCCD = new CycleImagesCCD();
            var rnd = new Random();
            var sockets = 96;
            ImageProcessParameters[] newImageProcessingParameters = new ImageProcessParameters[sockets];
            for (int i = 0; i < sockets; i++)
            {
                var ipp = new ImageProcessParameters() { BottomBorder = rnd.Next(512), LeftBorder = rnd.Next(512), RightBorder = rnd.Next(512), TopBorder = rnd.Next(512), };
                newImageProcessingParameters[i] = ipp;
            }
            ciCCD.SetImageProcessParameters(newImageProcessingParameters);
            Assert.AreNotSame(newImageProcessingParameters, ciCCD.ImageProcessParameters);
            CollectionAssert.AreEqual(newImageProcessingParameters, ciCCD.ImageProcessParameters, new ImageProcessParametersComparer());
        }

    }
}