using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl.Commands
{
    /// <summary>
    /// шаблон команды. Каждая команда сама выполняет код.
    /// Все команды выполняются асинхронно.
    /// При обработке: События логируются и посылается сообщение в Obsever
    /// "{CommandName}.Start" - InputData (logging - Informational
    /// "{CommandName}.Success" - OutputData (loggin - Informational)
    /// "{CommandName}.Error" - Exception (loggin - Critical)
    /// </summary>
    public abstract class AbstractCommandBase : ICommandStatus
    {
        //TODO: сделать синхронизацию для многопоточности
        /// <summary>
        /// Тип входных данных
        /// </summary>
        public Type? InputType { get; private set; }
        /// <summary>
        /// Тип выходных данных
        /// </summary>
        public Type? OutputType { get; private set; }
        /// <summary>
        /// Входные данные
        /// </summary>
        public object? InputData { get; set; }
        /// <summary>
        /// Выходные данные
        /// </summary>
        public object? OutputData { get; protected set; }
        /// <summary>
        /// Модуль с которым работает команда
        /// </summary>
        protected Modules.AbstractModuleBase Module { get; private set; }
        /// <summary>
        /// Статус: Команда запущена
        /// </summary>
        public bool IsRunning { get; protected set; }
        /// <summary>
        /// Статус: Команда завершена успешно
        /// </summary>
        public bool IsCompleteSuccessfully { get; protected set; }
        /// <summary>
        /// Статус: Команда отменена
        /// </summary>
        public bool IsCanceled { get; protected set; }
        /// <summary>
        /// Статус: Команда завершена с ошибкой
        /// </summary>
        public bool IsError { get; protected set; }
        /// <summary>
        /// Статус: ошибка приведшая к завершению
        /// </summary>
        public Exception? Error { get; private set; }

        public IMainController Controller { get; private set; }
        public CancellationTokenSource CancelationTokenSourceToCancelCommandExecution { get; private set; } = new CancellationTokenSource();
        public bool HasNotBeenRunningYet()
        {
            return !IsRunning && !IsCompleteSuccessfully && !IsError && !IsCanceled;
        }
        public bool HasStopedAlready()
        {
            return !IsRunning && (IsCompleteSuccessfully || IsError || IsCanceled);
        }
        public bool WasCompletedSuccessfully()
        {
            return !IsRunning && IsCompleteSuccessfully && !IsError && !IsCanceled;
        }

        public enum Events
        {
            Started,
            Suceeded,
            Error
        }


        /// <summary>
        /// Конструктор. Потомки сами определяют типы данных и передают их в этот конструктор
        /// </summary>
        /// <param name="module">Модуль с которым будет работать команда</param>
        /// <param name="inputType">Тип входных данных</param>
        /// <param name="outputType">Тип выходных данных</param>
        public AbstractCommandBase(IMainController mainController, Modules.AbstractModuleBase module, Type? inputType = null, Type? outputType = null)
        {
            Controller = mainController;
            Module = module;
            CancelationTokenSourceToCancelCommandExecution = new CancellationTokenSource();

        }

        /// <summary>
        /// Возвращает имя команды, по которому ее будет создавать главный контроллер
        /// </summary>
        public virtual string CommandName
        {
            get
            {
                // Получаем имя модуля и команды
                var moduleName = Module.GetType().Name;
                var commandName = GetType().Name;

                // Формируем полное имя команды
                return $"{moduleName}.{commandName}";
            }
        }

        public virtual void NotificationProcedure(string eventName, object? data)
        {

        }
        /// <summary>
        /// Метод запускающий команду в работу и регулирующий статусы и логирование
        /// </summary>
        private async Task ExecuteCommandBase()
        {
            IsRunning = true;
            IsCompleteSuccessfully = false;
            IsError = false;
            Error = null;
            try
            {
                Controller.GetLogger(Module.GetType().Name).Add(Logging.LoggerLevel.FullDetailedInformation, $"Начало выполнения кода команды {CommandName}.");
                Controller.GetObserver().Notify($"{CommandName}.{Events.Started}", InputData);
                if (InputType != null && InputData == null) throw new InvalidOperationException($"Не могу выполнить команду. Необходимо задать входные данные методом SetInputData с типом {InputType.Name}");
                await Executing();
                Controller.GetLogger(Module.GetType().Name).Add(Logging.LoggerLevel.FullDetailedInformation, $"Выполнение кода команды {CommandName} завершено");
                Controller.GetObserver().Notify($"{CommandName}.{Events.Suceeded}", OutputData);
                IsRunning = false;
                IsCompleteSuccessfully = true;
                IsError = false;
            }
            catch (Exception ex)
            {
                IsRunning = false;
                IsCompleteSuccessfully = false;
                IsError = true;
                Error = ex;
                Controller.GetLogger(Module.GetType().Name).Add(Logging.LoggerLevel.Critical, $"Ошибка при выполнении команды {CommandName}. ", ex);
                Controller.GetObserver().Notify($"{CommandName}.{Events.Error}", ex);
            }
            finally
            {
            }
        }
        public virtual async Task ExecuteCommandAsync()
        {
            await ExecuteCommandBase();

        }

        public virtual async Task<T> ExecuteCommandAsync<T>()
        {
            await ExecuteCommandBase();

            if (OutputData is T result)
                return result;

            throw new InvalidOperationException($"Тип выходных данных {OutputData?.GetType().Name} не соответствует ожидаемому {typeof(T).Name}.");
            
        }

        public virtual async Task<T> ExecuteCommandAsync<T>(object? inputData)
        {
            SetInputData(inputData);
            return await ExecuteCommandAsync<T>();
        }

        /// <summary>
        /// Метод описываемый в потомках, то есть в конкретной команде и выполняющий сам код команды. Классам реализующим этот метод не нужно заботиться о логировании или ошибках. Все это сделано в базовом классе, чтобы соблюсти стандартизацию
        /// </summary>
        protected abstract Task Executing();
        /// <summary>
        /// Установка входных данных
        /// </summary>
        /// <param name="inputData">Входные данные</param>
        /// <exception cref="ArgumentException">Вызывается, если тип переданых данных не совпадает с типом данных, которые должны быть переданы и указаны в InputType</exception>
        public virtual void SetInputData(object? inputData)
        {
            if (InputType == null || inputData == null) return;
            if (!InputType.IsInstanceOfType(inputData))
            {
                throw new ArgumentException($"Неверный тип данных. Передан тип {inputData.GetType().FullName}, а ожидается {InputType.FullName}.");
            }
            InputData = inputData;
        }
        /// <summary>
        /// Получает выходные данные команды
        /// </summary>
        /// <typeparam name="T">Тип выходные данных</typeparam>
        /// <returns>Данные поля OutputData</returns>
        /// <exception cref="InvalidOperationException">Если тип данных в OutputData отличается от типа TContext</exception>
        public T GetOutputData<T>()
        {
            if (OutputData is T result)
            {
                return result;
            }
            else
            {
                throw new InvalidOperationException($"Выходные данные не являются данными типа {typeof(T).Name}.");
            }
        }
        
    }

}
