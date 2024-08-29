//using DoMCUserInterface;
namespace DoMC
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // var type = typeof(DoMCUserInterface.WorkingForm);
            DoMCModuleControl.AssemblyLoader.LoadAssembliesFromEXEPath();
            var mc = DoMCModuleControl.MainController.Create();
            mc.RegisterAllCommands();
            var cl = mc.GetRegisteredCommandList();
            var ui = mc.GetMainUserInterface();
            //ui.Show();
            Application.Run((Form)ui);

        }
    }
}