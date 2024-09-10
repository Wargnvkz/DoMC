using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoMCLib.Classes.Model.RDPB
{
    public enum RDPBCommandType
    {
        SetIsOK = 0x3830,
        SetIsBad = 0x3831,
        On = 0x3832,
        Off = 0x3833,
        MakeBlockSendWorkingState = 0x3930,
        SetCoolingBlocks = 0x4130,

    }
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
    public enum RDPBTransporterSide
    {
        NotSet = 0,
        Stoped = 0x30,
        Right = 0x31,
        Left = 0x32,
        SensorError = 0x33
    }
    public enum RDPBErrors
    {
        NoErrors = 0x30,
        TransporterDriveUnit = 0x31,
        SensorOfInitialState = 0x32
    }
    public enum BoxDirectionType
    {
        Right,
        Left,
        Unknown
    }

}
