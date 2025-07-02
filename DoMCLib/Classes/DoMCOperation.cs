using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes
{
    public enum DoMCOperation
    {
        [Description("Простаивает")]
        Idle,

        [Description("Подключение к платам ПЗС")]
        StartCCD,
        [Description("Установка параметров экспозиции плат ПЗС")]
        SettingExposition,
        [Description("Установка параметров чтения гнез платами ПЗС")]
        SettingReadingParameters,
        [Description("Установка быстрого чтения платами ПЗС")]
        SetFastReading,
        [Description("Чтение гнезд платами ПЗС")]
        ReadingSockets,
        [Description("Ожидание синхроимпульса платами ПЗС для начала чтения")]
        ReadingSocketsExternal,
        [Description("Получение прочитанных данных изображений от плат ПЗС")]
        GettingImages,
        [Description("Проверка изображений")]
        CheckingImages,
        [Description("Создание эталона")]
        CreatingStandard,
        [Description("Остановка работы плат ПЗС")]
        StopCCD,

        [Description("Запуск БУС")]
        StartLCB,
        [Description("установка параметров работы БУС")]
        SettingLCBConfiguration,
        [Description("Установка рабочего режжима БУС")]
        SettingLCBWorkingMode,
        [Description("Отключение рабочего режжима БУС")]
        SettingLCBNonWorkingMode,
        [Description("Остановка работы БУС")]
        StopLCB,

        [Description("Запуск базы данных в работу")]
        StartDB,
        [Description("Установка параметров базы данных")]
        SetDBConfiguration,
        [Description("Сохранение данных съема")]
        SavingCurrentCtycle,
        [Description("Сохранение данных короба")]
        SavingCurrentBox,
        [Description("Остановка работы базы данных")]
        StopDB,

        [Description("Запуск архива в работу")]
        StartArchiveDB,
        [Description("Установка параметров работы архива")]
        SetArchiveDBConfiguration,
        [Description("Остановка работы архива")]
        StopArchiveDB,



        [Description("Сохранение конфигурации")]
        SavingConfiguration,

        [Description("Завершено без ошибок")]
        CompleteOk,
        [Description("Завершено с ошибками")]
        CompleteError

    }
}
