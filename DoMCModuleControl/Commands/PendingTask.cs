using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl.Commands
{
    public class PendingCommandController<T>
    {
        private TaskCompletionSource<T>? _pendingReadTask;
        private readonly SemaphoreSlim _commandLock = new SemaphoreSlim(1, 1); // блокирует параллельный доступ
        private Func<int, bool> ResponseFilter;
        public async Task<T> AsyncCommand(
            CancellationToken token,
            Func<int, bool> responseFilter,
            Action cmd,
            int timeoutMilliseconds = 5000) // ← можно передать таймаут
        {
            SetExpectingResponses(responseFilter);
            await _commandLock.WaitAsync(token);

            try
            {
                if (_pendingReadTask != null)
                    throw new InvalidOperationException("Команда уже выполняется на этой плате");

                _pendingReadTask = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);

                cmd?.Invoke();

                using (token.Register(() => _pendingReadTask.TrySetCanceled(token)))
                {
                    var completed = await Task.WhenAny(
                        _pendingReadTask.Task,
                        Task.Delay(timeoutMilliseconds, token));

                    if (completed == _pendingReadTask.Task)
                    {
                        return await _pendingReadTask.Task; // всё ок, ответ пришёл
                    }
                    else
                    {
                        _pendingReadTask.TrySetException(new TimeoutException($"Команда не ответила за {timeoutMilliseconds} мс"));
                        throw new TimeoutException($"Команда не ответила за {timeoutMilliseconds} мс");
                    }
                }
            }
            finally
            {
                _pendingReadTask = null;
                _commandLock.Release();
            }
        }

        public async Task<T> AsyncCommand(
               CancellationToken token,
               Func<int, bool> responseFilter,
               Func<Task<T>> asyncExecution, // ← добавляем
               Action sendCommand,
               int timeoutMilliseconds = 5000)
        {
            SetExpectingResponses(responseFilter);
            await _commandLock.WaitAsync(token);

            try
            {
                if (_pendingReadTask != null)
                    throw new InvalidOperationException("Команда уже выполняется на этой плате");

                _pendingReadTask = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);

                sendCommand?.Invoke(); // ← отправили команду 9 (типа "готовь изображения")

                // параллельно запускаем задачу, которая читает изображения
                _ = Task.Run(async () =>
                {
                    try
                    {
                        var result = await asyncExecution();
                        _pendingReadTask.TrySetResult(result); // <- результат чтения
                    }
                    catch (OperationCanceledException)
                    {
                        _pendingReadTask.TrySetCanceled(token);
                    }
                    catch (Exception ex)
                    {
                        _pendingReadTask.TrySetException(ex);
                    }
                });

                // таймаут + отмена
                using (token.Register(() => _pendingReadTask.TrySetCanceled(token)))
                {
                    var completed = await Task.WhenAny(
                        _pendingReadTask.Task,
                        Task.Delay(timeoutMilliseconds, token));

                    if (completed == _pendingReadTask.Task)
                        return await _pendingReadTask.Task;

                    _pendingReadTask.TrySetException(new TimeoutException("Таймаут получения изображений"));
                    throw new TimeoutException("Таймаут получения изображений");
                }
            }
            finally
            {
                _pendingReadTask = null;
                _commandLock.Release();
            }
        }


        protected void SetExpectingResponses(Func<int, bool> responseFilter)
        {
            ResponseFilter = responseFilter;
        }
        protected bool IsExpectedResponse(int ResponseCode)
        {
            return ResponseFilter?.Invoke(ResponseCode) ?? false;
        }

        public void SetCanceled()
        {
            _pendingReadTask?.SetCanceled();
        }
        public void SetException(Exception ex)
        {
            _pendingReadTask?.SetException(ex);
        }
        public bool TrySetResult(int ResponseCode, T result)
        {
            if (!IsExpectedResponse(ResponseCode)) return false;
            return _pendingReadTask.TrySetResult(result);
        }
    }
}
