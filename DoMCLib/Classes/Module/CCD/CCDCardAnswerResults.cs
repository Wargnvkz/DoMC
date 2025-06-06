using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes.Module.CCD
{
    public class CCDCardAnswerResults
    {
        public int CardNumber;
        public int ReadingSocketsTime;
        // 0 - успех, 1 - ошибка
        public int ReadingSocketsResult;
        public bool CommandSucceed;
    }
}
