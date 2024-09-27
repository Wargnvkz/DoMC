using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes.Module.CCD.Commands.Classes
{
    public class SetReadingParametersCommandResult : CCDCardDataCommandResponse
    {
        public int[] AnswerTime = new int[12];
        public bool[] ReadResult = new bool[12];
        public void SetAnswerTime(int i, int time) => AnswerTime[i] = time;
        public void SetReadResult(int i, bool result) => ReadResult[i] = result;
    }
}
