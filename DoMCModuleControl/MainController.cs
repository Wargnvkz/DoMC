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
using System.Runtime.InteropServices;
using DoMCModuleControl.External;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        /// список модулей, к которым можно получить доступ по именам
        /// </summary>
        private readonly Dictionary<string, Modules.AbstractModuleBase> _modules = [];

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
        private IMainUserInterface MainUserInterface;
        private Type MainUserInterfaceType;

        public MainController(IFileSystem? logFileSystem = null)
        {
            MainFileLogger = new BaseFilesLogger(logFileSystem == null ? new FileSystem() : logFileSystem);
            MainSystemLogger = new BaseSystemLogger(new ExternalFileSystem());
            SystemLogger = new Logger("DoMC", MainSystemLogger);
            MainFileLogger.RegisterExternalLogger(SystemLogger);
            Observer = new Observer(GetLogger("Events"));
        }

        /// <summary>
        /// Создает модуль управления. Перед вызовом нужно вызвать AssemblyLoader.LoadAssembliesFromPath иначе нужные библиотеки с командами, модулями и интрефейсом не будут найдены
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static MainController Create(object? data)
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
            var mainController = new MainController(null);
            mainController.CreateUserInterface(UIType, data);
            return mainController;
        }

        /// <summary>
        /// регистрация модуля
        /// </summary>
        /// <param name="module">экземпляр модуля</param>
        public void RegisterModule(Modules.AbstractModuleBase module)
        {
            if (module != null)
            {
                var modulename = module.GetType().Name;
                if (!_modules.ContainsKey(modulename))
                    _modules.Add(modulename, module);
            }
        }


        /// <summary>
        /// Создание основного контроллера с созданием модулей из библиотек в папке Modules/*.dll. Перед вызовом нужно вызвать AssemblyLoader.LoadAssembliesFromPath иначе нужные библиотеки с командами, модулями и интрефейсом не будут найдены
        /// </summary>
        /// <returns>Новый главный контроллер</returns>
        public static MainController LoadDLLModulesAndRegisterCommands(object? data = null)
        {

            var StartingConfiguration = GetMainApplicationConfiguration();
            List<string> AssembliesNames = new List<string>();
            string[] moduleAssemblies;
            // Поиск всех сборок с модулями
            try
            {
                AssembliesNames.AddRange(Directory.GetFiles(".", "*.dll"));
            }
            catch
            {
            }
            try
            {
                AssembliesNames.AddRange(Directory.GetFiles("Modules", "*.dll"));
            }
            catch
            {
            }
            moduleAssemblies = AssembliesNames.ToArray();
            List<Type> UITypes = [];
            List<Type> ModuleTypes = [];

            foreach (var assemblyPath in moduleAssemblies)
            {
                var assembly = Assembly.LoadFrom(assemblyPath);
            }
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                // Поиск всех классов, реализующих AbstractModuleBase
                var moduleTypes = assembly.GetTypes().Where(t => typeof(Modules.AbstractModuleBase).IsAssignableFrom(t) && !t.IsInterface);
                ModuleTypes.AddRange(moduleTypes);
                var uiTypes = assembly.GetTypes().Where(t => typeof(IMainUserInterface).IsAssignableFrom(t) && !t.IsInterface).ToList();
                UITypes.AddRange(uiTypes);
            }
            Type UIType;
            var interfaceClassName = StartingConfiguration[MainInterfaceFieldString];
            if (!string.IsNullOrEmpty(interfaceClassName))
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


            var mainController = new MainController(null);

            foreach (var moduleType in ModuleTypes)
            {
                if (!moduleType.IsAbstract)
                {
                    // Создание экземпляра модуля и регистрация его в MainController
                    var module = (Modules.AbstractModuleBase?)Activator.CreateInstance(moduleType, mainController);
                    if (module == null)
                    {
                        //TODO: залогировать, что модуль не AbstractModuleBase
                    }
                    else
                    {
                        mainController.RegisterModule(module);
                    }
                }
            }
            mainController.CreateUserInterface(UIType, data);

            return mainController;
        }

        public void CreateUserInterface(Type mainUserInterfaceType, object? data = null)
        {
            if (mainUserInterfaceType == null) throw new ArgumentNullException(nameof(mainUserInterfaceType));
            var mainUserInterface = (IMainUserInterface?)Activator.CreateInstance(mainUserInterfaceType);
            MainUserInterface = mainUserInterface ?? throw new InvalidOperationException($"Не удалось создать экземпляр типа \"{mainUserInterfaceType.Name}\".");
            MainUserInterface.SetMainController(this, data);
        }
        public void CreateUserInterface(object? data = null)
        {
            if (MainUserInterfaceType == null) throw new ArgumentNullException(nameof(MainUserInterfaceType));
            var MainUserInterface = (IMainUserInterface?)Activator.CreateInstance(MainUserInterfaceType);
            MainUserInterface = MainUserInterface ?? throw new InvalidOperationException($"Не удалось создать экземпляр типа \"{MainUserInterfaceType.Name}\".");
            MainUserInterface.SetMainController(this, data);
        }

        public void SetUserInterface(IMainUserInterface userInterface)
        {
            MainUserInterface = userInterface;
        }

        public void FindAndRegisterAllModules()
        {

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var moduleTypes = new List<Type>();
            foreach (var assembly in assemblies)
            {
                moduleTypes.AddRange(assembly.GetTypes().Where(t => typeof(AbstractModuleBase).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface));
            }

            foreach (var moduleType in moduleTypes)
            {
                var module = (Modules.AbstractModuleBase?)Activator.CreateInstance(moduleType, this);
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

        public AbstractModuleBase GetModule(Type ModuleType)
        {
            var moduleInstanceFound = _modules.Where(ni => ni.Value.GetType() == ModuleType);
            return moduleInstanceFound.FirstOrDefault().Value;
        }

        public AbstractModuleBase GetModule(string ModuleName)
        {
            var moduleInstanceFound = _modules.Where(ni => ni.Key == ModuleName);
            return moduleInstanceFound.FirstOrDefault().Value;
        }

    }

}