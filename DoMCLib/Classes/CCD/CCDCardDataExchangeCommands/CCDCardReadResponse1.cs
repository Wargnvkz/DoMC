namespace DoMCLib.Classes.CCD.CCDCardDataExchangeCommands
{
    public class CCDCardReadResponse1
    {
        [BinaryConverter.Byte]
        public byte Address = 1;
        [BinaryConverter.Byte]
        public byte Command = 1;
        [BinaryConverter.OneDimensionalArray(typeof(byte), 84)]
        public byte[] Data;
    }
}
