using DoMCModuleControl.Commands;
using DoMCModuleControl.Configuration;
using DoMCModuleControl.Logging;
using DoMCModuleControl.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl
{
    public interface IMainController
    {
        public Observer GetObserver();
        /// <summary>
        /// Создание команды по имени (из списка известных команд создается экземпляр команды, который выполняет код
        /// </summary>
        /// <param name="commandName">Текстовое имя команды</param>
        /// <returns>Созданная команда</returns>
        /// <exception cref="ArgumentNullException">Возникает, если класс команды не задан</exception>
        /// <exception cref="ArgumentException">Возникает, если команда не найдена в списке зарегистрированых</exception>
        public CommandBase? CreateCommand(string commandName, object? data = null);
        /// <summary>
        /// Регистрация команды в контроллере
        /// </summary>
        /// <param name="commandInfo"></param>
        public void RegisterCommand(CommandInfo commandInfo);
        /// <summary>
        /// Создает класс для логорования указанного модуля
        /// </summary>
        /// <param name="ModuleName">Имя модуля</param>
        /// <returns></returns>
        public ILogger GetLogger(string ModuleName);

        /// <summary>
        /// Получить главный интерфейс программы
        /// </summary>
        /// <returns></returns>
        public IMainUserInterface GetMainUserInterface();
    }
}
