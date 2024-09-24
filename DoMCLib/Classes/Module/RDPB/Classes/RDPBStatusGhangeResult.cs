using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoMCLib.Classes.Module.RDPB.Classes
{
    public enum RDPBStatusGhangeResult
    {
        WrongFormat = 1,
        CRCError = 2,
        SetIsOK = 0x3830,
        SetIsBad = 0x3831,
        On = 0x3832,
        Off = 0x3833,
        MakeBlockSendWorkingState = 0x3930,
        SetCoolingBlocks = 0x4130,

    }

}
