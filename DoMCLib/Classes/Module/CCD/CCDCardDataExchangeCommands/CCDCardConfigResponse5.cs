using DoMCLib.Tools;
namespace DoMCLib.Classes.Module.CCD.CCDCardDataExchangeCommands
{
    public class CCDCardConfigResponse5
    {
        [BinaryConverter.Byte]
        public byte Address = 1;
        [BinaryConverter.Byte]
        public byte Command = 5;
        [BinaryConverter.OneDimensionalArray(typeof(byte), 84)]
        public byte[] Data;
    }
}
