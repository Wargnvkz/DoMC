using DoMCLib.Tools;
namespace DoMCLib.Classes.Module.CCD.CCDCardDataExchangeCommandClasses
{
    public class CCDCardArrayResponse9
    {
        [BinaryConverter.Byte]
        public byte Address;
        [BinaryConverter.Byte]
        public byte Command;
        [BinaryConverter.UInt16]
        public ushort X;
        [BinaryConverter.UInt16]
        public ushort Y;
        [BinaryConverter.OneDimensionalArray(typeof(short), 512)]
        public short[] Data;
    }
}
