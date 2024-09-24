using DoMCLib.Tools;
namespace DoMCLib.Classes.Module.CCD.CCDCardDataExchangeCommandClasses
{
    public class CCDCardConfigRequestB
    {
        [BinaryConverter.Byte]
        public byte Address = 1;
        [BinaryConverter.Byte]
        public byte Command = 11;
        [BinaryConverter.UInt16]
        public ushort FilterModule = 5;
        [BinaryConverter.UInt16]
        public ushort Threshold = 20;
        [BinaryConverter.UInt16]
        public CCDCardConfigRequestBMode Mode = CCDCardConfigRequestBMode.Filtered;
        [BinaryConverter.UInt16]
        public ushort First = 10;
        [BinaryConverter.UInt16]
        public ushort End = 450;
        [BinaryConverter.UInt16]
        public ushort FirstMask = 140;
        [BinaryConverter.UInt16]
        public ushort EndMask = 160;
        [BinaryConverter.UInt16]
        public ushort MeasureDelay = 160;
        /*[BinaryConverter.OneDimensionalArray(typeof(short), 512)]
        public short[] Data;*/
    }
}
