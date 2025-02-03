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
        public int TransferFrequency=60; //как часто переносится из БД в архив
        public int ArchiveRecordAgeSeconds=600; // Сколько хранится съем в БД прежде чем он будет перенесен в архив
        public int DutyCycleInSeconds=300; //Периодиченость сохранения хороших съемов (скважность оставления хороших съемов)
        public int BeforeAndAfterErrorInSeconds=60; //Сколько по времени оставлять съемов до и после съема с браком

    }
}
