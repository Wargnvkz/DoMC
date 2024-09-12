using DoMCLib.Tools;
namespace DoMCLib.Classes.Module.CCD.CCDCardDataExchangeCommands
{
    public class CCDCardFrameParamsRequest4
    {
        [BinaryConverter.Byte]
        public byte Address = 1;
        [BinaryConverter.Byte]
        public byte Command = 4;
        [BinaryConverter.UInt16]
        public ushort FrameDuration = 1300;
        [BinaryConverter.UInt16]
        public ushort Exposition = 400;
    }
}
