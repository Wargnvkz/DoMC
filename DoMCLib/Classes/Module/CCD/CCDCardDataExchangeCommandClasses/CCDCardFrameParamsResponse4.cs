using DoMCLib.Tools;
namespace DoMCLib.Classes.Module.CCD.CCDCardDataExchangeCommandClasses
{
    public class CCDCardFrameParamsResponse4
    {
        [BinaryConverter.Byte]
        public byte Address = 1;
        [BinaryConverter.Byte]
        public byte Command = 4;
        [BinaryConverter.OneDimensionalArray(typeof(byte), 84)]
        public byte[] Data;
    }
}
