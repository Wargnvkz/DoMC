using DoMCModuleControl.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl.Commands
{
    /// <summary>
    /// Команда, которую поддерживает модуль. Ее имя используется для создания объепкта испольнителя команды
    /// </summary>
    public class CommandInfo
    {
        /// <summary>
        /// Имя команды
        /// </summary>
        public string? CommandName { get; set; }
        /// <summary>
        /// Тип входных данных команды. Если null, то команда не принимает входные данные
        /// </summary>
        public Type? InputType { get; set; }
        /// <summary>
        /// Тип выходных данных команды. Если null, то команда не имеет входных данные
        /// </summary>
        public Type? OutputType { get; set; }
        /// <summary>
        /// Тип класса команды, которая будет выполняться. Будет создан экземпляр этого класса
        /// </summary>
        public Type CommandClass { get; set; }
        /// <summary>
        /// Экземпляр модуля к которому будет обращаться команда
        /// </summary>
        public ModuleBase Module { get; set; }

        public CommandInfo(string? commandName, Type? inputType, Type? outputType, Type commandClass, ModuleBase module)
        {
            CommandName = commandName;
            InputType = inputType;
            OutputType = outputType;
            CommandClass = commandClass;
            Module = module;
        }
    }
}
