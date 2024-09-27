using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes.Module.CCD.Commands.Classes
{
    public class CCDCardDataCommandResponse
    {
        public bool[] answered = new bool[12];
        public bool[] requested = new bool[12];
        public void Clear()
        {
            Array.Fill(answered, false);
            Array.Fill(requested, false);
        }

        public void SetCardRequested(int i) => requested[i] = true;
        public void SetCardAnswered(int i) => answered[i] = true;
        public List<int> CardsNotAnswered()
        {
            return Enumerable.Range(0, 12).Where(i => requested[i] && !answered[i]).ToList();
        }
    }
}
