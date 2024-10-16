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
    public abstract class AbstractModuleBase
    {
        /// <summary>
        /// главный контроллер управляющий всеми связями, объектами, модулями, интерфесом и командой
        /// </summary>
        protected readonly IMainController MainController;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="MainController">Главный контроллер всего</param>
        public AbstractModuleBase(IMainController MainController)
        {
            this.MainController = MainController;
        }


    }
}
