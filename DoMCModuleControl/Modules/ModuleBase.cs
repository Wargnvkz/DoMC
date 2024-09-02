#pragma warning disable IDE0090
#pragma warning disable IDE0290
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoMCModuleControl.Commands;

namespace DoMCModuleControl.Modules
{
    /// <summary>
    /// Базовый класс для модулей программы. Модули сами разбираются что и как им делать
    /// </summary>
    public abstract class ModuleBase
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
        private readonly IMainController mainController;

        /// <summary>
        /// Список команд, которые поддерживает модуль
        /// </summary>
        protected List<CommandInfo> commandInfos = [];

        /// <summary>
        /// Переменная, чтобы быть уверенным, что ресурсы уже были освобождены
        /// </summary>
        private bool disposed = false;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="MainController">Главный контроллер всего</param>
        public ModuleBase(IMainController MainController)
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


    }
}
