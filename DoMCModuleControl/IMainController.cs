using DoMCModuleControl.Commands;
using DoMCModuleControl.Logging;
using DoMCModuleControl.Modules;
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

        public AbstractModuleBase GetModule(Type ModuleType);
        public AbstractModuleBase GetModule(string ModuleName);

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

        /// <summary>
        /// Последняя выполненная команда
        /// </summary>
        public Type? LastCommand { get; set; }
    }
}
