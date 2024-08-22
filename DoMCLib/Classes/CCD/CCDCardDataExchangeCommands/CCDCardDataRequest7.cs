namespace DoMCLib.Classes.CCD.CCDCardDataExchangeCommands
{
    public class CCDCardDataRequest7
    {
        [BinaryConverter.Byte]
        public byte Address = 1;
        [BinaryConverter.Byte]
        public byte Command = 7;
    }
}
