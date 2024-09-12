using DoMCLib.DB;
using DoMCModuleControl;
using DoMCModuleControl.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes.Module.ArchiveDB
{
    public class ArchiveDBConfiguration
    {
        public string LocalDBPath;
        public string ArchiveDBPath;
        public int TransferFrequency;
        public int ArchiveRecordAgeSeconds;

    }
}
