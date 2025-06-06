﻿
using DoMCModuleControl.Logging;
using DoMCModuleControl.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl
{
    /// <summary>
    /// Наблюдатель. Модули вызывают его, чтобы сообщить о событии
    /// Те, кто хочет знать о событии подписываются на события
    /// </summary>
    public class Observer
    {
        /// <summary>
        /// Для подписчиков на событие. При наступлении события подписчики вызываются асинхронно
        /// </summary>
        public event Action<string, object?> NotificationReceivers;
        private readonly ILogger Logger;
        public Observer(ILogger logger)
        {
            NotificationReceivers = delegate { };
            Logger = logger;
        }

        /// <summary>
        /// Посылает уведомления подписчикам
        /// </summary>
        /// <param name="eventName">Название события</param>
        /// <param name="eventData">Данные о событии</param>
        public void Notify(string eventName, object? eventData)
        {
            // Сначала вызываем асинхронные события
            if (NotificationReceivers != null)
            {
                var handlers = NotificationReceivers.GetInvocationList();

                //foreach (Func<string, object?, Task> handler in handlers.Cast<Func<string, object?, Task>>())
                foreach (var handler in handlers)
                {
                    Task.Run(() =>
                    {
                        try
                        {
                            Logger.Add(LoggerLevel.FullDetailedInformation, $"Событие {eventName} передано в {handler.GetType().Name}");
                            handler.DynamicInvoke(eventName, eventData);
                            Logger.Add(LoggerLevel.FullDetailedInformation, $"Событие {eventName} в {handler.GetType().Name} завершено");
                        }
                        catch (Exception ex)
                        {
                            Logger.Add(LoggerLevel.Critical, $"Ошибка при обработке сообщения {eventName}{(eventData != null ? $" с данными {eventData}" : string.Empty)} в методе {handler.Method.Name}:", ex);
                        }
                    });
                }
            }
        }
    }
}
