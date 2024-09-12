using DoMCLib.Tools;
namespace DoMCLib.Classes.Module.CCD.CCDCardDataExchangeCommands
{
    public class CCDCardReadRequest1
    {
        [BinaryConverter.Byte]
        public byte Address = 1;
        [BinaryConverter.Byte]
        public byte Command = 1;
    }
}
