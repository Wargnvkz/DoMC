using DoMCModuleControl;

namespace DoMC
{
    internal static class Program
    {
        private static DoMCLib.Classes.DoMCApplicationContext Context;
        static MainController Controller;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Context = new DoMCLib.Classes.DoMCApplicationContext();
            Context.Configuration.Load();
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            AssemblyLoader.LoadAssembliesFromPath(".");
            Controller = MainController.LoadDLLModules(Context);
            Controller.RegisterAllCommands();
            Application.Run((Form)Controller.GetMainUserInterface());
        }
    }
}