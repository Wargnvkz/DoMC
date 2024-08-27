namespace DoMCLib.Classes.Model.Configuration
{
    public class TimeoutOfActions
    {
        /// <summary>
        /// Время ожидания ответа от плат ПЗС в миллисекундах
        /// </summary>
        public int WaitForCCDCardAnswerTimeout = 30000;
        public int WaitForLCBCardAnswerTimeout = 30000;
        public int WaitForRDPBCardAnswerTimeout = 1000;
        public int DelayBeforeMoveDataToArchiveTimeInSeconds = 3600;
        public int WaitForSynchrosignalTimoutAfterCCDReadingFailed = 10000;
    }
}
