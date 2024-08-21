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
            
            DoMCModuleControl.MainController.LoadModules();
        }
    }
}