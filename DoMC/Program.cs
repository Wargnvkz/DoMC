using DoMCLib.Classes;
using DoMCLib.Classes.Module.API;
using DoMCModuleControl;
using DoMCModuleControl.External;
using DoMCModuleControl.Logging;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using WorkshopEquipmentData;


namespace DoMC
{
    internal static class Program
    {
        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern int SetCurrentProcessExplicitAppUserModelID(string appID);

        private static DoMCLib.Classes.DoMCApplicationContext Context;
        static MainController Controller;
        static StartingForm startingForm;
        static Thread splashThread;

        //Logger
        static BaseFilesLogger appLogger;
        static FileSystem appFileSystem;
        static string AppLogSectionName = "ApplicationLog";

        static string watchDogCmdLine = "--watchdog";
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            appFileSystem = new FileSystem();
            appLogger = new BaseFilesLogger(appFileSystem);
            if (args.Contains(watchDogCmdLine))
            {
                SetCurrentProcessExplicitAppUserModelID("DoMC.Watchdog");
                do
                {
                    var res = GetRunApplication();
                    if (res.Interfaces.Count == 0)
                    {
                        appLogger.AddMessage(AppLogSectionName, "WatchDog: Программа завершена");
                        appLogger.Flush(AppLogSectionName);
                        break;
                    }
                    if (res.watchDogs.Count > 1)
                    {
                        var firstwatchdog = res.watchDogs.OrderBy(wd => wd.Id).First();
                        if (Process.GetCurrentProcess().Id != firstwatchdog.Id) break;
                    }
                    Thread.Sleep(3000);
                } while (true);

            }
            else
            {
                SetCurrentProcessExplicitAppUserModelID("DoMC.Main");
                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                appLogger.AddMessage(AppLogSectionName, "---- Запуск программы ----");
                ApplicationConfiguration.Initialize();

                System.Timers.Timer timer = new System.Timers.Timer(5000);
                timer.Elapsed += (s, e) =>
                {
                    var res = GetRunApplication();
                    if (res.watchDogs.Count == 0)
                    {
                        var filename = System.Environment.ProcessPath;
                        if (filename != null)
                        {
                            ProcessStartInfo psi = new ProcessStartInfo(filename);
                            psi.ArgumentList.Add(watchDogCmdLine);
                            psi.UseShellExecute = true;
                            appLogger.AddMessage(AppLogSectionName, "Запуск watchdog");
                            Process.Start(psi);
                        }
                    }
                };
                timer.Start();


                appLogger.AddMessage(AppLogSectionName, "Показываем экран загрузки");
                splashThread = new Thread(ShowSplash);
                splashThread.SetApartmentState(ApartmentState.STA);
                splashThread.Start();

                try
                {
                    //Логируем ошибки и выходы
                    Application.ThreadException += Application_ThreadException;
                    AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
                    Microsoft.Win32.SystemEvents.SessionEnding += SystemEvents_SessionEnding;
                    Microsoft.Win32.SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
                    AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

                    Context = new DoMCLib.Classes.DoMCApplicationContext();
                    appLogger.AddMessage(AppLogSectionName, "  Загрузка конфигурации");
                    Context.Configuration.LoadConfiguration();
                    //Загружаем библиотеки, чтобы они были в памяти для дальнейшего поиска
                    appLogger.AddMessage(AppLogSectionName, "  Загрузка загрузка библиотек");
                    AssemblyLoader.LoadAssembliesFromPath(".");
                    appLogger.AddMessage(AppLogSectionName, "  Регистрация команд модулей");
                    Controller = MainController.LoadDLLModulesAndRegisterCommands(Context);
                }
                catch (Exception ex)
                {
                    appLogger.AddMessage(AppLogSectionName, $"Ошибка при запуске программы. {ex.Message} {ex.StackTrace}");
                    DoMCForms.Dialogs.DisplayMessage.Show(ex.Message, "Ошибка  в программе");
                    return;
                }
                finally
                {
                    appLogger.AddMessage(AppLogSectionName, "Закрываем экран загрузки");
                    CloseSplash();
                }

                appLogger.AddMessage(AppLogSectionName, "Ищем основной интерфейс");
                var mainForm = (Form)Controller.GetMainUserInterface();
                if (mainForm == null)
                {
                    var errMsg = $"Ошибка. Не найден основной интерфейс программы";
                    appLogger.AddMessage(AppLogSectionName, errMsg);
                    DoMCForms.Dialogs.DisplayMessage.Show(errMsg, "Ошибка  в программе");
                    return;
                }
                mainForm.Shown += (sender, args) => mainForm.Activate();
                appLogger.AddMessage(AppLogSectionName, "Задаем конфигурацию сервера REST API");
                new DoMCLib.Classes.Module.API.Commands.SetContextRESTAPIServerCommand(Controller, Controller.GetModule(typeof(APIModule))).ExecuteCommandAsync(new() { context = Context, controller = Controller }).FireAndForget();
                appLogger.AddMessage(AppLogSectionName, "Запускаем сервер REST API");
                new DoMCLib.Classes.Module.API.Commands.StartRESTAPIServerCommand(Controller, Controller.GetModule(typeof(APIModule))).ExecuteCommandAsync().FireAndForget();
                //APIModule
                appLogger.AddMessage(AppLogSectionName, "Показываем основную форму и запускаем приложение");
                Application.Run(mainForm);
                timer.Stop();
                appLogger.AddMessage(AppLogSectionName, "Закрыт главный интерфейс.");
                appLogger.AddMessage(AppLogSectionName, "Останавливаем сервер REST API");
                new DoMCLib.Classes.Module.API.Commands.StopRESTAPIServerCommand(Controller, Controller.GetModule(typeof(APIModule))).ExecuteCommandAsync().FireAndForget();
                appLogger.AddMessage(AppLogSectionName, "---- Выход из программы ----");

            }
        }
        private static (List<Process> watchDogs, List<Process> Interfaces) GetRunApplication()
        {
            var currentProcess = Process.GetCurrentProcess();
            var processName = currentProcess.ProcessName;
            var processes = Process.GetProcessesByName(processName);
            List<Process> watchdogs = new List<Process>();
            List<Process> interfaces = new List<Process>();
            foreach (var process in processes)
            {
                var cmdLine = GetCommandLine(process.Id);
                if (cmdLine != null && cmdLine.Contains(watchDogCmdLine))
                {
                    watchdogs.Add(process);
                }
                else
                {
                    interfaces.Add(process);
                }
            }
            return (watchdogs, interfaces);
        }
        static string GetCommandLine(int pid)
        {
            using var searcher = new ManagementObjectSearcher(
                $"SELECT CommandLine FROM Win32_Process WHERE ProcessId = {pid}");

            foreach (ManagementObject obj in searcher.Get())
            {
                return obj["CommandLine"]?.ToString();
            }

            return null;
        }

        private static void SystemEvents_PowerModeChanged(object sender, Microsoft.Win32.PowerModeChangedEventArgs e)
        {
            appLogger.AddMessage(AppLogSectionName, $"Изменение режима питания на {e.Mode}");
        }

        private static void SystemEvents_SessionEnding(object sender, Microsoft.Win32.SessionEndingEventArgs e)
        {
            appLogger.AddMessage(AppLogSectionName, $"Выход из системы: {e.Reason}");
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var errMsg = "Необработанное исключение.";
            string? errReason;
            string? errStackTrace;
            if (e.ExceptionObject is Exception ex)
            {
                errReason = ex.Message;
                errStackTrace = ex.StackTrace;
                appLogger.AddMessage(AppLogSectionName, $"{errMsg} .NET {errReason} {errStackTrace}");
            }
            else
            {
                errReason = e.ExceptionObject.ToString();
                appLogger.AddMessage(AppLogSectionName, $"{errMsg} Unsafe {errReason}");
            }
            DoMCForms.Dialogs.DisplayMessage.Show(errReason ?? "", "Ошибка  в программе");
        }

        private static void CurrentDomain_ProcessExit(object? sender, EventArgs e)
        {
            appLogger.AddMessage(AppLogSectionName, $"Процесс завершен");
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            appLogger.AddMessage(AppLogSectionName, $"Ошибка при запуске программы. {e.Exception.Message} {e.Exception.StackTrace}");
            DoMCForms.Dialogs.DisplayMessage.Show(e.Exception.Message, "Ошибка  в программе");
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