using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes
{
    public static class TaskExtensions
    {
        public static void FireAndForgetWithResult<T>(
            this Task<T> task,
            Action<T> onSuccess,
            Action<Exception>? onError = null,
            SynchronizationContext? context = null)
        {
            context ??= SynchronizationContext.Current;

            _ = Task.Run(async () =>
            {
                try
                {
                    var result = await task;
                    if (onSuccess != null)
                    {
                        if (context != null)
                            context.Post(_ => onSuccess(result), null);
                        else
                            onSuccess(result); // fallback если UI нет
                    }

                }
                catch (Exception ex)
                {
                    if (onError != null)
                    {
                        if (context != null)
                            context.Post(_ => onError(ex), null);
                        else
                            onError(ex);
                    }
                }
            });
        }
        public static void FireAndForgetWithResult(
            this Task task,
            Action onSuccess,
            Action<Exception>? onError = null,
            SynchronizationContext? context = null)
        {
            context ??= SynchronizationContext.Current;

            _ = Task.Run(async () =>
            {
                try
                {
                    await task;

                    if (context != null)
                        context.Post(_ => onSuccess(), null);
                    else
                        onSuccess(); // fallback если UI нет
                }
                catch (Exception ex)
                {
                    if (onError != null)
                    {
                        if (context != null)
                            context.Post(_ => onError(ex), null);
                        else
                            onError(ex);
                    }
                }
            });
        }
        public static void FireAndForget(
            this Task task,
            Action<Exception>? onError = null,
            SynchronizationContext? context = null)
        {
            context ??= SynchronizationContext.Current;

            _ = Task.Run(async () =>
            {
                try
                {
                    await task;
                }
                catch (Exception ex)
                {
                    if (onError != null)
                    {
                        if (context != null)
                            context.Post(_ => onError(ex), null);
                        else
                            onError(ex);
                    }
                }
            });
        }
    }

}
