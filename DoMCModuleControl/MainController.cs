using System.Data;
using System;
using System.Reflection;
using System.Windows.Input;

namespace DoMCModuleControl
{
    /// <summary>
    /// Главный контроллер управляющий всей программой
    /// </summary>
    public class MainController
    {
        /// <summary>
        /// список команд(паттерн команда), которые могут выполняться, к которым можно получить доступ по именам
        /// </summary>
        private readonly Dictionary<string, CommandInfo> _commands = new Dictionary<string, CommandInfo>();
        /// <summary>
        /// список модулей, к которым можно получить доступ по именам
        /// </summary>
        private readonly Dictionary<string, ModuleBase> _modules = new Dictionary<string, ModuleBase>();
        /// <summary>
        /// регистрация модуля
        /// </summary>
        /// <param name="module">экземпляр модуля</param>
        public void RegisterModule(ModuleBase module)
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
        public static MainController LoadModules()
        {
            var mainController = new MainController();

            // Поиск всех сборок с модулями
            var moduleAssemblies = Directory.GetFiles("Modules", "*.dll");

            foreach (var assemblyPath in moduleAssemblies)
            {
                var assembly = Assembly.LoadFrom(assemblyPath);

                // Поиск всех классов, реализующих ModuleBase
                var moduleTypes = assembly.GetTypes().Where(t => typeof(ModuleBase).IsAssignableFrom(t) && !t.IsInterface);

                foreach (var moduleType in moduleTypes)
                {
                    // Создание экземпляра модуля и регистрация его в MainController
                    var module = (ModuleBase?)Activator.CreateInstance(moduleType, mainController);
                    if (module == null)
                    {
                        //TODO: залогировать, что модуль не ModuleBase
                    }
                    else
                    {
                        mainController.RegisterModule(module);
                    }
                }
            }

            mainController.InitializeModules();
            return mainController;
        }
        /// <summary>
        /// Добавление стандартных модулей в программу
        /// </summary>
        /// <param name="CCDCards">Модуль чтения 12-ти плат управляющих 8-ю ПЗС матрицами на 96 гнездах</param>
        /// <param name="LCB">Модуль управления Блоком управления светодиодам (БУС)</param>
        /// <param name="Database">Модуль записи данных в базу данных</param>
        /// <param name="Archive">Модуль переноса данных из базы данных в архив</param>
        /// <param name="RDPB">Модуль бракёра (блок удаления дефектной преформы</param>
        public void SetStandardModules(ModuleBase CCDCards, ModuleBase LCB, ModuleBase Database, ModuleBase Archive, ModuleBase RDPB)
        {
            var mainController = new MainController();

            mainController.RegisterModule(CCDCards);
            mainController.RegisterModule(LCB);
            mainController.RegisterModule(Database);
            mainController.RegisterModule(Archive);
            mainController.RegisterModule(RDPB);

            mainController.InitializeModules();

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
        /// <summary>
        /// Создание команды по имени (из списка известных команд создается экземпляр команды, который выполняет код
        /// </summary>
        /// <param name="commandName">Текстовое имя команды</param>
        /// <returns>Созданная команда</returns>
        /// <exception cref="ArgumentNullException">Возникает, если класс команды не задан</exception>
        /// <exception cref="ArgumentException">Возникает, если команда не найдена в списке зарегистрированых</exception>
        public CommandBase CreateCommand(string commandName)
        {
            if (_commands.TryGetValue(commandName, out var commandInfo))
            {
                var commandType = commandInfo.CommandClass;
                if (commandType != null)
                {
                    return (CommandBase)Activator.CreateInstance(commandType, commandInfo.Module, commandInfo.InputType, commandInfo.OutputType);
                }
                throw new ArgumentNullException($"В команде \"{commandName}\" не задан код выполнения команды(ее класс)");//new InvalidOperationException($"Command class '{commandInfo.CommandClass.Name}' not found.");
            }
            throw new ArgumentException($"Команда \"{commandName}\" не зарегистрирована.");
        }


    }

}