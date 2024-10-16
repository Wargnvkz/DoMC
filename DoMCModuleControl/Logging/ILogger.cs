using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl.Logging
{
    public interface ILogger
    {
        /// <summary>
        /// Записать сообщение в лог
        /// </summary>
        /// <param name="level">Тип сообщения</param>
        /// <param name="Message">Сообщение</param>
        public void Add(LoggerLevel level, string Message);
        /// <summary>
        /// Записать исключение с сообщением в лог
        /// </summary>
        /// <param name="level">Тип сообщения</param>
        /// <param name="Message">Сообщение</param>
        /// <param name="exception">Исключение</param>
        public void Add(LoggerLevel level, string Message, Exception exception);
        /// <summary>
        /// Установить максимальный уровень типа сообщения, которое записывается в лог
        /// </summary>
        /// <param name="level"></param>
        public void SetMaxLogginLevel(LoggerLevel level);
        /// <summary>
        /// записывает все незаписынные сообщения
        /// </summary>
        /// <returns></returns>
        public void Flush();
    }
}
