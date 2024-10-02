#pragma warning disable IDE0270
using System.Data;
using System;
using System.Reflection;
using System.Windows.Input;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;// Для работы с JSON-файлами конфигурации
using DoMCModuleControl.Commands;
using DoMCModuleControl.UI;
using DoMCModuleControl.Logging;
using DoMCModuleControl.Modules;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.InteropServices.Marshalling;

namespace DoMCModuleControl
{
    /// <summary>
    /// Главный контроллер управляющий всей программой
    /// </summary>
    public class MainController : IMainController
    {
        private readonly static string appsettingsPath = "appsettings.json";
        private readonly static string MainInterfaceFieldString = "IMainUserInterface";
        /// <summary>
        /// список команд(паттерн команда), которые могут выполняться, к которым можно получить доступ по именам
        /// </summary>
        private readonly Dictionary<string, CommandInfo> _commands = [];
        /// <summary>
        /// список модулей, к которым можно получить доступ по именам
        /// </summary>
        private readonly Dictionary<string, Modules.ModuleBase> _modules = [];

        /// <summary>
        /// Наблюдательн
        /// </summary>
        protected readonly Observer Observer;

        protected BaseFilesLogger MainFileLogger;
        protected BaseSystemLogger MainSystemLogger;
        protected Logger SystemLogger;

        /// <summary>
        /// Основной интерфейс программы
        /// </summary>
        private readonly IMainUserInterface MainUserInterface;

        public MainController(Type mainUserInterfaceType)
        {
            // Создаем экземпляр интерфейса
            var mainUserInterface = (IMainUserInterface?)Activator.CreateInstance(mainUserInterfaceType);
            MainUserInterface = mainUserInterface ?? throw new InvalidOperationException($"Не удалось создать экземпляр типа \"{mainUserInterfaceType.Name}\".");
            MainUserInterface.SetMainController(this);
            MainFileLogger = new BaseFilesLogger();
            MainSystemLogger = new BaseSystemLogger();
            SystemLogger = new Logger("DoMC", MainSystemLogger);
            MainFileLogger.RegisterExternalLogger(SystemLogger);
            Observer = new Observer(GetLogger("Events"));
        }
        /// <summary>
        /// Создает модуль управления. Перед вызовом нужно вызвать AssemblyLoader.LoadAssembliesFromPath иначе нужные библиотеки с командами, модулями и интрефейсом не будут найдены
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static MainController Create()
        {

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type UIType;
            var UITypes = new List<Type>();
            foreach (var assembly in assemblies)
            {
                UITypes.AddRange(assembly.GetTypes().Where(t => typeof(IMainUserInterface).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface));
            }

            if (UITypes.Count == 1)
            {
                UIType = UITypes[0];

            }
            else
            {
                if (UITypes.Count > 1)
                {
                    throw new InvalidOperationException($"Найден несколько главных интерфейсов, но в файле \"{appsettingsPath}\" нет значения \"{MainInterfaceFieldString}\"");
                }
                else
                {
                    throw new InvalidOperationException("Не найден ни один главный интерфейс");
                }
            }
            var mainController = new MainController(UIType);
            return mainController;
        }

        /// <summary>
        /// регистрация модуля
        /// </summary>
        /// <param name="module">экземпляр модуля</param>
        public void RegisterModule(Modules.ModuleBase module)
        {
            _modules.Add(module.GetType().Name, module);
        }


        /// <summary>
        /// Создание основного контроллера с созданием модулей из библиотек в папке Modules/*.dll
        /// </summary>
        /// <returns>Новый главный контроллер</returns>
        public static MainController LoadDLLModules()
        {

            var StartingConfiguration = GetMainApplicationConfiguration();

            // Поиск всех сборок с модулями
            var moduleAssemblies = Directory.GetFiles("Modules", "*.dll");
            List<Type> UITypes = [];
            List<Type> ModuleTypes = [];

            foreach (var assemblyPath in moduleAssemblies)
            {
                var assembly = Assembly.LoadFrom(assemblyPath);

                // Поиск всех классов, реализующих ModuleBase
                var moduleTypes = assembly.GetTypes().Where(t => typeof(Modules.ModuleBase).IsAssignableFrom(t) && !t.IsInterface);
                ModuleTypes.AddRange(moduleTypes);
                var uiTypes = assembly.GetTypes().Where(t => typeof(IMainUserInterface).IsAssignableFrom(t) && !t.IsInterface).ToList();
                UITypes.AddRange(uiTypes);
            }
            Type UIType;
            var interfaceClassName = StartingConfiguration[MainInterfaceFieldString];
            if (!String.IsNullOrEmpty(interfaceClassName))
            {
                var uiType = Type.GetType(interfaceClassName);
                UIType = uiType ?? throw new InvalidOperationException($"Интерфейс \"{interfaceClassName}\", указанный в файле {appsettingsPath}, не найден.");
            }
            else
            {
                if (UITypes.Count == 1)
                {
                    UIType = UITypes[0];

                }
                else
                {
                    if (UITypes.Count > 1)
                    {
                        throw new InvalidOperationException($"Найден несколько главных интерфейсов, но в файле \"{appsettingsPath}\" нет значения \"{MainInterfaceFieldString}\"");
                    }
                    else
                    {
                        throw new InvalidOperationException("Не найден ни один главный интерфейс");
                    }
                }
            }
            // Проверяем, что найденный тип наследуется от IMainUserInterface
            if (!typeof(IMainUserInterface).IsAssignableFrom(UIType))
            {
                throw new InvalidOperationException($"Тип интерфейса \"{interfaceClassName}\" не является наследником IMainUserInterface.");
            }

            var mainController = new MainController(UIType);

            foreach (var moduleType in ModuleTypes)
            {

                // Создание экземпляра модуля и регистрация его в MainController
                var module = (Modules.ModuleBase?)Activator.CreateInstance(moduleType, mainController);
                if (module == null)
                {
                    //TODO: залогировать, что модуль не ModuleBase
                }
                else
                {
                    mainController.RegisterModule(module);
                }
            }

            return mainController;
        }


        public void FindAndRegisterAllModules()
        {

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var moduleTypes = new List<Type>();
            foreach (var assembly in assemblies)
            {
                moduleTypes.AddRange(assembly.GetTypes().Where(t => typeof(ModuleBase).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface));
            }

            foreach (var moduleType in moduleTypes)
            {
                var module = (Modules.ModuleBase?)Activator.CreateInstance(moduleType, this);
                if (module == null) throw new InvalidOperationException("Не получилось создать экземпляр модуля");
                RegisterModule(module);
            }
        }
        /// <summary>
        /// Загружает конфигурацию работы программы из файла appsettings.json
        /// </summary>
        private static IConfiguration GetMainApplicationConfiguration()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Установка базового пути
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true); // Добавление файла конфигурации

            return builder.Build(); // Построение конфигурации


        }

        /// <summary>
        /// Регистрация команды в контроллере
        /// </summary>
        /// <param name="commandInfo"></param>
        public void RegisterCommand(CommandInfo commandInfo)
        {
            if (commandInfo == null || commandInfo.CommandName == null) return;
            _commands[commandInfo.CommandName] = commandInfo;
        }

        public void RegisterAllCommands()
        {

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var commandTypes = new List<Type>();
            foreach (var assembly in assemblies)
            {
                commandTypes.AddRange(assembly.GetTypes().Where(t => typeof(CommandBase).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface));
            }


            foreach (var commandType in commandTypes)
            {
                ModuleBase? moduleInstance = null;

                if (commandType.DeclaringType != null && typeof(ModuleBase).IsAssignableFrom(commandType.DeclaringType))
                {
                    moduleInstance = (ModuleBase?)Activator.CreateInstance(commandType.DeclaringType, this);
                }
                else
                {
                    var moduleTypeAttribute = commandType.GetCustomAttribute<CommandModuleTypeAttribute>();
                    if (moduleTypeAttribute != null)
                    {
                        moduleInstance = (ModuleBase?)Activator.CreateInstance(moduleTypeAttribute.ModuleType, this);
                    }

                }
                //var moduleNameAttribute = commandType.GetCustomAttribute<CommandModuleNameAttribute>();

                if (moduleInstance != null)
                {
                    var commandInstance = (CommandBase?)Activator.CreateInstance(commandType, this, moduleInstance);
                    if (commandInstance != null)
                    {
                        var commandInfo = new CommandInfo(
                                //moduleNameAttribute == null ?
                                commandInstance.CommandName
                                //:$"{moduleInstance.GetType().Name}.{moduleNameAttribute.ModuleName}"
                                ,
                            commandInstance.InputType,
                            commandInstance.OutputType,
                            commandType,
                            moduleInstance
                        );
                        RegisterCommand(commandInfo);
                    }
                }
            }
        }

        public List<CommandInfo> GetRegisteredCommandList()
        {
            var result = new List<CommandInfo>();
            foreach (var kv in _commands)
            {
                result.Add(kv.Value.Clone());
            }
            return result;
        }

        /// <summary>
        /// Создание команды по имени (из списка известных команд создается экземпляр команды, который выполняет код)
        /// </summary>
        /// <param name="commandName">Текстовое имя команды</param>
        /// <returns>Созданная команда</returns>
        /// <exception cref="ArgumentNullException">Возникает, если класс команды не задан</exception>
        /// <exception cref="ArgumentException">Возникает, если команда не найдена в списке зарегистрированых</exception>
        public CommandBase? CreateCommand(string commandName, object? data = null)
        {
            if (_commands.TryGetValue(commandName, out var commandInfo))
            {
                if (commandInfo.CommandClass != null)
                {
                    return (CommandBase?)Activator.CreateInstance(commandInfo.CommandClass, this, commandInfo.Module, commandInfo.InputType, commandInfo.OutputType, data);
                }
                throw new ArgumentNullException($"В команде \"{commandName}\" не задан код выполнения команды(ее класс)");//new InvalidOperationException($"Command class \"{commandInfo.CommandClass.Name}\" not found.");
            }
            throw new ArgumentException($"Команда \"{commandName}\" не зарегистрирована.");
        }

        /// <summary>
        /// Создание команды по имени (из списка известных команд создается экземпляр команды, который выполняет код)
        /// </summary>
        /// <param name="commandName">Текстовое имя команды</param>
        /// <returns>Созданная команда</returns>
        /// <exception cref="ArgumentNullException">Возникает, если класс команды не задан</exception>
        /// <exception cref="ArgumentException">Возникает, если команда не найдена в списке зарегистрированых</exception>
        public CommandBase? CreateCommand(Type commandType, object? data = null)
        {
            var commandInfo = _commands.Where(c => c.Value.CommandClass == commandType).FirstOrDefault().Value;
            if (commandInfo != null)
            {
                if (commandInfo.CommandClass != null)
                {
                    return (CommandBase?)Activator.CreateInstance(commandInfo.CommandClass, this, commandInfo.Module, commandInfo.InputType, commandInfo.OutputType, data);
                }
                throw new ArgumentNullException($"В команде \"{commandInfo.CommandName}\" не задан код выполнения команды(ее класс)");//new InvalidOperationException($"Command class \"{commandInfo.CommandClass.Name}\" not found.");
            }
            throw new ArgumentException($"Команда \"{commandType.Name}\" не найдена.");
        }

        public Observer GetObserver()
        {
            return Observer;
        }

        public ILogger GetLogger(string ModuleName)
        {
            return new Logger(ModuleName, MainFileLogger);
        }

        public IMainUserInterface GetMainUserInterface()
        {
            return MainUserInterface;
        }
    }

}