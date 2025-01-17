using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes.Module.LCB
{
    public class LCBWorkingParameters
    {
        public int WaitForLCBCardAnswerTimeoutInSeconds;
        //public IPAddress LCBInterfaceAddress;

        public LCBWorkingParameters Clone()
        {
            var result = new LCBWorkingParameters();
            result.WaitForLCBCardAnswerTimeoutInSeconds = WaitForLCBCardAnswerTimeoutInSeconds;
            //result.LCBInterfaceAddress = new IPAddress(LCBInterfaceAddress.GetAddressBytes(), LCBInterfaceAddress.ScopeId);
            return result;
        }
    }
}
