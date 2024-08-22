using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl
{
    public class Observer
    {
        // Событие для синхронных операций
        public event Action<string, object?> SyncNotificationReceived;

        // Событие для асинхронных операций
        public event Func<string, object?> AsyncNotificationReceived;

        // Метод для вызова событий
        public async Task Notify(string eventName, object eventData)
        {
            // Сначала вызываем асинхронные события
            if (AsyncNotificationReceived != null)
            {
                var handlers = AsyncNotificationReceived.GetInvocationList();

                foreach (Func<string, object?, Task> handler in handlers)
                {
                    _ = Task.Run(() => handler.Invoke(eventName, eventData));
                }

            }

            // Затем вызываем синхронные события
            SyncNotificationReceived?.Invoke(eventName, eventData);
        }
    }


}
