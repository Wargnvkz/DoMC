namespace DoMCLib.Classes.CCD.CCDCardDataExchangeCommands
{
    public class CCDCardArrayRequest9
    {
        [BinaryConverter.Byte]
        public byte Address = 1;
        [BinaryConverter.Byte]
        public byte Command = 9;
        [BinaryConverter.UInt16]
        public ushort X = 0;
        [BinaryConverter.UInt16]
        public ushort Y = 0;
    }
}
