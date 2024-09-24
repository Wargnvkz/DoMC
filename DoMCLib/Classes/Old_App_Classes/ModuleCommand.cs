namespace DoMCLib.Classes
{
    //Команды 
    public enum ModuleCommand
    {
        /// <summary>
        /// Ничего не делает. Указывает, что ничего не происходит
        /// </summary>
        StartIdle,

        /// <summary>
        /// Установка конфигурации для всех гнезд. Не загружает их в плату
        /// </summary>
        SetAllCardsAndSocketsConfiguration,

        /// <summary>
        /// Открывает соединения с платами
        /// </summary>
        CCDStart,

        /// <summary>
        /// Закрывает соединения с платами
        /// </summary>
        CCDStop,


        /// <summary>
        /// Загрузка конфигурации в платы
        /// </summary>
        LoadConfiguration,

        /// <summary>
        /// Установка конфигурации в платы
        /// </summary>
        LoadConfigurationWithStandard,

        /// <summary>
        ///Сброс плат ПЗС
        /// </summary>
        CCDCardsReset,

        /// <summary>
        /// установка конфигурации блока управления светодиодами, и вкл/выкл светодиодов
        /// </summary>
        InitLCB,

        /// <summary>
        /// Получение состояния гнезд и плат (работают/не работают)
        /// </summary>
        CCDGetSocketStatus,

        /// <summary>
        /// Запуск чтения всех гнезд по параметрам конфигурации
        /// </summary>
        StartRead,

        /// <summary>
        /// Запуск чтения всех гнезд по параметрам конфигурации с внешним сигналом
        /// </summary>
        StartReadExternal,

        /// <summary>
        /// Запуск чтения гнезд по параметрам конфигурации 
        /// </summary>
        StartSeveralSocketRead,

        /// <summary>
        /// с внешним сигналом
        /// </summary>
        StartSeveralSocketReadExternal,

        /// <summary>
        /// Получения статусов проверки изображений гнезд с эталоном
        /// </summary>
        GetSocketGoodStatus,

        /// <summary>
        /// Получение изображений всех гнезд в соответствии с конфигурацией
        /// </summary>
        GetSocketImages,

        /// <summary>
        /// Получение изображений гнезд в соответствии с конфигурацией
        /// </summary>
        GetSeveralSocketImages,

        /* // Получение состояний светодиодов
         GetLEDStatus,*/

        /// <summary>
        /// чтение эталонов
        /// </summary>
        StandardRead,

        /// <summary>
        /// запись эталонов
        /// </summary>
        StandardWrite,

        StopModuleWork,

        SetLCBCurrentRequest,

        GetLCBCurrentRequest,

        SetLCBMovementParametersRequest,

        GetLCBMovementParametersRequest,

        SetLCBEquipmentStatusRequest,

        GetLCBEquipmentStatusRequest,

        SetLCBWorkModeRequest,

        SetLCBNonWorkModeRequest,

        GetLCBMaxPositionRequest,

        GetLCBCurrentPositionRequest,
        LCBUDPReconnect,
        LCBStop,

        RDPBStart,
        RDPBStop,
        RDPBSetIsOK,
        RDPBSetIsBad,
        RDPBOn,
        RDPBOff,
        RDPBGetParameters,
        RDPBSetCoolingBlockQuantity,
        RDPBSendManualCommand,

        DBStart,
        DBStop,
        ArchiveDBStart,
        ArchiveDBStop

    }
}
