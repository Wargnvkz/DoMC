
using DoMCModuleControl.Logging;
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
        public event Action<string, object?> NotificationReceived;
        private readonly ILogger Logger;
        public Observer(ILogger logger)
        {
            NotificationReceived = delegate { };
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
            if (NotificationReceived != null)
            {
                var handlers = NotificationReceived.GetInvocationList();

                foreach (Func<string, object?, Task> handler in handlers.Cast<Func<string, object?, Task>>())
                {
                    Task.Run(async () =>
                    {
                        try
                        {
                            await handler.Invoke(eventName, eventData);
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
