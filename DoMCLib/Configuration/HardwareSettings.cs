using DoMCLib.Classes.Module.Configuration;
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

    public class HardwareSettings
    {
        // 
        //public List<DoMCCardListSetting> CardListSettings;
         
        public int SocketQuantity = 96;

        public bool[] SocketsToCheck;
         
        public DoMCStandardRecalculationSettings WorkModeSettings;

         
        public LCBSettings LCBSettings = new LCBSettings();

         
        public int LCBPort;
         
        public int DoMCPort;

         
        public bool LogPackets;

         
        public RemoveDefectedPreformBlockConfig RemoveDefectedPreformBlockConfig;

         
        public string LocalDataStorageConnectionString;

         
        public string RemoteDataStorageConnectionString;

         
        public TimeoutOfActions Timeouts = new TimeoutOfActions();

         
        public bool[] SocketsToSave;

         
        public SocketsPositions SocketsPositions;

         
        public short AverageToHaveImage = 200;

         
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
