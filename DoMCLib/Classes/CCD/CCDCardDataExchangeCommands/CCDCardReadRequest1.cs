namespace DoMCLib.Classes.CCD.CCDCardDataExchangeCommands
{
    public class CCDCardReadRequest1
    {
        [BinaryConverter.Byte]
        public byte Address = 1;
        [BinaryConverter.Byte]
        public byte Command = 1;
    }
}
