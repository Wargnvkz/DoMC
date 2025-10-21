namespace DoMCLib.Classes
{
    public class DoMCInterfaceDataExchangeErrors
    {
        //Какие ошибки могут появляться
        //настройки
        //выбор сетевой платы
        //наличие IP и доступ к порту +
        //ПЗС
        //БУС +
        //SQL +
        //Память +
        //Скорость работы (все ли успевает отрабатывать)
        //Бракер
        //внешний модуль
        public bool ConfigurationNotSet;
        public bool NetworkCardError; // Общая ошибка сетевой плсаты или драйвера (не получилось найти ее с помощью драйвера N/Win PCap или получить о ней информацию)
        public bool NetworkCardHasNoIP; // не получилось определить IPv4 сетевой платы
        public bool CCDNotRespond; //плата ПЗС не отвечает
        public bool CCDFrameNotReceived; //изображения от платы ПЗС не получены
        public bool UDPError; // ошибка настройки и открытия UDP
        public bool LCBDoesNotRespond; // БУС не отвечает
        public bool LCBDoesNotSendSync; // БУС не послал синхроимульс
        public bool LEDStatusGettingError; // Не получены статусы светодиодов от БУС. Если после синхроимпульса уже есть данные от ПЗС, а статусов нет, то они не передавались.
        public bool UDPReceivesTrash; // По UDP приходит мусор (возможно проблема с БУС)
        public bool NoLocalSQL; // SQL недоступен или не работает
        public bool LocalSQLCycleSaveError;// ошибка при сохранении цикла
        public bool NoRemoteSQL; // SQL недоступен или не работает
        public bool ImageProcessError; //Ошибка при обработке изображений гнезд
        public bool NotEnoughMemory; // недостаточно памяти для хранения данных
        public bool LCBMemoryError; // недостаточно памяти для хранения данных БУС
        public bool CCDMemoryError; // недостаточно памяти при обработке ПЗС
        public bool NotEnoughTimeToGetCCD; // не хватает времени на получение данных от ПЗС
        public bool NotEnoughTimeToProcessData;
        public bool NotEnoughTimeToProcessSQL; // определять по накоплению данных в хранилищах. Среднее время каждого действия за некоторый период?
        public bool RemovingDefectedPreformsBlockError; // Ошибка бракёра
        public bool RemoteInterfaceModuleError; //Ошибка модуля удаленного интерфейса
        public bool ArchiveModuleError; // Ошибка модуля работы с архивом
        public int MissedSyncrosignalCounter = 0;


        public Dictionary<string, bool> ToDictionary()
        {
            var res = new Dictionary<string, bool>();
            res["ConfigurationNotSet"] = ConfigurationNotSet;
            res["NetworkCardError"] = NetworkCardError;
            res["NetworkCardHasNoIP"] = NetworkCardHasNoIP;
            res["CCDNotRespond"] = CCDNotRespond;
            res["CCDFrameNotReceived"] = CCDFrameNotReceived;
            res["UDPError"] = UDPError;
            res["LCBNotRespond"] = LCBDoesNotRespond;
            res["LCBDoesNotSendSync"] = LCBDoesNotSendSync;
            res["UDPReceivesTrash"] = UDPReceivesTrash;
            res["NoLocalSQL"] = NoLocalSQL;
            res["LocalSQLCycleSaveError"] = LocalSQLCycleSaveError;
            res["NoRemoteSQL"] = NoRemoteSQL;
            res["ImageProcessError"] = ImageProcessError;
            res["NotEnoughMemory"] = NotEnoughMemory;
            res["LCBMemoryError"] = LCBMemoryError;
            res["CCDMemoryError"] = CCDMemoryError;
            res["NotEnoughTimeToGetCCD"] = NotEnoughTimeToGetCCD;
            res["LEDStatusGettingError"] = LEDStatusGettingError;
            res["NotEnoughTimeToProcessData"] = NotEnoughTimeToProcessData;
            res["NotEnoughTimeToProcessSQL"] = NotEnoughTimeToProcessSQL;
            res["RemovingDefectedPreformsBlockError"] = RemovingDefectedPreformsBlockError;
            res["RemoteInterfaceModuleError"] = RemoteInterfaceModuleError;
            res["ArchiveModuleError"] = ArchiveModuleError;

            return res;
        }
        public string KeyToText(string key)
        {
            switch (key)
            {
                case "ConfigurationNotSet": return "Конфигурация не завершена";
                case "NetworkCardError": return "Ошибка сетевой карты";
                case "NetworkCardHasNoIP": return "У сетевой карты нет IP адреса";
                case "CCDNotRespond": return "Платы ПЗС не отвечают";
                case "CCDFrameNotReceived": return "Не получены изображения от плат ПЗС";
                case "UDPError": return "Ошибка UDP";
                case "LCBNotRespond": return "БУС не отвечает";
                case "LCBDoesNotSendSync": return "БУС не присылает синхросигнал";
                case "UDPReceivesTrash": return "По UDP приходит мусор";
                case "NoLocalSQL": return "Нет локальной БД";
                case "LocalSQLCycleSaveError": return "Ошибка при сохранении цикла";
                case "NoRemoteSQL": return "Нет удаленной БД";
                case "ImageProcessError": return "Ошибка при обработке изображений гнезд";
                case "NotEnoughMemory": return "Недостаточно памяти";
                case "LCBMemoryError": return "Недостаточно памяти для хранения данных БУС";
                case "CCDMemoryError": return "Недостаточно памяти при обработке ПЗС";
                case "NotEnoughTimeToGetCCD": return "Не хватает времени на получение данных от ПЗС";
                case "LEDStatusGettingError": return "Не получены статусы светодиодов от БУС.";
                case "NotEnoughTimeToProcessData": return "Не хватает времени на обработку данных от ПЗС";
                case "NotEnoughTimeToProcessSQL": return "Не хватает времени на обработку данных SQL";
                case "RemovingDefectedPreformsBlockError": return "Ошибка бракёра";
                case "RemoteInterfaceModuleError": return "Ошибка модуля удаленного интерфейса";
                case "ArchiveModuleError": return "Ошибка модуля работы с архивом";
            }
            return "";
        }

        public List<string> ActualErrors()
        {
            return ToDictionary().Where(kv => kv.Value).Select(kv => KeyToText(kv.Key)).ToList();
        }

    }
}
