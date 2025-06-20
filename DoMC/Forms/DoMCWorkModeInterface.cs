using DoMCLib.Classes;
using DoMCLib.Configuration;
using DoMCLib.Tools;
using DoMCLib.DB;
using DoMCLib.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoMC.Classes;
using DoMCModuleControl.UI;
using DoMCModuleControl.Logging;
using DoMCModuleControl;
using DoMC.UserControls;
using DoMC.Forms;
using DoMCLib.Classes.Configuration.CCD;
using DoMCLib.Classes.Module.CCD;
using DoMCLib.Classes.Module.LCB;
using DoMCLib.Classes.Module.RDPB;
using DoMCLib.Classes.Module.DB;
using DoMCLib.Classes.Module.ArchiveDB;
using static DoMCLib.Classes.Module.LCB.LCBModule;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DoMCLib.Classes.Module.RDPB.Classes;
using System.Diagnostics;
using DoMC.Tools;
using System.Configuration;
using System.CodeDom;
using System.Drawing.Drawing2D;
using DoMCLib.Classes.Module.CCD.Commands.Classes;
using DoMCLib.Classes.Module.LCB.Commands;
using DoMCLib.Classes.Module.ArchiveDB.Commands;
using DoMCLib.Classes.Module.CCD.Commands;

namespace DoMC
{
    public partial class DoMCWorkModeInterface : Form, IDoMCSettingsUpdatedProvider, IMainUserInterface
    {

        int CardTimeout = 30000;
        bool IsConfigurationLoadedSuccessfully;
        bool IsWorkingModeStarted;


        SocketStatuses socketStatuses;

        int[] ErrorsBySockets = new int[96];

        Task WorkingThread;

        //ошибки устройств
        WorkingModule DevicesErrors;

        //работает ли устройство или мы его отключили вручную
        WorkingModule ActiveDevices;
        //PressButton[] DeviceButtons;
        Dictionary<WorkingModule, PressButton> DevicesFlagButtons;

        bool ForceStop;

        DateTime lastDrawCycleTime = DateTime.Now;
        DateTime lastErrorCheck = DateTime.Now;
        DateTime lastBoxRead = DateTime.MinValue;
        TimeSpan lastBoxReadTime = new TimeSpan(0, 2, 0);

        WorkStep WorkingStep;
        long[] WorkingStepTime = new long[Enum.GetNames(typeof(WorkStep)).Count() + 1];

        IMainController Controller;
        ILogger WorkingLog;
        Observer observer;
        DoMCApplicationContext Context;
        DoMCArchiveForm? archiveForm = null;

        CycleImagesCCD? CurrentCycleData;
        DateTime LastSynchosignal = DateTime.MinValue;

        public event EventHandler SettingsUpdated;

        ButtonsPresses[] switches;

        int CCDReadsFailed;
        int CCDReadsFailedMax = 5;
        DateTime CCDStart, CCDEnd;
        bool WasErrorWhileWorking;
        public DoMCInterfaceDataExchangeErrors Errors = new DoMCInterfaceDataExchangeErrors();
        bool wasSynchro = true;

        Timings Timings = new Timings();
        RDPBStatus RDPBCurrentStatus = new RDPBStatus();
        LCBStatus LCBStatus = new LCBStatus();
        ArchiveDBModuleStatus ArchiveDBModuleStatus = new ArchiveDBModuleStatus();
        //DBModuleStatus

        int MaxDegreeOfParallelism = 16;
        bool WasErrorWhileWorked = false;

        int DBCyclesCCDLeftInQueue = 0;
        (Exception, DateTime) LastDBException;
        int TotalDefectCycles = 0;
        CancellationTokenSource WorkCancellationTokenSource;
        DateTime lastSavedConfiguration;
        int SaveTimeoutInSecodns = 300;


        public DoMCWorkModeInterface()
        {
            InitializeComponent();
            Application.ThreadException += Application_ThreadException;
            switches = new ButtonsPresses[] {
                new ButtonsPresses(){ Button= pbStartStop, ButtonPressed=false },
                new ButtonsPresses(){ Button= pbRDPB,ButtonPressed=false },
                new ButtonsPresses(){ Button=pbLocalDB ,ButtonPressed=false},
                new ButtonsPresses(){ Button=pbRemoteDB, ButtonPressed=false }
            };

            MaxDegreeOfParallelism = Environment.ProcessorCount;

        }
        public void SetMainController(IMainController controller, object? data)
        {
            Context = (DoMCLib.Classes.DoMCApplicationContext)data;
            Controller = controller;
            WorkingLog = controller.GetLogger("WorkingMode");
            observer = controller.GetObserver();
            //CurrentContext = config;
            WorkingLog.Add(LoggerLevel.Critical, "Запуск интерфейса настройки");
            observer.NotificationReceivers += Observer_NotificationReceivers;
            NotifyConfigurationUpdated();
            DevicesControlInit();
            PressAllDevicesButton();
            ShowDevicesButtonStatuses();
            LoadArchiveTab();
            StartArchiveDB();
        }

        private async Task Observer_NotificationReceivers(string EventName, object? data)
        {
            if (EventName == DoMCApplicationContext.ConfigurationUpdateEventName)
            {
                var config = data as DoMCLib.Configuration.ApplicationConfiguration;
                if (config == null) return;
                archiveForm?.InvokeAsync(() => archiveForm?.SetParameters(config.HardwareSettings.ArchiveDBConfig.LocalDBPath, config.HardwareSettings.ArchiveDBConfig.ArchiveDBPath));
            }
            if (EventName.EndsWith($"{LEDCommandType.LEDSynchrosignalResponse}.{EventType.Received}"))
            {
                DateTime date;
                if (data == null)
                {
                    date = DateTime.Now;
                }
                else
                {
                    date = (DateTime)data;
                }
                LCBStatus.TimePreviousSyncSignalGot = LCBStatus.TimeOfLCBSynchrosignal;
                LCBStatus.TimeOfLCBSynchrosignal = date;
                wasSynchro = true;
                WorkingLog.Add(LoggerLevel.Critical, $"Пришел синхросигнал БУС.");
            }
            if (EventName.EndsWith($"{LEDCommandType.LEDStatusResponse}.{EventType.Received}"))
            {
                var les = (LEDEquipmentStatus)data!;
                LCBStatus.LEDStatus = les.LEDStatuses;
                LCBStatus.LastLedStatusesGotTime = DateTime.Now;
                WorkingLog.Add(LoggerLevel.Critical, $"Пришел статус светодиодов.");

            }
            var eventNameParts = EventName.Split(".");
            if (eventNameParts.Length >= 3)
            {
                if (eventNameParts[0] == nameof(RDPBModule))
                {
                    if (Enum.TryParse(eventNameParts[1], out RDPBCommandType commandType))
                    {
                        RDPBCurrentStatus = (RDPBStatus)data;
                        WorkingLog.Add(LoggerLevel.Critical, $"Получено от бракера {commandType}. {RDPBCurrentStatus.ToString()}");
                    }
                    else { }
                }
                if (eventNameParts[0] == nameof(DBModule))
                {
                    if (eventNameParts[1] == "CycleSave")
                    {
                        switch (eventNameParts[2])
                        {
                            case "Error":

                                LastDBException = ((Exception)data, DateTime.Now);
                                WorkingLog.Add(LoggerLevel.Critical, $"Ошибка при сохранении цикла в БД");

                                break;

                            case "NonSaved":

                                DBCyclesCCDLeftInQueue = (int)data;

                                break;
                            default:
                                WorkingLog.Add(LoggerLevel.Critical, $"Цикл сохранен в БД.");
                                break;

                        }
                    }
                }
            }
            else
            {
                if (eventNameParts.Length == 2)
                {
                    if (eventNameParts[0] == typeof(ArchiveDBModule).Name)
                    {
                        switch (eventNameParts[1])
                        {
                            case "Error":
                                {
                                    ArchiveDBModuleStatus.LastErrorTime = DateTime.Now;
                                    ArchiveDBModuleStatus.LastError = data as Exception;

                                }
                                break;
                            case "Success":
                                {
                                    ArchiveDBModuleStatus.LastErrorTime = DateTime.MinValue;
                                    ArchiveDBModuleStatus.LastError = null;
                                }
                                break;
                            case "Delete":
                                {
                                    ArchiveDBModuleStatus.LastErrorTime = DateTime.MinValue;
                                    ArchiveDBModuleStatus.LastError = null;
                                }
                                break;
                        }
                    }
                }
            }
        }

        private void DevicesControlInit()
        {
            //DeviceButtons = new PressButton[] { pbRDPB, pbLocalDB, pbRemoteDB };
            DevicesFlagButtons = new Dictionary<WorkingModule, DoMC.UserControls.PressButton>()
            {
                {WorkingModule.RDPB, pbRDPB},
                {WorkingModule.LocalDB, pbLocalDB},
                {WorkingModule.RemoteDB, pbRemoteDB},
                {WorkingModule.LCB, pbLCB }
            };
        }

        private void ShowDevicesButtonStatuses()
        {

            foreach (WorkingModule wm in Enum.GetValues(typeof(WorkingModule)))
            {
                if (DevicesErrors.HasFlag(wm))
                {
                    DevicesFlagButtons[wm].BackColor = Color.Red;

                }
                else
                {
                    DevicesFlagButtons[wm].BackColor = Color.LimeGreen;
                }
            }

        }

        private void GetIsDevicesButtonWorking()
        {
            ActiveDevices = 0;
            foreach (WorkingModule wm in Enum.GetValues(typeof(WorkingModule)))
            {
                if (DevicesFlagButtons[wm].IsPressed)
                {
                    ActiveDevices |= wm;
                }
            }
        }

        private void PressAllDevicesButton()
        {
            ActiveDevices = 0;
            foreach (WorkingModule wm in Enum.GetValues(typeof(WorkingModule)))
            {
                ActiveDevices |= wm;
            }
            SetWorkingStatusesOfDevicesButtons();
        }

        private void SetWorkingStatusesOfDevicesButtons()
        {
            foreach (WorkingModule wm in Enum.GetValues(typeof(WorkingModule)))
            {
                if (ActiveDevices.HasFlag(wm))
                {
                    DevicesFlagButtons[wm].IsPressed = true;
                }
                else
                {
                    DevicesFlagButtons[wm].IsPressed = false;
                }
            }
        }

        private void LoadArchiveTab()
        {
            archiveForm = new DoMC.Forms.DoMCArchiveForm(Controller, Context.Configuration.HardwareSettings.ArchiveDBConfig.LocalDBPath, Context.Configuration.HardwareSettings.ArchiveDBConfig.ArchiveDBPath);
            archiveForm.TopLevel = false;
            archiveForm.Parent = tbpArchive;
            archiveForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            archiveForm.StartPosition = FormStartPosition.Manual;
            archiveForm.Location = new Point(0, 0);
            //tbpArchive.Scale(new SizeF(1f, 1f));
            archiveForm.Scale(new SizeF(1f, 1f));
            archiveForm.Size = new Size(tbpArchive.ClientSize.Width, tbpArchive.ClientSize.Height);
            archiveForm.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            archiveForm.Visible = true;

        }

        private void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            WorkingLog.Add(LoggerLevel.Critical, "Необработанное исключение: ", e.Exception);
            if (e.Exception is DoMCException)
            {
                DisplayMessage.Show(e.Exception.Message, "Ошибка");
            }
            else
            {
                DisplayMessage.Show(e.Exception.Message + "\r\n" + e.Exception.StackTrace, "Ошибка");
            }
        }

        private async void btnStartStop_Click(object sender, EventArgs e)
        {
            if (IsWorkingModeStarted)
            {
                StopWork();
            }
            else
            {
                socketStatuses = new SocketStatuses(96);
                WorkCancellationTokenSource = new CancellationTokenSource();
                GetIsDevicesButtonWorking();
                if (await PrepareToStartWork())
                {
                    StartReading();
                }
                else
                {
                    StartFailed();
                }
            }
        }

        private async Task<bool> PrepareToStartWork()
        {
            string ErrorMsg;
            ResetTemporaryStatistics();
            WorkingLog.Add(LoggerLevel.Critical, "");
            IsConfigurationLoadedSuccessfully = false;
            WasErrorWhileWorked = false;
            Errors.CCDNotRespond = false;
            WorkingLog.Add(LoggerLevel.Critical, "Загрузка конфигурации гнезд");
            if (!(await DoMCEquipmentCommands.StartCCD(Controller, Context, WorkingLog)).Item1)
            {
                WorkingLog.Add(LoggerLevel.Critical, "Ошибка при подключении к платам ПЗС");
                Errors.CCDNotRespond = true;
                DoMCNotAbleLoadConfigurationErrorMessage();
                return false;
            }

            if (!(await DoMCEquipmentCommands.LoadCCDConfiguration(Controller, Context, WorkingLog)).Item1)
            {
                WorkingLog.Add(LoggerLevel.Critical, "Ошибка загрузки конфигураций гнезд. Остановка работы");
                Errors.CCDNotRespond = true;
                DoMCNotAbleLoadConfigurationErrorMessage();
                return false;
            }

            if (!(await DoMCEquipmentCommands.SetFastRead(Controller, Context, WorkingLog)).Item1)
            {
                WorkingLog.Add(LoggerLevel.Critical, "Ошибка при подключении к платам ПЗС");
                Errors.CCDNotRespond = true;
                DoMCNotAbleLoadConfigurationErrorMessage();
                return false;
            }


            Errors.LCBDoesNotRespond = false;
            if (ActiveDevices.HasFlag(WorkingModule.LCB))
            {
                WorkingLog.Add(LoggerLevel.Critical, "Запуск модуля БУС и загрузка конфигурации");

                if (!await DoMCEquipmentCommands.StartLCB(Controller, WorkingLog))
                {
                    WorkingLog.Add(LoggerLevel.Critical, $"Ошибка при запуске БУС. Остановка работы");
                    Errors.LCBDoesNotRespond = true;
                    DoMCNotAbleLoadConfigurationErrorMessage();
                    return false;
                }
                else
                {
                    Errors.LCBDoesNotRespond = false;
                }


                var startLCBEWorkModeTime = DateTime.Now;
                if (!await SetWorkingModeLCB())
                {
                    WorkingLog.Add(LoggerLevel.Critical, $"Ошибка при запуске БУС в работу. Остановка работы");
                    return false;
                }
            }
            IsConfigurationLoadedSuccessfully = true;
            if (ActiveDevices.HasFlag(WorkingModule.LocalDB))
            {

                WorkingLog.Add(LoggerLevel.Critical, "Запуск модуля работы с базой данных");
                await new SetConfigurationCommand(Controller, Controller.GetModule(typeof(LCBModule))).ExecuteCommandAsync(Context.Configuration.HardwareSettings.ArchiveDBConfig);
                await new DoMCLib.Classes.Module.CCD.Commands.StartCommand(Controller, Controller.GetModule(typeof(LCBModule))).ExecuteCommandAsync();

                /*if (Errors.NoLocalSQL)
                {
                    ErrorMsg = "Ошибка при запуске модуля работы с базой данных";
                    WorkingLog.Add(LoggerLevel.Critical, ErrorMsg);
                    DevicesErrors |= WorkingModule.LocalDB;
                    if (ActiveDevices.HasFlag(WorkingModule.LocalDB))
                    {
                        WorkingLog.Add(LoggerLevel.Critical, "Остановка работы");
                        DoMCShowErrorMessage(ErrorMsg);
                        return false;
                    }
                    WorkingLog.Add(LoggerLevel.Critical, "Ошибка пропущена");

                }*/
                Errors.NoLocalSQL = false;
            }
            else
            {
                Errors.NoLocalSQL = true;
            }

            /*if (ActiveDevices.HasFlag(WorkingModule.RemoteDB))
            {
                WorkingLog.Add(LoggerLevel.Critical, "Запуск модуля работы с базой данных архива");
                Controller.CreateCommandInstance(typeof(ArchiveDBModule.SetConfigurationCommand)).ExecuteCommand(Context.Configuration.HardwareSettings.ArchiveDBConfig);
                Controller.CreateCommandInstance(typeof(ArchiveDBModule.StartCommand)).ExecuteCommand();
                if (Errors.NoRemoteSQL)
                {
                    ErrorMsg = "Ошибка при запуске модуля работы с архивом";
                    WorkingLog.Add(LoggerLevel.Critical, ErrorMsg);
                    DevicesErrors |= WorkingModule.RemoteDB;
                    if (ActiveDevices.HasFlag(WorkingModule.RemoteDB))
                    {
                        WorkingLog.Add(LoggerLevel.Critical, "Остановка работы");
                        DoMCShowErrorMessage(ErrorMsg);
                        return false;
                    }
                    WorkingLog.Add(LoggerLevel.Critical, "Ошибка пропущена");
                }
            }
            else
            {
                Errors.NoRemoteSQL = true;
            }*/

            if (ActiveDevices.HasFlag(WorkingModule.RDPB))
            {
                if (!await StartRDPB())
                {
                    return false;
                }
            }

            pbStartStop.Text = "Стоп";
            IsWorkingModeStarted = true;
            pbStartStop.BackColor = Color.Green;

            IsConfigurationLoadedSuccessfully = true;
            return true;
        }

        private async Task<bool> SetRDPBParameters()
        {
            return await new DoMCLib.Classes.Module.RDPB.Commands.LoadConfigurationToModuleCommand(Controller, Controller.GetModule(typeof(RDPBModule))).ExecuteCommandAsync(Context.Configuration);
        }

        private async Task<bool> StartRDPB()
        {
            string ErrorMsg = "";
            WorkingLog.Add(LoggerLevel.Critical, "Установка параметров работы бракёра");
            if (!await SetRDPBParameters())
            {
                WorkingLog.Add(LoggerLevel.Critical, "Ошибка при параметров работы бракёра");
                return false;
            }
            WorkingLog.Add(LoggerLevel.Critical, "Запуск модуля бракёра");
            try
            {
                await new DoMCLib.Classes.Module.RDPB.Commands.StartCommand(Controller, Controller.GetModule(typeof(RDPBModule))).ExecuteCommandAsync();
                RDPBCurrentStatus.IsStarted = true;
            }
            catch { RDPBCurrentStatus.IsStarted = false; }
            if (!RDPBCurrentStatus.IsStarted)
            {
                ErrorMsg = "Ошибка при запуске модуля бракёра";
                WorkingLog.Add(LoggerLevel.Critical, ErrorMsg);
                DevicesErrors |= WorkingModule.RDPB;
                if (ActiveDevices.HasFlag(WorkingModule.RDPB))
                {
                    WorkingLog.Add(LoggerLevel.Critical, "Остановка работы");
                    DoMCShowErrorMessage(ErrorMsg);
                    return false;
                }
                WorkingLog.Add(LoggerLevel.Critical, "Ошибка пропущена");
            }
            DevicesErrors &= ~WorkingModule.RDPB;
            var coolingBlocks = Context.Configuration.HardwareSettings.RemoveDefectedPreformBlockConfig.CoolingBlocksQuantity;
            WorkingLog.Add(LoggerLevel.Critical, $"Установка количества охлаждающих блоков: {coolingBlocks}");
            RDPBCurrentStatus.CoolingBlocksQuantity = coolingBlocks;

            RDPBCurrentStatus = await new DoMCLib.Classes.Module.RDPB.Commands.SetCoolingBlockQuantityCommand(Controller, Controller.GetModule(typeof(RDPBModule))).ExecuteCommandAsync(coolingBlocks);

            Thread.Sleep(10);
            WorkingLog.Add(LoggerLevel.Critical, "Включение бракера");

            await new DoMCLib.Classes.Module.RDPB.Commands.TurnOnCommand(Controller, Controller.GetModule(typeof(RDPBModule))).ExecuteCommandAsync();

            Thread.Sleep(10);

            return true;

        }

        private async Task StopRDPB()
        {

            WorkingLog.Add(LoggerLevel.Critical, "Отключение бракёра");
            await new DoMCLib.Classes.Module.RDPB.Commands.RDPBStopCommand(Controller, Controller.GetModule(typeof(RDPBModule))).ExecuteCommandAsync();
            RDPBCurrentStatus.IsStarted = false;

        }

        public async Task StartArchiveDB()
        {
            WorkingLog.Add(LoggerLevel.Critical, "Запуск модуля архивирования");
            await new DoMCLib.Classes.Module.ArchiveDB.Commands.StartCommand(Controller, Controller.GetModule(typeof(ArchiveDBModule))).ExecuteCommandAsync();
        }
        public async Task StopArchiveDB()
        {
            WorkingLog.Add(LoggerLevel.Critical, "Остановка модуля архивирования");
            await new DoMCLib.Classes.Module.ArchiveDB.Commands.StopCommand(Controller, Controller.GetModule(typeof(ArchiveDBModule))).ExecuteCommandAsync();

        }
        public async Task<bool> GetArchiveDBModuleStatus()
        {
            return ArchiveDBModuleStatus.IsStarted = await new DoMCLib.Classes.Module.ArchiveDB.Commands.GetWorkingStatusCommand(Controller, Controller.GetModule(typeof(ArchiveDBModule))).ExecuteCommandAsync();
        }

        public async Task StartDB()
        {
            WorkingLog.Add(LoggerLevel.Critical, "Запуск модуля базы данных");
            await new DoMCLib.Classes.Module.DB.Commands.StartCommand(Controller, Controller.GetModule(typeof(DBModule))).ExecuteCommandAsync();
        }
        public async Task StopDB()
        {
            WorkingLog.Add(LoggerLevel.Critical, "Остановка модуля базы данных");
            await new DoMCLib.Classes.Module.DB.Commands.StopCommand(Controller, Controller.GetModule(typeof(DBModule))).ExecuteCommandAsync();
        }
        private async Task<bool> SetNonWorkingModeLCB()
        {
            return await new DoMCLib.Classes.Module.LCB.Commands.SetLCBNonWorkModeCommand(Controller, Controller.GetModule(typeof(LCBModule))).ExecuteCommandAsync();

        }
        private async Task<bool> SetWorkingModeLCB()
        {
            return await new DoMCLib.Classes.Module.LCB.Commands.SetLCBWorkModeCommand(Controller, Controller.GetModule(typeof(LCBModule))).ExecuteCommandAsync();

        }

        private void StopWork()
        {
            WorkCancellationTokenSource.Cancel();
            Context.IsInWorkingMode = false;
            try
            {
                StopReading();
            }
            catch
            {
            }
            pbStartStop.Text = "Пуск";

            if (!ForceStop)
            {
                pbStartStop.BackColor = SystemColors.Control;
            }
            else
            {
                pbStartStop.BackColor = Color.OrangeRed;
            }

            WorkingLog.Add(LoggerLevel.Critical, "Остановка БУС");
            //var startLCBEWorkModeTime = DateTime.Now;
            //InterfaceDataExchange.SendCommand(ModuleCommand.SetLCBNonWorkModeRequest);

            WorkingLog.Add(LoggerLevel.Critical, "Остановка чтения и сброс плат ПЗС");
            //InterfaceDataExchange.SendCommand(ModuleCommand.StartIdle);
            //InterfaceDataExchange.SendResetToCCDCards();
            //InterfaceDataExchange.SendCommand(ModuleCommand.CCDStop);
            Thread.Sleep(200);

            SetNonWorkingModeLCB();

            StopRDPB();

            WorkingLog.Add(LoggerLevel.Critical, "Остановка базы данных");
            //InterfaceDataExchange.SendCommand(ModuleCommand.DBStop);
            WorkingLog.Add(LoggerLevel.Critical, "Остановка базы данных архива");
            // InterfaceDataExchange.SendCommand(ModuleCommand.ArchiveDBStop);


            pbStartStop.IsPressed = false;
            StopModules();
        }

        public async Task StartFailed()
        {
            WorkCancellationTokenSource.Cancel();


            await new DoMCLib.Classes.Module.CCD.Commands.CCDStopCommand(Controller, Controller.GetModule(typeof(CCDCardDataModule))).ExecuteCommandAsync();
            await new DoMCLib.Classes.Module.LCB.Commands.LCBStopCommand(Controller, Controller.GetModule(typeof(LCBModule))).ExecuteCommandAsync();
            await new DoMCLib.Classes.Module.RDPB.Commands.RDPBStopCommand(Controller, Controller.GetModule(typeof(RDPBModule))).ExecuteCommandAsync();

            pbStartStop.BackColor = Color.Red;
            pbStartStop.IsPressed = false;
            StopModules();

        }

        private void StartReading()
        {
            WorkingLog.Add(LoggerLevel.Critical, "Запуск чтения");
            IsWorkingModeStarted = true;
            WasErrorWhileWorking = false;
            ForceStop = false;
            WorkingThread = Task.Run(WorkProc);

        }
        private async Task StopReading()
        {

            IsWorkingModeStarted = false;
            try
            {
                await WorkingThread;
            }
            catch { }
            WorkingLog.Add(LoggerLevel.Critical, "Остановка работы");
            await DoMCEquipmentCommands.StopCCD(Controller, Context, WorkingLog);

        }

        private void DoMCShowErrorMessage(string Text)
        {
            MessageBox.Show(Text, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        private async Task WorkProc()
        {
            WorkingLog.Add(LoggerLevel.Critical, "Начало чтения");
            CCDReadsFailed = 0;
            RDPBCurrentStatus.PreviousDirection = RDPBTransporterSide.NotSet;
            var sw = new Stopwatch();
            sw.Start();
            try
            {
                while (IsWorkingModeStarted)
                {
                    WorkingLog.Add(LoggerLevel.Critical, "--------------- Начало цикла чтения ---------------");

                    //Алгоритм
                    //Запускаю ожидание синхроимпульса
                    // (авто)Получение синхросигнала от БУС
                    //Жду ответа от платы, что изображения прочитаны
                    // (авто) получение статуса светодиодов
                    //Проверка получены ли данные по светодиодам
                    // Если нет - Ошибка получения данных по светодиодам, пропустить проверку, сказать, что все преформы хорошие
                    // Если да, проверяем все ли светодиоды горят, если нет выдаем ошибку и проверяем без этих светодиодов
                    // Проверяем изображения и замеряем время
                    // если за время проверки пришел еще один синхросигнал, то все плохо, программа не успевает
                    // сохранить статусы изображений, чтобы показать их
                    // если есть ошибка, сказать бракеру заблокировать этот съем
                    // поставить в очередь на запись
                    //

                    var startCycle = sw.ElapsedTicks;
                    WorkingStep = WorkStep.Prepare;
                    WorkingStepTime[(int)WorkStep.Prepare] = sw.ElapsedTicks;
                    WorkingLog.Add(LoggerLevel.Critical, "Подготовка к чтению");
                    // Инициализация данных текущего цикла
                    var CurrentCycleCCD = new CycleImagesCCD();
                    CurrentCycleCCD.Differences = new short[Context.Configuration.HardwareSettings.SocketQuantity][,];
                    CurrentCycleCCD.CurrentImages = new short[Context.Configuration.HardwareSettings.SocketQuantity][,];
                    CurrentCycleCCD.StandardImages = new short[Context.Configuration.HardwareSettings.SocketQuantity][,];
                    CurrentCycleCCD.IsSocketGood = new bool[Context.Configuration.HardwareSettings.SocketQuantity];
                    CurrentCycleCCD.IsSocketHasImage = new bool[Context.Configuration.HardwareSettings.SocketQuantity];

                    CurrentCycleCCD.SocketsToCheck = new bool[96];
                    Array.Copy(Context.Configuration.HardwareSettings.SocketsToCheck, CurrentCycleCCD.SocketsToCheck, 96);

                    // Если рабочий режим не запущен или система не может работать, потому что не загружена конфигурация, останавливаем и выходим с ошибкой
                    if (!IsWorkingModeStarted || !IsConfigurationLoadedSuccessfully)
                    {
                        IsWorkingModeStarted = false;
                        WasErrorWhileWorking = true;
                        IsConfigurationLoadedSuccessfully = false;
                        break;
                    }

                    WorkingStep = WorkStep.WaitForSyncroSignal;
                    WorkingStepTime[(int)WorkStep.WaitForSyncroSignal] = sw.ElapsedTicks;

                    bool WasLastOperationSuccessful = true;

                    // сохраняем для проверки был ли синхросигнал
                    var synchrosignalTimeBeforeCCDRead = LCBStatus.TimeOfLCBSynchrosignal;

                    CCDStart = DateTime.Now;
                    WorkingLog.Add(LoggerLevel.Critical, "Запуск ожидания результатов чтения");

                    //Запрашиваем чтение ПЗС матриц по внешнему имульсу
                    //WasLastOperationSuccessful = CurrentContext.ReadSockets(Controller, WorkingLog, true);

                    var CCDCardDataCommandResponse=await new DoMCLib.Classes.Module.CCD.Commands.SendReadAllSocketsCommand(Controller, Controller.GetModule(typeof(CCDCardDataModule))).ExecuteCommandAsync(context);


                    WasLastOperationSuccessful = Context.ReadSockets(Controller, WorkingLog, true, out CCDCardDataCommandResponse resdResult, cancellationTokenSource: WorkCancellationTokenSource);
                    //Ждем пока произойдет чтение
                    CCDEnd = DateTime.Now;

                    WorkingStep = WorkStep.ReadingSocketsCompleted;
                    WorkingStepTime[(int)WorkStep.ReadingSocketsCompleted] = sw.ElapsedTicks;

                    // Если таймаут то говорим, что ошибка и выходим
                    if (!WasLastOperationSuccessful && ActiveDevices.HasFlag(WorkingModule.LCB))
                    {
                        WorkingLog.Add(LoggerLevel.Critical, $"Ошибка при чтении данных гнезд. Чтение не завершено.");
                        WorkingLog.Add(LoggerLevel.Critical, "Чтение состояния БУС");
                        var start = DateTime.Now;
                        var LCBres = Controller.CreateCommandInstance(typeof(LCBModule.GetLCBEquipmentStatusCommand)).Wait(null, 2, out LEDEquipmentStatus Status);
                        if (LCBres)
                        {
                            // если БУС - Авария
                            if (Status.Outputs[3])
                            {
                                WorkingLog.Add(LoggerLevel.Critical, "БУС авария");
                                if (Status.Inputs[6])
                                {
                                    WorkingLog.Add(LoggerLevel.Critical, "Маячки спрятаны. Остановка БУС");
                                    if (!Controller.CreateCommandInstance(typeof(LCBModule.SetLCBNonWorkModeCommand)).Wait(null, 2, out bool NWresult) || !NWresult)
                                    {
                                        WorkingLog.Add(LoggerLevel.Critical, "Не удалось остановить работу БУС");
                                        Errors.CCDNotRespond = true;
                                        Errors.LCBDoesNotRespond = true;
                                        IsWorkingModeStarted = false;
                                        WasErrorWhileWorking = true;
                                    }
                                    WorkingLog.Add(LoggerLevel.Critical, "Маячки спрятаны. Перезапуск");
                                    if (!Controller.CreateCommandInstance(typeof(LCBModule.SetLCBNonWorkModeCommand)).Wait(null, 2, out bool Wresult) || !Wresult)
                                    {
                                        WorkingLog.Add(LoggerLevel.Critical, "Не удалось запустить БУС");
                                        Errors.CCDNotRespond = true;
                                        Errors.LCBDoesNotRespond = true;
                                        IsWorkingModeStarted = false;
                                        WasErrorWhileWorking = true;

                                    }
                                    continue;
                                }
                                else
                                {
                                    WorkingLog.Add(LoggerLevel.Critical, "Маячки выдвинуты. Остановка работы");
                                    Errors.CCDNotRespond = true;
                                    Errors.LCBDoesNotRespond = true;
                                    IsWorkingModeStarted = false;
                                    WasErrorWhileWorking = true;
                                    break;
                                }
                            }
                            else
                            {
                                Errors.CCDNotRespond = true;
                                Errors.LCBDoesNotRespond = false;
                                WorkingLog.Add(LoggerLevel.Critical, "БУС - OK");
                            }

                        }
                        else
                        {
                            WorkingLog.Add(LoggerLevel.Critical, "БУС не ответил");
                            Errors.CCDNotRespond = true;
                            Errors.LCBDoesNotRespond = true;
                            IsWorkingModeStarted = false;
                            WasErrorWhileWorking = true;
                            break;
                        }

                        if (CCDReadsFailed >= CCDReadsFailedMax)
                        {
                            WorkingLog.Add(LoggerLevel.Critical, "Ошибка при чтении данных гнезд. Чтение не завершено. Остановка.");
                            Errors.CCDNotRespond = true;
                            IsWorkingModeStarted = false;
                            WasErrorWhileWorking = true;
                            break;
                        }
                        else
                        {
                            CCDReadsFailed++;
                            //wait for syncrosignal
                            WorkingLog.Add(LoggerLevel.Critical, $"Увеличение счетчика ошибок. Ошибок: {CCDReadsFailed}");

                            WorkingLog.Add(LoggerLevel.Critical, $"Ожидание синхросигнала");
                            //ждать синхросигнал за таймаут чтения
                            wasSynchro = false;
                            start = DateTime.Now;
                            while ((DateTime.Now - start).TotalSeconds < Context.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds && !wasSynchro)
                            {
                                if (!IsWorkingModeStarted) throw new DoMCException("Работа остановлена пользователем");
                                Application.DoEvents();
                            }


                            if (wasSynchro)
                            {
                                WorkingLog.Add(LoggerLevel.Critical, $"Синхросигнал получен");
                            }
                            else
                            {
                                if (Errors.LCBDoesNotRespond)
                                {
                                    WorkingLog.Add(LoggerLevel.Critical, $"Синхросигнал не получен. Ошибка работы БУС?");
                                }
                                else
                                {
                                    WorkingLog.Add(LoggerLevel.Critical, $"Синхросигнал не получен. Машина стоит?");
                                }
                            }

                            continue;

                        }
                    }
                    else
                    {
                        CCDReadsFailed = 0;
                    }
                    //если успешно, то ошибки нет
                    Errors.CCDNotRespond = false;

                    /*var synchrotimeOnCCD = InterfaceDataExchange.CardsConnection.TimeSinceLastSynchrosignalOnCCD();
                    WorkingLog.Add(LoggerLevel.Critical, String.Join(" ", synchrotimeOnCCD.Select(s => s / 1000d)));*/


                    //Фиксируем момент, когда закончилось чтение изображений
                    //InterfaceDataExchange.CurrentCycleCCD.CycleCCDDateTime = DateTime.Now;

                    var synchrosignalTimeAfterCCDRead = LCBStatus.TimeOfLCBSynchrosignal;
                    //проверяем пришел ли нам синхросигнал от БУС во время чтения гнезд
                    if (synchrosignalTimeAfterCCDRead == synchrosignalTimeBeforeCCDRead)
                    {
                        //Если нет, то выставляем ошибку и время синхроимульса ставим, как время получения изображений
                        // Тест: Синхроимпульс не ставим, а пропускаем этот съем
                        Errors.LCBDoesNotSendSync = true;
                        Errors.MissedSyncrosignalCounter++;
                        LCBStatus.TimePreviousSyncSignalGot = LCBStatus.TimeOfLCBSynchrosignal;
                        LCBStatus.TimeSyncSignalGotForShowInCycle = CCDStart.AddSeconds((CCDEnd - CCDStart).TotalSeconds / 2);
                        WorkingLog.Add(LoggerLevel.Critical, $"Не обнаружен синхросигнал. Устанавливаем время синхросигнала в {LCBStatus.TimeSyncSignalGotForShowInCycle:dd-MM-yyyy HH\\:mm\\:ss.fff}");
                    }
                    else
                    {
                        //Если пришел, то ошибку убираем
                        Errors.LCBDoesNotSendSync = false;
                        Errors.MissedSyncrosignalCounter = 0;
                        if (LCBStatus.TimeOfLCBSynchrosignal == DateTime.MinValue)
                        {
                            LCBStatus.TimeSyncSignalGotForShowInCycle = CCDStart.AddSeconds((CCDEnd - CCDStart).TotalSeconds / 2);
                            WorkingLog.Add(LoggerLevel.Critical, $"Синхросигнал был, но его время неизвестно. Устанавливаем время синхросигнала в {LCBStatus.TimeSyncSignalGotForShowInCycle:dd-MM-yyyy HH\\:mm\\:ss.fff}");
                        }
                        else
                        {
                            LCBStatus.TimeSyncSignalGotForShowInCycle = LCBStatus.TimeOfLCBSynchrosignal;
                        }
                    }
                    var CycleCCDDateTime = LCBStatus.TimeSyncSignalGotForShowInCycle;

                    //Если прошло меньше 1 секунды между синхросигналом и ответом о завершении, то в программе ошибка
                    WorkingLog.Add(LoggerLevel.Critical, $"Время начала чтения: {CCDStart:dd-MM-yyyy HH\\:mm\\:ss.fff}");
                    WorkingLog.Add(LoggerLevel.Critical, $"Время синхросигнала БУС: {LCBStatus.TimeOfLCBSynchrosignal:dd-MM-yyyy HH\\:mm\\:ss.fff}");
                    WorkingLog.Add(LoggerLevel.Critical, $"Время синхросигнала для цикла: {CycleCCDDateTime:dd-MM-yyyy HH\\:mm\\:ss.fff}");
                    WorkingLog.Add(LoggerLevel.Critical, $"Время завершения чтения: {CCDEnd:dd-MM-yyyy HH\\:mm\\:ss.fff}");
                    /*if (Math.Abs((CCDEnd - CCDStart).TotalSeconds) < 1)
                    {

                        WorkingLog.Add(LoggerLevel.Critical, "Программная ошибка при проверке на готовность данных");
                        WorkingLog.Add(LoggerLevel.Critical, $"Статус модуля: {InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus}");
                        WorkingLog.Add(LoggerLevel.Critical, $"Шаг модуля: {InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep}");

                        InterfaceDataExchange.CardsConnection.WriteSocketStatusLog(WorkingLog, "Статусы гнезд перед посылкой на чтение изображения");
                    }*/

                    //WorkingLog.Add(LoggerLevel.Critical, "Эталоны 3: " + InterfaceDataExchange.SocketStandardExist());

                    WorkingStep = WorkStep.StartReadingImages;
                    WorkingStepTime[(int)WorkStep.StartReadingImages] = sw.ElapsedTicks;

                    WorkingLog.Add(LoggerLevel.Critical, "Запрос на получение изображений гнезд");
                    //Запрашиваем сами изображения гнезд
                    var getImgSuccess = Context.GetSocketsImages(Controller, WorkingLog, out GetImageDataCommandResponse getImageResult, cancellationTokenSource: WorkCancellationTokenSource);
                    WasLastOperationSuccessful = (getImageResult?.CardsAnswered().Count ?? 0) == 0;// .completedSuccessfully.All(c => c);

                    WorkingStep = WorkStep.ReadingImagesCompleted;
                    WorkingStepTime[(int)WorkStep.ReadingImagesCompleted] = sw.ElapsedTicks;

                    //если не получили или в модуле ПЗС ошибка, то выставляем ошибку
                    if (!WasLastOperationSuccessful)
                    {
                        WorkingLog.Add(LoggerLevel.Critical, "Ошибка при получении изображения.");// Остановка");
                        Errors.CCDNotRespond = true;

                        WorkingLog.Add(LoggerLevel.Critical, "Полученные кадры гнезд:");
                        try
                        {
                            for (int i = 0; i < 16; i++)
                            {
                                StringBuilder sb = new StringBuilder();
                                for (int j = 0; j < 6; j++)
                                {
                                    var socketNum = j * 16 + i + 1;
                                    sb.Append(getImageResult.Data[socketNum].ImageData != null ? "+" : " ");
                                    sb.Append("  ");
                                }
                                WorkingLog.Add(LoggerLevel.Critical, sb.ToString());
                                if (i == 7) WorkingLog.Add(LoggerLevel.Critical, "");
                            }
                        }
                        catch { }


                        continue;

                    }
                    else
                    {
                        WorkingLog.Add(LoggerLevel.Critical, "Изображения получены");
                    }

                    var synchrosignalTimeAfterCCDImagesGot = LCBStatus.TimeOfLCBSynchrosignal;
                    if (synchrosignalTimeAfterCCDImagesGot != synchrosignalTimeAfterCCDRead)
                    {
                        Errors.NotEnoughTimeToGetCCD = true;
                        WorkingLog.Add(LoggerLevel.Critical, $"Был получен еще один синхросигнал. Время: {LCBStatus.TimeOfLCBSynchrosignal:dd-MM-yyyy HH\\:mm\\:ss.fff}");

                    }
                    else
                    {
                        Errors.NotEnoughTimeToGetCCD = false;
                    }

                    // иначе все хорошо и ошибки нет
                    Errors.CCDNotRespond = false;
                    // проверяем есть ли что-нибудь в изображениях


                    //если нет, то ошибка и выходим
                    if (getImageResult == null)
                    {
                        WorkingLog.Add(LoggerLevel.Critical, "Изображения не получены.");// Остановка");
                        Errors.CCDFrameNotReceived = true;
                        //InterfaceDataExchange.IsWorkingModeStarted = false;
                        //InterfaceDataExchange.WasErrorWhileWorking = true;
                        //break;
                    }
                    else
                    {
                        //иначе все ок, ошибки нет
                        Errors.CCDFrameNotReceived = false;
                    }

                    //сохраняем изображения в данные текущего цикла
                    CurrentCycleCCD.CurrentImages = getImageResult?.Data?.Select(d => d?.Image ?? null).ToArray() ?? new short[0][,];

                    //если получены данные о состоянии светодиодов, то
                    if (LCBStatus.LastLedStatusesGotTime >= LCBStatus.TimeSyncSignalGotForShowInCycle)
                    {
                        Errors.LEDStatusGettingError = false;
                    }
                    else
                    {
                        Errors.LEDStatusGettingError = true;
                        LCBStatus.LEDStatus = new bool[12];
                    }

                    var BlockLedStatus = new bool[] {
                    LCBStatus.LEDStatus[0],
                    LCBStatus.LEDStatus[3],
                    LCBStatus.LEDStatus[1],
                    LCBStatus.LEDStatus[4],
                    LCBStatus.LEDStatus[2],
                    LCBStatus.LEDStatus[5],
                    LCBStatus.LEDStatus[6],
                    LCBStatus.LEDStatus[9],
                    LCBStatus.LEDStatus[7],
                    LCBStatus.LEDStatus[10],
                    LCBStatus.LEDStatus[8],
                    LCBStatus.LEDStatus[11],
                };
                    LCBStatus.LEDStatus = BlockLedStatus;

                    if (!Errors.CCDFrameNotReceived)
                    {
                        WorkingStep = WorkStep.SearchForDefectedPreforms;
                        WorkingStepTime[(int)WorkStep.SearchForDefectedPreforms] = sw.ElapsedTicks;

                        Timings.CCDImagesProcessStarted = DateTime.Now;

                        // проверяем есть ли статусы и устанавливаем статусы линеек светодиодов в данных текущего цикла
                        CurrentCycleCCD.SetLEDStatuses(BlockLedStatus, LCBStatus.TimeSyncSignalGotForShowInCycle);

                        ImageProcessParameters[] IPParray = new ImageProcessParameters[Context.Configuration.HardwareSettings.SocketQuantity];

                        for (int i = 0; i < Context.Configuration.HardwareSettings.SocketQuantity; i++)
                        {
                            IPParray[i] = Context.Configuration.ReadingSocketsSettings.CCDSocketParameters[i].ImageCheckingParameters.Clone();

                        }

                        CurrentCycleCCD.SetImageProcessParameters(IPParray);


                        // сохраняем время синхроимпульса до начала обработки, для определения будет ли еще один синхроимульс во время обработки,
                        // т.е. хватает ли нам времени на обработку
                        var beforeSync = LCBStatus.TimeSyncSignalGotForShowInCycle; // Время получения сихроимпульса

                        // замеряем время  обработки
                        Timings.ImageProcessStart = sw.ElapsedTicks;
                        Errors.ImageProcessError = false;

                        //Тестовое значение, чтобы не запутаться в потоках
                        //InterfaceDataExchange.MaxDegreeOfParallelism = 1;
                        //распараллеливаем процесс


                        WorkingLog.Add(LoggerLevel.Critical, "Поиск отклонений в гнездах");
                        //сохраняем изображения эталонов во все гнезда
                        SetStandardForAllCurrentCCDSockets(CurrentCycleCCD);
                        CheckIfSocketsHasImage(CurrentCycleCCD);
                        var IsAllHaveImages = CurrentCycleCCD.IsSocketHasImage.All(p => p);
                        WorkingLog.Add(LoggerLevel.Critical, "Наличие эталонов: " + System.String.Join("", CurrentCycleCCD.StandardImages.Select((si, i) => $"{(si == null ? 0 : 1)}{(i % 8 == 7 ? " " : "")}")));

                        WorkingLog.Add(LoggerLevel.Critical, "Наличие изображений: " + System.String.Join("", CurrentCycleCCD.CurrentImages.Select((ci, i) => $"{(ci == null ? 0 : 1)}{(i % 8 == 7 ? " " : "")}")));
                        WorkingLog.Add(LoggerLevel.Critical, "Наличие изображений по платам: " + System.String.Join("", CurrentCycleCCD.CurrentImages.Select((ci, i) => new { CardSocket = new TCPCardSocket(Context.EquipmentSocket2CardSocket[i]), HasImage = ci != null }).OrderBy(i => i.CardSocket.CCDCardNumber).ThenBy(i => i.CardSocket.InnerSocketNumber).Select((r, i) => $"{(r.HasImage ? "1" : "0")}{(i % 8 == 7 ? " " : "")}")));

                        CheckIfAllSocketsGood(CurrentCycleCCD);

                        if (!Context.Configuration.HardwareSettings.RegisterEmptyImages)
                        {
                            for (int i = 0; i < Context.Configuration.HardwareSettings.SocketQuantity; i++)
                            {
                                if (!CurrentCycleCCD.IsSocketHasImage[i])
                                {
                                    CurrentCycleCCD.IsSocketGood[i] = true;
                                }
                            }
                        }

                        var IsSetBad = CurrentCycleCCD.IsSocketGood.Any(p => !p);
                        WorkingLog.Add(LoggerLevel.Critical, "Хорошие гнезда: " + System.String.Join("", CurrentCycleCCD.IsSocketGood.Select((ci, i) => $"{(ci == null ? 0 : 1)}{(i % 8 == 7 ? " " : "")}")));

                        Timings.CCDImagesProcessEnded = DateTime.Now;

                        //Проверить, работает ли бракер, если он был включен
                        bool RDPBResult = true;

                        if (RDPBCurrentStatus.IsStarted)
                        {
                            if (RDPBCurrentStatus.BlockIsOn != ((ActiveDevices & WorkingModule.RDPB) == WorkingModule.RDPB))
                            {
                                RDPBCurrentStatus.ErrorCounter++;
                                WorkingLog.Add(LoggerLevel.Critical, $"Состояние бракера не соответствует состоянию выбранному пользователем. Проход: {RDPBCurrentStatus.ErrorCounter}");
                                if (RDPBCurrentStatus.ErrorCounter > 2)
                                {
                                    if ((ActiveDevices & WorkingModule.RDPB) == WorkingModule.RDPB)
                                    {
                                        WorkingLog.Add(LoggerLevel.Critical, "Бракер выключен, но должен быть включен.");
                                        //WorkingLog.Add(LoggerLevel.Critical, "Бракер выключен, но должен быть включен. Включаю бракер");
                                        Controller.CreateCommandInstance(typeof(RDPBModule.TurnOnCommand)).ExecuteCommand();
                                    }
                                    else
                                    {
                                        WorkingLog.Add(LoggerLevel.Critical, "Бракер включен, но должен выключен.");
                                        //WorkingLog.Add(LoggerLevel.Critical, "Бракер включен, но должен выключен. Выключаю бракер");
                                        Controller.CreateCommandInstance(typeof(RDPBModule.TurnOffCommand)).ExecuteCommand();
                                    }
                                    RDPBCurrentStatus.ErrorCounter = 0;
                                }
                            }
                            else
                            {
                                RDPBCurrentStatus.ErrorCounter = 0;
                            }
                            WorkingStep = WorkStep.RDPBSend;
                            WorkingStepTime[(int)WorkStep.RDPBSend] = sw.ElapsedTicks;

                            // сообщить бракеру о съеме
                            if (IsSetBad && IsAllHaveImages)
                            {
                                if (Context.Configuration.HardwareSettings.RemoveDefectedPreformBlockConfig.SendBadCycleToRDPB)
                                {
                                    WorkingLog.Add(LoggerLevel.Critical, "Съем: Плохой");
                                    try
                                    {
                                        Controller.CreateCommandInstance(typeof(RDPBModule.SendSetIsBadCommand)).ExecuteCommand();
                                    }
                                    catch
                                    {
                                        RDPBResult = false;
                                    }
                                }
                                else
                                {
                                    WorkingLog.Add(LoggerLevel.Critical, "Съем: Плохой (но посылаем, что хороший)");
                                    try
                                    {
                                        Controller.CreateCommandInstance(typeof(RDPBModule.SendSetIsOkCommand)).ExecuteCommand();
                                    }
                                    catch
                                    {
                                        RDPBResult = false;
                                    }
                                }
                                RDPBCurrentStatus.CurrentBoxDefectCycles++;
                                RDPBCurrentStatus.TotalDefectCycles++;
                            }
                            else
                            {
                                if (IsAllHaveImages)
                                {
                                    WorkingLog.Add(LoggerLevel.Critical, "Съем: OK");
                                }
                                else
                                {
                                    WorkingLog.Add(LoggerLevel.Critical, "Съем: Не все гнезда прочитаны правильно, считаем, что съем хороший");

                                }
                                try
                                {
                                    Controller.CreateCommandInstance(typeof(RDPBModule.SendSetIsOkCommand)).ExecuteCommand();
                                }
                                catch
                                {
                                    RDPBResult = false;
                                }
                            }

                            if (!RDPBResult)
                            {
                                Errors.RemovingDefectedPreformsBlockError = true;
                                WorkingLog.Add(LoggerLevel.Critical, "Ошибка бракера");
                            }
                            else
                            {
                                Errors.RemovingDefectedPreformsBlockError = false;
                            }

                            var boxdir = RDPBCurrentStatus.TransporterSide == RDPBTransporterSide.Left ? "Левый" : (
                                RDPBCurrentStatus.TransporterSide == RDPBTransporterSide.Right ? "Правый" :
                                (RDPBCurrentStatus.TransporterSide == RDPBTransporterSide.Stoped ? "Стоит" : "Ошибка"));
                            WorkingLog.Add(LoggerLevel.Critical, "Короб: " + boxdir);

                            RDPBCurrentStatus.CurrentTransporterSide = RDPBCurrentStatus.TransporterSide;

                            if (RDPBCurrentStatus.PreviousDirection != RDPBTransporterSide.NotSet && RDPBCurrentStatus.PreviousDirection != RDPBCurrentStatus.CurrentTransporterSide)
                            {
                                var box = new DoMCLib.Classes.Box();
                                box.BadCyclesCount = RDPBCurrentStatus.CurrentBoxDefectCycles;
                                box.CompletedTime = DateTime.Now;
                                box.TransporterSide = RDPBCurrentStatus.PreviousDirection;

                                RDPBCurrentStatus.CurrentBoxDefectCycles = 0;
                                WorkingLog.Add(LoggerLevel.Critical, "Смена короба");
                                lastBoxRead = DateTime.MinValue;
                                if (ActiveDevices.HasFlag(WorkingModule.LocalDB))
                                {
                                    WorkingLog.Add(LoggerLevel.Critical, "Создаем новый короб");
                                    Controller.CreateCommandInstance(typeof(DBModule.EnqueueBoxDateCommand)).ExecuteCommand(box);

                                }
                            }


                            if (RDPBCurrentStatus.CoolingBlocksQuantity != Context.Configuration.HardwareSettings.RemoveDefectedPreformBlockConfig.CoolingBlocksQuantity)
                            {
                                Controller.CreateCommandInstance(typeof(RDPBModule.SetCoolingBlockQuantityCommand)).ExecuteCommand(Context.Configuration.HardwareSettings.RemoveDefectedPreformBlockConfig.CoolingBlocksQuantity);
                            }
                        }
                        CurrentCycleCCD.TransporterSide = RDPBCurrentStatus.CurrentTransporterSide;
                        if (IsAllHaveImages)
                        {
                            WorkingStep = WorkStep.RecalcStandards;
                            WorkingStepTime[(int)WorkStep.RecalcStandards] = sw.ElapsedTicks;

                            Timings.CCDEtalonsRecalculateStarted = DateTime.Now;
                            WorkingLog.Add(LoggerLevel.Critical, "Перерасчет эталонов");
                            RecalcAllStandards(CurrentCycleCCD);
                        }
                        else
                        {
                            WorkingLog.Add(LoggerLevel.Critical, "Эталоны не пересчитываем");

                        }
                        if ((DateTime.Now - lastSavedConfiguration).TotalSeconds > SaveTimeoutInSecodns)
                        {
                            WorkingStep = WorkStep.SaveConfiguration;
                            WorkingStepTime[(int)WorkStep.SaveConfiguration] = sw.ElapsedTicks;

                            WorkingLog.Add(LoggerLevel.Critical, "Сохранение текущей конфигурации");
                            var savetask = new Task(() =>
                            {
                                Context.Configuration.SaveProcessingDataSettings();
                            });
                            savetask.Start();
                            lastSavedConfiguration = DateTime.Now;
                        }
                        Timings.CCDEtalonsRecalculateEnded = DateTime.Now;

                        // запоминаем время завершения обработки изображений
                        //Timings.StopProcessImages = InterfaceDataExchange.PreciseTimer.ElapsedTicks;
                        //var ProcessDuration = InterfaceDataExchange.CCDDataEchangeStatuses.ProcessDuration * 1e-4;

                        //Если к этому моменту в очереди циклов больше 10 циклов, значит они не успевают сохраняться в базу данных или БД не работает
                        if (DBCyclesCCDLeftInQueue > 10)
                        {
                            Errors.NotEnoughTimeToProcessSQL = true;
                        }
                        else
                        {
                            Errors.NotEnoughTimeToProcessSQL = false;
                        }

                        if (!Errors.NoLocalSQL && ((ActiveDevices & WorkingModule.LocalDB) == WorkingModule.LocalDB))
                        {
                            if (DBCyclesCCDLeftInQueue < 20)
                            {
                                try
                                {
                                    WorkingStep = WorkStep.SendToDB;
                                    WorkingStepTime[(int)WorkStep.SendToDB] = sw.ElapsedTicks;

                                    // ставим текущий цикл в общую очередь циклов для сохранения
                                    Controller.CreateCommandInstance(typeof(DBModule.EnqueueCycleDateCommand)).ExecuteCommand(CurrentCycleCCD);

                                    WorkingLog.Add(LoggerLevel.Critical, $"Данные съема поставлены в очередь на запись. В очереди {DBCyclesCCDLeftInQueue} элемент(а,ов)");
                                }
                                catch (Exception ex)
                                {
                                    WorkingLog.Add(LoggerLevel.Critical, $"Ошибка при постановке в очередь ({ex.Message}).");
                                    if (ActiveDevices.HasFlag(WorkingModule.LocalDB))
                                    {
                                        WasErrorWhileWorked = true;
                                        MessageBox.Show(ex.Message, "Ошибка передачи в БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        continue;
                                    }

                                }
                            }
                            else
                            {

                            }
                        }

                        var afterSync = LCBStatus.TimeOfLCBSynchrosignal; // Время получения сихроимпульса
                                                                          // Если был синхроимпульс пока мы обрабатывали, значит мы не успеваем и второй съем не читался вообще

                        if (afterSync != beforeSync && afterSync != DateTime.MinValue)
                        {
                            //выставляем ошибку, что мы не успеваем обрабатывать изображения
                            Errors.NotEnoughTimeToProcessData = true;
                            WorkingLog.Add(LoggerLevel.Critical, $"Был получен еще один синхросигнал. Время: {LCBStatus.TimeOfLCBSynchrosignal:dd-MM-yyyy HH\\:mm\\:ss.fff}");
                        }
                        else
                        {
                            Errors.NotEnoughTimeToProcessData = false;
                        }

                        //WorkingLog.Add("Эталоны 14: " + InterfaceDataExchange.SocketStandardExist());

                        //добавляем результаты проверки гнезд в список
                        socketStatuses.Add(CurrentCycleCCD.TimeLCBSyncSignalGot, CurrentCycleCCD.IsSocketGood);
                        for (int i = 0; i < CurrentCycleCCD.IsSocketGood.Length; i++)
                            if (!CurrentCycleCCD.IsSocketGood[i])
                                ErrorsBySockets[i]++;

                    }

                    //WorkingLog.Add("Эталоны 15: " + InterfaceDataExchange.SocketStandardExist());
                    if (Errors.MissedSyncrosignalCounter > 5)
                    {
                        if (!Errors.LCBDoesNotRespond)
                        {
                            Errors.LCBDoesNotSendSync = true;
                            ReconnectToLCB();
                            Errors.MissedSyncrosignalCounter = 0;
                        }
                    }
                    //WorkingLog.Add("Эталоны 16: " + InterfaceDataExchange.SocketStandardExist());
                    RDPBCurrentStatus.PreviousDirection = RDPBCurrentStatus.CurrentTransporterSide;

                    WorkingStep = WorkStep.ClearMemory;
                    WorkingStepTime[(int)WorkStep.ClearMemory] = sw.ElapsedTicks;

                    WorkingLog.Add(LoggerLevel.Critical, "Очистка неиспользуемой памяти");
                    GC.Collect();

                }
            }
            catch (ThreadAbortException taex)
            {
                WorkingLog.Add(LoggerLevel.Critical, "Работа принудительно остановлена. Сохранение текущей конфигурации");
                Context.Configuration.SaveProcessingDataSettings();
            }
            catch (Exception ex)
            {
                WorkingLog.Add(LoggerLevel.Critical, "Ошибка при работе.", ex);
                WasErrorWhileWorked = true;
            }
            // если была критическая ошибка, останавливаем работу
            if (WasErrorWhileWorked)
            {
                ForceStop = true;
            }
        }

        private void GetWorkModeStandard_Click(object sender, EventArgs e)
        {
            /*var ctrl = (Control)sender;
            var socketnumber = (int)ctrl.Tag;
            if (CurrentCycleCCD != null)
            {
                var sf = new DoMCInterface.ShowFrameForm();
                sf.Text = "Цикл: " + CurrentCycleCCD.CycleCCDDateTime.ToString() + " Номер гнезда: " + socketnumber;
                sf.Image = CurrentCycleCCD.Differences[socketnumber - 1];
                sf.Show();
            }*/
        }

        private void ResetTemporaryStatistics()
        {
            LCBStatus.TimeOfLCBSynchrosignal = DateTime.MinValue;
            LCBStatus.TimePreviousSyncSignalGot = DateTime.MinValue;
        }

        private void miLoadStandard_Click(object sender, EventArgs e)
        {

            var dir = System.IO.Path.Combine(Application.StartupPath, DoMCApplicationContext.StandardFolder);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            var od = new OpenFileDialog();
            od.Filter = "Эталоны (*.std)|*.std|Все файлы (*.*)|*.*";
            od.InitialDirectory = dir;
            od.DefaultExt = "std";
            od.AddExtension = true;
            if (od.ShowDialog() == DialogResult.OK)
            {
                Context.Configuration.LoadStandardSettings(od.FileName);
                SetWindowStandardTitle(od.FileName);
            }

        }


        private Bitmap DrawMatrix(Size rectSize, bool[] IsSocketGood, bool[] SocketsToSave, int[] ErrorSumBySocket, bool[] IsSocketChecking, Color goodColor, Color badColor, Color blockedSocketColor, Color forSavingColor, bool showErrors)
        {
            var indent = 5;
            var bmp = new Bitmap(rectSize.Width - 2 * indent, rectSize.Height - 2 * indent);
            var Y = 16;
            var X = 6;
            var middlespace = 50;
            var kx = (rectSize.Width - 2 * indent) / (double)X;
            var ky = (rectSize.Height - middlespace - 2 * indent - 10) / (double)Y;
            var diameter = Math.Min(kx, ky) * 0.9;

            var ToExcludeCoords = new Point[] { new Point(2, 7), new Point(3, 7), new Point(2, 8), new Point(3, 8) };
            var graph = Graphics.FromImage(bmp);

            var badbrush = new SolidBrush(badColor);
            var goodbrush = new SolidBrush(goodColor);
            var saveSocketBrush = new SolidBrush(forSavingColor);

            //if (k>5)//(kx > 5 && ky > 5)
            {
                bool addText = false;
                var fontsize = 0;
                Font font = null;
                if (diameter > 10)//(kx > 10 && ky > 10)
                {
                    fontsize = (int)diameter / 2;//Math.Min(kx, ky);
                    font = new Font("Arial", fontsize);

                    fontsize = FindSize((fs) =>
                     {
                         var sz = graph.MeasureString("99", new Font(font.FontFamily, fs));
                         return (int)Math.Max(sz.Width, sz.Height);

                     }, (int)diameter//Math.Min(kx, ky)*
                              , 20, 0);
                    if (fontsize >= 8)
                    {
                        addText = true;
                        font = new Font(font.FontFamily, fontsize);
                    }
                }
                SolidBrush textBrush = new SolidBrush(Color.Black);
                for (int y = 0; y < Y; y++)
                {
                    for (int x = 0; x < X; x++)
                    {
                        var n = x * 16 + y + 1;
                        var addy = 0;
                        if (y >= 8) addy = middlespace;
                        //ToExcludeCoords
                        SolidBrush brush;
                        //InterfaceDataExchange.CurrentCycleCCD.SocketsToSave
                        //InterfaceDataExchange.CurrentCycleCCD.ImageStatuses[n]==1

                        if (SocketsToSave[n - 1])
                        {
                            graph.FillRectangle(saveSocketBrush, (int)(x * kx + (kx - diameter) / 2) - 2 + indent, (int)(y * ky + (ky - diameter) / 2) + addy - 2 + indent, (int)diameter + 4, (int)diameter + 4);
                        }
                        if (IsSocketChecking[n - 1])
                        {
                            if (showErrors)
                            {
                                if (ErrorSumBySocket[n - 1] > 0)
                                {
                                    brush = badbrush;
                                }
                                else
                                {
                                    brush = goodbrush;
                                }
                            }
                            else
                            {
                                if (!IsSocketGood[n - 1])
                                    brush = badbrush;
                                else
                                    brush = goodbrush;
                            }
                        }
                        else
                        {
                            brush = new SolidBrush(blockedSocketColor);
                        }
                        //graph.FillEllipse(brush, (int)(x * kx), (int)(y * ky), (int)kx, (int)ky);
                        graph.FillEllipse(brush, (int)(x * kx + (kx - diameter) / 2) + indent, (int)(y * ky + (ky - diameter) / 2) + addy + indent, (int)diameter, (int)diameter);

                        if (addText && font != null)
                        {
                            string str;
                            if (showErrors)
                            {
                                str = ErrorSumBySocket[n - 1].ToString();
                            }
                            else
                            {
                                str = n.ToString();
                            }
                            var sz = graph.MeasureString(str, font);
                            //graph.DrawString(str,font,brush, (int)(x * kx), (int)(y * ky));
                            graph.DrawString(str, font, textBrush, (int)(x * kx + (kx - diameter) / 2 + (diameter / 2 - sz.Width / 2)) + indent, (int)(y * ky + (ky - diameter) / 2 + (diameter / 2 - sz.Height / 2)) + addy + indent);
                        }
                    }
                }

            }

            return bmp;
        }

        private int FindSize(Func<int, int> function, int compareTo, int higherBound, int lowerBound)
        {
            int v = lowerBound, prv_v;
            do
            {
                prv_v = v;
                v = (higherBound + lowerBound) / 2;
                var res = function(v);
                if (res > compareTo)
                {
                    higherBound = v - 1;
                }
                else
                {
                    lowerBound = v + 1;
                }
            } while (higherBound != lowerBound && prv_v != v);
            return v;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var now = DateTime.Now;

            if (Context != null)
            {
                //если в этот съем еще не рисовали, то рисуем
                if (lastDrawCycleTime < LCBStatus.TimeOfLCBSynchrosignal)
                {
                    CurrentDraw();
                    lastDrawCycleTime = LCBStatus.TimeOfLCBSynchrosignal;
                    lblCycleDurationValue.Text = (LCBStatus.TimeOfLCBSynchrosignal - LCBStatus.TimePreviousSyncSignalGot).TotalSeconds.ToString("F1") + " с";

                    lblCurrentBoxDefectCycles.Text = RDPBCurrentStatus.CurrentBoxDefectCycles.ToString();
                }
                //if (IsWorkingModeStarted)
                {
                    if ((DateTime.Now - LCBStatus.TimeOfLCBSynchrosignal).TotalSeconds < 2)
                    {
                        pnlCurrentSockets.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        pnlCurrentSockets.BackColor = this.BackColor;
                    }
                }
                lblTotalDefectCycles.Text = TotalDefectCycles.ToString();

                /*if (InterfaceDataExchange.DataStorage != null && InterfaceDataExchange.DataStorage.LocalIsActive && (now - lastBoxRead) > lastBoxReadTime)
                {
                    lastBoxRead = now;
                    var start = now.AddHours(-5);
                    List<DoMCLib.DB.Box> boxes = new List<DoMCLib.DB.Box>();
                    var localboxes = InterfaceDataExchange?.DataStorage?.LocalGetBox(start, now) ?? new List<DoMCLib.DB.Box>();
                    if (localboxes != null && localboxes.Count > 0)
                        localboxes = localboxes.OrderBy(b => b.CompletedTime).ToList();
                    if (localboxes != null)
                        boxes.AddRange(localboxes);
                    var remoteboxes = InterfaceDataExchange?.DataStorage?.RemoteGetBox(start, now) ?? new List<DoMCLib.DB.Box>();
                    if (remoteboxes != null && remoteboxes.Count > 0)
                        remoteboxes = remoteboxes.OrderBy(b => b.CompletedTime).ToList();
                    if (remoteboxes != null)
                        boxes.AddRange(remoteboxes);
                    boxes = boxes.OrderBy(b => b.CompletedTime).ToList();

                    lvBoxes.Items.Clear();
                    for (int i = 0; i < boxes.Count; i++)
                    {
                        var box = boxes[i];
                        var lvi = new ListViewItem(new string[] { box.CompletedTime.ToString("G"), box.BadCyclesCount.ToString(), box.TransporterSideToString() });
                        lvBoxes.Items.Add(lvi);
                    }
                }*/
            }
            //если прошло больше 2 секунд после последней провеки ошибок, то показываем их
            if ((DateTime.Now - lastErrorCheck).TotalSeconds > 2)
            {
                lastErrorCheck = DateTime.Now;
                ShowCurrentErrors();
                ShowDevicesButtonStatuses();

            }
            // обновляем данные о нажатых/отжатых кнопках
            GetIsDevicesButtonWorking();
            if (ForceStop)
            {
                StopWork();
                ForceStop = false;
            }
            if (RDPBCurrentStatus.IsTimeout)
            {
                WorkingLog.Add(LoggerLevel.Critical, "Бракер не ответил");
                DevicesErrors |= WorkingModule.RDPB;
            }
            else
            {
                DevicesErrors &= ~WorkingModule.RDPB;
            }
            GetArchiveDBModuleStatus();
        }

        private void CurrentDraw()
        {
            pnlCurrentSockets.Invalidate();
            chCurrentLastHourSumBad.Series[0].Points.Clear();
            if (socketStatuses == null) return;
            var badPreformQuantity = socketStatuses.GetBadPreformsForSockets();
            for (int i = 0; i < badPreformQuantity.Length; i++)
            {
                chCurrentLastHourSumBad.Series[0].Points.AddXY(i + 1, badPreformQuantity[i]);
            }

            var maxbpq = badPreformQuantity.Max();
            var calulatedInterval = (maxbpq / 5 / 5) * 5;
            if (calulatedInterval == 0) calulatedInterval = maxbpq >= 5 ? maxbpq / 5 : 1;
            chCurrentLastHourSumBad.ChartAreas[0].AxisY.Interval = calulatedInterval;
            chCurrentLastHourSumBad.ChartAreas[0].AxisY.RoundAxisValues();
        }
        private void ShowCurrentErrors()
        {

            lbCurrentErrors.Items.Clear();
            var errors = Errors;
            var dict = errors.ToDictionary();
            foreach (var p in dict)
            {
                if (p.Value)
                    lbCurrentErrors.Items.Add(errors.KeyToText(p.Key));
            }

        }

        private void pnlCurrentSockets_Paint(object sender, PaintEventArgs e)
        {
            var goodsockets = socketStatuses?.GetLast() ?? new bool[96];// InterfaceDataExchange?.CurrentCycleCCD?.IsSocketGood ?? new bool[96];
            var socketsToSave = new bool[96];
            var IsSocketsChecking = Context?.Configuration?.HardwareSettings?.SocketsToCheck ?? new bool[96];
            var bmp = DrawMatrix(pnlCurrentSockets.Size, goodsockets, socketsToSave, ErrorsBySockets, IsSocketsChecking, Color.LawnGreen, Color.Red, Color.LightGray, Color.LimeGreen, pbCurrentShowStatistics.IsPressed);
            e.Graphics.DrawImage(bmp, 0, 0);

        }



        private void miResetStatistics_Click(object sender, EventArgs e)
        {
            ErrorsBySockets = new int[96];
            pnlCurrentSockets.Invalidate();
            //Statuses = new List<SocketStatus>();
        }

        private void pbCurrentShowStatistics_Click(object sender, EventArgs e)
        {
            if (pbCurrentShowStatistics.IsPressed)
            {
                pbCurrentShowStatistics.Text = "Убрать статистику";
                pbCurrentShowStatistics.BackColor = Color.LimeGreen;
            }
            else
            {
                pbCurrentShowStatistics.Text = "Показать статистику";
                pbCurrentShowStatistics.BackColor = SystemColors.Control;

            }
            CurrentDraw();
        }

        private void pbDevices_Click(object sender, EventArgs e)
        {
            GetIsDevicesButtonWorking();
            if (IsWorkingModeStarted)
            {
                if (ActiveDevices.HasFlag(WorkingModule.RDPB))
                {
                    if (!StartRDPB())
                    {
                        UnPressDevice(WorkingModule.RDPB);
                        return;
                    }
                }
                else
                {
                    StopRDPB();
                }
                if (ActiveDevices.HasFlag(WorkingModule.LocalDB))
                {
                    StartDB();
                }
                else
                {
                    StopDB();
                }
            }

        }

        private void UnPressDevice(WorkingModule wm)
        {
            DevicesFlagButtons[wm].IsPressed = false;
        }

        private void DoMCWorkModeInterface_FormClosed(object sender, FormClosedEventArgs e)
        {

            try
            {
                StopWork();
                StopArchiveDB();
            }
            catch { }


        }
        private void StopModules()
        {
            try
            {
                Controller.CreateCommandInstance(typeof(DoMCLib.Classes.Module.CCD.CCDCardDataModule.StopCommand)).ExecuteCommand();
            }
            catch { }
            try
            {
                Controller.CreateCommandInstance(typeof(DoMCLib.Classes.Module.LCB.LCBModule.SetLCBNonWorkModeCommand)).ExecuteCommand();
            }
            catch { }
            try
            {
                Controller.CreateCommandInstance(typeof(DoMCLib.Classes.Module.LCB.LCBModule.LCBStopCommand)).ExecuteCommand();
            }
            catch { }
            try
            {
                Controller.CreateCommandInstance(typeof(DoMCLib.Classes.Module.RDPB.RDPBModule.StopCommand)).ExecuteCommand();
            }
            catch { }
            try
            {
                Controller.CreateCommandInstance(typeof(DoMCLib.Classes.Module.DB.DBModule.StopCommand)).ExecuteCommand();
            }
            catch { }
            try
            {
                Controller.CreateCommandInstance(typeof(DoMCLib.Classes.Module.ArchiveDB.ArchiveDBModule.StopCommand)).ExecuteCommand();
            }
            catch { }
        }

        private void tabWorkAndArchive_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabPage page = tabWorkAndArchive.TabPages[e.Index];
            Brush brush = new SolidBrush(page.BackColor);
            if (tabWorkAndArchive.SelectedTab == page)
                brush = new SolidBrush(Color.LawnGreen);
            e.Graphics.FillRectangle(brush, e.Bounds);

            Rectangle paddedBounds = e.Bounds;
            int yOffset = (e.State == DrawItemState.Selected) ? -2 : 1;
            paddedBounds.Offset(1, yOffset);
            TextRenderer.DrawText(e.Graphics, page.Text, e.Font, paddedBounds, page.ForeColor);
        }


        private void miRDPBLogs_Click(object sender, EventArgs e)
        {
            // FileAndDirectoryTools.OpenNotepad(Log.GetLogFileName(Log.LogModules.RDPB, Log.GetCurrentShiftDate()));

        }

        private void miDBLogs_Click(object sender, EventArgs e)
        {
            // FileAndDirectoryTools.OpenNotepad(Log.GetLogFileName(Log.LogModules.DB, Log.GetCurrentShiftDate()));

        }

        private void DoMCWorkModeInterface_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти из программы ПМК", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void miResetCounter_Click(object sender, EventArgs e)
        {
            TotalDefectCycles = 0;
        }

        private void miSettings_Click(object sender, EventArgs e)
        {

            try
            {
                if (!Context.IsInWorkingMode || MessageBox.Show("ПМК запущено. Вы уверены, что хотите закрыть окно рабочего режима?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        StopWork();
                    }
                    catch { }
                    var wmf = new DoMCSettingsInterface();
                    wmf.SetMainController(Controller, Context);
                    this.Hide();
                    try
                    {
                        wmf.ShowDialog();
                    }
                    catch { }
                    this.Show();
                }
            }
            catch (Exception ex)
            {
                DisplayMessage.Show(ex.Message + ex.StackTrace, "Ошибка");
            }

        }

        private void miCreateNewStandard_Click(object sender, EventArgs e)
        {

            if (IsWorkingModeStarted)
            {
                MessageBox.Show("ПМК запущено. Для создания эталонов нужно остановить работу ПМК", "Создание эталонов");
                return;
            }
            var frm = new DoMCStandardCreateInterface(Controller, Context, WorkingLog);
            this.Enabled = false;
            try
            {
                frm.ShowDialog();
            }
            finally
            {
                this.Enabled = true;
            }
            SetWindowStandardTitle();

        }

        private void miSaveStandard_Click(object sender, EventArgs e)
        {

            var dir = System.IO.Path.Combine(Application.StartupPath, DoMCApplicationContext.StandardFolder);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            var sd = new SaveFileDialog();
            sd.InitialDirectory = dir;
            sd.DefaultExt = "std";
            sd.AddExtension = true;
            sd.Filter = "Эталоны (*.std)|*.std|Все файлы (*.*)|*.*";
            if (sd.ShowDialog() == DialogResult.OK)
            {
                Context.Configuration.SaveStandardSettings(sd.FileName);
                SetWindowStandardTitle(sd.FileName);
            }

        }

        private void SetWindowStandardTitle(string standardName = null)
        {
            var title = "Рабочий режим ПМК";
            if (!string.IsNullOrEmpty(standardName))
            {
                title = $"{title}. Эталон: {standardName}";
            }
            Text = title;
        }

        private void DoMCWorkModeInterface_Resize(object sender, EventArgs e)
        {
            msWorkingModeMenu.Width = this.ClientSize.Width - 20;
        }

        private void miSocketsSettings_Click(object sender, EventArgs e)
        {

            var form = new DoMCLib.Forms.DoMCImageProcessSettingsListForm();

            form.SocketParameters = Context.Configuration.ReadingSocketsSettings.CCDSocketParameters;
            if (form.ShowDialog() == DialogResult.OK)
            {
                Context.Configuration.SaveReadingSocketsSettings();
                NotifyConfigurationUpdated();
            }

        }
        private void NotifyConfigurationUpdated()
        {
            observer.Notify(DoMCApplicationContext.ConfigurationUpdateEventName, Context.Configuration);
        }

        public DoMCApplicationContext GetContext()
        {
            return Context;
        }

        public void SetStandardForAllCurrentCCDSockets(CycleImagesCCD CurrentCycleCCD)
        {
            Parallel.For(0, CurrentCycleCCD.CurrentImages.Length, new ParallelOptions() { MaxDegreeOfParallelism = this.MaxDegreeOfParallelism }, i =>
            {
                try
                {
                    SetStandardForCurrentCCDSocket(CurrentCycleCCD, i);
                }
                catch (Exception ex)
                {
                    WorkingLog?.Add(LoggerLevel.Critical, $"Ошибка при установке эталона гнезда {i + 1}. ", ex);
                }
            });
        }
        public void CheckIfAllSocketsGood(CycleImagesCCD CurrentCycleCCD)
        {
            Parallel.For(0, CurrentCycleCCD.CurrentImages.Length, new ParallelOptions() { MaxDegreeOfParallelism = this.MaxDegreeOfParallelism }, i =>
            {
                try
                {
                    CheckIfSocketGood(CurrentCycleCCD, i);
                }
                catch (Exception ex)
                {
                    WorkingLog?.Add(LoggerLevel.Critical, $"Ошибка при проверке гнезда {i + 1}. ", ex);
                }
            });
        }
        public void CheckIfSocketGood(CycleImagesCCD CurrentCycleCCD, int EquipmentSocketNumber)
        {
            try
            {
                var LEDLineNumber = EquipmentSocketNumber / (Context.Configuration.HardwareSettings.SocketQuantity / 12);
                // нужно ли проверять это гнездо, есть ли в нем изображение и был ли включен светодиод в его линейке(или что-то сгорело)
                if (Context.Configuration.HardwareSettings.SocketsToCheck[EquipmentSocketNumber]
                    && CurrentCycleCCD.CurrentImages[EquipmentSocketNumber] != null
                    && (CurrentCycleCCD.LEDStatuses == null
                    || CurrentCycleCCD.LEDStatuses[LEDLineNumber]))
                {
                    //есть ли конфигурация этого гнезда
                    if (Context.Configuration.ProcessingDataSettings.CCDSocketStandardsImage[EquipmentSocketNumber].StandardImage != null)
                    {
                        if (Context.Configuration.HardwareSettings.SocketsToCheck[EquipmentSocketNumber])
                        {
                            var result = DoMCLib.Tools.ImageTools.CheckIfSocketGood(
                                CurrentCycleCCD.CurrentImages[EquipmentSocketNumber],
                                Context.Configuration.ProcessingDataSettings.CCDSocketStandardsImage[EquipmentSocketNumber].StandardImage,
                                Context.Configuration.ReadingSocketsSettings.CCDSocketParameters[EquipmentSocketNumber].ImageCheckingParameters);

                            CurrentCycleCCD.Average = result.Average;
                            CurrentCycleCCD.IsSocketGood[EquipmentSocketNumber] = result.IsSocketGood;
                            CurrentCycleCCD.MaxDeviation = result.MaxDeviation;
                            CurrentCycleCCD.MaxDeviationPoint = result.MaxDeviationPoint;
                            CurrentCycleCCD.Differences[EquipmentSocketNumber] = result.ResultImage;
                            CurrentCycleCCD.SocketErrorType = result.SocketErrorType;
                        }

                    }
                }
                else
                {
                    // если нет, то гнездо автоматически считается хорошим
                    CurrentCycleCCD.IsSocketGood[EquipmentSocketNumber] = true;
                }
            }
            catch (Exception ex)
            {
                //Если что-то пошло не так, говорим, что ошибка и считаем гнездо хорошим
                this.Errors.ImageProcessError = true;
                CurrentCycleCCD.IsSocketGood[EquipmentSocketNumber] = true;
            }
        }

        public void CheckIfSocketsHasImage(CycleImagesCCD CurrentCycleCCD)
        {
            Parallel.For(0, CurrentCycleCCD.CurrentImages.Length, new ParallelOptions() { MaxDegreeOfParallelism = this.MaxDegreeOfParallelism }, i =>
            {
                CheckIfSocketHasImage(CurrentCycleCCD, i);
            });
        }
        public void CheckIfSocketHasImage(CycleImagesCCD CurrentCycleCCD, int EquipmentSocketNumber)
        {
            try
            {
                var LEDLineNumber = EquipmentSocketNumber / (Context.Configuration.HardwareSettings.SocketQuantity / 12);
                // нужно ли проверять это гнездо, есть ли в нем изображение и был ли включен светодиод в его линейке(или что-то сгорело)
                if (Context.Configuration.HardwareSettings.SocketsToCheck[EquipmentSocketNumber]
                    && CurrentCycleCCD.CurrentImages[EquipmentSocketNumber] != null
                    && (CurrentCycleCCD.LEDStatuses == null
                    || CurrentCycleCCD.LEDStatuses[LEDLineNumber]))
                {
                    var result = DoMCLib.Tools.ImageTools.Average(
                        CurrentCycleCCD.CurrentImages[EquipmentSocketNumber],
                        Context.Configuration.ReadingSocketsSettings.CCDSocketParameters[EquipmentSocketNumber].ImageCheckingParameters.GetRectangle());

                    CurrentCycleCCD.IsSocketHasImage[EquipmentSocketNumber] = result > Context.Configuration.HardwareSettings.AverageToHaveImage;

                }
                else
                {
                    // если нет, то гнездо автоматически считается хорошим
                    CurrentCycleCCD.IsSocketHasImage[EquipmentSocketNumber] = true;
                }
            }
            catch (Exception ex)
            {
                //Если что-то пошло не так, говорим, что ошибка и считаем гнездо хорошим
                this.Errors.ImageProcessError = true;
                CurrentCycleCCD.IsSocketHasImage[(EquipmentSocketNumber - 1)] = true;
            }

        }
        public void RecalcAllStandards(CycleImagesCCD CurrentCycleCCD)
        {
            Parallel.For(0, CurrentCycleCCD.CurrentImages.Length, new ParallelOptions() { MaxDegreeOfParallelism = this.MaxDegreeOfParallelism }, i =>
            {
                try
                {
                    RecalcStandard(CurrentCycleCCD, i);
                }
                catch (Exception ex)
                {
                    WorkingLog?.Add(LoggerLevel.Critical, $"Ошибка при перерасчете эталона гнезда {i + 1}. ", ex);
                }
            });
        }
        public void RecalcStandard(CycleImagesCCD CurrentCycleCCD, int EquipmentSocketNumber)
        {

            var LEDLineNumber = (EquipmentSocketNumber) / (Context.Configuration.HardwareSettings.SocketQuantity / 12);
            if (CurrentCycleCCD.IsSocketGood[EquipmentSocketNumber]
                && CurrentCycleCCD.LEDStatuses[LEDLineNumber])
            {
                var newStandard = ImageTools.GetNewStandard(
                    Context.Configuration.ProcessingDataSettings.CCDSocketStandardsImage[EquipmentSocketNumber].StandardImage,
                    CurrentCycleCCD.CurrentImages[EquipmentSocketNumber],
                    Context.Configuration.HardwareSettings.StandardRecalculationParameters.Koefficient);
                if (newStandard != null)
                {
                    Context.Configuration.ProcessingDataSettings.CCDSocketStandardsImage[EquipmentSocketNumber].StandardImage = newStandard;
                }
            }
        }
        public void SetStandardForCurrentCCDSocket(CycleImagesCCD CurrentCycleCCD, int EquipmentSocketNumber)
        {
            var newImg = ImageTools.ImageCopy(Context.Configuration.ProcessingDataSettings.CCDSocketStandardsImage[EquipmentSocketNumber].StandardImage);
            CurrentCycleCCD.StandardImages[EquipmentSocketNumber] = newImg;
        }
        public async void ReconnectToLCB()
        {
            await new LCBStopCommand(Controller, Controller.GetModule(typeof(LCBModule))).ExecuteCommandAsync();
            await new LCBStartCommand(Controller, Controller.GetModule(typeof(LCBModule))).ExecuteCommandAsync();
            Controller.CreateCommandInstance(typeof(LCBModule.LCBStopCommand)).ExecuteCommand();
            System.Threading.Thread.Sleep(10);
            Controller.CreateCommandInstance(typeof(LCBModule.LCBStartCommand)).ExecuteCommand();
        }

        private void tsmiLogsArchive_Click(object sender, EventArgs e)
        {
            var dir = System.IO.Path.Combine(Application.StartupPath, "Logs");
            FileAndDirectoryTools.OpenFolder(dir);
        }

        private void DoMCNotFoundErrorMessage()
        {
            MessageBox.Show("Не найдено устройство управления платами ПМК", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.Close();
        }
        private void DoMCNoConfigurationErrorMessage()
        {
            MessageBox.Show("Не закончено конфигурирование системы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        private void DoMCNotAbleLoadConfigurationErrorMessage()
        {
            MessageBox.Show("Не могу загрузить конфигурацию в плату", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        private void DoMCNotAbleToReadSocketErrorMessage()
        {
            MessageBox.Show("Не удалось прочитать гнездо", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        private void DoMCSocketIsNotConfiguredErrorMessage()
        {
            MessageBox.Show("Гнездо не сконфигурировано", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        private void DoMCConfigurationNotLoadedErrorMessage()
        {
            MessageBox.Show("Конфигурация не загружена в плату", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        private void DoMCNotInitializedErrorMessage()
        {
            MessageBox.Show("Плата не инициализирована", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        private void DoMCNetworkCardNotSelectedErrorMessage()
        {
            MessageBox.Show("Не выбрана сетевая плата", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

    }

    public class SocketStatus
    {
        public DateTime CycleDT;
        public bool[] IsSocketGood;
    }

    public class SocketStatuses
    {
        private int SocketQuantity;
        private List<SocketStatus> Statuses;
        private double DeleteAfter = 3600;
        public SocketStatuses(int socketQuantity)
        {
            Statuses = new List<SocketStatus>();
            SocketQuantity = socketQuantity;
        }
        public void Add(SocketStatus ss)
        {
            if (ss.IsSocketGood.Length != SocketQuantity) return;
            lock (Statuses)
            {
                ClearByTime();
                Statuses.Add(ss);
            }
        }
        public void Add(DateTime cycleDT, bool[] isSocketsGood)
        {
            if (isSocketsGood.Length != SocketQuantity) return;
            lock (Statuses)
            {
                ClearByTime();
                Statuses.Add(new SocketStatus() { CycleDT = cycleDT, IsSocketGood = isSocketsGood });
            }
        }

        private void ClearByTime()
        {
            var now = DateTime.Now;
            Statuses.RemoveAll(s => (now - s.CycleDT).TotalSeconds > DeleteAfter);
            Statuses.RemoveAll(s => s == null);
            Statuses.TrimExcess();
        }

        public int[] GetBadPreformsForSockets()
        {
            int[] sum = new int[SocketQuantity];
            lock (Statuses)
            {
                foreach (var ss in Statuses)
                {
                    if (ss != null)
                    {
                        for (int n = 0; n < SocketQuantity; n++)
                        {
                            sum[n] += ss.IsSocketGood[n] ? 0 : 1;
                        }
                    }
                }
            }
            return sum;
        }

        public bool[] GetLast()
        {
            if (Statuses.Count == 0) return Enumerable.Repeat(true, 96).ToArray();
            var maxDT = Statuses.Max(s => s.CycleDT);
            var lastStatus = Statuses.Find(s => s.CycleDT == maxDT);
            if (lastStatus == null) return new bool[96];
            else return lastStatus.IsSocketGood;
        }
    }

    public class DisplayCycleData
    {
        public bool IsRemote;
        public CycleData CycleData;
    }

    public class CycleTag
    {
        public int CycleID;
        public bool IsRemote;
        public CycleTag(int cycleID, bool isRemote)
        {
            CycleID = cycleID;
            IsRemote = isRemote;
        }
    }

    public enum WorkStep
    {
        Prepare = 1,
        WaitForSyncroSignal = 2,
        ReadingSocketsCompleted = 3,
        StartReadingImages = 4,
        ReadingImagesCompleted = 5,
        SearchForDefectedPreforms = 6,
        RDPBSend = 7,
        SendToDB = 8,
        RecalcStandards = 9,
        SaveConfiguration = 10,
        ClearMemory = 11
    }

    public class ButtonsPresses
    {
        public PressButton Button;
        public bool ButtonPressed;
    }
    public class Timings
    {
        public DateTime CCDStart;
        public DateTime CCDEnd;
        public DateTime CCDGetImagesStarted;
        public DateTime CCDGetImagesEnded;
        public DateTime CCDImagesProcessStarted;
        public DateTime CCDImagesProcessEnded;
        public DateTime CCDEtalonsRecalculateStarted;
        public DateTime CCDEtalonsRecalculateEnded;

        public long ImageProcessStart;

    }

    public class LCBStatus
    {
        public bool[] LEDStatus = new bool[12];
        public DateTime TimePreviousSyncSignalGot;
        public DateTime TimeOfLCBSynchrosignal;
        public DateTime TimeSyncSignalGotForShowInCycle;
        public DateTime LastLedStatusesGotTime;
    }

    public class ArchiveDBModuleStatus
    {
        public bool IsStarted;
        public DateTime LastErrorTime;
        public Exception LastError;
    }

}
