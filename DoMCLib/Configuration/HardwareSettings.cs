using DoMCLib.Classes.Configuration;
using DoMCLib.Classes.Module.LCB;
using DoMCLib.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DoMCLib.Configuration
{
    /// <summary>
    /// Параметры оборудования, связи, распределения гнезд по платам. То, что не зависит от выпускаемой преформы. Эти параметры почти никогда не меняются для одного набора оборудования.
    /// </summary>
    public class HardwareSettings
    {
        // 
        //public List<DoMCCardListSetting> CardListSettings;

        public int SocketQuantity = 96;

        public bool[] SocketsToCheck;

        public DoMCStandardRecalculationSettings WorkModeSettings;

        public int DoMCPort;


        public bool LogPackets;


        public RemoveDefectedPreformBlockConfig RemoveDefectedPreformBlockConfig;


        public string LocalDataStoragePath;


        public string RemoteDataStoragePath;


        public TimeoutOfActions Timeouts = new TimeoutOfActions();


        public bool[] SocketsToSave;


        public SocketsPositions SocketsPositions;


        public short ThresholdAverageToHaveImage = 200;


        public bool RegisterEmptyImages = false;

        public bool LoggingDBRequests = false;

    }


    public class SocketDoMCConfiguration
    {

        public int SocketQuantity = 96;


        public bool[] SocketStatus;


        public bool[] LEDOn;


        public int[] SocketToRead;

    }



}
