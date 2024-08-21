using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl.Modules
{
    /// <summary>
    /// Статус работы модуля
    /// </summary>
    public class ModuleStatus : ICloneable
    {
        /// <summary>
        /// Инициализирован ли модуль
        /// </summary>
        public bool IsInitialized;
        /// <summary>
        /// Запущен ли модуль
        /// </summary>
        public bool IsRuning;
        /// <summary>
        /// Клонирование статуса.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
