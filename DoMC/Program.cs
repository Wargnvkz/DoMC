using DoMCLib.Classes;
using DoMCLib.Classes.Module.API;
using DoMCModuleControl;
using WorkshopEquipmentData;

namespace DoMC
{
    internal static class Program
    {
        private static DoMCLib.Classes.DoMCApplicationContext Context;
        static MainController Controller;
        static StartingForm startingForm;
        static Thread splashThread;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            ApplicationConfiguration.Initialize();

            splashThread = new Thread(ShowSplash);
            splashThread.SetApartmentState(ApartmentState.STA);
            splashThread.Start();

            try
            {
                Application.ThreadException += Application_ThreadException;
                Context = new DoMCLib.Classes.DoMCApplicationContext();
                Context.Configuration.LoadConfiguration();
                AssemblyLoader.LoadAssembliesFromPath(".");
                Controller = MainController.LoadDLLModulesAndRegisterCommands(Context);
            }
            finally { CloseSplash(); }

            var mainForm = (Form)Controller.GetMainUserInterface();
            mainForm.Shown += (sender, args) => mainForm.Activate();
            new DoMCLib.Classes.Module.API.Commands.StartRESTAPIServerCommand(Controller, Controller.GetModule(typeof(APIModule))).ExecuteCommandAsync().FireAndForget();
            //APIModule
            Application.Run(mainForm);
            new DoMCLib.Classes.Module.API.Commands.StopRESTAPIServerCommand(Controller, Controller.GetModule(typeof(APIModule))).ExecuteCommandAsync().FireAndForget();
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            DisplayMessage.Show(e.Exception.Message, "Îרטבךא  ג ןנמדנאללו");
            Controller.GetLogger("Îרטבךא ןנמדנאללû").Add(DoMCModuleControl.Logging.LoggerLevel.Critical, e.Exception.Message);
        }

        static void ShowSplash()
        {
            startingForm = new StartingForm();
            Application.Run(startingForm);
        }

        static void CloseSplash()
        {
            if (startingForm != null && startingForm.InvokeRequired)
            {
                startingForm.Invoke(new Action(() =>
                {
                    startingForm.Close();
                }));
            }
            else
            {
                startingForm?.Close();
            }
        }
    }
}