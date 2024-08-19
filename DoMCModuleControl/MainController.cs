using System.Reflection;

namespace DoMCModuleControl
{
    public class MainController
    {
        private readonly List<IModule> _modules = new List<IModule>();

        public void RegisterModule(IModule module)
        {
            _modules.Add(module);
        }

        public void InitializeModules()
        {
            foreach (var module in _modules)
            {
                module.Initialize();
            }
        }

        public static MainController LoadModules()
        {
            var mainController = new MainController();

            // Поиск всех сборок с модулями
            var moduleAssemblies = Directory.GetFiles("Modules", "*.dll");

            foreach (var assemblyPath in moduleAssemblies)
            {
                var assembly = Assembly.LoadFrom(assemblyPath);

                // Поиск всех классов, реализующих IModule
                var moduleTypes = assembly.GetTypes().Where(t => typeof(IModule).IsAssignableFrom(t) && !t.IsInterface);

                foreach (var moduleType in moduleTypes)
                {
                    // Создание экземпляра модуля и регистрация его в MainController
                    var module = (IModule)Activator.CreateInstance(moduleType);
                    if (module == null)
                    {
                        //TODO: залогировать, что модуль не IModule
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

        public void SetStandartModules(IModule CCDCards, IModule LCB, IModule Database, IModule Archive, IModule RDPB)
        {
            var mainController = new MainController();

            mainController.RegisterModule(CCDCards);
            mainController.RegisterModule(LCB);
            mainController.RegisterModule(Database);
            mainController.RegisterModule(Archive);
            mainController.RegisterModule(RDPB);

            mainController.InitializeModules();

        }

    }

}