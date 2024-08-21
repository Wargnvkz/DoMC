using System.Data;
using System;
using System.Reflection;
using System.Windows.Input;
using DoMCModuleControl.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using DoMCModuleControl.Interface; // Для работы с JSON-файлами конфигурации

namespace DoMCModuleControl
{
    /// <summary>
    /// Главный контроллер управляющий всей программой
    /// </summary>
    public class MainController
    {
        private static string appsettingsPath = "appsettings.json";
        private static string MainInterfaceFieldString = "MainUserInterface";
        /// <summary>
        /// список команд(паттерн команда), которые могут выполняться, к которым можно получить доступ по именам
        /// </summary>
        private readonly Dictionary<string, Command.CommandInfo> _commands = new Dictionary<string, Command.CommandInfo>();
        /// <summary>
        /// список модулей, к которым можно получить доступ по именам
        /// </summary>
        private readonly Dictionary<string, Modules.ModuleBase> _modules = new Dictionary<string, Modules.ModuleBase>();
        /// <summary>
        /// конфигурация и данные работы приложения
        /// </summary>
        public ApplicationContext Context { get; private set; }
        /// <summary>
        /// Основной интерфейс программы
        /// </summary>
        public Interface.MainUserInterface MainUserInterface { get; private set; }

        public MainController(ApplicationContext context, Type mainUserInterfaceType)
        {
            Context = context;
            // Создаем экземпляр интерфейса
            var mainUserInterface = (Interface.MainUserInterface?)Activator.CreateInstance(mainUserInterfaceType, this);
            if (mainUserInterface == null)
            {
                throw new InvalidOperationException($"Не удалось создать экземпляр типа '{mainUserInterfaceType.Name}'.");
            }
            MainUserInterface = mainUserInterface;
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
        /// Инициализация всех модулей
        /// </summary>
        public void InitializeModules()
        {
            foreach (var module in _modules)
            {
                module.Value.Initialize();
            }
        }
        /// <summary>
        /// Подготовка всех модулей к уничтожению
        /// </summary>
        public void ShutdownModules()
        {
            foreach (var module in _modules)
            {
                module.Value.Shutdown();
            }
        }
        /// <summary>
        /// Запуск модуля по имени
        /// </summary>
        /// <param name="moduleName">Имя модуля</param>
        public void StartModule(string moduleName)
        {
            if (_modules.TryGetValue(moduleName, out var module))
            {
                module.Start();
            }
            else
            {
                Console.WriteLine($"Module '{moduleName}' not found.");
            }
        }
        /// <summary>
        /// Остановка модуля по имени
        /// </summary>
        /// <param name="moduleName">Имя модуля</param>
        public void StopModule(string moduleName)
        {
            if (_modules.TryGetValue(moduleName, out var module))
            {
                module.Stop();
            }
            else
            {
                Console.WriteLine($"Module '{moduleName}' not found.");
            }
        }
        /// <summary>
        /// Создание основного контроллера с созданием модулей из библиотек в папке Modules/*.dll
        /// </summary>
        /// <returns>Новый главный контроллер</returns>
        public static MainController LoadModules(ApplicationContext context)
        {

            var StartingConfiguration = GetMainApplicationConfiguration();

            // Поиск всех сборок с модулями
            var moduleAssemblies = Directory.GetFiles("Modules", "*.dll");
            List<Type> UITypes = new List<Type>();
            List<Type> ModuleTypes = new List<Type>();

            foreach (var assemblyPath in moduleAssemblies)
            {
                var assembly = Assembly.LoadFrom(assemblyPath);

                // Поиск всех классов, реализующих ModuleBase
                var moduleTypes = assembly.GetTypes().Where(t => typeof(Modules.ModuleBase).IsAssignableFrom(t) && !t.IsInterface);
                ModuleTypes.AddRange(moduleTypes);
                var uiTypes = assembly.GetTypes().Where(t => typeof(Interface.MainUserInterface).IsAssignableFrom(t) && !t.IsInterface).ToList();
                UITypes.AddRange(uiTypes);
            }
            Type UIType;
            var interfaceClassName = StartingConfiguration[MainInterfaceFieldString];
            if (!String.IsNullOrEmpty(interfaceClassName))
            {
                var uiType = Type.GetType(interfaceClassName);
                if (uiType == null)
                {
                    throw new InvalidOperationException($"Интерфейс \"{interfaceClassName}\", указанный в файле {appsettingsPath}, не найден.");
                }
                UIType = uiType;
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
            // Проверяем, что найденный тип наследуется от MainUserInterface
            if (!typeof(Interface.MainUserInterface).IsAssignableFrom(UIType))
            {
                throw new InvalidOperationException($"Тип интерфейса '{interfaceClassName}' не является наследником MainUserInterface.");
            }

            var mainController = new MainController(context, UIType);

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

            mainController.InitializeModules();
            return mainController;
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
        public void RegisterCommand(Command.CommandInfo commandInfo)
        {
            if (commandInfo == null || commandInfo.CommandName == null) return;
            _commands[commandInfo.CommandName] = commandInfo;
        }
        /// <summary>
        /// Создание команды по имени (из списка известных команд создается экземпляр команды, который выполняет код
        /// </summary>
        /// <param name="commandName">Текстовое имя команды</param>
        /// <returns>Созданная команда</returns>
        /// <exception cref="ArgumentNullException">Возникает, если класс команды не задан</exception>
        /// <exception cref="ArgumentException">Возникает, если команда не найдена в списке зарегистрированых</exception>
        public Command.CommandBase? CreateCommand(string commandName)
        {
            if (_commands.TryGetValue(commandName, out var commandInfo))
            {
                if (commandInfo.CommandClass != null)
                {
                    return (Command.CommandBase?)Activator.CreateInstance(commandInfo.CommandClass, commandInfo.Module, commandInfo.InputType, commandInfo.OutputType);
                }
                throw new ArgumentNullException($"В команде \"{commandName}\" не задан код выполнения команды(ее класс)");//new InvalidOperationException($"Command class '{commandInfo.CommandClass.Name}' not found.");
            }
            throw new ArgumentException($"Команда \"{commandName}\" не зарегистрирована.");
        }


    }

}