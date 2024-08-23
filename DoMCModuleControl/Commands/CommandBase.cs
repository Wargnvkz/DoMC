using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl.Commands
{
    /// <summary>
    /// шаблон команды. Каждая команда сама выполняет код.
    /// Все команды выполняются асинхронно.
    /// </summary>
    public abstract class CommandBase
    {

        //TODO: сделать синхронизаци для многопоточности
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
        public object? OutputData { get; set; }
        /// <summary>
        /// Модуль с которым работает команда
        /// </summary>
        protected Modules.ModuleBase Module { get; private set; }
        /// <summary>
        /// Статус: Команда запущена
        /// </summary>
        public bool IsRunning { get; private set; }
        /// <summary>
        /// Статус: Команда завершена успешно
        /// </summary>
        public bool IsComplete { get; private set; }
        /// <summary>
        /// Статус: Команда завершена с ошибкой
        /// </summary>
        public bool IsError { get; private set; }
        /// <summary>
        /// Статус: ошибка приведшая к завершению
        /// </summary>
        public Exception? Error { get; private set; }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="module">Модуль с которым будет работать команда</param>
        /// <param name="inputType">Тип входных данных</param>
        /// <param name="outputType">Тип выходных данных</param>
        public CommandBase(Modules.ModuleBase module, Type? inputType, Type? outputType)
        {
            Module = module;
            InputType = inputType;
            OutputType = outputType;
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="module">Модуль с которым будет работать команда</param>
        /// <param name="inputType">Тип входных данных</param>
        /// <param name="outputType">Тип выходных данных</param>
        /// <param name="inputData">Входные данные</param>
        public CommandBase(Modules.ModuleBase module, Type? inputType, Type? outputType, object? inputData=null)
        {
            Module = module;
            InputType = inputType;
            OutputType = outputType;
            InputData = inputData;
        }
        /// <summary>
        /// Метод запускающий команду в работу и регулирующий статусы и логирование
        /// </summary>
        private void ExecuteCommandBase()
        {
            //TODO: добавить логирование
            IsRunning = true;
            IsComplete = false;
            IsError = false;
            Error = null;
            try
            {
                if (InputType != null && InputData == null) throw new InvalidOperationException("Не могу выполнить команду. Необходимо задать входные данные методом SetInputData с типом {InputType.Name}");
                Executing();
            }
            catch (Exception ex)
            {
                IsRunning = false;
                IsComplete = false;
                IsError = true;
                Error = ex;
            }
            finally
            {
                IsRunning = false;
                IsComplete = true;
            }
        }
        /// <summary>
        /// Запуск команды на асинхронное выполнение
        /// </summary>
        public void ExecuteCommand()
        {
            var task = new Task(ExecuteCommandBase);
            task.Start();
        }
        /// <summary>
        /// Метод описываемый в потомках, то есть в конкретной команде и выполняющий сам код команды
        /// </summary>
        protected abstract void Executing();
        /// <summary>
        /// Установка входных данных
        /// </summary>
        /// <param name="inputData">Входные данные</param>
        /// <exception cref="ArgumentException">Вызывается, если тип переданых данных не совпадает с типом данных, которые должны быть переданы и указаны в InputType</exception>
        public void SetInputData(object? inputData)
        {
            if (InputType == null || inputData == null) return;
            if (InputType.IsInstanceOfType(inputData))
            {
                throw new ArgumentException($"Неверный тип данных. Ожидается {InputType.Name}.");
            }
            InputData = inputData;
        }
        /// <summary>
        /// Получает выходные данные команды
        /// </summary>
        /// <typeparam name="T">Тип выходные данных</typeparam>
        /// <returns>Данные поля OutputData</returns>
        /// <exception cref="InvalidOperationException">Если тип данных в OutputData отличается от типа T</exception>
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
        /// <summary>
        /// Ждет завершения выполнения команды.
        /// Если команда не запущена, то запускает ее
        /// </summary>
        public void Wait()
        {
            if (!IsRunning) ExecuteCommand();
            while (!IsComplete && !IsError) Thread.Sleep(1);
        }
    }

}
