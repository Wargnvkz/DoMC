using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl.Modules
{
    /// <summary>
    /// Базовый класс для модулей программы. Модули сами разбираются что и как им делать
    /// </summary>
    public abstract class ModuleBase : IDisposable
    {
        /// <summary>
        /// Статусы работы модуля
        /// </summary>
        private readonly ModuleStatus _status = new ModuleStatus();
        /// <summary>
        /// статусы доступные снаружи модуля
        /// </summary>
        public ModuleStatus Status => (ModuleStatus)_status.Clone();
        /// <summary>
        /// гравные контроллер управляющий всеми связями, объектами, модулями, интерфесом и командой
        /// </summary>
        private MainController mainController;

        /// <summary>
        /// Список команд, которые поддерживает модуль
        /// </summary>
        protected List<Command.CommandInfo> commandInfos = new List<Command.CommandInfo>();
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="MainController">Главный контроллер всего</param>
        public ModuleBase(MainController MainController)
        {
            mainController = MainController;
        }
        /// <summary>
        /// Метод регистрирующий команды, выполняемые модулем, в основном контроллере
        /// </summary>
        public void RegisterCommands()
        {
            commandInfos.ForEach(ci =>
            {
                ci.Module = this;
                mainController.RegisterCommand(ci);
            });
        }
        /// <summary>
        /// Первичная инициализация модуля
        /// </summary>
        public abstract void Initialize();
        /// <summary>
        /// Полное отключение модуля
        /// </summary>
        public abstract void Shutdown();
        /// <summary>
        /// Запуск модуля в работу
        /// </summary>
        public abstract void Start();
        /// <summary>
        /// Отстановка работы модуля
        /// </summary>
        public abstract void Stop();
        /// <summary>
        /// Остановка и полное отключение модуля перед уничтожением
        /// </summary>
        public void Dispose()
        {
            Stop();
            Shutdown();
        }
    }
}
