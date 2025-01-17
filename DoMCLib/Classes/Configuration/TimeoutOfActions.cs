namespace DoMCLib.Classes.Configuration
{
    public class TimeoutOfActions
    {
        /// <summary>
        /// Время ожидания ответа от плат ПЗС в миллисекундах
        /// </summary>
        public int WaitForCCDCardAnswerTimeoutInSeconds = 30;
        public int WaitForLCBCardAnswerTimeoutInSeconds = 30;
        public int WaitForRDPBCardAnswerTimeoutInSeconds = 1;
        public int DelayBeforeMoveDataToArchiveTimeInSeconds = 3600;
        public int WaitForSynchrosignalTimoutAfterCCDReadingFailedInSeconds = 10;
    }
}
