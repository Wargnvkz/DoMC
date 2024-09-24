using DoMCLib.Tools;
namespace DoMCLib.Classes.Module.CCD.CCDCardDataExchangeCommandClasses
{
    public class CCDCardDataResponse7
    {
        [BinaryConverter.Byte]
        public byte Address = 1;
        [BinaryConverter.Byte]
        public byte Command = 7;
        [BinaryConverter.Byte]
        public byte X;
        [BinaryConverter.Byte]
        public byte Y;
        [BinaryConverter.OneDimensionalArray(typeof(byte), 82)]
        public byte[] Data;
    }
}
