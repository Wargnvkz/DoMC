using DoMCLib.Tools;
namespace DoMCLib.Classes.Module.CCD.CCDCardDataExchangeCommands
{
    public class CCDCardArrayResponse9PosOnly
    {
        [BinaryConverter.Byte]
        public byte Address;
        [BinaryConverter.Byte]
        public byte Command;
        [BinaryConverter.UInt16]
        public ushort X;
        [BinaryConverter.UInt16]
        public ushort Y;
        [BinaryConverter.OneDimensionalArray(typeof(ushort), 512)]
        public ushort[] Data;
    }
}
