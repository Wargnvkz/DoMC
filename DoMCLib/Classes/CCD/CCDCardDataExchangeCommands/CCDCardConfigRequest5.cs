namespace DoMCLib.Classes.CCD.CCDCardDataExchangeCommands
{
    public class CCDCardConfigRequest5
    {
        [BinaryConverter.Byte]
        public byte Address = 1;
        [BinaryConverter.Byte]
        public byte Command = 5;
        [BinaryConverter.UInt16]
        public ushort Config = 4; //бит 0 - внешний старт, 1 - нет выдачи готовности без запроса, 2 - сброс флага готовности
        public static CCDCardConfigRequest5 GetConfiguration(bool ResetReady, bool AnswerWithoutRequest, bool ExternalStart, bool FastRead)
        {
            //бит 0 - внешний старт, 1 - нет выдачи готовности без запроса, 2 - сброс флага готовности
            var cfg = new CCDCardConfigRequest5() { Config = (byte)((FastRead ? 1 : 0) << 3 | (ResetReady ? 1 : 0) << 2 | (AnswerWithoutRequest ? 1 : 0) << 1 | (ExternalStart ? 1 : 0)) };
            return cfg;

        }
    }
}
