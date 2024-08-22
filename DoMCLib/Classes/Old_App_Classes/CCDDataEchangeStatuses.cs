namespace DoMCLib.Classes
{
    public class CCDDataEchangeStatuses
    {
        public bool IsNetworkCardSet;
        public bool IsConfigurationLoaded;
        public bool ExternalStart;
        public bool FastRead;
        public int[] SocketsToSave; // список гнезд, изображения которых надо получить или сохранить
        public int[] ErrorSocket;  // ошибочные гнезда
        public ModuleCommand ModuleStatus;   // текущая команда модуля ПЗС
        public ModuleCommandStep ModuleStep; // статус/шаг такущей команды
        public int ModuleProcessingStep;
        public short[][,] Images;   //изображения гнезд полученные в последний раз
        public short[][,] PreviousImages; //Изображения предыдущего съема
        //public long[] SocketReadImageStart; //Тик начала получения данных по гнезду
        //public long[] SocketReadImageStop; // тик последнего прихода данных
        public DateTime ReadyToSendTime;  // Время получения ответа, что данные с матриц прочитаны
        //public DateTime TheFirstLineTime;  // Время получения первого пакета с линией изображения
        //public DateTime TheLastLineTime;  // Время получения последнего пакета с линией изображения
        public long StartProcessImages;
        public long StopProcessImages;
        public long StartDecisionImages;
        public long StopDecisionImages;

        public long ProcessDuration
        {
            get
            {
                return StopProcessImages - StartProcessImages;
            }
        }

    }
}
