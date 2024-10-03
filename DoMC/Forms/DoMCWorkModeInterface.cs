using DataExchangeKernel.ACS_Core;
using DataExchangeKernel.Interface;
using DoMCLib.Classes;
using DoMCLib.Configuration;
using DoMCLib.Tools;
using DoMCLib.DB;
using DoMCLib.Exceptions;
using DoMCInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoMCInterface
{
    public partial class DoMCWorkModeInterface : Form, IUserKernelInterface
    {
        DoMCInterfaceDataExchange InterfaceDataExchange = null;
        int CardTimeout = 30000;
        bool IsConfigurationLoadedSuccessfully;

        /*Panel[] WorkModeStandardSettingsSocketsPanelList;
        int[] SocketErrorStatus;*/
        GlobalMemory globalMemory;
        SocketStatuses socketStatuses;

        /* DateTime dtArchiveFrom, dtArchiveTo;
         List<DisplayCycleData> ArchiveCycles;
         List<CycleData> LocalArchiveCycles;
         List<CycleData> RemoteArchiveCycles;*/
        int[] ErrorsBySockets = new int[96];

        Thread WorkingThread;

        //ошибки устройств
        WorkingModule DevicesErrors;

        //работает ли устройство или мы его отключили вручную
        WorkingModule ActiveDevices;
        //PressButton[] DeviceButtons;
        Dictionary<WorkingModule, PressButton> DevicesFlagButtons;

        bool ForceStop;

        Log WorkingLog = new Log(Log.LogModules.MainSystem);

        DateTime lastDrawCycleTime = DateTime.Now;
        DateTime lastErrorCheck = DateTime.Now;
        DateTime lastBoxRead = DateTime.MinValue;
        TimeSpan lastBoxReadTime = new TimeSpan(0, 2, 0);

        WorkStep WorkingStep;

        public DoMCWorkModeInterface()
        {
            CreateDataExchange();
            InitializeComponent();
            Application.ThreadException += Application_ThreadException;
            DevicesControlInit();
            PressAllDevicesButton();
            ShowDevicesButtonStatuses();
        }

        private void CreateDataExchange()
        {
            WorkingLog.Add("Старт");
            WorkingLog.Add("Создание InterfaceDataExchange");
            InterfaceDataExchange = new DoMCInterfaceDataExchange();

            WorkingLog.Add("Загрузка конфигурации");
            var Configuration = new FullDoMCConfiguration();
            Configuration.Load();
            InterfaceDataExchange.Configuration = Configuration;

        }

        private void DevicesControlInit()
        {
            //DeviceButtons = new PressButton[] { pbRDPB, pbLocalDB, pbRemoteDB };
            DevicesFlagButtons = new Dictionary<WorkingModule, PressButton>()
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



        public void SetMemoryReference(GlobalMemory memory)
        {
            globalMemory = memory;
            memory.OverallMemory[ApplicationCardParameters.DoMCCardControlInstance] = InterfaceDataExchange;

            //InterfaceDataExchange = globalMemory?.OverallMemory[ApplicationCardParameters.DoMCCardControlInstance] as DoMCInterfaceDataExchange;
            //if (InterfaceDataExchange == null)
            //{
            //    DoMCNotFoundErrorMessage();
            //}
            //var ss = new bool[96];
            socketStatuses = new SocketStatuses(96);
            //socketStatuses.Add(DateTime.Now, ss);
            ErrorsBySockets = new int[96];
            CardTimeout = InterfaceDataExchange?.Configuration?.Timeouts.WaitForCCDCardAnswerTimeout ?? 10000;
            SetConfiguration();

            LoadArchiveTab();
            InterfaceDataExchange.LEDStatus.ResetCycleDuration();
        }

        private void LoadArchiveTab()
        {
            var archiveForm = new DoMCLib.Forms.DoMCArchiveForm(InterfaceDataExchange.Configuration.LocalDataStorageConnectionString, InterfaceDataExchange.Configuration.RemoteDataStorageConnectionString);
            archiveForm.TopLevel = false;
            archiveForm.Parent = tbpArchive;
            archiveForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            archiveForm.StartPosition = FormStartPosition.Manual;
            archiveForm.Location = new Point(0, 0);
            archiveForm.Size = new Size(tbpArchive.ClientSize.Width, tbpArchive.ClientSize.Height);
            archiveForm.Scale(new SizeF(1f, 1f));
            archiveForm.Visible = true;

        }

        private void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            WorkingLog.Add(e.Exception);
            if (e.Exception is DoMCException)
            {
                DisplayMessage.Show(e.Exception.Message, "Ошибка");
            }
            else
            {
                DisplayMessage.Show(e.Exception.Message + "\r\n" + e.Exception.StackTrace, "Ошибка");
            }
        }

        private void SetConfiguration()
        {
            InterfaceDataExchange?.RDPBCurrentStatus?.SetFromConfiguration(InterfaceDataExchange?.Configuration ?? null);
        }

        public new DialogResult ShowDialog()
        {
            if (InterfaceDataExchange == null)
            {
                DoMCNotFoundErrorMessage();
                return DialogResult.Cancel;
            }
            return base.ShowDialog();
        }

        private void DoMCNotFoundErrorMessage()
        {
            MessageBox.Show("Не найдено устройство управления платами ПМК", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.Close();
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (InterfaceDataExchange.IsWorkingModeStarted)
            {
                StopWork();
            }
            else
            {
                socketStatuses = new SocketStatuses(96);
                GetIsDevicesButtonWorking();
                if (PrepareToStartWork())
                {
                    StartReading();
                }
                else
                {
                    StartFailed();
                }
            }
        }
        private bool PrepareToStartWork()
        {
            string ErrorMsg;
            WorkingLog.Add("");
            IsConfigurationLoadedSuccessfully = false;
            InterfaceDataExchange.CardsConnection.PacketLogActive = false;
            DevicesErrors = 0;
            InterfaceDataExchange.LEDStatus.ResetCycleDuration();
            WorkingLog.Add("Загрузка конфигурации гнезд");
            var loadresult = InterfaceDataExchange.LoadCCDConfigurationAndStart();
            if (loadresult != DoMCInterfaceDataExchange.LoadError.None)
            {
                WorkingLog.Add("Ошибка загрузки конфигураций гнезд. Остановка работы");
                InterfaceDataExchange.Errors.CCDNotRespond = true;
                DoMCNotAbleLoadConfigurationErrorMessage(loadresult);
                return false;
            }
            else
            {
                InterfaceDataExchange.Errors.CCDNotRespond = false;
            }

            InterfaceDataExchange.Errors.LCBDoesNotRespond = false;
            if (ActiveDevices.HasFlag(WorkingModule.LCB))
            {
                WorkingLog.Add("Запуск модуля БУС и загрузка конфигурации");
                loadresult = InterfaceDataExchange.LoadLCBConfigurationAndStart(true);
                if (loadresult != DoMCInterfaceDataExchange.LoadError.None)
                {
                    WorkingLog.Add($"Ошибка ({loadresult}) при запуске БУС. Остановка работы");
                    InterfaceDataExchange.Errors.LCBDoesNotRespond = true;
                    DoMCNotAbleLoadConfigurationErrorMessage(loadresult);
                    return false;
                }
                else
                {
                    InterfaceDataExchange.Errors.LCBDoesNotRespond = false;
                }


                var startLCBEWorkModeTime = DateTime.Now;
                InterfaceDataExchange.SendCommand(ModuleCommand.SetLCBWorkModeRequest);
                var res = UserInterfaceControls.Wait(CardTimeout, () => InterfaceDataExchange.LEDStatus.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.SetLCBWorkModeResponse && InterfaceDataExchange.LEDStatus.LastCommandResponseReceived > startLCBEWorkModeTime);
                if (!res)
                {
                    WorkingLog.Add($"Ошибка при запуске БУС в работу. Остановка работы");
                    return false;
                }
            }
            IsConfigurationLoadedSuccessfully = true;
            if (ActiveDevices.HasFlag(WorkingModule.LocalDB))
            {

                WorkingLog.Add("Запуск модуля работы с базой данных");
                InterfaceDataExchange.SendCommand(ModuleCommand.DBStart);
                if (InterfaceDataExchange.Errors.NoLocalSQL)
                {
                    ErrorMsg = "Ошибка при запуске модуля работы с базой данных";
                    WorkingLog.Add(ErrorMsg);
                    DevicesErrors |= WorkingModule.LocalDB;
                    if (ActiveDevices.HasFlag(WorkingModule.LocalDB))
                    {
                        WorkingLog.Add("Остановка работы");
                        DoMCShowErrorMessage(ErrorMsg);
                        return false;
                    }
                    WorkingLog.Add("Ошибка пропущена");

                }
            }
            else
            {
                InterfaceDataExchange.Errors.NoLocalSQL = true;
            }

            if (ActiveDevices.HasFlag(WorkingModule.RemoteDB))
            {
                WorkingLog.Add("Запуск модуля работы с базой данных архива");
                InterfaceDataExchange.SendCommand(ModuleCommand.ArchiveDBStart);
                if (InterfaceDataExchange.Errors.NoRemoteSQL)
                {
                    ErrorMsg = "Ошибка при запуске модуля работы с архивом";
                    WorkingLog.Add(ErrorMsg);
                    DevicesErrors |= WorkingModule.RemoteDB;
                    if (ActiveDevices.HasFlag(WorkingModule.RemoteDB))
                    {
                        WorkingLog.Add("Остановка работы");
                        DoMCShowErrorMessage(ErrorMsg);
                        return false;
                    }
                    WorkingLog.Add("Ошибка пропущена");
                }
            }
            else
            {
                InterfaceDataExchange.Errors.NoRemoteSQL = true;
            }

            if (ActiveDevices.HasFlag(WorkingModule.RDPB))
            {
                if (!StartRDPB())
                {
                    return false;
                }
            }
            InterfaceDataExchange.CCDDataEchangeStatuses.ExternalStart = true;
            InterfaceDataExchange.CCDDataEchangeStatuses.FastRead = true;
            pbStartStop.Text = "Стоп";
            InterfaceDataExchange.IsWorkingModeStarted = true;
            pbStartStop.BackColor = Color.Green;


            return true;
        }

        private bool StartRDPB()
        {
            string ErrorMsg = "";
            WorkingLog.Add("Запуск модуля бракёра");
            try
            {
                InterfaceDataExchange.SendCommand(ModuleCommand.RDPBStart);
            }
            catch { InterfaceDataExchange.RDPBCurrentStatus.IsStarted = false; }
            if (!InterfaceDataExchange.RDPBCurrentStatus.IsStarted)
            {
                ErrorMsg = "Ошибка при запуске модуля бракёра";
                WorkingLog.Add(ErrorMsg);
                DevicesErrors |= WorkingModule.RDPB;
                if (ActiveDevices.HasFlag(WorkingModule.RDPB))
                {
                    WorkingLog.Add("Остановка работы");
                    DoMCShowErrorMessage(ErrorMsg);
                    return false;
                }
                WorkingLog.Add("Ошибка пропущена");
            }
            DevicesErrors &= ~WorkingModule.RDPB;
            var coolingBlocks = InterfaceDataExchange.Configuration.RemoveDefectedPreformBlockConfig.CoolingBlocksQuantity;
            WorkingLog.Add($"Установка количества охлаждающих блоков: {coolingBlocks}");
            InterfaceDataExchange.RDPBCurrentStatus.CoolingBlocksQuantity = coolingBlocks;
            InterfaceDataExchange.SendCommand(ModuleCommand.RDPBSetCoolingBlockQuantity);
            Thread.Sleep(10);
            WorkingLog.Add("Включение бракера");
            InterfaceDataExchange.SendCommand(ModuleCommand.RDPBOn);
            Thread.Sleep(10);
            return true;
        }

        private void StopRDPB()
        {
            WorkingLog.Add("Отключение бракёра");
            InterfaceDataExchange.SendCommand(ModuleCommand.RDPBOff);
            WorkingLog.Add("Остановка модуля бракера");
            InterfaceDataExchange.SendCommand(ModuleCommand.RDPBStop);
        }

        private void StopWork()
        {
            InterfaceDataExchange.IsWorkingModeStarted = false;
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

            WorkingLog.Add("Остановка БУС");
            //var startLCBEWorkModeTime = DateTime.Now;
            InterfaceDataExchange.SendCommand(ModuleCommand.SetLCBNonWorkModeRequest);

            WorkingLog.Add("Остановка чтения и сброс плат ПЗС");
            InterfaceDataExchange.SendCommand(ModuleCommand.StartIdle);
            InterfaceDataExchange.SendResetToCCDCards();
            InterfaceDataExchange.SendCommand(ModuleCommand.CCDStop);
            Thread.Sleep(200);

            /*var res = UserInterfaceControls.Wait(CardTimeout, () => InterfaceDataExchange.LEDStatus.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.SetLCBWorkModeResponse && InterfaceDataExchange.LEDStatus.LastCommandResponseReceived > startLCBEWorkModeTime);
            if (!res)
            {
                WorkingLog.Add($"Ошибка при остановке работы БУС");
            }*/

            StopRDPB();

            WorkingLog.Add("Остановка базы данных");
            InterfaceDataExchange.SendCommand(ModuleCommand.DBStop);
            WorkingLog.Add("Остановка базы данных архива");
            InterfaceDataExchange.SendCommand(ModuleCommand.ArchiveDBStop);

            pbStartStop.IsPressed = false;

        }

        public void StartFailed()
        {
            InterfaceDataExchange.SendCommand(ModuleCommand.CCDStop);
            pbStartStop.BackColor = Color.Red;
            pbStartStop.IsPressed = false;
        }

        private void StartReading()
        {
            WorkingLog.Add("Запуск чтения");
            InterfaceDataExchange.IsWorkingModeStarted = true;
            InterfaceDataExchange.WasErrorWhileWorked = false;
            InterfaceDataExchange.CCDDataEchangeStatuses.FastRead = true;
            ForceStop = false;
            WorkingThread = new Thread(WorkProc);
            WorkingThread.Start();

        }
        private void StopReading()
        {
            InterfaceDataExchange.IsWorkingModeStarted = false;
            WorkingThread?.Abort();
            WorkingLog.Add("Остановка работы");
            InterfaceDataExchange.SendCommand(ModuleCommand.CCDStop);

        }




        private void DoMCNotAbleLoadConfigurationErrorMessage(DoMCInterfaceDataExchange.LoadError error)
        {
            MessageBox.Show($"Не могу загрузить конфигурацию в плату ({error})", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        private void DoMCShowErrorMessage(string Text)
        {
            MessageBox.Show(Text, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        private void WorkProc()
        {
            WorkingLog.Add("Начало чтения");
            InterfaceDataExchange.CCDReadsFailed = 0;
            InterfaceDataExchange.RDPBCurrentStatus.PreviousDirection = RDPBTransporterSide.NotSet;
            try
            {
                while (InterfaceDataExchange.IsWorkingModeStarted)
                {
                    WorkingLog.Add("--------------- Начало цикла чтения ---------------");

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

                    // Если нет очереди для хранения циклов перед записью в базу, то создаем ее
                    if (InterfaceDataExchange.CyclesCCD == null) InterfaceDataExchange.CyclesCCD = new System.Collections.Concurrent.ConcurrentQueue<CycleImagesCCD>();
                    WorkingLog.Add("Эталоны 0: " + InterfaceDataExchange.SocketStandardExist());

                    WorkingStep = WorkStep.Prepare;
                    WorkingLog.Add("Подготовка к чтению");
                    // Инициализация данных текущего цикла
                    InterfaceDataExchange.CurrentCycleCCD = new CycleImagesCCD();
                    InterfaceDataExchange.CurrentCycleCCD.Differences = new short[InterfaceDataExchange.Configuration.SocketQuantity][,];
                    InterfaceDataExchange.CurrentCycleCCD.WorkModeImages = new short[InterfaceDataExchange.Configuration.SocketQuantity][,];
                    InterfaceDataExchange.CurrentCycleCCD.StandardImage = new short[InterfaceDataExchange.Configuration.SocketQuantity][,];
                    InterfaceDataExchange.CurrentCycleCCD.IsSocketGood = new bool[InterfaceDataExchange.Configuration.SocketQuantity];
                    InterfaceDataExchange.CurrentCycleCCD.IsSocketHasImage = new bool[InterfaceDataExchange.Configuration.SocketQuantity];
                    // В текущий съем говорим, какие гнезда мы сохраняем всегда, не зависимо от их состояния
                    InterfaceDataExchange.CurrentCycleCCD.SocketsToSave = new bool[96];
                    Array.Copy(InterfaceDataExchange.Configuration.SocketsToSave, InterfaceDataExchange.CurrentCycleCCD.SocketsToSave, 96);

                    InterfaceDataExchange.CurrentCycleCCD.SocketsToCheck = new bool[96];
                    Array.Copy(InterfaceDataExchange.Configuration.SocketsToCheck, InterfaceDataExchange.CurrentCycleCCD.SocketsToCheck, 96);

                    // Если рабочий режим не запущен или система не может работать, потому что не загружена конфигурация, останавливаем и выходим с ошибкой
                    if (!InterfaceDataExchange.IsWorkingModeStarted || !IsConfigurationLoadedSuccessfully)
                    {

                        InterfaceDataExchange.IsWorkingModeStarted = false;
                        InterfaceDataExchange.WasErrorWhileWorked = true;
                        IsConfigurationLoadedSuccessfully = false;
                        break;

                    }

                    WorkingStep = WorkStep.WaitForSyncroSignal;
                    bool WasLastOperationSuccessful = true;

                    WorkingLog.Add("Эталоны 1: " + InterfaceDataExchange.SocketStandardExist());

                    // сохраняем для проверки был ли синхросигнал
                    var synchrosignalTimeBeforeCCDRead = InterfaceDataExchange.LEDStatus.TimeSyncSignalGot;

                    InterfaceDataExchange.Timings.CCDStart = DateTime.Now;
                    WorkingLog.Add("Запуск ожидания результатов чтения");

                    //Запрашиваем чтение ПЗС матриц по внешнему имульсу
                    InterfaceDataExchange.SendCommand(ModuleCommand.StartReadExternal);
                    //Ждем пока произойдет чтение
                    WasLastOperationSuccessful = UserInterfaceControls.Wait(CardTimeout, () =>
                    {
                        //return InterfaceDataExchange.CardsConnection.IsLastCommandComplete(1);
                        return InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartReadExternal && InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Complete;
                    }, () => InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
                    InterfaceDataExchange.Timings.CCDEnd = DateTime.Now;
                    // Если таймаут то говорим, что ошибка и выходим
                    if (!WasLastOperationSuccessful && ActiveDevices.HasFlag(WorkingModule.LCB))
                    {
                        WorkingLog.Add($"Ошибка при чтении данных гнезд. Чтение не завершено.");
                        WorkingLog.Add("Чтение состояния БУС");
                        var start = DateTime.Now;
                        InterfaceDataExchange.SendCommand(ModuleCommand.GetLCBEquipmentStatusRequest);
                        var LCBres = UserInterfaceControls.Wait(1000, () => { return InterfaceDataExchange.LEDStatus.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.GetLCBEquipmentStatusResponse && InterfaceDataExchange.LEDStatus.LastCommandResponseReceived > start; });
                        if (LCBres)
                        {
                            // если БУС - Авария
                            if (InterfaceDataExchange.LEDStatus.Outputs[3])
                            {
                                WorkingLog.Add("БУС авария");
                                if (InterfaceDataExchange.LEDStatus.Inputs[6])
                                {
                                    WorkingLog.Add("Маячки спрятаны. Перезапуск");
                                    InterfaceDataExchange.SendCommand(ModuleCommand.SetLCBNonWorkModeRequest);
                                    Thread.Sleep(200);
                                    InterfaceDataExchange.SendCommand(ModuleCommand.SetLCBWorkModeRequest);
                                    Thread.Sleep(200);
                                    continue;
                                }
                                else
                                {
                                    WorkingLog.Add("Маячки выдвинуты. Остановка работы");
                                    InterfaceDataExchange.Errors.CCDNotRespond = true;
                                    InterfaceDataExchange.Errors.LCBDoesNotRespond = true;
                                    InterfaceDataExchange.IsWorkingModeStarted = false;
                                    InterfaceDataExchange.WasErrorWhileWorked = true;
                                    break;
                                }
                            }
                            else
                            {
                                InterfaceDataExchange.Errors.CCDNotRespond = true;
                                InterfaceDataExchange.Errors.LCBDoesNotRespond = false;
                                WorkingLog.Add("БУС - OK");
                            }

                        }
                        else
                        {
                            WorkingLog.Add("БУС не ответил");
                            InterfaceDataExchange.Errors.CCDNotRespond = true;
                            InterfaceDataExchange.Errors.LCBDoesNotRespond = true;
                            InterfaceDataExchange.IsWorkingModeStarted = false;
                            InterfaceDataExchange.WasErrorWhileWorked = true;
                            break;
                        }

                        if (InterfaceDataExchange.CCDReadsFailed >= InterfaceDataExchange.CCDReadsFailedMax)
                        {
                            WorkingLog.Add("Ошибка при чтении данных гнезд. Чтение не завершено. Остановка.");
                            InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                            InterfaceDataExchange.Errors.CCDNotRespond = true;
                            InterfaceDataExchange.IsWorkingModeStarted = false;
                            InterfaceDataExchange.WasErrorWhileWorked = true;
                            break;
                        }
                        else
                        {
                            InterfaceDataExchange.CCDReadsFailed++;
                            //wait for syncrosignal
                            WorkingLog.Add($"Увеличение счетчика ошибок. Ошибок: {InterfaceDataExchange.CCDReadsFailed}");

                            WorkingLog.Add($"Ожидание синхросигнала");
                            var wasSynchro = UserInterfaceControls.Wait(
                                InterfaceDataExchange.Configuration.Timeouts.WaitForSynchrosignalTimoutAfterCCDReadingFailed,
                                () => synchrosignalTimeBeforeCCDRead != InterfaceDataExchange.LEDStatus.TimeSyncSignalGot
                                );
                            if (wasSynchro)
                            {
                                WorkingLog.Add($"Синхросигнал получен");
                            }
                            else
                            {
                                if (InterfaceDataExchange.Errors.LCBDoesNotRespond)
                                {
                                    WorkingLog.Add($"Синхросигнал не получен. Ошибка работы БУС?");
                                }
                                else
                                {
                                    WorkingLog.Add($"Синхросигнал не получен. Машина стоит?");

                                }
                            }

                            continue;

                        }
                    }
                    else
                    {
                        InterfaceDataExchange.CCDReadsFailed = 0;
                    }
                    //если успешно, то ошибки нет
                    InterfaceDataExchange.Errors.CCDNotRespond = false;

                    //if (!InterfaceDataExchange.Configuration.RegisterEmptyImages) 
                    {
                        var synchrotimeOnCCD = InterfaceDataExchange.CardsConnection.TimeSinceLastSynchrosignalOnCCD();
                        WorkingLog.Add(String.Join(" ", synchrotimeOnCCD.Select(s => s / 1000d)));


                    }
                    WorkingLog.Add("Эталоны 2: " + InterfaceDataExchange.SocketStandardExist());

                    //Фиксируем момент, когда закончилось чтение изображений
                    //InterfaceDataExchange.CurrentCycleCCD.CycleCCDDateTime = DateTime.Now;

                    var synchrosignalTimeAfterCCDRead = InterfaceDataExchange.LEDStatus.TimeSyncSignalGot;
                    //проверяем пришел ли нам синхросигнал от БУС
                    if (synchrosignalTimeAfterCCDRead == synchrosignalTimeBeforeCCDRead)
                    {
                        //Если нет, то выставляем ошибку и время синхроимульса ставим, как время получения изображений
                        // Тест: Синхроимпульс не ставим, а пропускаем этот съем
                        InterfaceDataExchange.Errors.LCBDoesNotSendSync = true;
                        InterfaceDataExchange.Errors.MissedSyncrosignalCounter++;
                        InterfaceDataExchange.LEDStatus.TimePreviousSyncSignalGot = InterfaceDataExchange.LEDStatus.TimeSyncSignalGot;
                        InterfaceDataExchange.LEDStatus.TimeSyncSignalGot = InterfaceDataExchange.Timings.CCDEnd.AddSeconds(-5);
                    }
                    else
                    {
                        //Если пришел, то ошибку убираем
                        InterfaceDataExchange.Errors.LCBDoesNotSendSync = false;
                        InterfaceDataExchange.Errors.MissedSyncrosignalCounter = 0;
                    }
                    InterfaceDataExchange.CurrentCycleCCD.CycleCCDDateTime = InterfaceDataExchange.LEDStatus.TimeSyncSignalGot;

                    //Если прошло меньше 1 секунды между синхросигналом и ответом о завершении, то в программе ошибка
                    WorkingLog.Add($"Время начала чтения: {InterfaceDataExchange.Timings.CCDStart:dd-MM-yyyy HH\\:mm\\:ss.fff}");
                    WorkingLog.Add($"Время синхросигнала: {InterfaceDataExchange.CurrentCycleCCD.CycleCCDDateTime:dd-MM-yyyy HH\\:mm\\:ss.fff}");
                    WorkingLog.Add($"Время завершения чтения: {InterfaceDataExchange.Timings.CCDEnd:dd-MM-yyyy HH\\:mm\\:ss.fff}");
                    if (Math.Abs((InterfaceDataExchange.Timings.CCDEnd - InterfaceDataExchange.Timings.CCDStart).TotalSeconds) < 1)
                    {
                        WorkingLog.Add("Программная ошибка при проверке на готовность данных");
                        WorkingLog.Add($"Статус модуля: {InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus}");
                        WorkingLog.Add($"Шаг модуля: {InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep }");

                        InterfaceDataExchange.CardsConnection.WriteSocketStatusLog(WorkingLog, "Статусы гнезд перед посылкой на чтение изображения");
                    }

                    WorkingLog.Add("Эталоны 3: " + InterfaceDataExchange.SocketStandardExist());

                    WorkingStep = WorkStep.StartReadingImages;

                    WorkingLog.Add("Запрос на получение изображений гнезд");
                    InterfaceDataExchange.Timings.CCDGetImagesStarted = DateTime.Now;
                    //Запрашиваем сами изображения гнезд
                    InterfaceDataExchange.SendCommand(ModuleCommand.GetSocketImages);
                    WasLastOperationSuccessful = UserInterfaceControls.Wait(5 * CardTimeout, () =>
                    {
                        var mst = InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus;
                        var stp = InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep;
                        return mst == ModuleCommand.GetSocketImages && (stp == ModuleCommandStep.Complete || stp == ModuleCommandStep.Error);
                    }, () => InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
                    InterfaceDataExchange.Timings.CCDGetImagesEnded = DateTime.Now;

                    //если не получили или в модуле ПЗС ошибка, то прерываем работу и выставляем ошибку
                    if (!WasLastOperationSuccessful || InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Error)
                    {
                        WorkingLog.Add("Ошибка при получении изображения.");// Остановка");
                        InterfaceDataExchange.Errors.CCDNotRespond = true;

                        WorkingLog.Add("Полученные кадры гнезд:");
                        try
                        {
                            for (int i = 0; i < 16; i++)
                            {
                                StringBuilder sb = new StringBuilder();
                                for (int j = 0; j < 6; j++)
                                {
                                    var socketNum = j * 16 + i + 1;
                                    sb.Append(InterfaceDataExchange.CardsConnection[socketNum].IsImageReady ? "+" : " ");
                                    sb.Append("  ");
                                }
                                WorkingLog.Add(sb.ToString());
                                if (i == 7) WorkingLog.Add("");
                            }
                        }
                        catch { }

                        /*
                        var timages = new short[InterfaceDataExchange.CardsConnection.SocketQuantity][,];
                        foreach (var socketE in InterfaceDataExchange.CardsConnection.Sockets)
                        {
                            var socket = socketE.Key;
                            if (!InterfaceDataExchange.Configuration.SocketsToCheck[socket - 1]) continue;
                                var image = InterfaceDataExchange.CardsConnection.GetImage(socket);
                                timages[socket - 1] = image;
                        }
                        InterfaceDataExchange.CCDDataEchangeStatuses.Images = timages;
                        */

                        continue;
                        //InterfaceDataExchange.IsWorkingModeStarted = false;
                        //InterfaceDataExchange.WasErrorWhileWorked = true;
                        //break;
                    }
                    else
                    {
                        WorkingLog.Add("Изображения получены");
                    }

                    var synchrosignalTimeAfterCCDImagesGot = InterfaceDataExchange.LEDStatus.TimeSyncSignalGot;
                    if (synchrosignalTimeAfterCCDImagesGot != synchrosignalTimeAfterCCDRead)
                    {
                        InterfaceDataExchange.Errors.NotEnoughTimeToGetCCD = true;

                    }
                    else
                    {
                        InterfaceDataExchange.Errors.NotEnoughTimeToGetCCD = false;
                    }

                    // иначе все хорошо и ошибки нет
                    InterfaceDataExchange.Errors.CCDNotRespond = false;
                    // проверяем есть ли что-нибудь в изображениях
                    var images = InterfaceDataExchange.CCDDataEchangeStatuses.Images;

                    WorkingLog.Add("Эталоны 4: " + InterfaceDataExchange.SocketStandardExist());


                    //если нет, то ошибка и выходим
                    if (images == null)
                    {
                        WorkingLog.Add("Изображения не получены.");// Остановка");
                        InterfaceDataExchange.Errors.CCDFrameNotReceived = true;
                        //InterfaceDataExchange.IsWorkingModeStarted = false;
                        //InterfaceDataExchange.WasErrorWhileWorked = true;
                        //break;
                    }
                    else
                    {
                        //иначе все ок, ошибки нет
                        InterfaceDataExchange.Errors.CCDFrameNotReceived = false;
                    }

                    //сохраняем изображения в данные текущего цикла
                    InterfaceDataExchange.CurrentCycleCCD.WorkModeImages = images;

                    //если получены данные о состоянии светодиодов, то
                    if (InterfaceDataExchange.LEDStatus.TimeLEDStatusGot > InterfaceDataExchange.LEDStatus.TimeSyncSignalGot)
                    {
                        InterfaceDataExchange.Errors.LEDStatusGettingError = false;
                    }
                    else
                    {
                        InterfaceDataExchange.Errors.LEDStatusGettingError = true;
                        InterfaceDataExchange.LEDStatus.LEDStatuses = new bool[12];
                    }

                    var newLedStatus = new bool[] {
                    InterfaceDataExchange.LEDStatus.LEDStatuses[0],
                    InterfaceDataExchange.LEDStatus.LEDStatuses[3],
                    InterfaceDataExchange.LEDStatus.LEDStatuses[1],
                    InterfaceDataExchange.LEDStatus.LEDStatuses[4],
                    InterfaceDataExchange.LEDStatus.LEDStatuses[2],
                    InterfaceDataExchange.LEDStatus.LEDStatuses[5],
                    InterfaceDataExchange.LEDStatus.LEDStatuses[6],
                    InterfaceDataExchange.LEDStatus.LEDStatuses[9],
                    InterfaceDataExchange.LEDStatus.LEDStatuses[7],
                    InterfaceDataExchange.LEDStatus.LEDStatuses[10],
                    InterfaceDataExchange.LEDStatus.LEDStatuses[8],
                    InterfaceDataExchange.LEDStatus.LEDStatuses[11],
                };
                    InterfaceDataExchange.LEDStatus.LEDStatuses = newLedStatus;

                    WorkingLog.Add("Эталоны 5: " + InterfaceDataExchange.SocketStandardExist());


                    if (!InterfaceDataExchange.Errors.CCDFrameNotReceived)
                    {
                        WorkingStep = WorkStep.SearchForDefectedPreforms;

                        InterfaceDataExchange.Timings.CCDImagesProcessStarted = DateTime.Now;

                        // проверяем есть ли статусы и устанавливаем статусы линеек светодиодов в данных текущего цикла
                        InterfaceDataExchange.CurrentCycleCCD.SetLEDStatuses(InterfaceDataExchange.LEDStatus.LEDStatuses, InterfaceDataExchange.LEDStatus.TimeSyncSignalGot);

                        ImageProcessParameters[] IPParray = new ImageProcessParameters[InterfaceDataExchange.Configuration.SocketQuantity];
                        WorkingLog.Add("Эталоны 6: " + InterfaceDataExchange.SocketStandardExist());

                        foreach (var kvp in InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations)
                        {
                            IPParray[kvp.Key - 1] = kvp.Value.ImageProcessParameters.Clone();
                        }
                        InterfaceDataExchange.CurrentCycleCCD.SetImageProcessParameters(IPParray);

                        WorkingLog.Add("Эталоны 7: " + InterfaceDataExchange.SocketStandardExist());

                        // сохраняем время синхроимпульса до начала обработки, для определения будет ли еще один синхроимульс во время обработки,
                        // т.е. хватает ли нам времени на обработку
                        var beforeSync = InterfaceDataExchange.LEDStatus.TimeSyncSignalGot; // Время получения сихроимпульса

                        // замеряем время  обработки
                        InterfaceDataExchange.CCDDataEchangeStatuses.StartProcessImages = InterfaceDataExchange.PreciseTimer.ElapsedTicks;
                        InterfaceDataExchange.Errors.ImageProcessError = false;

                        //Тестовое значение, чтобы не запутаться в потоках
                        //InterfaceDataExchange.MaxDegreeOfParallelism = 1;
                        //распараллеливаем процесс


                        WorkingLog.Add("Поиск отклонений в гнездах");
                        //сохраняем изображения эталонов во все гнезда
                        InterfaceDataExchange.SetStandardForAllCurrentCCDSockets();
                        WorkingLog.Add("Эталоны 8: " + InterfaceDataExchange.SocketStandardExist());

                        InterfaceDataExchange.CheckIfSocketHasImage();
                        var IsAllHaveImages = InterfaceDataExchange.CurrentCycleCCD.IsSocketHasImage.All(p => p);
                        WorkingLog.Add("Наличие изображений: " + InterfaceDataExchange.BoolArrayToHex(InterfaceDataExchange.CurrentCycleCCD.IsSocketHasImage) + InterfaceDataExchange.FalseBoolIndexPlus1(InterfaceDataExchange.CurrentCycleCCD.IsSocketHasImage));

                        InterfaceDataExchange.CheckIfAllSocketsGood();

                        if (!InterfaceDataExchange.Configuration.RegisterEmptyImages)
                        {
                            for (int i = 0; i < InterfaceDataExchange.Configuration.SocketQuantity; i++)
                            {
                                if (!InterfaceDataExchange.CurrentCycleCCD.IsSocketHasImage[i])
                                {
                                    InterfaceDataExchange.CurrentCycleCCD.IsSocketGood[i] = true;
                                }
                            }
                        }

                        var IsSetBad = InterfaceDataExchange.CurrentCycleCCD.IsSocketGood.Any(p => !p);
                        WorkingLog.Add("Хорошие гнезда: " + InterfaceDataExchange.BoolArrayToHex(InterfaceDataExchange.CurrentCycleCCD.IsSocketGood));

                        InterfaceDataExchange.Timings.CCDImagesProcessEnded = DateTime.Now;
                        WorkingLog.Add("Эталоны 9: " + InterfaceDataExchange.SocketStandardExist());

                        bool RDPBResult = true;

                        if (InterfaceDataExchange.RDPBCurrentStatus.IsStarted)
                        {
                            if (InterfaceDataExchange.RDPBCurrentStatus.BlockIsOn != ((ActiveDevices & WorkingModule.RDPB) == WorkingModule.RDPB))
                            {
                                InterfaceDataExchange.RDPBCurrentStatus.ErrorCounter++;
                                WorkingLog.Add($"Состояние бракера не соответствует состоянию выбранному пользователем. Проход: {InterfaceDataExchange.RDPBCurrentStatus.ErrorCounter}");
                                if (InterfaceDataExchange.RDPBCurrentStatus.ErrorCounter > 2)
                                {
                                    if ((ActiveDevices & WorkingModule.RDPB) == WorkingModule.RDPB)
                                    {
                                        WorkingLog.Add("Бракер выключен, но должен быть включен. Включаю бракер");
                                        InterfaceDataExchange.SendCommand(ModuleCommand.RDPBOn);
                                    }
                                    else
                                    {
                                        WorkingLog.Add("Бракер включен, но должен выключен. Выключаю бракер");
                                        InterfaceDataExchange.SendCommand(ModuleCommand.RDPBOff);
                                    }
                                }
                            }
                            else
                            {
                                InterfaceDataExchange.RDPBCurrentStatus.ErrorCounter = 0;
                            }
                            WorkingStep = WorkStep.RDPBSend;

                            // сообщить бракеру о съеме
                            if (IsSetBad && IsAllHaveImages)
                            {
                                if (InterfaceDataExchange.Configuration.RemoveDefectedPreformBlockConfig.SendBadCycleToRDPB)
                                {
                                    WorkingLog.Add("Съем: Плохой");
                                    try
                                    {
                                        InterfaceDataExchange.SendCommand(ModuleCommand.RDPBSetIsBad);
                                    }
                                    catch
                                    {
                                        RDPBResult = false;
                                    }
                                }
                                else
                                {
                                    WorkingLog.Add("Съем: Плохой (но посылаем, что хороший)");
                                    try
                                    {
                                        InterfaceDataExchange.SendCommand(ModuleCommand.RDPBSetIsOK);
                                    }
                                    catch
                                    {
                                        RDPBResult = false;
                                    }
                                }
                                InterfaceDataExchange.RDPBCurrentStatus.CurrentBoxDefectCycles++;
                                InterfaceDataExchange.RDPBCurrentStatus.TotalDefectCycles++;
                            }
                            else
                            {
                                if (IsAllHaveImages)
                                {
                                    WorkingLog.Add("Съем: OK");
                                }
                                else
                                {
                                    WorkingLog.Add("Съем: Не все гнезда прочитаны правильно, считаем, что съем хороший");

                                }
                                try
                                {
                                    InterfaceDataExchange.SendCommand(ModuleCommand.RDPBSetIsOK);
                                }
                                catch
                                {
                                    RDPBResult = false;
                                }
                            }

                            if (!RDPBResult)
                            {
                                InterfaceDataExchange.Errors.RemovingDefectedPreformsBlockError = true;
                                WorkingLog.Add("Ошибка бракера");
                            }
                            else
                            {
                                InterfaceDataExchange.Errors.RemovingDefectedPreformsBlockError = false;
                            }

                            var RDPBResponded = UserInterfaceControls.Wait(
                                    InterfaceDataExchange.Configuration.Timeouts.WaitForRDPBCardAnswerTimeout,
                                    () => InterfaceDataExchange.RDPBCurrentStatus.TimeLastSent < InterfaceDataExchange.RDPBCurrentStatus.TimeLastReceived
                                    );

                            if (!RDPBResponded)
                            {
                                WorkingLog.Add("Бракер не ответил");
                                DevicesErrors |= WorkingModule.RDPB;
                            }
                            else
                            {
                                DevicesErrors &= ~WorkingModule.RDPB;
                            }

                            var boxdir = InterfaceDataExchange.RDPBCurrentStatus.TransporterSide == RDPBTransporterSide.Left ? "Левый" : (
                                InterfaceDataExchange.RDPBCurrentStatus.TransporterSide == RDPBTransporterSide.Right ? "Правый" :
                                (InterfaceDataExchange.RDPBCurrentStatus.TransporterSide == RDPBTransporterSide.Stoped ? "Стоит" : "Ошибка"));
                            WorkingLog.Add("Короб: " + boxdir);

                            InterfaceDataExchange.RDPBCurrentStatus.CurrentTransporterSide = InterfaceDataExchange.RDPBCurrentStatus.TransporterSide;

                            if (InterfaceDataExchange.RDPBCurrentStatus.PreviousDirection != RDPBTransporterSide.NotSet && InterfaceDataExchange.RDPBCurrentStatus.PreviousDirection != InterfaceDataExchange.RDPBCurrentStatus.CurrentTransporterSide)
                            {
                                var box = new DoMCLib.Classes.Box();
                                box.BadCyclesCount = InterfaceDataExchange.RDPBCurrentStatus.CurrentBoxDefectCycles;
                                box.CompletedTime = DateTime.Now;
                                box.TransporterSide = InterfaceDataExchange.RDPBCurrentStatus.PreviousDirection;

                                InterfaceDataExchange.RDPBCurrentStatus.CurrentBoxDefectCycles = 0;
                                WorkingLog.Add("Смена короба");
                                lastBoxRead = DateTime.MinValue;
                                if (ActiveDevices.HasFlag(WorkingModule.LocalDB))
                                {
                                    WorkingLog.Add("Создаем новый короб");
                                    InterfaceDataExchange.Boxes.Enqueue(box);
                                }
                            }


                            if (InterfaceDataExchange.RDPBCurrentStatus.CoolingBlocksQuantity != InterfaceDataExchange.Configuration.RemoveDefectedPreformBlockConfig.CoolingBlocksQuantity)
                            {
                                InterfaceDataExchange.SendCommand(ModuleCommand.RDPBSetCoolingBlockQuantity);
                            }
                        }
                        WorkingLog.Add("Эталоны 10: " + InterfaceDataExchange.SocketStandardExist());
                        InterfaceDataExchange.CurrentCycleCCD.TransporterSide = InterfaceDataExchange.RDPBCurrentStatus.CurrentTransporterSide;
                        if (IsAllHaveImages)
                        {
                            WorkingStep = WorkStep.RecalcStandards;

                            InterfaceDataExchange.Timings.CCDEtalonsRecalculateStarted = DateTime.Now;
                            WorkingLog.Add("Перерасчет эталонов");
                            InterfaceDataExchange.RecalcAllStandards();
                        }
                        else
                        {
                            WorkingLog.Add("Эталоны не пересчитываем");

                        }
                        WorkingLog.Add("Эталоны 11: " + InterfaceDataExchange.SocketStandardExist());

                        WorkingStep = WorkStep.SaveConfiguration;

                        WorkingLog.Add("Сохранение текущей конфигурации");
                        InterfaceDataExchange.Configuration.Save();
                        InterfaceDataExchange.Timings.CCDEtalonsRecalculateEnded = DateTime.Now;
                        WorkingLog.Add("Эталоны 12: " + InterfaceDataExchange.SocketStandardExist());

                        // запоминаем время завершения обработки изображений
                        InterfaceDataExchange.CCDDataEchangeStatuses.StopProcessImages = InterfaceDataExchange.PreciseTimer.ElapsedTicks;
                        var ProcessDuration = InterfaceDataExchange.CCDDataEchangeStatuses.ProcessDuration * 1e-4;

                        //Если к этому моменту в очереди циклов больше 10 циклов, значит они не успевают сохраняться в базу данных или БД не работает
                        if (InterfaceDataExchange.CyclesCCD.Count > 10)
                        {
                            InterfaceDataExchange.Errors.NotEnoughTimeToProcessSQL = true;
                        }
                        else
                        {
                            InterfaceDataExchange.Errors.NotEnoughTimeToProcessSQL = false;
                        }

                        if (!InterfaceDataExchange.Errors.NoLocalSQL)
                        {
                            try
                            {
                                WorkingStep = WorkStep.SendToDB;

                                // ставим текущий цикл в общую очередь циклов для сохранения
                                InterfaceDataExchange.CyclesCCD.Enqueue(InterfaceDataExchange.CurrentCycleCCD);
                                WorkingLog.Add($"Данные съема поставлены в очередь на запись. В очереди {InterfaceDataExchange.CyclesCCD.Count} элемент(а,ов)");
                            }
                            catch (Exception ex)
                            {
                                WorkingLog.Add($"Ошибка при постановке в очередь ({ex.Message}).");
                                if (ActiveDevices.HasFlag(WorkingModule.LocalDB))
                                {
                                    InterfaceDataExchange.WasErrorWhileWorked = true;
                                    MessageBox.Show(ex.Message, "Ошибка передачи в БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    continue;
                                }

                            }
                        }

                        WorkingLog.Add("Эталоны 13: " + InterfaceDataExchange.SocketStandardExist());


                        var afterSync = InterfaceDataExchange.LEDStatus.TimeSyncSignalGot; // Время получения сихроимпульса
                                                                                           // Если был синхроимпульс пока мы обрабатывали, значит мы не успеваем и второй съем не читался вообще

                        if (afterSync != beforeSync)
                        {
                            //выставляем ошибку, что мы не успеваем обрабатывать изображения
                            InterfaceDataExchange.Errors.NotEnoughTimeToProcessData = true;
                        }
                        else
                        {
                            InterfaceDataExchange.Errors.NotEnoughTimeToProcessData = false;
                        }

                        //WorkingLog.Add("Эталоны 14: " + InterfaceDataExchange.SocketStandardExist());

                        //добавляем результаты проверки гнезд в список
                        socketStatuses.Add(InterfaceDataExchange.CurrentCycleCCD.TimeLCBSyncSignalGot, InterfaceDataExchange.CurrentCycleCCD.IsSocketGood);
                        for (int i = 0; i < InterfaceDataExchange.CurrentCycleCCD.IsSocketGood.Length; i++)
                            if (!InterfaceDataExchange.CurrentCycleCCD.IsSocketGood[i])
                                ErrorsBySockets[i]++;
                        /*
                             InterfaceDataExchange.Errors.LEDStatusGettingError = false;
                             InterfaceDataExchange.Errors.NotEnoughTimeToProcessData = false;
                             InterfaceDataExchange.Errors.NotEnoughTimeToProcessSQL = false;
                             InterfaceDataExchange.Errors.NotEnoughTimeToGetCCD = false;
                         */
                    }

                    //WorkingLog.Add("Эталоны 15: " + InterfaceDataExchange.SocketStandardExist());
                    if (InterfaceDataExchange.Errors.MissedSyncrosignalCounter > 5)
                    {
                        if (!InterfaceDataExchange.Errors.LCBDoesNotRespond)
                        {
                            InterfaceDataExchange.Errors.LCBDoesNotSendSync = true;
                            InterfaceDataExchange.ReconnectToLCB();
                            InterfaceDataExchange.Errors.MissedSyncrosignalCounter = 0;
                        }
                    }
                    //WorkingLog.Add("Эталоны 16: " + InterfaceDataExchange.SocketStandardExist());
                    InterfaceDataExchange.RDPBCurrentStatus.PreviousDirection = InterfaceDataExchange.RDPBCurrentStatus.CurrentTransporterSide;

                    WorkingStep = WorkStep.ClearMemory;
                    WorkingLog.Add("Очистка неиспользуемой памяти");
                    GC.Collect();

                }
            }
            catch (ThreadAbortException taex)
            {
                WorkingLog.Add("Работа принудительно остановлена. Сохранение текущей конфигурации");
                InterfaceDataExchange.Configuration.Save();
            }
            catch (Exception ex)
            {
                WorkingLog.Add("Ошибка при работе.", ex);
            }
            // если была критическая ошибка, останавливаем работу
            if (InterfaceDataExchange.WasErrorWhileWorked)
            {
                ForceStop = true;
            }
        }

        private void GetWorkModeStandard_Click(object sender, EventArgs e)
        {
            var ctrl = (Control)sender;
            var socketnumber = (int)ctrl.Tag;
            if (InterfaceDataExchange.CurrentCycleCCD != null)
            {
                var sf = new DoMCInterface.ShowFrameForm();
                sf.Text = "Цикл: " + InterfaceDataExchange.CurrentCycleCCD.CycleCCDDateTime.ToString() + " Номер гнезда: " + socketnumber;
                sf.Image = InterfaceDataExchange.CurrentCycleCCD.Differences[socketnumber - 1];
                sf.Show();
            }
        }

        private void miLoadStandard_Click(object sender, EventArgs e)
        {
            var dir = System.IO.Path.Combine(Application.StartupPath, ApplicationCardParameters.StandardsPath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            var od = new OpenFileDialog();
            od.Filter = "Эталоны (*.std)|*.std|Все файлы (*.*)|*.*";
            od.InitialDirectory = dir;
            od.DefaultExt = "std";
            od.AddExtension = true;
            if (od.ShowDialog() == DialogResult.OK)
            {
                InterfaceDataExchange.Configuration.LoadStandard(od.FileName);
                SetWindowStandardTitle(od.FileName);
            }
        }

        /*private void pbFrame_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, pbFrame.ClientRectangle, Color.Red, ButtonBorderStyle.Solid);
        }*/

        private Bitmap DrawMatrix(Size rectSize, bool[] IsSocketGood, bool[] SocketsToSave, int[] ErrorSumBySocket, bool[] IsSocketChecking, Color goodColor, Color badColor, Color blockedSocketColor, Color forSavingColor, bool showErrors)
        {
            var indent = 5;
            var bmp = new Bitmap(rectSize.Width - 2 * indent, rectSize.Height - 2 * indent);
            if (InterfaceDataExchange.Configuration.SocketQuantity != 96) return bmp;
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

            if (InterfaceDataExchange != null)
            {
                //если в этот съем еще не рисовали, то рисуем
                if (lastDrawCycleTime != (InterfaceDataExchange?.CurrentCycleCCD?.CycleCCDDateTime ?? DateTime.MinValue))
                {
                    CurrentDraw();
                    lastDrawCycleTime = InterfaceDataExchange?.CurrentCycleCCD?.CycleCCDDateTime ?? DateTime.MinValue;
                    lblCycleDurationValue.Text = InterfaceDataExchange.LEDStatus.CycleDuration().TotalSeconds.ToString("F1") + " с";

                    lblCurrentBoxDefectCycles.Text = InterfaceDataExchange.RDPBCurrentStatus.CurrentBoxDefectCycles.ToString();
                }
                if (InterfaceDataExchange.IsWorkingModeStarted)
                {
                    if ((DateTime.Now - InterfaceDataExchange.LEDStatus.TimeSyncSignalGot).TotalSeconds < 2)
                    {
                        pnlCurrentSockets.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        pnlCurrentSockets.BackColor = this.BackColor;
                    }
                }
                lblTotalDefectCycles.Text = InterfaceDataExchange.RDPBCurrentStatus.TotalDefectCycles.ToString();

                if (InterfaceDataExchange.DataStorage != null && InterfaceDataExchange.DataStorage.LocalIsActive && (now - lastBoxRead) > lastBoxReadTime)
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
                }
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
            if (InterfaceDataExchange != null)
            {
                lbCurrentErrors.Items.Clear();
                var errors = InterfaceDataExchange.GetErrors();
                var dict = errors.ToDictionary();
                foreach (var p in dict)
                {
                    if (p.Value)
                        lbCurrentErrors.Items.Add(errors.KeyToText(p.Key));
                }
                if (Log.WasError)
                {
                    lbCurrentErrors.Items.Add($"Ошибка записи журналов ({Log.ErrorMessage})");
                }
            }

        }

        private void pnlCurrentSockets_Paint(object sender, PaintEventArgs e)
        {
            var goodsockets = socketStatuses.GetLast();// InterfaceDataExchange?.CurrentCycleCCD?.IsSocketGood ?? new bool[96];
            var socketsToSave = InterfaceDataExchange?.Configuration?.SocketsToSave ?? new bool[96];
            var IsSocketsChecking = InterfaceDataExchange?.Configuration?.SocketsToCheck ?? new bool[96];
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
            if (InterfaceDataExchange.IsWorkingModeStarted)
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
            }
            catch { }
            InterfaceDataExchange.SendCommand(ModuleCommand.StopModuleWork);
            try
            {
                WorkingLog?.StopLog();
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



        private void miMainInterfaceLogsArchive_Click(object sender, EventArgs e)
        {
            FileAndDirectoryTools.OpenFolder(Log.GetPath(Log.LogModules.MainSystem));
        }

        private void miLCBLogsArchive_Click(object sender, EventArgs e)
        {
            FileAndDirectoryTools.OpenFolder(Log.GetPath(Log.LogModules.LCB));
        }

        private void miRDPBLogsArchive_Click(object sender, EventArgs e)
        {
            FileAndDirectoryTools.OpenFolder(Log.GetPath(Log.LogModules.RDPB));
        }

        private void miDBLogsArchive_Click(object sender, EventArgs e)
        {
            FileAndDirectoryTools.OpenFolder(Log.GetPath(Log.LogModules.DB));
        }

        private void miInterfaceLogs_Click(object sender, EventArgs e)
        {
            FileAndDirectoryTools.OpenNotepad(Log.GetLogFileName(Log.LogModules.MainSystem, Log.GetCurrentShiftDate()));
        }

        private void miLCBLogs_Click(object sender, EventArgs e)
        {
            FileAndDirectoryTools.OpenNotepad(Log.GetLogFileName(Log.LogModules.LCB, Log.GetCurrentShiftDate()));

        }

        private void miRDPBLogs_Click(object sender, EventArgs e)
        {
            FileAndDirectoryTools.OpenNotepad(Log.GetLogFileName(Log.LogModules.RDPB, Log.GetCurrentShiftDate()));

        }

        private void miDBLogs_Click(object sender, EventArgs e)
        {
            FileAndDirectoryTools.OpenNotepad(Log.GetLogFileName(Log.LogModules.DB, Log.GetCurrentShiftDate()));

        }

        private void DoMCWorkModeInterface_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (InterfaceDataExchange.IsWorkingModeStarted)
                {
                    if (MessageBox.Show("ПМК запущено. Вы уверены, что хотите закрыть окно рабочего режима", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    {
                        e.Cancel = true;
                    }
                }
            }
            catch
            {

            }
        }

        private void miInnerVariables_Click(object sender, EventArgs e)
        {
            var innerform = new DoMCInnerVarsForm(InterfaceDataExchange);
            innerform.Show();
        }

        private void miResetCounter_Click(object sender, EventArgs e)
        {
            InterfaceDataExchange.RDPBCurrentStatus.TotalDefectCycles = 0;
        }

        private void miSettings_Click(object sender, EventArgs e)
        {
            try
            {
                if (!InterfaceDataExchange.IsWorkingModeStarted || MessageBox.Show("ПМК запущено. Вы уверены, что хотите закрыть окно рабочего режима?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        StopWork();
                    }
                    catch { }
                    var wmf = new DoMCInterface.DoMCSettingsInterface();
                    wmf.SetMemoryReference(globalMemory);
                    this.Hide();
                    try
                    {
                        wmf.ShowDialog();
                    }
                    catch { }
                    this.Show();
                }
            }
            catch
            {

            }
        }

        private void miCreateNewStandard_Click(object sender, EventArgs e)
        {
            if (InterfaceDataExchange.IsWorkingModeStarted)
            {
                MessageBox.Show("ПМК запущено. Для создания эталонов нужно остановить работу ПМК", "Создание эталонов");
                return;
            }
            var frm = new DoMCInterface.DoMCStandardCreateInterface();
            frm.SetMemoryReference(globalMemory);
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
            var dir = System.IO.Path.Combine(Application.StartupPath, ApplicationCardParameters.StandardsPath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            var sd = new SaveFileDialog();
            sd.InitialDirectory = dir;
            sd.DefaultExt = "std";
            sd.AddExtension = true;
            sd.Filter = "Эталоны (*.std)|*.std|Все файлы (*.*)|*.*";
            if (sd.ShowDialog() == DialogResult.OK)
            {
                InterfaceDataExchange.Configuration.SaveStandard(sd.FileName);
                SetWindowStandardTitle(sd.FileName);
            }
        }

        private void SetWindowStandardTitle(string standardName = null)
        {
            var title = "Рабочий режим ПМК";
            if (!String.IsNullOrEmpty(standardName))
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
            form.SocketParameters = InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations;
            if (form.ShowDialog() == DialogResult.OK)
            {
                InterfaceDataExchange.Configuration.Save();
            }
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
        Prepare,
        WaitForSyncroSignal,
        StartReadingImages,
        SearchForDefectedPreforms,
        RDPBSend,
        SendToDB,
        RecalcStandards,
        SaveConfiguration,
        ClearMemory
    }
}
