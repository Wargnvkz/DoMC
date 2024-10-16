using DoMCLib.Tools;
using DoMCLib.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DoMCLib.Classes;
using System.Collections.Concurrent;
using System.IO;
using DoMC;
using System.Threading.Tasks;
using System.Net;
using DoMCLib.Exceptions;
using System.Data.SqlClient;
using DoMCModuleControl;
using DoMCModuleControl.Logging;
using DoMC.Tools;
using DoMCLib.Classes.Module.LCB;
using DoMCLib.Classes.Module.RDPB;
using DoMCLib.Classes.Module.CCD;
using DoMC.Classes;
using DoMCLib.Classes.Module.CCD.Commands.Classes;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Modules;

namespace DoMC
{
    public partial class DoMCSettingsInterface : Form
    {

        DoMCLib.Forms.ShowPreformImages checkpreformalgorithmsForm;

        bool invertColors = false;
        //bool IsAbleToWork = false;
        bool TestLCBConnected = false;
        bool TestRDPBConnected = false;
        bool showMaxDevPoint = false;
        int AdminPinCode = 1234;
        bool LCBSettingsPreformLengthGotFromConfig = false;
        bool LCBSettingsDelayLengthGotFromConfig = false;

        IMainController Controller;
        ILogger WorkingLog;
        Observer observer;
        DoMCLib.Classes.ApplicationContext Context;

        SettingsPageSocketsStatus SettingsPageSocketsStatusShow = SettingsPageSocketsStatus.IsSocketSettingsOk;

        Bitmap bmpCheckSign = new Bitmap(512, 512);
        DoMCArchiveForm archiveForm = null;

        ModelCommandProcessor contextProcessor;

        public DoMCSettingsInterface(IMainController controller, DoMCLib.Classes.ApplicationContext context)
        {
            InitializeComponent();
            WorkingLog = controller.GetLogger("SettingInterface");
            observer = controller.GetObserver();
            Context = context;
            WorkingLog.Add(LoggerLevel.Critical, "Запускт интерфейса настройки");
            //observer.NotificationReceivers += Observer_NotificationReceived;
            //CreateDataExchange();
            contextProcessor = new ModelCommandProcessor(controller, Context);
            InitControls();

        }

        private void InitControls()
        {
            WorkingLog.Add(LoggerLevel.Critical, "SetFormSchema");
            SetFormSchema();
            WorkingLog.Add(LoggerLevel.Critical, "FillSettingPage");
            FillSettingPage();
            TestLCBOutputs = new CheckBox[] { cbTestLCBOutput0, cbTestLCBOutput1, cbTestLCBOutput2, cbTestLCBOutput3, cbTestLCBOutput4, cbTestLCBOutput5 };
            TestLCBInputs = new CheckBox[] { cbTestLCBInput0, cbTestLCBInput1, cbTestLCBInput2, cbTestLCBInput3, cbTestLCBInput4, cbTestLCBInput5, cbTestLCBInput6, cbTestLCBInput7 };
            TestLCBLEDs = new CheckBox[] { cbTestLCBLED0, cbTestLCBLED1, cbTestLCBLED2, cbTestLCBLED3, cbTestLCBLED4, cbTestLCBLED5, cbTestLCBLED6, cbTestLCBLED7, cbTestLCBLED8, cbTestLCBLED9, cbTestLCBLED10, cbTestLCBLED11 };
            IsCycleStarted = false;
            TestCCDIsReading = false;

            try
            {
                checkpreformalgorithmsForm = new DoMCLib.Forms.ShowPreformImages();
                //checkpreformalgorithmsForm.WindowState = FormWindowState.Maximized;
                checkpreformalgorithmsForm.TopLevel = false;
                checkpreformalgorithmsForm.Parent = tbTestImages;
                checkpreformalgorithmsForm.Visible = true;
                checkpreformalgorithmsForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;


                var k = 0.8f;
                var screenpoint = tbTestImages.PointToScreen(new Point(0, 0));
                var thisscreen = Screen.FromControl(this);
                var screenrect = thisscreen.WorkingArea;

                checkpreformalgorithmsForm.StartPosition = FormStartPosition.Manual;
                checkpreformalgorithmsForm.Location = new Point(0, 0);
                checkpreformalgorithmsForm.Scale(k);

                checkpreformalgorithmsForm.Size = new Size((int)((screenrect.Width - screenpoint.X) / k), (int)((screenrect.Height - screenpoint.Y) / k));



            }
            catch (Exception) { }

            #region TestRDPB (Remove Defective Preforms Block)
            cbTestRDPBCoolingBlocksQuantity.SelectedIndex = 0;
            #endregion

            Application.ThreadException += Application_ThreadException;

            var bmpGraphics = Graphics.FromImage(bmpCheckSign);
            bmpGraphics.DrawString("✓", new Font("Arial", 300), new SolidBrush(Color.LimeGreen), new PointF(0, 0));
        }
        private void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            if (e.Exception is DoMCException)
            {
                DisplayMessage.Show(e.Exception.Message, "Ошибка");
            }
            else
            {
                DisplayMessage.Show(e.Exception.Message + "\r\n" + e.Exception.StackTrace, "Ошибка");
            }
        }

        #region Settings

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


        Panel[] SettingsSocketsPanelList;
        Panel[] StandardSettingsSocketsPanelList;
        Panel[] WorkModeStandardSettingsSocketsPanelList;
        Panel[] TestSocketsSettingsSocketsPanelList;

        int[] TestCCDSocketStatuses;

        private void FillSettingPage()
        {
            /*var q = Context.Configuration.HardwareSettings.SocketQuantity;
            //var q = Context.Configuration.HardwareSettings.SocketQuantity;
            lvDoMCCards.Items.Clear();
            var cardsWorking = InterfaceDataExchange.GetIsCardsWorking();
            for (int i = 0; i < CCDCardMAC.LastCardNumber(q); i++)
            {
                var f = CCDCardMAC.FirstSocket(i);
                var sq = CCDCardMAC.SocketQuantity(q, i);
                Color color = Color.White;
                if (SettingsPageSocketsStatusShow == SettingsPageSocketsStatus.SocketCardsIsWorking)
                {
                    if (cardsWorking[i])
                    {
                        color = Color.LimeGreen;
                    }
                    else
                    {
                        color = Color.FromArgb(255, 255, 128, 128);
                    }
                }
                var lvi = new ListViewItem(new string[] { (i + 1).ToString(), "🗸", UserInterfaceControls.MacToString(CCDCardMAC.ToMAC((byte)(i + 1))), f.ToString(), (f + sq - 1).ToString() });
                lvi.BackColor = color;
                lvDoMCCards.Items.Add(lvi);
            }*/

            SettingsSocketsPanelList = UserInterfaceControls.CreateSocketStatusPanels(Context.Configuration.HardwareSettings.SocketQuantity, ref pnlSockets);
            //UserInterfaceControls.SetSocketStatuses(SettingsSocketsPanelList, UserInterfaceControls.GetListOfSetSocketConfiguration(Context.Configuration.HardwareSettings.SocketQuantity, InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations), Color.Green, Color.DarkGray);
            /*switch (SettingsPageSocketsStatusShow)
            {
                case SettingsPageSocketsStatus.IsSocketSettingsOk:
                    UserInterfaceControls.SetSocketStatuses(SettingsSocketsPanelList, UserInterfaceControls.GetListOfSetSocketConfiguration(Configuration.SocketQuantity, Configuration.SocketToCardSocketConfigurations), Color.Green, Color.DarkGray);
                    break;
                case SettingsPageSocketsStatus.SocketCardsIsWorking:
                    UserInterfaceControls.SetSocketStatuses(SettingsSocketsPanelList, InterfaceDataExchange.GetIsCardWorking(), Color.Green, Color.OrangeRed);
                    break;
            }*/
            StandardSettingsSocketsPanelList = UserInterfaceControls.CreateSocketStatusPanels(Context.Configuration.HardwareSettings.SocketQuantity, ref pnlGetStandardSockets, GetStandard_Click);
            //UserInterfaceControls.SetSocketStatuses(StandardSettingsSocketsPanelList, InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations.Select(s => s.Value.StandardImage != null).ToArray(), Color.Green, Color.DarkGray);

            /*WorkModeStandardSettingsSocketsPanelList = UserInterfaceControls.CreateSocketStatusPanels(Configuration.SocketQuantity, ref pnlWorkSockets, GetWorkModeStandard_Click);
            UserInterfaceControls.SetSocketStatuses(WorkModeStandardSettingsSocketsPanelList, UserInterfaceControls.GetListOfSetSocketConfiguration(Configuration.SocketQuantity, Configuration.SocketToCardSocketConfigurations), Color.Green, Color.DarkGray);*/

            //SetTestCCDSocketStatuses();
            CreateTestCCDPanelSocketStatuses();
            SetTestCCDPanelSocketStatuses();

            SetCheckedMenuItems();
        }

        private void SetTestCCDSocketStatuses()
        {
            //TestCCDSocketStatuses = UserInterfaceControls.GetIntListOfSetSocketConfiguration(Context.Configuration.HardwareSettings.SocketQuantity, InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations);
        }

        private void CreateTestCCDPanelSocketStatuses()
        {
            TestSocketsSettingsSocketsPanelList = UserInterfaceControls.CreateSocketStatusPanels(Context.Configuration.HardwareSettings.SocketQuantity, ref pnlTestSockets, TestShowSocketImages_Click);
        }
        private void SetTestCCDPanelSocketStatuses()
        {
            UserInterfaceControls.SetSocketStatuses(TestSocketsSettingsSocketsPanelList, TestCCDSocketStatuses, Color.DarkGray, Color.Green, Color.Red);
        }

        private void SetFormSchema()
        {
            WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Maximize window");
            this.WindowState = FormWindowState.Maximized;

            Application.DoEvents();

            #region Test tab
            WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Screen.FromControl");
            var screen = Screen.FromControl(this);
            var swidth = screen.WorkingArea.Width;
            var sheight = screen.WorkingArea.Height - tabControl1.Top - SystemInformation.CaptionHeight - ssFooter.Height;
            WorkingLog.Add(LoggerLevel.FullDetailedInformation, $"WxH={swidth}x{sheight}");

            var pbtotalwidth = swidth - tabControl1.Left - btnCycleStop.Width;// - 300;
            WorkingLog.Add(LoggerLevel.FullDetailedInformation, $"pbtotalwidth ={pbtotalwidth}");
            var pbwidth = pbtotalwidth / 3.2 - 18;
            WorkingLog.Add(LoggerLevel.FullDetailedInformation, $"pbwidth={pbwidth}");
            var pbheight = sheight / 2 - 20;
            var socketButtonsHeight = 380;// sheight / 2 - 100;
            double kwidth = (double)pbwidth / 512;
            double kheight = (double)pbheight / 512;
            WorkingLog.Add(LoggerLevel.FullDetailedInformation, $"KW x KH= {kwidth} x {kheight}");
            if (kwidth > 1) kwidth = 1;
            if (kheight > 1) kheight = 1;
            var wd = (int)(screen.WorkingArea.Width - 520 * kwidth * 3 - 50);
            WorkingLog.Add(LoggerLevel.FullDetailedInformation, $"wd={wd}");
            pnlTestSockets.Size = new Size(wd, socketButtonsHeight);//new Size(wd, (int)(512*kheight));

            pbTestReadImage.Location = new Point(wd + 10, pbTestReadImage.Location.Y);
            pbTestReadImage.Size = new Size((int)(512 * kwidth), (int)(512 * kheight));

            pbTestDifference.Location = new Point(wd + (int)(512 * kwidth) + 10 * 2, pbTestDifference.Location.Y);
            pbTestDifference.Size = new Size((int)(512 * kwidth), (int)(512 * kheight));

            pbTestStandard.Location = new Point(wd + (int)(512 * 2 * kwidth) + 10 * 3, pbTestStandard.Location.Y);
            pbTestStandard.Size = new Size((int)(512 * kwidth), (int)(512 * kheight));


            //var lblTestRead = new Label();
            //this.Controls.Add(lblTestRead);
            lblTestRead.AutoSize = true;
            lblTestRead.Location = new Point(pbTestReadImage.Location.X, pbTestReadImage.Location.Y - 20);
            lblTestRead.Text = "Прочитанное изображение:";

            // var lblTestStandard = new Label();
            //this.Controls.Add(lblTestStandard);
            lblTestStandard.AutoSize = true;
            lblTestStandard.Location = new Point(pbTestStandard.Location.X, pbTestStandard.Location.Y - 20);
            lblTestStandard.Text = "Эталон:";

            //var lblTestDifference = new Label();
            //this.Controls.Add(lblTestDifference);
            lblTestDifference.AutoSize = true;
            lblTestDifference.Location = new Point(pbTestDifference.Location.X, pbTestDifference.Location.Y - 20);
            lblTestDifference.Text = "Разница:";

            Application.DoEvents();
            //chTestReadLine.Location = new Point(pnlTestSockets.Location.X, pnlTestSockets.Location.Y + pnlTestSockets.Size.Height + 10);
            //chTestReadLine.Size = new Size(swidth - chTestReadLine.Location.X - 10, sheight - chTestReadLine.Location.Y - 20);


            var topchart = pbTestReadImage.Location.Y + pbTestReadImage.Size.Height;
            var graphHeight = sheight - topchart - 20;
            if (graphHeight < 1) graphHeight = 1;
            WorkingLog.Add(LoggerLevel.FullDetailedInformation, $"topchart={topchart}");
            WorkingLog.Add(LoggerLevel.FullDetailedInformation, $"graphHeight ={graphHeight}");

            chTestReadLine.Location = new Point(pbTestReadImage.Location.X, topchart + 10);
            chTestReadLine.Size = new Size((int)(512 * kwidth), graphHeight);

            chTestDiff.Location = new Point(pbTestDifference.Location.X, topchart + 10);
            chTestDiff.Size = new Size((int)(512 * kwidth), graphHeight);

            chTestStandard.Location = new Point(pbTestStandard.Location.X, topchart + 10);
            chTestStandard.Size = new Size((int)(512 * kwidth), graphHeight);

            /*
            chTestReadLine.Location = new Point(pbTestReadImage.Location.X, pnlTestSockets.Location.Y + pnlTestSockets.Size.Height + 10);
            chTestReadLine.Size = new Size(512, sheight - chTestReadLine.Location.Y - 20);

            chTestDiff.Location = new Point(pbTestDifference.Location.X, pnlTestSockets.Location.Y + pnlTestSockets.Size.Height + 10);
            chTestDiff.Size = new Size(512, sheight - chTestReadLine.Location.Y - 20);

            chTestStandard.Location = new Point(pbTestStandard.Location.X, pnlTestSockets.Location.Y + pnlTestSockets.Size.Height + 10);
            chTestStandard.Size = new Size(512, sheight - chTestReadLine.Location.Y - 20);*/

            btnCycleStart.Location = new Point(pnlTestSockets.Location.X, pnlTestSockets.Location.Y + pnlTestSockets.Size.Height + 10);
            //btnCycleStop.Location = new Point(pnlTestSockets.Location.X + pnlTestSockets.Width - btnCycleStop.Width, pnlTestSockets.Location.Y + pnlTestSockets.Size.Height + 10);
            btnCycleStop.Location = new Point(pnlTestSockets.Location.X, btnCycleStart.Location.Y + btnCycleStart.Size.Height + 10);
            #endregion
        }

        private void SetConfigurationIsNotLoaded()
        {
            /*InterfaceDataExchange.CCDDataEchangeStatuses.IsConfigurationLoaded = false;
            //InterfaceDataExchange.Configuration = InterfaceDataExchange.Configuration;
            if ((InterfaceDataExchange?.CardsConnection ?? null) != null)
            {
                InterfaceDataExchange.CardsConnection.PacketLogActive = InterfaceDataExchange.Configuration.LogPackets;
            }*/
        }

        private bool WaitForCommand<T1, T2>(object? InputData, int TimeoutInSeconds, string LogMessage, LoggerLevel LogMessageLevel, out T2? outputData)
            where T1 : AbstractCommandBase
            where T2 : class
        {
            WorkingLog.Add(LogMessageLevel, LogMessage);

            var CommandInstance = Controller.CreateCommand(typeof(T1));
            if (CommandInstance == null)
            {
                outputData = null;
                return false;
            };
            var resultExpositionCommand = CommandInstance.Wait(InputData, TimeoutInSeconds);
            outputData = resultExpositionCommand as T2;
            return true;
        }
        private bool WaitForCommand<T1, T2, T3>(object? InputData, int TimeoutInSeconds, string LogMessage, LoggerLevel LogMessageLevel, out T3? outputData)
            where T1 : AbstractCommandBase
            where T2 : AbstractModuleBase
            where T3 : class
        {
            WorkingLog.Add(LogMessageLevel, LogMessage);

            var CommandInstance = Controller.CreateCommand(typeof(T1), typeof(T2));
            if (CommandInstance == null)
            {
                outputData = null;
                return false;
            };
            var resultExpositionCommand = CommandInstance.Wait(InputData, TimeoutInSeconds);
            outputData = resultExpositionCommand as T3;
            return true;
        }

        private bool LoadConfiguration(bool WorkingMode, bool LoadCCD = true, bool LoadLCB = true, int[] Sockets = null, bool ShowMessages = true)
        {

            if (LoadCCD)
            {
                if (!WaitForCommand<CCDCardDataModule.SetExpositionCommand, SetReadingParametersCommandResult>(Context, Context.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeout, "Передача настроек экспозиции гнезд в модуль плат ПЗС", LoggerLevel.FullDetailedInformation, out SetReadingParametersCommandResult? result))
                {
                    if (result != null)
                    {
                        WorkingLog.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", result.CardsNotAnswered())}) не отвечают");
                    }
                }
                if (!WaitForCommand<CCDCardDataModule.SetReadingParametersCommand, SetReadingParametersCommandResult>(Context, Context.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeout, "Передача настроек чтения гнезд в модуль плат ПЗС", LoggerLevel.FullDetailedInformation, out SetReadingParametersCommandResult? result))
                {
                    if (result != null)
                    {
                        WorkingLog.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", result.CardsNotAnswered())}) не отвечают");
                    }
                }
            }

            if (LoadLCB)
            {
                if (!WaitForCommand<CCDCardDataModule.SetExpositionCommand, SetReadingParametersCommandResult>(Context, Context.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeout, "Передача настроек экспозиции гнезд в модуль плат ПЗС", LoggerLevel.FullDetailedInformation, out SetReadingParametersCommandResult? result))
                {
                    if (result != null)
                    {
                        WorkingLog.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", result.CardsNotAnswered())}) не отвечают");
                    }
                }
                //InterfaceDataExchange.CardsConnection.PacketLogActive = false;
                WorkingLog.Add("Подключение к БУС");
                InterfaceDataExchange.SendCommand(ModuleCommand.InitLCB);
                res = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () => InterfaceDataExchange.InitLCBStatus == 2);
                if (!res)
                {
                    InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                    if (ShowMessages) DoMCNotAbleLoadConfigurationErrorMessage();
                    return false;
                }
                InterfaceDataExchange.LEDStatus.LСBInitialized = true;

                if (WorkingMode)
                {
                    WorkingLog.Add("Вход в рабочий режим");
                    if (!InterfaceDataExchange.IsLEDConfiguartionFull) return false;
                    // Загрузить в БУС параметры работы и перевести в рабочий режим

                    WorkingLog.Add("Установка рабочего режима БУС");
                    InterfaceDataExchange.SendCommand(ModuleCommand.SetLCBCurrentRequest);
                    res = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () => InterfaceDataExchange.LEDStatus.NumberOfLastCommandSent == 1 && InterfaceDataExchange.LEDStatus.LastCommandReceivedStatusIsOK);
                    if (!res)
                    {
                        InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                        if (ShowMessages) DoMCNotAbleLoadConfigurationErrorMessage();
                        return false;
                    }


                    WorkingLog.Add("Установка параметров движения в БУС");
                    InterfaceDataExchange.SendCommand(ModuleCommand.SetLCBMovementParametersRequest);
                    res = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () => InterfaceDataExchange.InitLCBStatus == 2, () => InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
                    if (!res)
                    {
                        InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                        if (ShowMessages) DoMCNotAbleLoadConfigurationErrorMessage();
                        return false;
                    }


                    InterfaceDataExchange.LEDStatus.LCBConfigurationLoaded = true;
                }
            }
            return res;
        }

        /*private void StopCCDWork()
        {
            InterfaceDataExchange.SendCommand(ModuleCommand.CCDStop);
        }*/


        private void SetCheckedMenuItems()
        {
            /*if (InterfaceDataExchange.Configuration != null)
            {
                if (InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations != null)
                {
                    if (InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations.Keys.Count == Context.Configuration.HardwareSettings.SocketQuantity)
                        miReadParameters.Checked = true;
                    else
                        miReadParameters.Checked = false;
                }

                if (InterfaceDataExchange.Configuration.WorkModeSettings != null)
                {
                    var k = InterfaceDataExchange.Configuration.WorkModeSettings.Koefficient;
                    if (k == 0 || double.IsNaN(k) || double.IsInfinity(k))
                    {
                        miStandardSavingModeSetting.Checked = false;
                    }
                    else
                    {
                        miStandardSavingModeSetting.Checked = true;
                    }
                }
                else
                {
                    miStandardSavingModeSetting.Checked = false;

                }
                if (InterfaceDataExchange.Configuration.DisplaySockets2PhysicalSockets != null)
                {
                    miPhysicToDisplaySocket.Checked = true;
                }
                else
                {
                    miPhysicToDisplaySocket.Checked = false;
                }
            }*/
        }

        #endregion Settings




        #region tblTest

        short[,] testImage;
        int TestSocketNumberSelected;

        short[,] TestDiffImage;
        short[,] TestReadImage;
        short[,] TestStandardImage;
        Bitmap TestBmpReadImage;
        Bitmap TestBmpDiffImage;
        Bitmap TestBmpStandardImage;

        bool TestCCDIsReading;

        private void AbleForRead(bool IsReading)
        {
            btnTest_ReadAllSocket.Enabled = !IsReading;
            btnTest_ReadSelectedSocket.Enabled = !IsReading;
            btnCycleStart.Enabled = !IsReading;

        }

        private void btnTest_ReadAllSockets_Click(object sender, EventArgs e)
        {
            if (TestCCDIsReading) return;
            TestCCDIsReading = true;
            AbleForRead(true);
            long Start = 0;
            try
            {
                TestSocketNumberSelected = 0;
                lblTestSelectedSocket.Text = "";
                InterfaceDataExchange.CardsConnection.PacketLogActive = InterfaceDataExchange.Configuration.LogPackets;
                InterfaceDataExchange.CCDDataEchangeStatuses.FastRead = true;
                InterfaceDataExchange.CCDDataEchangeStatuses.ExternalStart = cbTest_ExternalStart.Checked;
                InterfaceDataExchange.CardsConnection.StartReadBytes();
                Start = InterfaceDataExchange.PreciseTimer.ElapsedTicks;
                TestReadAllSockets();
                InterfaceDataExchange.SendCommand(ModuleCommand.CCDStop);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source);
            }
            finally
            {
                TestCCDIsReading = false;
            }
            //FillSettingPage();
            SetTestCCDPanelSocketStatuses();
            AbleForRead(false);
            /*DisplayMessage.Show(
                $"Чтений: {InterfaceDataExchange.CardsConnection.ReadBytesCounter()}\r\n" +
                $"Время чтения: {(InterfaceDataExchange.PreciseTimer.ElapsedTicks - Start) / 10}мкс\r\n" +
                $"Время среднего чтения: {InterfaceDataExchange.CardsConnection.ReadBytesCounter() * 8 / ((InterfaceDataExchange.PreciseTimer.ElapsedTicks - Start) / 10)} Мбит/сек\r\n",
                "Завершено");*/
        }


        private bool TestReadAllSockets()
        {
            WorkingLog.Add("--------------- Получение изображений по всем гнездам ---------------");
            InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Start;
            CheckForDoMCModule();
            WorkingLog.Add("Начало загрузки конфигурации");
            if (!LoadConfiguration(false, true, false))
            {
                //DoMCNotAbleLoadConfigurationErrorMessage();
                return false;
            }
            //IsAbleToWork = true;
            //InterfaceDataExchange.SendCommand(ModuleCommand.CCDCardsReset);
            InterfaceDataExchange.SendResetToCCDCards();
            Thread.Sleep(50);
            InterfaceDataExchange.CurrentCycleCCD = new CycleImagesCCD();
            InterfaceDataExchange.CurrentCycleCCD.Differences = new short[Context.Configuration.HardwareSettings.SocketQuantity][,];
            InterfaceDataExchange.CurrentCycleCCD.WorkModeImages = new short[Context.Configuration.HardwareSettings.SocketQuantity][,];
            InterfaceDataExchange.CurrentCycleCCD.StandardImage = new short[Context.Configuration.HardwareSettings.SocketQuantity][,];
            InterfaceDataExchange.CurrentCycleCCD.IsSocketGood = new bool[Context.Configuration.HardwareSettings.SocketQuantity];
            if (InterfaceDataExchange.CCDDataEchangeStatuses.SocketsToSave != null)
                InterfaceDataExchange.CurrentCycleCCD.SocketsToSave =
                    new bool[Context.Configuration.HardwareSettings.SocketQuantity].Select((s, i) => InterfaceDataExchange.CCDDataEchangeStatuses.SocketsToSave.Contains(i + 1)).ToArray();
            else
                InterfaceDataExchange.CurrentCycleCCD.SocketsToSave = new bool[Context.Configuration.HardwareSettings.SocketQuantity];

            InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Processing;

            //if (!IsAbleToWork) return false;
            WorkingLog.Add("Начало чтения гнезда");
            var startReading = DateTime.Now;
            bool rc = true;
            if (InterfaceDataExchange.CCDDataEchangeStatuses.ExternalStart)
            {
                WorkingLog.Add("Запуск чтения по внешнему сигналу");
                InterfaceDataExchange.SendCommand(ModuleCommand.StartReadExternal);
                rc = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () =>
                {
                    return InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartReadExternal && InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Complete;
                }, null);
                if (!rc)
                {
                    InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                    InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Error;
                    WorkingLog.Add("Платы не смогли вовремя получить данные");
                    WorkingLog.Add($"Статус модуля: {InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus}");
                    WorkingLog.Add($"Шаг модуля: {InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep}");
                    InterfaceDataExchange.CardsConnection.WriteSocketStatusLog(WorkingLog, "Статусы гнезд после ошибки");
                    return false;
                }
            }
            else
            {
                WorkingLog.Add("Запуск чтения");
                Thread.Sleep(10);
                InterfaceDataExchange.SendCommand(ModuleCommand.StartRead);
                rc = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () =>
                {
                    return InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartRead && InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Complete;
                }, () => InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
                if (!rc)
                {
                    //DoMCNotAbleToReadSocketErrorMessage();
                    //IsWorkingModeStarted = false;
                    InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                    InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Error;
                    WorkingLog.Add("Платы не смогли вовремя получить данные");
                    WorkingLog.Add($"Статус модуля: {InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus}");
                    WorkingLog.Add($"Шаг модуля: {InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep}");
                    InterfaceDataExchange.CardsConnection.WriteSocketStatusLog(WorkingLog, "Статусы гнезд после ошибки");
                    return false;
                }
            }
            var CCDEndTime = DateTime.Now;
            if ((CCDEndTime - startReading).TotalSeconds < 1)
            {
                WorkingLog.Add("Программная ошибка при проверке на готовность данных");
                WorkingLog.Add($"Время начала чтения: {startReading:dd-MM-yyyy HH\\:mm\\:ss.fff}");
                WorkingLog.Add($"Время завершения чтения: {CCDEndTime:dd-MM-yyyy HH\\:mm\\:ss.fff}");
                WorkingLog.Add($"Статус модуля: {InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus}");
                WorkingLog.Add($"Шаг модуля: {InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep}");
                InterfaceDataExchange.CardsConnection.WriteSocketStatusLog(WorkingLog, "Статусы гнезд после ошибки");
            }
            WorkingLog.Add("Получение изображения");
            InterfaceDataExchange.CurrentCycleCCD.CycleCCDDateTime = DateTime.Now;
            InterfaceDataExchange.SendCommand(ModuleCommand.GetSocketImages);
            var ri = UserInterfaceControls.Wait(2 * InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () =>
            {
                var mst = InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus;
                var stp = InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep;
                return mst == ModuleCommand.GetSocketImages && (stp == ModuleCommandStep.Complete || stp == ModuleCommandStep.Error);
            }, () => InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
            if (!ri || InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Error)
            {
                //DoMCNotAbleToReadSocketErrorMessage();
                //IsWorkingModeStarted = false;
                InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Error;
                return false;
            }
            WorkingLog.Add("Изображение получено");
            var images = InterfaceDataExchange.CCDDataEchangeStatuses.Images;
            if (images is null)
            {
                //DoMCNotAbleToReadSocketErrorMessage();
                //IsWorkingModeStarted = false;
                InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Error;
                return false;
            }
            InterfaceDataExchange.CurrentCycleCCD.WorkModeImages = images;


            //var beforeDataReady = InterfaceDataExchange.CCDDataEchangeStatuses.ReadyToSendTime; // время, когда можно начинать получать данные
            var beforeSync = InterfaceDataExchange.LEDStatus.TimeSyncSignalGot; // Время получения сихроимпульса

            InterfaceDataExchange.CCDDataEchangeStatuses.StartProcessImages = InterfaceDataExchange.PreciseTimer.ElapsedTicks;
            InterfaceDataExchange.CCDDataEchangeStatuses.StartDecisionImages = InterfaceDataExchange.PreciseTimer.ElapsedTicks;

            /*if (System.Diagnostics.Debugger.IsAttached)
            {
                for (int i = 0; i < Context.Configuration.HardwareSettings.SocketQuantity; i++)
                {
                    InterfaceDataExchange.CheckIfSocketGood(i + 1);
                }
            }
            else
            {*/
            WorkingLog.Add("Проверка изображения");
            InterfaceDataExchange.CheckIfAllSocketsGood();
            //}

            for (int i = 0; i < Context.Configuration.HardwareSettings.SocketQuantity; i++)
            {
                TestCCDSocketStatuses[i] = InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations[i + 1] != null ? (InterfaceDataExchange.CurrentCycleCCD.IsSocketGood[i] ? 1 : 2) : 0;
            }

            InterfaceDataExchange.CCDDataEchangeStatuses.StopProcessImages = InterfaceDataExchange.PreciseTimer.ElapsedTicks;
            InterfaceDataExchange.CCDDataEchangeStatuses.StopDecisionImages = InterfaceDataExchange.PreciseTimer.ElapsedTicks;

            /*if (InterfaceDataExchange.CyclesCCD.Count > 2)
            {
                InterfaceDataExchange.Errors.NotEnoughTimeToProcessSQL = true;
            }
            else
            {
                InterfaceDataExchange.Errors.NotEnoughTimeToProcessSQL = false;

            }
            //InterfaceDataExchange.CyclesCCD.Enqueue(InterfaceDataExchange.CurrentCycleCCD);

            //var afterDataReady = InterfaceDataExchange.CCDDataEchangeStatuses.ReadyToSendTime; // время, когда можно было получать 
            var afterSync = InterfaceDataExchange.LEDStatus.TimeSyncSignalGot; // Время получения сихроимпульса
            // Если был синхроимпульс пока мы обрабатывали, значит мы не успеваем
            if (afterSync != beforeSync)
            {
                InterfaceDataExchange.Errors.NotEnoughTimeToProcessData = true;
            }
            else
            {
                InterfaceDataExchange.Errors.NotEnoughTimeToProcessData = false;

            }*/
            //Application.DoEvents();
            WorkingLog.Add("Остановка связи с плат ПЗС");
            StopCCDWork();
            InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Complete;
            return true;
        }

        private void btnReadSelectedSocket_Click(object sender, EventArgs e)
        {
            if (TestCCDIsReading) return;
            if (TestSocketNumberSelected < 1 || TestSocketNumberSelected > Context.Configuration.HardwareSettings.SocketQuantity) return;
            AbleForRead(true);
            TestCCDIsReading = true;
            try
            {
                InterfaceDataExchange.CardsConnection.PacketLogActive = InterfaceDataExchange.Configuration.LogPackets;
                InterfaceDataExchange.CCDDataEchangeStatuses.ExternalStart = cbTest_ExternalStart.Checked;
                InterfaceDataExchange.CCDDataEchangeStatuses.FastRead = true;
                TestReadOneSocket(TestSocketNumberSelected);
                InterfaceDataExchange.SendCommand(ModuleCommand.CCDStop);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source);
            }
            finally
            {
                TestCCDIsReading = false;
            }
            ShowSocket(TestSocketNumberSelected);
            /*            FillSettingPage();

                        TestCCDSocketStatuses[TestSocketNumberSelected - 1] = 2;

                        SetTestCCDPanelSocketStatuses();*/
            SetTestCCDPanelSocketStatuses();
            AbleForRead(false);

        }

        private bool TestReadOneSocket(int SelectedSocket)
        {
            if (SelectedSocket == 0) return false;
            InterfaceDataExchange.CCDDataEchangeStatuses.SocketsToSave = new int[1] { SelectedSocket };
            InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Start;
            CheckForDoMCModule();
            if (!LoadConfiguration(false, true, false)) //------ загрузить конфигурацию для одного гнезда
            {
                //DoMCNotAbleLoadConfigurationErrorMessage();
                return false;
            }
            //IsAbleToWork = true;
            InterfaceDataExchange.SendResetToCCDCards();
            Thread.Sleep(50);
            InterfaceDataExchange.CurrentCycleCCD = new CycleImagesCCD();
            InterfaceDataExchange.CurrentCycleCCD.Differences = new short[Context.Configuration.HardwareSettings.SocketQuantity][,];
            InterfaceDataExchange.CurrentCycleCCD.WorkModeImages = new short[Context.Configuration.HardwareSettings.SocketQuantity][,];
            InterfaceDataExchange.CurrentCycleCCD.StandardImage = new short[Context.Configuration.HardwareSettings.SocketQuantity][,];
            InterfaceDataExchange.CurrentCycleCCD.IsSocketGood = new bool[Context.Configuration.HardwareSettings.SocketQuantity];

            //UserInterfaceControls.SetSocketStatuses(WorkModeStandardSettingsSocketsPanelList, new bool[WorkModeStandardSettingsSocketsPanelList.Length], Color.Green, Color.DarkGray);
            //if (!IsAbleToWork) return false;

            InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Processing;

            //InterfaceDataExchange.SocketsToSave = new int[] { 1 };
            bool rc = true;
            if (InterfaceDataExchange.CCDDataEchangeStatuses.ExternalStart)
            {
                InterfaceDataExchange.SendCommand(ModuleCommand.StartSeveralSocketReadExternal);
                rc = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () =>
                {
                    return InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartSeveralSocketReadExternal && InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Complete;
                }, () => InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle || InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Error);
                if (!rc)
                {
                    //DoMCNotAbleToReadSocketErrorMessage();
                    //IsWorkingModeStarted = false;
                    InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                    InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Error;
                    return false;
                }
            }
            else
            {
                InterfaceDataExchange.SendCommand(ModuleCommand.CCDCardsReset);
                Thread.Sleep(10);
                InterfaceDataExchange.SendCommand(ModuleCommand.StartSeveralSocketRead);
                rc = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () =>
                {
                    return InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartSeveralSocketRead && InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Complete;
                }, () => InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle || InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Error);
                if (!rc)
                {
                    //DoMCNotAbleToReadSocketErrorMessage();
                    //IsWorkingModeStarted = false;
                    InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                    InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Error;
                    return false;
                }
            }
            InterfaceDataExchange.CurrentCycleCCD.CycleCCDDateTime = DateTime.Now;
            InterfaceDataExchange.SendCommand(ModuleCommand.GetSeveralSocketImages);
            var ri = UserInterfaceControls.Wait(2 * InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () =>
              {
                  var mst = InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus;
                  var stp = InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep;
                  return mst == ModuleCommand.GetSeveralSocketImages && (stp == ModuleCommandStep.Complete || stp == ModuleCommandStep.Error);
              }, () => InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
            if (!ri || InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Error)
            {
                //DoMCNotAbleToReadSocketErrorMessage();
                //IsWorkingModeStarted = false;
                InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Error;
                return false;
            }
            var images = InterfaceDataExchange.CCDDataEchangeStatuses.Images;
            if (images is null)
            {
                //DoMCNotAbleToReadSocketErrorMessage();
                //IsWorkingModeStarted = false;
                InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Error;
                return false;
            }
            InterfaceDataExchange.CurrentCycleCCD.WorkModeImages = images;

            InterfaceDataExchange.CCDDataEchangeStatuses.StartProcessImages = InterfaceDataExchange.PreciseTimer.ElapsedTicks;


            /*Parallel.For(0, images.Length, new ParallelOptions() { MaxDegreeOfParallelism = InterfaceDataExchange.MaxDegreeOfParallelism }, i =>
            {
                try
                {
                    var socketnumber = i + 1;
                    if (Configuration.SocketToCardSocketConfigurations.ContainsKey(socketnumber))
                    {
                        var socket = Configuration.SocketToCardSocketConfigurations[socketnumber];
                        if (socket == null) return;
                        InterfaceDataExchange.CurrentCycleCCD.StandardImage[i] = Configuration.SocketToCardSocketConfigurations[socketnumber].StandardImage;
                        var diffImg = ImageTools.GetDifference(InterfaceDataExchange.CurrentCycleCCD.WorkModeImages[i], Configuration.SocketToCardSocketConfigurations[socketnumber].StandardImage);

                        var deviationImg = ImageTools.DeviationByLine(diffImg, InterfaceDataExchange.Configuration.ImageProcessParameters.DeviationWindow);
                        var dev = ImageTools.MaxDeviation(deviationImg, out Point pMaxDev, InterfaceDataExchange.Configuration.ImageProcessParameters.GetRectangle());
                        InterfaceDataExchange.CurrentCycleCCD.Differences[i] = deviationImg;
                        InterfaceDataExchange.CurrentCycleCCD.MaxDeviationPoint = pMaxDev;
                        var socketresult = (dev < InterfaceDataExchange.Configuration.ImageProcessParameters.MaxDeviation);
                        InterfaceDataExchange.CurrentCycleCCD.IsSocketGood[i] = socketresult;
                        TestCCDSocketStatuses[i] = socketresult ? 1 : 2;
                        //var dev = ImageTools.CalculateDeviationFull(new short[][,] { InterfaceDataExchange.CurrentCycle.Differences[i] }, socket.StartLine, socket.EndLine);

                    }
                }
                catch (Exception ex)
                {

                }
            });*/

            InterfaceDataExchange.CCDDataEchangeStatuses.StartDecisionImages = InterfaceDataExchange.PreciseTimer.ElapsedTicks;
            if (System.Diagnostics.Debugger.IsAttached)
            {
                for (int i = 0; i < Context.Configuration.HardwareSettings.SocketQuantity; i++)
                {
                    InterfaceDataExchange.CheckIfSocketGood(i + 1);
                }
            }
            else
            {
                InterfaceDataExchange.CheckIfAllSocketsGood();
            }
            InterfaceDataExchange.CCDDataEchangeStatuses.StopDecisionImages = InterfaceDataExchange.PreciseTimer.ElapsedTicks;

            for (int i = 0; i < Context.Configuration.HardwareSettings.SocketQuantity; i++)
            {
                TestCCDSocketStatuses[i] = InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations[i + 1] != null ? (InterfaceDataExchange.CurrentCycleCCD.IsSocketGood[i] ? 1 : 2) : 0;
            }

            InterfaceDataExchange.CCDDataEchangeStatuses.StopProcessImages = InterfaceDataExchange.PreciseTimer.ElapsedTicks;


            //SetTestCCDPanelSocketStatuses();
            //Application.DoEvents();
            StopCCDWork();
            InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Complete;
            return true;
        }
        private void TestTabDrawGraphLine(int linen)
        {
            short[] ReadLine;
            short[] StandardLine;
            short[] DiffLine;
            if (cbFullMax.Checked)
            {
                ReadLine = LineFrom2D(TestReadImage, linen);
                StandardLine = LineFrom2D(TestStandardImage, linen);
                DiffLine = LineFrom2D(TestDiffImage, linen);
                for (int y = 0; y < 512; y++) for (int x = 0; x < 512; x++) if (ReadLine[x] < TestReadImage[y, x]) ReadLine[x] = TestReadImage[y, x]; ;
                for (int y = 0; y < 512; y++) for (int x = 0; x < 512; x++) if (StandardLine[x] < TestStandardImage[y, x]) StandardLine[x] = TestStandardImage[y, x]; ;
                for (int y = 0; y < 512; y++) for (int x = 0; x < 512; x++) if (DiffLine[x] < TestDiffImage[y, x]) DiffLine[x] = TestDiffImage[y, x]; ;
            }
            else
            {
                ReadLine = LineFrom2D(TestReadImage, linen);
                StandardLine = LineFrom2D(TestStandardImage, linen);
                DiffLine = LineFrom2D(TestDiffImage, linen);
            }

            {
                #region Graph Read
                if (ReadLine != null)
                {
                    chTestReadLine.ChartAreas.Clear();
                    var ca = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
                    ca.AxisX.Minimum = 0;
                    ca.AxisX.Interval = 32;
                    ca.AxisX.Maximum = 512;
                    chTestReadLine.ChartAreas.Add(ca);
                    chTestReadLine.Series.Clear();


                    var ns = new System.Windows.Forms.DataVisualization.Charting.Series();
                    ns.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    ns.Points.Clear();
                    for (int i = 0; i < 512; i++)
                        ns.Points.AddXY(i, ReadLine[i]);
                    chTestReadLine.Series.Add(ns);
                }
                #endregion
                /*
                #region Standard
                {
                    var ns = new System.Windows.Forms.DataVisualization.Charting.Series();
                    ns.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    ns.Points.Clear();
                    for (int i = 0; i < 512; i++)
                        ns.Points.AddXY(i, StandardLine[i]);
                    chTestReadLine.Series.Add(ns);
                }
                #endregion
                
                #region Difference
                {
                    var ns = new System.Windows.Forms.DataVisualization.Charting.Series();
                    ns.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    ns.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
                    ns.Points.Clear();
                    for (int i = 0; i < 512; i++)
                        ns.Points.AddXY(i, DiffLine[i]);
                    chTestReadLine.Series.Add(ns);
                }
                #endregion

                */

                #region Standard
                if (StandardLine != null)
                {
                    chTestStandard.ChartAreas.Clear();
                    var ca = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
                    ca.AxisX.Minimum = 0;
                    ca.AxisX.Interval = 32;
                    ca.AxisX.Maximum = 512;
                    chTestStandard.ChartAreas.Add(ca);
                    chTestStandard.Series.Clear();


                    var ns = new System.Windows.Forms.DataVisualization.Charting.Series();
                    ns.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    ns.Points.Clear();
                    for (int i = 0; i < 512; i++)
                        ns.Points.AddXY(i, StandardLine[i]);
                    chTestStandard.Series.Add(ns);
                }
                #endregion

                #region Difference
                if (DiffLine != null)
                {

                    chTestDiff.ChartAreas.Clear();
                    var ca = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
                    ca.AxisX.Minimum = 0;
                    ca.AxisX.Interval = 32;
                    ca.AxisX.Maximum = 512;
                    chTestDiff.ChartAreas.Add(ca);
                    chTestDiff.Series.Clear();


                    var ns = new System.Windows.Forms.DataVisualization.Charting.Series();
                    ns.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    ns.Points.Clear();
                    for (int i = 0; i < 512; i++)
                        ns.Points.AddXY(i, DiffLine[i]);
                    chTestDiff.Series.Add(ns);
                }
                #endregion
            }

        }

        private short[] LineFrom2D(short[,] Image, int frame)
        {
            if (Image == null) return null;
            short[] line = new short[512];
            if (cbVertical.Checked)
            {
                for (int x = 0; x < 512; x++) line[x] = Image[x, frame];
            }
            else
            {
                for (int x = 0; x < 512; x++) line[x] = Image[frame, x];
            }
            return line;
        }
        private void TestShowSocketImages_Click(object sender, EventArgs e)
        {
            if (TestCCDIsReading) return;
            CheckForDoMCModule();
            var ctrl = (Control)sender;
            var socketnumber = (int)ctrl.Tag;
            ShowSocket(socketnumber);
            /*TestSocketNumberSelected = socketnumber;
            lblTestSelectedSocket.Text = TestSocketNumberSelected.ToString();

            if (InterfaceDataExchange.CurrentCycleCCD == null || InterfaceDataExchange.CurrentCycleCCD.WorkModeImages.Length < TestSocketNumberSelected || InterfaceDataExchange.CurrentCycleCCD.WorkModeImages[TestSocketNumberSelected - 1] == null)
            {
                //MessageBox.Show("Изображения для гнезда еще не получены");
                TestBmpReadImage = null;
                TestBmpDiffImage = null;
                TestBmpStandardImage = null;
                TestRedrawBitmap();
                TestTabDrawGraphLine(0);
                return;
            }

            InterfaceDataExchange.CurrentCycleCCD.Differences[socketnumber - 1] = ImageTools.GetDifference(InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations[socketnumber].StandardImage, InterfaceDataExchange.CurrentCycleCCD.WorkModeImages[socketnumber - 1]);

            TestDiffImage = InterfaceDataExchange.CurrentCycleCCD.Differences[socketnumber - 1];
            TestReadImage = InterfaceDataExchange.CurrentCycleCCD.WorkModeImages[socketnumber - 1];
            TestStandardImage = InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations[socketnumber].StandardImage;

            TestBmpReadImage = ImageTools.DrawImage(TestReadImage, true);
            TestBmpDiffImage = ImageTools.DrawImage(TestDiffImage, true);
            TestBmpStandardImage = ImageTools.DrawImage(TestStandardImage, true);
            //pbTestReadImage.Image = TestBmpReadImage;
            //pbTestDifference.Image = TestBmpDiffImage;
            //pbTestStandard.Image = TestBmpStandardImage;


            var timedur = InterfaceDataExchange.CCDDataEchangeStatuses.SocketReadImageStop[socketnumber - 1] - InterfaceDataExchange.CCDDataEchangeStatuses.SocketReadImageStart[socketnumber - 1];
            var stimedur = $"{timedur * 1e-4} ms";
            lblTimeDuration.Text = stimedur;

            numFrame.Value = 0;
            TestRedrawBitmap();
            TestTabDrawGraphLine(0);*/
        }

        private void ShowSocket(int socketnumber)
        {
            TestSocketNumberSelected = socketnumber;
            lblTestSelectedSocket.Text = TestSocketNumberSelected.ToString();

            if (InterfaceDataExchange.CurrentCycleCCD == null || InterfaceDataExchange.CurrentCycleCCD.WorkModeImages.Length < TestSocketNumberSelected || InterfaceDataExchange.CurrentCycleCCD.WorkModeImages[TestSocketNumberSelected - 1] == null)
            {
                //MessageBox.Show("Изображения для гнезда еще не получены");
                TestBmpReadImage = null;
                TestBmpDiffImage = null;
                TestBmpStandardImage = null;
                TestRedrawBitmap();
                TestTabDrawGraphLine((int)numFrame.Value);
                return;
            }
            /*var diff = ImageTools.GetDifference(InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations[socketnumber].StandardImage, InterfaceDataExchange.CurrentCycleCCD.WorkModeImages[socketnumber - 1]);*/
            InterfaceDataExchange.CurrentCycleCCD.StandardImage[socketnumber - 1] = InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations[socketnumber].StandardImage;
            InterfaceDataExchange.CheckIfSocketGood(TestSocketNumberSelected);

            TestDiffImage = InterfaceDataExchange.CurrentCycleCCD.Differences[socketnumber - 1];
            TestReadImage = InterfaceDataExchange.CurrentCycleCCD.WorkModeImages[socketnumber - 1];
            TestStandardImage = InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations[socketnumber].StandardImage;

            TestBmpReadImage = ImageTools.DrawImage(TestReadImage, invertColors);
            if (!InterfaceDataExchange.CurrentCycleCCD.IsSocketGood[TestSocketNumberSelected - 1] || showMaxDevPoint)
                TestBmpDiffImage = ImageTools.DrawImage(TestDiffImage, invertColors, InterfaceDataExchange.CurrentCycleCCD.MaxDeviationPoint);
            else
                TestBmpDiffImage = ImageTools.DrawImage(TestDiffImage, invertColors);
            TestBmpStandardImage = ImageTools.DrawImage(TestStandardImage, invertColors);
            /*pbTestReadImage.Image = TestBmpReadImage;
            pbTestDifference.Image = TestBmpDiffImage;
            pbTestStandard.Image = TestBmpStandardImage;*/



            var sp = InterfaceDataExchange.CCDDataEchangeStatuses.StartProcessImages;
            var ep = InterfaceDataExchange.CCDDataEchangeStatuses.StopProcessImages;

            lblTimeImageProcess.Text = $"Обработка изображений и принятие решения:{(ep - sp) * 1e-4:F3} мс";

            //numFrame.Value = 0;
            TestRedrawBitmap();
            TestTabDrawGraphLine((int)numFrame.Value);

            if (checkpreformalgorithmsForm != null)
            {
                checkpreformalgorithmsForm.SetStandardImage(TestStandardImage);
                checkpreformalgorithmsForm.SetImage(TestReadImage);
                checkpreformalgorithmsForm.SetImageProcessParameters(InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations[socketnumber].ImageProcessParameters);
                checkpreformalgorithmsForm.RecalcAndRedrawImages();

            }
        }

        private void TestRedrawBitmap()
        {
            pbTestReadImage.Invalidate();
            pbTestDifference.Invalidate();
            pbTestStandard.Invalidate();
        }

        private void numFrame_ValueChanged(object sender, EventArgs e)
        {
            TestTabDrawGraphLine((int)numFrame.Value);
            TestRedrawBitmap();
        }


        private void pbTestReadImage_Paint(object sender, PaintEventArgs e)
        {

            if (TestBmpReadImage == null) return;
            e.Graphics.DrawImage(TestBmpReadImage, 0, 0, pbTestReadImage.Width, pbTestReadImage.Height);
            var lineN = (int)numFrame.Value;
            if (cbVertical.Checked)
            {
                var k = (double)pbTestReadImage.Width / 512;
                e.Graphics.DrawLine(new Pen(Color.Red), (int)(lineN * k), 0, (int)(lineN * k), 511);

            }
            else
            {
                var k = (double)pbTestReadImage.Height / 512;
                e.Graphics.DrawLine(new Pen(Color.Red), 0, (int)(lineN * k), 511, (int)(lineN * k));
            }
        }

        private void pbTestDifference_Paint(object sender, PaintEventArgs e)
        {

            if (TestBmpDiffImage == null) return;
            e.Graphics.DrawImage(TestBmpDiffImage, 0, 0, pbTestDifference.Width, pbTestDifference.Height);
            var lineN = (int)numFrame.Value;
            if (cbVertical.Checked)
            {
                var k = (double)pbTestDifference.Width / 512;
                e.Graphics.DrawLine(new Pen(Color.Red), (int)(lineN * k), 0, (int)(lineN * k), 511);

            }
            else
            {
                var k = (double)pbTestDifference.Height / 512;
                e.Graphics.DrawLine(new Pen(Color.Red), 0, (int)(lineN * k), 511, (int)(lineN * k));
            }
        }

        private void pbTestStandard_Paint(object sender, PaintEventArgs e)
        {
            if (TestBmpStandardImage == null) return;
            e.Graphics.DrawImage(TestBmpStandardImage, 0, 0, pbTestStandard.Width, pbTestStandard.Height);
            var lineN = (int)numFrame.Value;
            if (cbVertical.Checked)
            {
                var k = (double)pbTestStandard.Width / 512;
                e.Graphics.DrawLine(new Pen(Color.Red), (int)(lineN * k), 0, (int)(lineN * k), 511);

            }
            else
            {
                var k = (double)pbTestStandard.Height / 512;
                e.Graphics.DrawLine(new Pen(Color.Red), 0, (int)(lineN * k), 511, (int)(lineN * k));
            }
        }

        private void pbTestReadImage_MouseUp(object sender, MouseEventArgs e)
        {
            if (cbVertical.Checked)
            {
                var k = (double)pbTestReadImage.Width / 512;
                numFrame.Value = (int)(e.X / k);
            }
            else
            {
                var k = (double)pbTestReadImage.Height / 512;
                numFrame.Value = (int)(e.Y / k);
            }
        }

        private void pbTestDifference_MouseUp(object sender, MouseEventArgs e)
        {
            if (cbVertical.Checked)
            {
                var k = (double)pbTestDifference.Width / 512;
                numFrame.Value = (int)(e.X / k);
            }
            else
            {
                var k = (double)pbTestDifference.Height / 512;
                numFrame.Value = (int)(e.Y / k);
            }
        }

        private void pbTestStandard_MouseUp(object sender, MouseEventArgs e)
        {
            if (cbVertical.Checked)
            {
                var k = (double)pbTestStandard.Width / 512;
                numFrame.Value = (int)(e.X / k);
            }
            else
            {
                var k = (double)pbTestStandard.Height / 512;
                numFrame.Value = (int)(e.Y / k);
            }
        }

        private void cbFullMax_CheckedChanged(object sender, EventArgs e)
        {
            if (cbFullMax.Checked)
            {
                numFrame.Enabled = false;
                cbVertical.Enabled = false;
            }
            else
            {
                numFrame.Enabled = true;
                cbVertical.Enabled = true;

            }
            //numFrame.Value = 0;
            TestRedrawBitmap();
            TestTabDrawGraphLine((int)numFrame.Value);
        }

        #endregion TestTab

        int TempStandardSocketNumber = -1;
        private void GetStandard_Click(object sender, EventArgs e)
        {
            CheckForDoMCModule();
            var ctrl = (Control)sender;
            var socketnumber = (int)ctrl.Tag;
            TempStandardSocketNumber = socketnumber;
            lblStandardSocketNumber.Text = TempStandardSocketNumber.ToString();

            if (!InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations.ContainsKey(TempStandardSocketNumber))
            {
                DoMCSocketIsNotConfiguredErrorMessage();
                return;
            }
            var socket = InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations[TempStandardSocketNumber];

            var img = socket.StandardImage;
            socket.TempAverageImage = img;
            /*if (img != null)
                socket.ImageText = ImageTools.ToBase64(ImageTools.Compress(ImageTools.ImageToArray(img)));*/
            var msbmp = ImageTools.DrawImage(img, invertColors);
            pbAverage.Image = msbmp;
            InterfaceDataExchange.Configuration.Save();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            /*if (InterfaceDataExchange == null) return;
            string text;
            switch (InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus)
            {
                case ModuleCommand.StartIdle: text = "Не занят"; break;
                case ModuleCommand.SetAllCardsAndSocketsConfiguration: text = "Настройка платы чтения гнезд"; break;
                case ModuleCommand.LoadConfiguration: text = "Загрузка настроек в платы"; break;
                case ModuleCommand.StartRead: text = "Чтение"; break;
                case ModuleCommand.StartReadExternal: text = "Чтение по внешнему сигналу"; break;
                case ModuleCommand.StartSeveralSocketRead: text = "Чтение нескольких гнезд"; break;
                case ModuleCommand.StartSeveralSocketReadExternal: text = "Чтение нескольких гнезд по внешнему сигналу"; break;
                case ModuleCommand.GetSocketGoodStatus: text = "Чтение статусов гнезд"; break;
                case ModuleCommand.GetSocketImages: text = "Чтение изображения гнезд"; break;
                case ModuleCommand.GetSeveralSocketImages: text = "Чтение изображения отдельных гнезд"; break;
                case ModuleCommand.StandardRead: text = "Чтение эталона"; break;
                case ModuleCommand.StandardWrite: text = "Запись эталона"; break;
                case ModuleCommand.StopModuleWork: text = "Остановка работы"; break;
                default: text = InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus.ToString(); break;
            }
            var stepText = "";
            switch (InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep)
            {
                case ModuleCommandStep.Start:
                    stepText = "Запуск";
                    break;
                case ModuleCommandStep.Processing:
                    stepText = "В процессе";
                    break;
                case ModuleCommandStep.Complete:
                    stepText = "Завершено";
                    break;
                case ModuleCommandStep.Error:
                    stepText = "Ошибка";
                    break;
            }
            string overallText;
            switch (InterfaceDataExchange.OperationOverallStatus)
            {
                case ModuleCommandStep.Start:
                    overallText = "Запуск";
                    break;
                case ModuleCommandStep.Processing:
                    overallText = "В процессе";
                    break;
                case ModuleCommandStep.Complete:
                    overallText = "Завершено";
                    break;
                case ModuleCommandStep.Error:
                    overallText = "Ошибка";
                    break;
                default:
                    overallText = InterfaceDataExchange.OperationOverallStatus.ToString();
                    break;
            }*/
            var fulltext = $"{text}: {stepText} ({overallText})";
            lblStatus.Text = fulltext;
            tslTestCCDCardReadStatus.Text = "Статус: " + fulltext;
            lblGetStandardWorkStatus.Text = fulltext;

            if ((DateTime.Now - InterfaceDataExchange.LEDStatus.TimeSyncSignalGot).TotalMilliseconds < 700)
                cbTestLCBSyncrosignal.CheckState = CheckState.Indeterminate;
            else
                cbTestLCBSyncrosignal.CheckState = CheckState.Unchecked;

            if (TestRDPBConnected)
            {
                if (InterfaceDataExchange.RDPBCurrentStatus.IsCurrentStatusActual())
                {
                    txbTestRDPBCoolingBlocksStatus.Text = InterfaceDataExchange.RDPBCurrentStatus.CoolingBlocksQuantity.ToString();
                }

                if (InterfaceDataExchange.RDPBCurrentStatus.IsParametersActual())
                {

                }
            }

            if (InterfaceDataExchange.DataStorage != null && InterfaceDataExchange.DataStorage.IsMovingReportWorking())
            {
                btnMoveToArchive.BackColor = Color.LimeGreen;
            }
            else
            {
                if (MovingCyclesToArchive)
                {
                    btnMoveToArchive.BackColor = Color.DarkGreen;

                }
                else
                {
                    btnMoveToArchive.BackColor = SystemColors.Control;
                }

            }
        }

        private void DoMCMainInterface_FormClosed(object sender, FormClosedEventArgs e)
        {
            InterfaceDataExchange.SendCommand(ModuleCommand.SetLCBNonWorkModeRequest);
            InterfaceDataExchange.SendCommand(ModuleCommand.LCBStop);
            InterfaceDataExchange.SendCommand(ModuleCommand.StopModuleWork);
        }

        private void pbStandard1_DoubleClick(object sender, EventArgs e)
        {
            if (TempStandardSocketNumber < 0) return;
            if (!InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations.ContainsKey(TempStandardSocketNumber))
            {
                DoMCSocketIsNotConfiguredErrorMessage();
                return;
            }
            var socket = InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations[TempStandardSocketNumber];
            if (socket.TempImages == null) return;
            var sf = new ShowFrameForm();
            sf.Image = socket.TempImages[0];
            sf.Show();
            //pbMainStandard.Image = pbStandard1.Image;
        }

        private void pbStandard2_DoubleClick(object sender, EventArgs e)
        {
            if (TempStandardSocketNumber < 0) return;
            if (!InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations.ContainsKey(TempStandardSocketNumber))
            {
                DoMCSocketIsNotConfiguredErrorMessage();
                return;
            }
            var socket = InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations[TempStandardSocketNumber];
            if (socket.TempImages == null) return;
            var sf = new ShowFrameForm();
            sf.Image = socket.TempImages[1];
            sf.Show();
            //pbMainStandard.Image = pbStandard2.Image;
        }

        private void pbStandard3_DoubleClick(object sender, EventArgs e)
        {
            if (TempStandardSocketNumber < 0) return;
            if (!InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations.ContainsKey(TempStandardSocketNumber))
            {
                DoMCSocketIsNotConfiguredErrorMessage();
                return;
            }
            var socket = InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations[TempStandardSocketNumber];
            if (socket.TempImages == null) return;
            var sf = new ShowFrameForm();
            sf.Image = socket.TempImages[2];
            sf.Show();
            //pbMainStandard.Image = pbStandard3.Image;
        }

        private void pbAverage_DoubleClick(object sender, EventArgs e)
        {
            if (TempStandardSocketNumber < 0) return;
            if (!InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations.ContainsKey(TempStandardSocketNumber))
            {
                DoMCSocketIsNotConfiguredErrorMessage();
                return;
            }
            var socket = InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations[TempStandardSocketNumber];
            if (socket.TempAverageImage == null) return;
            var sf = new ShowFrameForm();
            sf.Image = socket.TempAverageImage;
            sf.Show();
            //pbMainStandard.Image = pbAverage.Image;
        }

        private void btnMakeAverage_Click(object sender, EventArgs e)
        {
            if (!InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations.ContainsKey(TempStandardSocketNumber))
            {
                DoMCSocketIsNotConfiguredErrorMessage();
                return;
            }
            var socket = InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations[TempStandardSocketNumber];
            var lvl = (double)nudStandardLevel.Value / 100.0d;
            var imagesForStandards = new short[0][,];
            for (int i = 0; i < 3; i++)
            {
                //if (lvl > Math.Abs(socket.TempDeviations.TotalDeviation- socket.TempDeviations.SocketAverage[i]))
                if (lvl * socket.TempDeviations.TotalDeviation > Math.Abs(socket.TempDeviations.TotalAverage - socket.TempDeviations.SocketAverage[i]))
                {
                    Array.Resize(ref imagesForStandards, imagesForStandards.Length + 1);
                    imagesForStandards[imagesForStandards.Length - 1] = socket.TempImages[i];
                }
            }
            var AvgIm = ImageTools.CalculateAverage(imagesForStandards);
            socket.TempAverageImage = AvgIm;
            socket.StandardImage = AvgIm;
            InterfaceDataExchange.Configuration.Save();
            var msbmp = ImageTools.DrawImage(AvgIm, invertColors);
            pbAverage.Image = msbmp;
            FillSettingPage();
        }
        bool StandardIsReading = false;
        private void btnReadImagesForStandard_Click(object sender, EventArgs e)
        {
            if (!StandardIsReading)
            {
                StandardIsReading = true;
                var tsk = new Task(new Action(() => ImagesForStandard()));
                tsk.Start();
            }
        }

        private void ImagesForStandard()
        {

            try
            {
                InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Start;
                InterfaceDataExchange.CardsConnection.PacketLogActive = InterfaceDataExchange.Configuration.LogPackets;

                CheckForDoMCModule();
                if (TempStandardSocketNumber < 0) return;
                if (!InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations.ContainsKey(TempStandardSocketNumber))
                {
                    DoMCSocketIsNotConfiguredErrorMessage();
                    //InterfaceDataExchange.SendCommand(ModuleCommand.StartIdle);
                    return;
                }
                var socket = InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations[TempStandardSocketNumber];
                socket.DataType = 0;
                if (!LoadConfiguration(false))
                {
                    InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Error;
                    return;
                }
                InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Processing;
                InterfaceDataExchange.SendResetToCCDCards();
                Thread.Sleep(200);
                PictureBox[] StandardPictures = new PictureBox[3] { pbStandard1, pbStandard2, pbStandard3 };

                //short[][,] StandardImages = new short[3][,];
                socket.TempImages = new short[3][,];
                for (int i = 0; i < StandardPictures.Length; i++) StandardPictures[i].Image = null;
                pbAverage.Image = null;
                Application.DoEvents();


                InterfaceDataExchange.CCDDataEchangeStatuses.ExternalStart = cbExternalSignalForStandard.Checked;
                InterfaceDataExchange.CCDDataEchangeStatuses.SocketsToSave = new int[] { TempStandardSocketNumber };
                for (int repeat = 0; repeat < 3; repeat++)
                {
                    DataExchangeKernel.Log.Add(new Guid(), $"Начало чтения гнезда", true);
                    bool rc = true;
                    if (InterfaceDataExchange.CCDDataEchangeStatuses.ExternalStart)
                    {
                        InterfaceDataExchange.SendCommand(ModuleCommand.StartSeveralSocketReadExternal);
                        rc = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () =>
                        {
                            return InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartSeveralSocketReadExternal && InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Complete;
                        }, null);
                        if (!rc)
                        {
                            InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                            InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Error;
                            DoMCNotAbleToReadSocketErrorMessage();
                            //InterfaceDataExchange.SendCommand(ModuleCommand.StartIdle);
                            return;
                        }

                    }
                    else
                    {
                        InterfaceDataExchange.SendCommand(ModuleCommand.StartSeveralSocketRead);
                        rc = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () =>
                        {
                            return InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartSeveralSocketRead && InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Complete;
                        }, () => InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
                        if (!rc)
                        {
                            InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                            InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Error;
                            DoMCNotAbleToReadSocketErrorMessage();
                            //InterfaceDataExchange.SendCommand(ModuleCommand.StartIdle);

                            return;
                        }
                    }
                    DataExchangeKernel.Log.Add(new Guid(), $"Начало чтения картинки", true);
                    InterfaceDataExchange.SendCommand(ModuleCommand.GetSeveralSocketImages);
                    var ri = UserInterfaceControls.Wait(1 * InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () =>
                    {
                        var mst = InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus;
                        var stp = InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep;
                        return mst == ModuleCommand.GetSeveralSocketImages && (stp == ModuleCommandStep.Complete || stp == ModuleCommandStep.Error);
                    }, () => InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
                    if (!ri || InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Error)
                    {
                        InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                        InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Error;
                        DoMCNotAbleToReadSocketErrorMessage();
                        //InterfaceDataExchange.SendCommand(ModuleCommand.StartIdle);
                        return;
                    }
                    DataExchangeKernel.Log.Add(new Guid(), $"Картинка получена", true);
                    var images = InterfaceDataExchange.CCDDataEchangeStatuses.Images;
                    if (images is null)
                    {
                        InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Error;
                        DoMCNotAbleToReadSocketErrorMessage();
                        break;
                    }
                    socket.TempImages[repeat] = images[TempStandardSocketNumber - 1];
                    var sbmp = ImageTools.DrawImage(images[TempStandardSocketNumber - 1], invertColors);
                    StandardPictures[repeat].Image = sbmp;

                    //Application.DoEvents();
                }
                InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus = ModuleCommand.StartIdle;


                var dev = ImageTools.CalculateDeviationFull(socket.TempImages, socket.ImageProcessParameters.GetRectangle());
                socket.TempDeviations = dev;
                this.Invoke((MethodInvoker)delegate { ImagesForStandardSetLabelValues(); });
                InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Complete;

            }
            finally
            {
                StandardIsReading = false;
                StopCCDWork();
            }
        }

        private void ImagesForStandardSetLabelValues()
        {
            try
            {
                var socket = InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations[TempStandardSocketNumber];
                var dev = socket.TempDeviations;
                lblDeviation1.Text = dev.SocketAverage[0].ToString("F3");
                lblDeviation2.Text = dev.SocketAverage[1].ToString("F3");
                lblDeviation3.Text = dev.SocketAverage[2].ToString("F3");
                lblDeviationTotal.Text = dev.TotalDeviation.ToString("F3");
                lblTotalAverage.Text = dev.TotalAverage.ToString("F3");
                nudStandardLevel_ValueChanged(null, null);
            }
            catch { }
        }

        private void nudStandardLevel_ValueChanged(object sender, EventArgs e)
        {
            if (TempStandardSocketNumber < 0) return;
            if (!InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations.ContainsKey(TempStandardSocketNumber))
            {
                DoMCSocketIsNotConfiguredErrorMessage();
                return;
            }
            var socket = InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations[TempStandardSocketNumber];
            var lvl = (double)nudStandardLevel.Value / 100.0d;
            var avg = socket.TempDeviations.TotalAverage;
            var s = socket.TempDeviations.TotalDeviation;
            lblRange.Text = $"{avg - s * lvl:F3} - {avg + s * lvl:F3}";
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            var sf = new ShowFrameForm();
            sf.Image = testImage;
            sf.Show();
        }

        private void btnStandardSettings_Click(object sender, EventArgs e)
        {
            if (!StandardIsReading)
            {
                var task = new Task(new Action(() => FullStandardGet()));
                task.Start();
            }
        }
        private void FullStandardGet()
        {
            try
            {
                InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Start;
                WorkingLog.Add(LoggerLevel.Information, "--------------- Получение эталонов по всем гнездам ---------------");
                InterfaceDataExchange.CardsConnection.PacketLogActive = InterfaceDataExchange.Configuration.LogPackets;

                CheckForDoMCModule();
                //var socket = Configuration.SocketToCardSocketConfigurations[TempStandardSocketNumber];
                foreach (var socket in InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations)
                {
                    socket.Value.DataType = 0;
                    socket.Value.TempImages = new short[3][,];
                }
                WorkingLog.Add(LoggerLevel.Information, "Начало загрузки конфигурации");
                if (!LoadConfiguration(false, true, false)) return;
                InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Processing;
                WorkingLog.Add(LoggerLevel.Information, "Сброс гнезд");
                InterfaceDataExchange.SendResetToCCDCards();
                Thread.Sleep(200);

                PictureBox[] StandardPictures = new PictureBox[3] { pbStandard1, pbStandard2, pbStandard3 };

                //short[][,] StandardImages = new short[3][,];
                for (int i = 0; i < StandardPictures.Length; i++) StandardPictures[i].Image = null;
                pbAverage.Image = null;


                InterfaceDataExchange.CCDDataEchangeStatuses.ExternalStart = cbExternalSignalForStandard.Checked;
                //InterfaceDataExchange.SocketsToSave = new int[] { TempStandardSocketNumber };
                for (int repeat = 0; repeat < 3; repeat++)
                {
                    DataExchangeKernel.Log.Add(new Guid(), $"Начало чтения гнезда", true);
                    WorkingLog.Add(LoggerLevel.Information, "Начало чтения гнезда");
                    bool rc = true;
                    if (InterfaceDataExchange.CCDDataEchangeStatuses.ExternalStart)
                    {
                        WorkingLog.Add(LoggerLevel.Information, "Запуск чтения по внешнему сигналу");
                        InterfaceDataExchange.SendCommand(ModuleCommand.StartReadExternal);
                        rc = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () =>
                        {
                            return InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartReadExternal && InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Complete;
                        }, () => InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
                        if (!rc)
                        {
                            InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                            InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Error;
                            DoMCNotAbleToReadSocketErrorMessage();
                            return;
                        }
                    }
                    else
                    {
                        WorkingLog.Add(LoggerLevel.Information, "Запуск чтения");
                        InterfaceDataExchange.SendCommand(ModuleCommand.StartRead);
                        rc = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () =>
                        {
                            return InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartRead && InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Complete;
                        }, () => InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
                        if (!rc)
                        {
                            InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                            InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Error;
                            DoMCNotAbleToReadSocketErrorMessage();
                            return;
                        }
                    }
                    DataExchangeKernel.Log.Add(new Guid(), $"Начало чтения картинки", true);
                    WorkingLog.Add("Получение изображения");
                    InterfaceDataExchange.SendCommand(ModuleCommand.GetSocketImages);
                    var ri = UserInterfaceControls.Wait(2 * InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () =>
                    {
                        var mst = InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus;
                        var stp = InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep;
                        return mst == ModuleCommand.GetSocketImages && (stp == ModuleCommandStep.Complete || stp == ModuleCommandStep.Error);
                    }, () => InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
                    if (!ri || InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Error)
                    {
                        InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                        InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Error;
                        DoMCNotAbleToReadSocketErrorMessage();
                        return;
                    }
                    DataExchangeKernel.Log.Add(new Guid(), $"Изображение получено", true);
                    WorkingLog.Add("Изображение получено");


                    var images = InterfaceDataExchange.CCDDataEchangeStatuses.Images;
                    if (images is null)
                    {
                        InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Error;
                        DoMCNotAbleToReadSocketErrorMessage();
                        break;
                    }
                    for (int i = 0; i < Context.Configuration.HardwareSettings.SocketQuantity; i++)
                    {
                        var socket = InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations[i + 1];
                        socket.TempImages[repeat] = images[i];
                    }
                    StandardPictures[repeat].Image = bmpCheckSign;
                }
                for (int s = 0; s < Context.Configuration.HardwareSettings.SocketQuantity; s++)
                {
                    var socket = InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations[s + 1];

                    var dev = ImageTools.CalculateDeviationFull(socket.TempImages, socket.ImageProcessParameters.GetRectangle());
                    socket.TempDeviations = dev;

                    var lvl = (double)nudStandardLevel.Value / 100.0d;
                    var imagesForStandards = new short[0][,];
                    for (int i = 0; i < 3; i++)
                    {
                        //if (lvl > Math.Abs(socket.TempDeviations.TotalDeviation- socket.TempDeviations.SocketAverage[i]))
                        if (lvl * socket.TempDeviations.TotalDeviation > Math.Abs(socket.TempDeviations.TotalAverage - socket.TempDeviations.SocketAverage[i]))
                        {
                            Array.Resize(ref imagesForStandards, imagesForStandards.Length + 1);
                            imagesForStandards[imagesForStandards.Length - 1] = socket.TempImages[i];
                        }
                    }
                    var AvgIm = ImageTools.CalculateAverage(imagesForStandards);
                    socket.TempAverageImage = AvgIm;
                    socket.StandardImage = AvgIm;
                }
                InterfaceDataExchange.Configuration.Save();
                WorkingLog.Add("Отключение от плат ПЗС");
                StopCCDWork();
                pbAverage.Image = bmpCheckSign;
                FillSettingPage();
                DisplayMessage.Show("Эталоны по всем гнездам созданы.", "Завершено");
                InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Complete;
            }
            finally
            {
                InterfaceDataExchange.SendCommand(ModuleCommand.CCDStop);
                StandardIsReading = false;
            }
        }

        #region Menu

        private void miReadParameters_Click(object sender, EventArgs e)
        {
            CheckForDoMCModule();
            var ssf = new DoMCLib.Forms.DoMCSocketSettingsListForm();
            ssf.SocketQuantity = Context.Configuration.HardwareSettings.SocketQuantity;
            ssf.SocketConfigurations = InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations;
            if (ssf.ShowDialog() == DialogResult.OK)
            {
                Context.Configuration.HardwareSettings.SocketQuantity = ssf.SocketQuantity;
                InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations = ssf.SocketConfigurations;
                if (InterfaceDataExchange.Configuration.SocketsToCheck == null || InterfaceDataExchange.Configuration.SocketsToCheck.Length != ssf.SocketQuantity)
                {
                    InterfaceDataExchange.Configuration.SocketsToCheck = new bool[ssf.SocketQuantity];
                    for (int i = 0; i < ssf.SocketQuantity; i++) InterfaceDataExchange.Configuration.SocketsToCheck[i] = true;
                }
            }
            InterfaceDataExchange.Configuration.Save();
            SetConfigurationIsNotLoaded();


            FillSettingPage();
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
                FillSettingPage();
            }

        }

        private void miWorkModeSettings_Click(object sender, EventArgs e)
        {
            var gsf = new DoMCLib.Forms.DoMCGeneralSettingsForm();
            gsf.Value = InterfaceDataExchange.Configuration.WorkModeSettings;
            if (gsf.ShowDialog() == DialogResult.OK)
            {
                InterfaceDataExchange.Configuration.WorkModeSettings = gsf.Value;
                InterfaceDataExchange.Configuration.Save();
                FillSettingPage();
            }
        }

        private void miLEDSettings_Click(object sender, EventArgs e)
        {
            var ls = new DoMCLib.Forms.LEDSettingsForm();
            ls.Current = InterfaceDataExchange.Configuration.LCBSettings.LEDCurrent;
            ls.PreformLength = InterfaceDataExchange.Configuration.LCBSettings.PreformLength;
            ls.DelayLength = InterfaceDataExchange.Configuration.LCBSettings.DelayLength;
            ls.LCBKoefficient = InterfaceDataExchange.Configuration.LCBSettings.LCBKoefficient;
            if (ls.ShowDialog() == DialogResult.OK)
            {
                InterfaceDataExchange.Configuration.LCBSettings.LEDCurrent = ls.Current;
                InterfaceDataExchange.Configuration.LCBSettings.PreformLength = ls.PreformLength;
                InterfaceDataExchange.Configuration.LCBSettings.DelayLength = ls.DelayLength;
                InterfaceDataExchange.Configuration.LCBSettings.LCBKoefficient = ls.LCBKoefficient;
                InterfaceDataExchange.Configuration.Save();
                FillSettingPage();
            }
        }

        #endregion


        private void DoMCMainInterface_Load(object sender, EventArgs e)
        {
            SetFormSchema();

        }
        #region TestLCB
        CheckBox[] TestLCBOutputs;
        CheckBox[] TestLCBInputs;
        CheckBox[] TestLCBLEDs;
        private void btnTestLCBReadStatuses_Click(object sender, EventArgs e)
        {
            if (TestLCBTestStarted | !TestLCBConnected) return;

            var start = DateTime.Now;
            InterfaceDataExchange.SendCommand(ModuleCommand.GetLCBEquipmentStatusRequest);
            var res = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () => { return InterfaceDataExchange.LEDStatus.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.GetLCBEquipmentStatusResponse && InterfaceDataExchange.LEDStatus.LastCommandResponseReceived > start; });
            if (!res)
            {
                MessageBox.Show("Не удалось получить данные");
                return;
            }
            for (int o = 0; o < TestLCBOutputs.Length; o++)
            {
                TestLCBOutputs[o].CheckState = InterfaceDataExchange.LEDStatus.Outputs[o] ? CheckState.Indeterminate : CheckState.Unchecked;
            }
            for (int i = 0; i < TestLCBInputs.Length; i++)
            {
                TestLCBInputs[i].CheckState = InterfaceDataExchange.LEDStatus.Inputs[i] ? CheckState.Indeterminate : CheckState.Unchecked;
            }
            if (InterfaceDataExchange.LEDStatus.LEDStatuses == null) InterfaceDataExchange.LEDStatus.LEDStatuses = new bool[12];
            for (int l = 0; l < TestLCBLEDs.Length; l++)
            {
                TestLCBLEDs[l].CheckState = InterfaceDataExchange.LEDStatus.LEDStatuses[l] ? CheckState.Indeterminate : CheckState.Unchecked;
            }
        }

        private void btnTestLCBWriteStatuses_Click(object sender, EventArgs e)
        {
            if (TestLCBTestStarted | !TestLCBConnected) return;

            for (int o = 0; o < TestLCBOutputs.Length; o++)
            {
                InterfaceDataExchange.LEDStatus.Outputs[o] = TestLCBOutputs[o].CheckState != CheckState.Unchecked;
            }

            for (int i = 0; i < TestLCBInputs.Length; i++)
            {
                InterfaceDataExchange.LEDStatus.Inputs[i] = false;
            }

            if (InterfaceDataExchange.LEDStatus.LEDStatuses == null) InterfaceDataExchange.LEDStatus.LEDStatuses = new bool[12];
            for (int l = 0; l < TestLCBLEDs.Length; l++)
            {
                InterfaceDataExchange.LEDStatus.LEDStatuses[l] = TestLCBLEDs[l].CheckState != CheckState.Unchecked;
            }

            var start = DateTime.Now;
            InterfaceDataExchange.SendCommand(ModuleCommand.SetLCBEquipmentStatusRequest);
            var res = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () => { return InterfaceDataExchange.LEDStatus.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.SetLCBEquipmentStatusResponse && InterfaceDataExchange.LEDStatus.LastCommandResponseReceived > start; });
            if (!res || !InterfaceDataExchange.LEDStatus.LastCommandReceivedStatusIsOK)
            {
                MessageBox.Show("Не удалось передать данные");
                return;
            }
        }


        private void btnTestLCBInit_Click(object sender, EventArgs e)
        {
            if (TestLCBTestStarted) return;
            if (TestLCBConnected)
            {
                TestLCBStop();
            }
            else
            {
                TestLCBStart();

            }
        }

        private void btnTestLCBStop_Click(object sender, EventArgs e)
        {
            TestLCBStop();
        }

        private void TestLCBStart()
        {
            btnTestLCBInit.BackColor = Color.Gray;
            //InterfaceDataExchange.Configuration = InterfaceDataExchange.Configuration;
            if (!LoadConfiguration(false, false, true))
            {
                btnTestLCBInit.BackColor = Color.Red;
                DoMCNotInitializedErrorMessage();
                return;
            }
            TestLCBConnected = true;
            InterfaceDataExchange.CardsConnection.PacketLogActive = false;
            btnTestLCBInit.BackColor = Color.Green;
        }

        private void TestLCBStop()
        {
            InterfaceDataExchange.SendCommand(ModuleCommand.LCBStop);
            TestLCBTestStarted = false;
            TestLCBConnected = false;
            btnTestLCBInit.BackColor = SystemColors.Control;
        }


        private void btnTestLCBClearAll_Click(object sender, EventArgs e)
        {
            if (TestLCBTestStarted | !TestLCBConnected) return;

            for (int o = 0; o < TestLCBOutputs.Length; o++)
            {
                TestLCBOutputs[o].CheckState = CheckState.Unchecked;
            }
            for (int i = 0; i < TestLCBInputs.Length; i++)
            {
                TestLCBInputs[i].CheckState = CheckState.Unchecked;
            }
            if (InterfaceDataExchange.LEDStatus.LEDStatuses == null) InterfaceDataExchange.LEDStatus.LEDStatuses = new bool[12];
            for (int l = 0; l < TestLCBLEDs.Length; l++)
            {
                TestLCBLEDs[l].CheckState = CheckState.Unchecked;
            }
        }

        private void btnTestLCBSetAll_Click(object sender, EventArgs e)
        {
            if (TestLCBTestStarted | !TestLCBConnected) return;

            for (int o = 0; o < TestLCBOutputs.Length; o++)
            {
                TestLCBOutputs[o].CheckState = CheckState.Checked;
            }
            for (int i = 0; i < TestLCBInputs.Length; i++)
            {
                TestLCBInputs[i].CheckState = CheckState.Checked;
            }
            if (InterfaceDataExchange.LEDStatus.LEDStatuses == null) InterfaceDataExchange.LEDStatus.LEDStatuses = new bool[12];
            for (int l = 0; l < TestLCBLEDs.Length; l++)
            {
                TestLCBLEDs[l].CheckState = CheckState.Checked;
            }
        }

        bool TestLCBTestStarted = false;

        private void btnTestLCBFullTest_Click(object sender, EventArgs e)
        {
            if (TestLCBTestStarted | !TestLCBConnected) return;
            TestLCBTestStarted = true;
            List<CheckBox> checkboxes = new List<CheckBox>();
            checkboxes.AddRange(TestLCBLEDs);
            checkboxes.AddRange(TestLCBOutputs);
            checkboxes.Add(null);

            for (int o1 = 0; o1 < checkboxes.Count; o1++)
            {
                for (int i1 = 0; i1 < checkboxes.Count; i1++)
                {
                    if (checkboxes[i1] != null)
                        checkboxes[i1].CheckState = CheckState.Unchecked;
                }
                if (checkboxes[o1] != null)
                    checkboxes[o1].CheckState = CheckState.Checked;

                for (int o = 0; o < TestLCBOutputs.Length; o++)
                {
                    InterfaceDataExchange.LEDStatus.Outputs[o] = TestLCBOutputs[o].CheckState != CheckState.Unchecked;
                }

                for (int i = 0; i < TestLCBInputs.Length; i++)
                {
                    InterfaceDataExchange.LEDStatus.Inputs[i] = false;
                }

                if (InterfaceDataExchange.LEDStatus.LEDStatuses == null) InterfaceDataExchange.LEDStatus.LEDStatuses = new bool[12];
                for (int l = 0; l < TestLCBLEDs.Length; l++)
                {
                    InterfaceDataExchange.LEDStatus.LEDStatuses[l] = TestLCBLEDs[l].CheckState != CheckState.Unchecked;
                }

                Application.DoEvents();
                var start = DateTime.Now;
                InterfaceDataExchange.SendCommand(ModuleCommand.SetLCBEquipmentStatusRequest);
                var res = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () => { return InterfaceDataExchange.LEDStatus.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.SetLCBEquipmentStatusResponse && InterfaceDataExchange.LEDStatus.LastCommandResponseReceived > start; });
                if (!res || !InterfaceDataExchange.LEDStatus.LastCommandReceivedStatusIsOK)
                {
                    MessageBox.Show("Не удалось передать данные");
                    return;
                }
                Thread.Sleep(500);
            }
            TestLCBTestStarted = false;

        }



        private void btnTestLCBGetCurrent_Click(object sender, EventArgs e)
        {
            if (TestLCBTestStarted | !TestLCBConnected) return;
            var start = DateTime.Now;
            InterfaceDataExchange.SendCommand(ModuleCommand.GetLCBCurrentRequest);
            var res = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () => { return InterfaceDataExchange.LEDStatus.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.GetLEDCurrentResponse && InterfaceDataExchange.LEDStatus.LastCommandResponseReceived > start; });
            if (!res)
            {
                MessageBox.Show("Не удалось получить данные");
                return;
            }
            txbTestLCBCurrent.Text = (InterfaceDataExchange.LEDStatus.LEDCurrent).ToString();
        }

        private void btnTestLCBSetCurrent_Click(object sender, EventArgs e)
        {
            if (TestLCBTestStarted | !TestLCBConnected) return;
            if (!int.TryParse(txbTestLCBCurrent.Text, out int current))
            {
                MessageBox.Show("Значение тока должно быть целым числом");
                txbTestLCBCurrent.Focus();
                return;
            }
            var start = DateTime.Now;
            //InterfaceDataExchange.LEDStatus.LEDCurrent = current ;
            InterfaceDataExchange.Configuration.LCBSettings.LEDCurrent = current;
            InterfaceDataExchange.SendCommand(ModuleCommand.SetLCBCurrentRequest);
            var res = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () => { return InterfaceDataExchange.LEDStatus.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.SetLEDCurrentResponse && InterfaceDataExchange.LEDStatus.LastCommandResponseReceived > start; });
            if (!res || !InterfaceDataExchange.LEDStatus.LastCommandReceivedStatusIsOK)
            {
                MessageBox.Show("Не удалось передать данные");
                return;
            }
        }

        private void btnTestLCBGetMovementParameters_Click(object sender, EventArgs e)
        {
            if (TestLCBTestStarted | !TestLCBConnected) return;
            var start = DateTime.Now;
            InterfaceDataExchange.SendCommand(ModuleCommand.GetLCBMovementParametersRequest);
            var res = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () => { return InterfaceDataExchange.LEDStatus.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.GetLCBMovementParametersResponse && InterfaceDataExchange.LEDStatus.LastCommandResponseReceived > start; });
            if (!res)
            {
                MessageBox.Show("Не удалось получить данные");
                return;
            }
            SetImpulsesToTextBoxes(txbTestLCBPreformLength, txbTestLCBPreformLengthMm, InterfaceDataExchange.LEDStatus.PreformLength);
            SetImpulsesToTextBoxes(txbTestLCBDelayLength, txbTestLCBDelayLengthMm, InterfaceDataExchange.LEDStatus.DelayLength);

            /*txbTestLCBPreformLength.Text = InterfaceDataExchange.LEDStatus.PreformLength.ToString();
            txbTestLCBDelayLength.Text = InterfaceDataExchange.LEDStatus.DelayLength.ToString();*/

        }

        private void btnTestLCBSetMovementParameters_Click(object sender, EventArgs e)
        {
            if (TestLCBTestStarted | !TestLCBConnected) return;
            if (!int.TryParse(txbTestLCBPreformLength.Text, out int preformLength))
            {
                MessageBox.Show("Значение длины преформы в импульсах должно быть целым числом");
                txbTestLCBPreformLength.Focus();
                return;
            }
            if (!int.TryParse(txbTestLCBDelayLength.Text, out int delayLength))
            {
                MessageBox.Show("Значение расстояния задержки в импульсах должно быть целым числом");
                txbTestLCBDelayLength.Focus();
                return;
            }
            InterfaceDataExchange.Configuration.LCBSettings.PreformLength = preformLength;
            InterfaceDataExchange.Configuration.LCBSettings.DelayLength = delayLength;
            var start = DateTime.Now;
            InterfaceDataExchange.SendCommand(ModuleCommand.SetLCBMovementParametersRequest);
            var res = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () => { return InterfaceDataExchange.LEDStatus.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.SetLCBMovementParametersResponse && InterfaceDataExchange.LEDStatus.LastCommandResponseReceived > start; });
            if (!res || !InterfaceDataExchange.LEDStatus.LastCommandReceivedStatusIsOK)
            {
                MessageBox.Show("Не удалось передать данные");
                return;
            }
        }

        private void TestLCBSetWorkMode()
        {
            var start = DateTime.Now;
            InterfaceDataExchange.IsWorkingModeStarted = true;
            InterfaceDataExchange.SendCommand(ModuleCommand.SetLCBWorkModeRequest);
            InterfaceDataExchange.IsWorkingModeStarted = false;
            var res = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () => InterfaceDataExchange.LEDStatus.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.SetLCBWorkModeResponse && InterfaceDataExchange.LEDStatus.LastCommandResponseReceived > start);
            if (!res)
            {
                MessageBox.Show("Не удалось получить данные");
                return;
            }
            //txbTestLCBMaximumHorizontalStroke.Text = (InterfaceDataExchange.LEDStatus.MaximumHorizontalStroke).ToString();
        }

        private void TestLCBSetSettingMode()
        {
            var start = DateTime.Now;
            InterfaceDataExchange.IsWorkingModeStarted = false;
            InterfaceDataExchange.SendCommand(ModuleCommand.SetLCBNonWorkModeRequest);
            var res = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () => InterfaceDataExchange.LEDStatus.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.SetLCBWorkModeResponse && InterfaceDataExchange.LEDStatus.LastCommandResponseReceived > start);
            if (!res)
            {
                MessageBox.Show("Не удалось получить данные");
                return;
            }
            //txbTestLCBMaximumHorizontalStroke.Text = (InterfaceDataExchange.LEDStatus.MaximumHorizontalStroke).ToString();
        }
        private void chbTestLCBWorkMode_CheckedChanged(object sender, EventArgs e)
        {
            if (TestLCBTestStarted | !TestLCBConnected) return;
            if (chbTestLCBWorkMode.Checked)
            {
                chbTestLCBWorkMode.BackColor = Color.Green;
                TestLCBSetWorkMode();
            }
            else
            {
                chbTestLCBWorkMode.BackColor = Color.Transparent;
                TestLCBSetSettingMode();
            }
        }
        private void btnTestLCBGetMaxPosition_Click(object sender, EventArgs e)
        {
            if (TestLCBTestStarted | !TestLCBConnected) return;
            var start = DateTime.Now;
            InterfaceDataExchange.SendCommand(ModuleCommand.GetLCBMaxPositionRequest);
            var res = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () => InterfaceDataExchange.LEDStatus.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.GetLCBMaxHorizontalStrokeResponse && InterfaceDataExchange.LEDStatus.LastCommandResponseReceived > start);
            if (!res)
            {
                MessageBox.Show("Не удалось получить данные");
                return;
            }
            txbTestLCBMaximumHorizontalStroke.Text = (InterfaceDataExchange.LEDStatus.MaximumHorizontalStroke).ToString();
            if ((InterfaceDataExchange?.Configuration?.LCBSettings.LCBKoefficient ?? 0) > 0)
            {
                txbTestLCBMaximumHorizontalStrokeMm.Text = (InterfaceDataExchange.LEDStatus.MaximumHorizontalStroke / InterfaceDataExchange.Configuration.LCBSettings.LCBKoefficient).ToString();
            }
        }

        System.Windows.Forms.Timer TestLCBGetCurrentPositionTimer;
        private void btnTestLCBGetCurrentPosition_Click(object sender, EventArgs e)
        {
            if (TestLCBTestStarted | !TestLCBConnected) return;
            if (TestLCBGetCurrentPositionTimer == null)
            {
                TestLCBGetCurrentPositionTimer = new System.Windows.Forms.Timer();
                TestLCBGetCurrentPositionTimer.Interval = 200;
                TestLCBGetCurrentPositionTimer.Tick += TestLCBGetCurrentPositionTimer_Tick;
                TestLCBGetCurrentPositionTimer.Start();
                TestLCBSetGetCurrentButtonActive(true);
            }
            else
            {
                TestLCBGetCurrentPositionTimer.Stop();
                TestLCBGetCurrentPositionTimer = null;
                TestLCBSetGetCurrentButtonActive(false);
            }
        }

        private void TestLCBGetCurrentPositionTimer_Tick(object sender, EventArgs e)
        {
            if (!TestLCBGetCurrentPosition())
            {
                if (TestLCBGetCurrentPositionTimer != null)
                {
                    TestLCBGetCurrentPositionTimer.Stop();
                    TestLCBGetCurrentPositionTimer = null;
                }
                TestLCBSetGetCurrentButtonActive(false);
                MessageBox.Show("Не удалось получить данные");
            }
        }
        private void TestLCBSetGetCurrentButtonActive(bool active)
        {
            Color clr;
            if (active)
                clr = Color.Green;
            else
                clr = SystemColors.Control;

            btnTestLCBGetCurrentPosition.BackColor = clr;
        }

        private bool TestLCBGetCurrentPosition()
        {
            var start = DateTime.Now;
            InterfaceDataExchange.SendCommand(ModuleCommand.GetLCBCurrentPositionRequest);
            var res = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () => InterfaceDataExchange.LEDStatus.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.GetLCBCurrentHorizontalStrokeResponse && InterfaceDataExchange.LEDStatus.LastCommandResponseReceived > start);
            if (!res)
            {
                return false;
            }
            txbTestLCBCurrentHorizontalStroke.Text = (InterfaceDataExchange.LEDStatus.CurrentHorizontalStroke).ToString();
            if ((InterfaceDataExchange?.Configuration?.LCBSettings.LCBKoefficient ?? 0) > 0)
            {
                txbTestLCBCurrentHorizontalStrokeMm.Text = (InterfaceDataExchange.LEDStatus.CurrentHorizontalStroke / InterfaceDataExchange.Configuration.LCBSettings.LCBKoefficient).ToString();
            }

            return true;
        }


        #endregion TestLCB

        #region Опрос в цикле
        bool IsCycleStarted;
        private void AbleForCycle(bool InCycle)
        {
            btnCycleStart.Enabled = !InCycle;
            btnCycleStop.Enabled = InCycle;
        }

        private void btnCycleStartStop_Click(object sender, EventArgs e)
        {

            if (TestCCDIsReading) return;
            TestCCDIsReading = true;
            IsCycleStarted = true;
            AbleForCycle(true);
            AbleForRead(true);
            InterfaceDataExchange.CCDDataEchangeStatuses.ExternalStart = cbTest_ExternalStart.Checked;
            InterfaceDataExchange.CCDDataEchangeStatuses.FastRead = true;
            CycleReadingProc();
            IsCycleStarted = false;
            TestCCDIsReading = false;
        }

        private void btnCycleStop_Click(object sender, EventArgs e)
        {
            IsCycleStarted = false;
            TestCCDIsReading = false;
        }

        private void CycleReadingProc()
        {
            try
            {
                InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Start;

                if (TestSocketNumberSelected < 1)
                {
                    MessageBox.Show("Нужно выбрать гнездо");
                    TestCCDIsReading = false;
                    return;
                }
                InterfaceDataExchange.CCDDataEchangeStatuses.SocketsToSave = new int[1] { TestSocketNumberSelected };
                CheckForDoMCModule();
                if (!LoadConfiguration(false, true, false)) //------ загрузить конфигурацию для одного гнезда
                {
                    DoMCNotAbleLoadConfigurationErrorMessage();
                    return;
                }
                //IsAbleToWork = true;

                InterfaceDataExchange.CurrentCycleCCD = new CycleImagesCCD();
                InterfaceDataExchange.CurrentCycleCCD.Differences = new short[Context.Configuration.HardwareSettings.SocketQuantity][,];
                InterfaceDataExchange.CurrentCycleCCD.WorkModeImages = new short[Context.Configuration.HardwareSettings.SocketQuantity][,];
                InterfaceDataExchange.CurrentCycleCCD.StandardImage = new short[Context.Configuration.HardwareSettings.SocketQuantity][,];
                InterfaceDataExchange.CurrentCycleCCD.IsSocketGood = new bool[Context.Configuration.HardwareSettings.SocketQuantity];

                //UserInterfaceControls.SetSocketStatuses(WorkModeStandardSettingsSocketsPanelList, new bool[WorkModeStandardSettingsSocketsPanelList.Length], Color.Green, Color.DarkGray);
                //if (!IsAbleToWork) return;


                InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Processing;

                while (IsCycleStarted)
                {
                    Application.DoEvents();
                    bool rc = true;
                    if (InterfaceDataExchange.CCDDataEchangeStatuses.ExternalStart)
                    {
                        InterfaceDataExchange.SendCommand(ModuleCommand.StartSeveralSocketReadExternal);
                        rc = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () =>
                        {
                            Application.DoEvents();
                            return InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartSeveralSocketReadExternal && InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Complete;
                        }, () => InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
                        if (!rc)
                        {
                            //DoMCNotAbleToReadSocketErrorMessage();
                            //IsWorkingModeStarted = false;
                            InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                            InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Error;
                            return;
                        }
                    }
                    else
                    {
                        InterfaceDataExchange.SendCommand(ModuleCommand.CCDCardsReset);
                        Thread.Sleep(10);
                        InterfaceDataExchange.SendCommand(ModuleCommand.StartSeveralSocketRead);
                        rc = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () =>
                        {
                            Application.DoEvents();
                            return InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartSeveralSocketRead && InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Complete;
                        }, () => InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
                        if (!rc)
                        {
                            //DoMCNotAbleToReadSocketErrorMessage();
                            //IsWorkingModeStarted = false;
                            InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                            InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Error;
                            return;
                        }
                    }
                    InterfaceDataExchange.CurrentCycleCCD.CycleCCDDateTime = DateTime.Now;
                    InterfaceDataExchange.SendCommand(ModuleCommand.GetSeveralSocketImages);
                    var ri = UserInterfaceControls.Wait(2 * InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () =>
                    {
                        Application.DoEvents();

                        var mst = InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus;
                        var stp = InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep;
                        return mst == ModuleCommand.GetSeveralSocketImages && (stp == ModuleCommandStep.Complete || stp == ModuleCommandStep.Error);
                    }, () => InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
                    if (!ri || InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Error)
                    {
                        //DoMCNotAbleToReadSocketErrorMessage();
                        //IsWorkingModeStarted = false;
                        InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                        InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Error;
                        return;
                    }
                    var images = InterfaceDataExchange.CCDDataEchangeStatuses.Images;
                    if (images is null)
                    {
                        //DoMCNotAbleToReadSocketErrorMessage();
                        //IsWorkingModeStarted = false;
                        InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                        InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Error;
                        return;
                    }
                    InterfaceDataExchange.CurrentCycleCCD.WorkModeImages = images;

                    Application.DoEvents();

                    for (int i = 0; i < InterfaceDataExchange.CurrentCycleCCD.WorkModeImages.Length; i++)
                    {
                        var socketnumber = i + 1;
                        if (InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations.ContainsKey(socketnumber))
                        {
                            var socket = InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations[socketnumber];
                            InterfaceDataExchange.CurrentCycleCCD.Differences[i] = ImageTools.GetDifference(InterfaceDataExchange.CurrentCycleCCD.WorkModeImages[i], InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations[socketnumber].StandardImage);
                            //var dev = ImageTools.CalculateDeviationFull(new short[][,] { InterfaceDataExchange.CurrentCycle.Differences[i] }, socket.StartLine, socket.EndLine);
                        }
                    }
                    Application.DoEvents();

                    ShowSocket(TestSocketNumberSelected);
                }
            }
            finally
            {
                AbleForCycle(false);
                AbleForRead(false);

            }
            StopCCDWork();
            InterfaceDataExchange.OperationOverallStatus = ModuleCommandStep.Complete;
        }

        #endregion

        private void pbTestReadImage_DoubleClick(object sender, EventArgs e)
        {
            if (TestReadImage == null) return;
            var sf = new ShowFrameForm();
            sf.Image = TestReadImage;
            sf.Show();
        }

        private void pbTestDifference_DoubleClick(object sender, EventArgs e)
        {
            if (TestDiffImage == null) return;
            var sf = new ShowFrameForm();
            sf.Image = TestDiffImage;
            sf.Show();
        }

        private void pbTestStandard_DoubleClick(object sender, EventArgs e)
        {
            if (TestStandardImage == null) return;
            var sf = new ShowFrameForm();
            sf.Image = TestStandardImage;
            sf.Show();
        }

        private void cbInvertColors_CheckedChanged(object sender, EventArgs e)
        {
            invertColors = cbInvertColors.Checked;
            ShowSocket(TestSocketNumberSelected);
        }

        private void miSetCheckSockets_Click(object sender, EventArgs e)
        {
            if (InterfaceDataExchange != null && InterfaceDataExchange.Configuration != null)
            {
                if (InterfaceDataExchange.Configuration.SocketsToCheck == null) InterfaceDataExchange.Configuration.SocketsToCheck = new bool[Context.Configuration.HardwareSettings.SocketQuantity];
                var scf = new DoMCSocketOnOffForm();
                scf.SocketIsOn = InterfaceDataExchange.Configuration.SocketsToCheck;
                scf.Text = "Включение проверки данных по гнездам";
                scf.ShowDialog();
                InterfaceDataExchange.Configuration.SocketsToCheck = scf.SocketIsOn;
                InterfaceDataExchange.Configuration.Save();
            }
        }

        private void miWorkInterfaceStart_Click(object sender, EventArgs e)
        {
            TestLCBStop();
            var wmf = new DoMCWorkModeInterface();
            wmf.SetMemoryReference(globalMemory);
            this.Hide();
            wmf.ShowDialog();
            this.Show();
        }

        private void miDBSettings_Click(object sender, EventArgs e)
        {
            if (InterfaceDataExchange?.Configuration == null) return;

            var dbsf = new DoMCLib.Forms.DoMCDBSettingsForm();
            dbsf.LocalDBConnectionString = InterfaceDataExchange.Configuration.LocalDataStorageConnectionString;
            dbsf.RemoteDBConnectionString = InterfaceDataExchange.Configuration.RemoteDataStorageConnectionString;
            dbsf.DelayBeforeMoveDataToArchive = InterfaceDataExchange.Configuration.Timeouts.DelayBeforeMoveDataToArchiveTimeInSeconds;
            if (dbsf.ShowDialog() == DialogResult.OK)
            {
                InterfaceDataExchange.Configuration.LocalDataStorageConnectionString = dbsf.LocalDBConnectionString;
                InterfaceDataExchange.Configuration.RemoteDataStorageConnectionString = dbsf.RemoteDBConnectionString;
                InterfaceDataExchange.Configuration.Timeouts.DelayBeforeMoveDataToArchiveTimeInSeconds = dbsf.DelayBeforeMoveDataToArchive;
                InterfaceDataExchange.Configuration.Save();
            }
        }

        private void miSaveSockets_Click(object sender, EventArgs e)
        {
            if (InterfaceDataExchange != null && InterfaceDataExchange.Configuration != null)
            {
                if (InterfaceDataExchange.Configuration.SocketsToSave == null || InterfaceDataExchange.Configuration.SocketsToSave.Length != Context.Configuration.HardwareSettings.SocketQuantity)
                    InterfaceDataExchange.Configuration.SocketsToSave = new bool[Context.Configuration.HardwareSettings.SocketQuantity];
                var scf = new DoMCSocketOnOffForm();
                scf.SocketIsOn = InterfaceDataExchange.Configuration.SocketsToSave;
                scf.Text = "Включение сохранения данных по гнездам";
                scf.ShowDialog();
                InterfaceDataExchange.Configuration.SocketsToSave = scf.SocketIsOn;
                InterfaceDataExchange.Configuration.Save();
            }
        }

        private void miRDPSettings_Click(object sender, EventArgs e)
        {
            if (InterfaceDataExchange != null && InterfaceDataExchange.Configuration != null)
            {
                if (InterfaceDataExchange.Configuration.RemoveDefectedPreformBlockConfig == null)
                    InterfaceDataExchange.Configuration.RemoveDefectedPreformBlockConfig = new RemoveDefectedPreformBlockConfig();
                var rdpsf = new RDPSettingsForm();
                rdpsf.IPPort = InterfaceDataExchange.Configuration.RemoveDefectedPreformBlockConfig.Port;
                rdpsf.IPAddress = IPAddress.Parse(InterfaceDataExchange.Configuration.RemoveDefectedPreformBlockConfig.IP ?? "0.0.0.0");
                rdpsf.CoolingBlocks = InterfaceDataExchange.Configuration.RemoveDefectedPreformBlockConfig.CoolingBlocksQuantity;
                rdpsf.MachineNumber = InterfaceDataExchange.Configuration.RemoveDefectedPreformBlockConfig.MachineNumber;
                rdpsf.SendBadCycleToRDPB = InterfaceDataExchange.Configuration.RemoveDefectedPreformBlockConfig.SendBadCycleToRDPB;
                rdpsf.Text = "Параметры бракера";
                if (rdpsf.ShowDialog() == DialogResult.OK)
                {
                    InterfaceDataExchange.Configuration.RemoveDefectedPreformBlockConfig.Port = rdpsf.IPPort;
                    InterfaceDataExchange.Configuration.RemoveDefectedPreformBlockConfig.IP = rdpsf.IPAddress.ToString();
                    InterfaceDataExchange.Configuration.RemoveDefectedPreformBlockConfig.CoolingBlocksQuantity = rdpsf.CoolingBlocks;
                    InterfaceDataExchange.Configuration.RemoveDefectedPreformBlockConfig.MachineNumber = rdpsf.MachineNumber;
                    InterfaceDataExchange.Configuration.RemoveDefectedPreformBlockConfig.SendBadCycleToRDPB = rdpsf.SendBadCycleToRDPB;
                    InterfaceDataExchange.Configuration.Save();
                }
            }
        }

        private void TestRDPBStop()
        {
            InterfaceDataExchange.SendCommand(ModuleCommand.RDPBStop);
            if (!InterfaceDataExchange.RDPBCurrentStatus.IsStarted)
            {
                TestRDPBConnected = false;
                btnRDPBTestConnect.BackColor = SystemColors.Control;
                btnRDPBTestConnect.Text = "Подключить";
            }
        }
        private void TestRDPBStart()
        {
            InterfaceDataExchange.SendCommand(ModuleCommand.RDPBStart);
            if (InterfaceDataExchange.RDPBCurrentStatus.IsStarted)
            {
                TestRDPBConnected = true;
                btnRDPBTestConnect.BackColor = Color.Green;
                btnRDPBTestConnect.Text = "Отключить";
            }
        }

        private void btnRDPBTestConnect_Click(object sender, EventArgs e)
        {
            if (TestRDPBConnected)
            {
                TestRDPBStop();
            }
            else
            {
                TestRDPBStart();
                TestRDPBStatusFill();
            }
        }

        private void btnTestRDPBN80_Click(object sender, EventArgs e)
        {
            if (!TestRDPBConnected) return;
            this.Enabled = false;
            InterfaceDataExchange.SendCommand(ModuleCommand.RDPBSetIsOK);
            var res = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForRDPBCardAnswerTimeout, () => InterfaceDataExchange.RDPBCurrentStatus.SentCommandType == DoMCLib.Classes.RDPBCommandType.SetIsOK && InterfaceDataExchange.RDPBCurrentStatus.IsCurrentStatusActual());
            if (!res)
            {
                btnRDPBTestConnect.BackColor = Color.Red;
                TestRDPBConnected = false;
                TestRDPBStop();
            }
            TestRDPBStatusFill();
            this.Enabled = true;
        }

        private void btnTestRDPBN81_Click(object sender, EventArgs e)
        {
            //if (!TestRDPBConnected) return;
            this.Enabled = false;
            InterfaceDataExchange.SendCommand(ModuleCommand.RDPBSetIsBad);
            var res = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForRDPBCardAnswerTimeout, () => InterfaceDataExchange.RDPBCurrentStatus.SentCommandType == DoMCLib.Classes.RDPBCommandType.SetIsBad && InterfaceDataExchange.RDPBCurrentStatus.IsCurrentStatusActual());
            if (!res)
            {
                btnRDPBTestConnect.BackColor = Color.Red;
                TestRDPBConnected = false;
                TestRDPBStop();
            }
            TestRDPBStatusFill();
            this.Enabled = true;
        }

        private void btnTestRDPBN82_Click(object sender, EventArgs e)
        {
            if (!TestRDPBConnected) return;
            InterfaceDataExchange.SendCommand(ModuleCommand.RDPBOn);
            var res = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForRDPBCardAnswerTimeout, () => InterfaceDataExchange.RDPBCurrentStatus.SentCommandType == DoMCLib.Classes.RDPBCommandType.On);
            if (!res)
            {
                btnRDPBTestConnect.BackColor = Color.Red;
                TestRDPBConnected = false;
                TestRDPBStop();
            }
            TestRDPBStatusFill();
            this.Enabled = true;
        }

        private void btnTestRDPBN83_Click(object sender, EventArgs e)
        {
            if (!TestRDPBConnected) return;
            this.Enabled = false;
            InterfaceDataExchange.SendCommand(ModuleCommand.RDPBOff);
            var res = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForRDPBCardAnswerTimeout, () => InterfaceDataExchange.RDPBCurrentStatus.SentCommandType == DoMCLib.Classes.RDPBCommandType.Off);
            if (!res)
            {
                btnRDPBTestConnect.BackColor = Color.Red;
                TestRDPBConnected = false;
                TestRDPBStop();
            }
            TestRDPBStatusFill();
            this.Enabled = true;
        }

        private void btnTestRDPBN90_Click(object sender, EventArgs e)
        {
            if (!TestRDPBConnected) return;
            this.Enabled = false;
            InterfaceDataExchange.SendCommand(ModuleCommand.RDPBGetParameters);
            var res = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForRDPBCardAnswerTimeout, () => InterfaceDataExchange.RDPBCurrentStatus.SentCommandType == DoMCLib.Classes.RDPBCommandType.GetParameters && InterfaceDataExchange.RDPBCurrentStatus.IsParametersActual());
            if (!res)
            {
                btnRDPBTestConnect.BackColor = Color.Red;
                TestRDPBConnected = false;
                TestRDPBStop();
            }
            TestRDPBStatusFill();
            this.Enabled = true;
        }

        private void btnTestRDPBSendCommand_Click(object sender, EventArgs e)
        {
            if (!TestRDPBConnected) return;
            InterfaceDataExchange.RDPBCurrentStatus.ManualCommand = txbTestRDPBManualCommand.Text;
            InterfaceDataExchange.SendCommand(ModuleCommand.RDPBSendManualCommand);
            Thread.Sleep(InterfaceDataExchange.Configuration.Timeouts.WaitForRDPBCardAnswerTimeout / 10);
            TestRDPBStatusFill();
        }

        private void cbTestRDPBCoolingBlocksQuantity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!TestRDPBConnected) return;
            InterfaceDataExchange.RDPBCurrentStatus.CoolingBlocksQuantityToSet = int.Parse(cbTestRDPBCoolingBlocksQuantity.SelectedItem?.ToString() ?? "0");
            InterfaceDataExchange.SendCommand(ModuleCommand.RDPBSetCoolingBlockQuantity);
            UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForRDPBCardAnswerTimeout, () => InterfaceDataExchange.RDPBCurrentStatus.SentCommandType == DoMCLib.Classes.RDPBCommandType.SetCoolingBlocks && InterfaceDataExchange.RDPBCurrentStatus.IsParametersActual());
            TestRDPBStatusFill();
        }

        private void TestRDPBStatusFill()
        {
            lvTestRDPBStatuses.Items.Clear();
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[]{"Короб",
                InterfaceDataExchange.RDPBCurrentStatus.BoxDirection == DoMCLib.Classes.BoxDirectionType.Left ? "Левый" :
                InterfaceDataExchange.RDPBCurrentStatus.BoxDirection == DoMCLib.Classes.BoxDirectionType.Right ? "Правый" :
                "Неизвестно"
            }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[] { "Номер короба", InterfaceDataExchange.RDPBCurrentStatus.BoxNumber.ToString() }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[] { "Бракер ", (InterfaceDataExchange.RDPBCurrentStatus.BlockIsOn ? "Включен" : "Выключен") }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[] { "Охлаждающих блоков", InterfaceDataExchange.RDPBCurrentStatus.CoolingBlocksQuantity.ToString() }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[] { "Номер цикла", InterfaceDataExchange.RDPBCurrentStatus.CycleNumber.ToString() }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[] { "Забракованных съемов", InterfaceDataExchange.RDPBCurrentStatus.BadSetQuantityInBox.ToString() }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[] { "Хороших съемов", InterfaceDataExchange.RDPBCurrentStatus.GoodSetQuantityInBox.ToString() }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[] { "Съемов в коробе", InterfaceDataExchange.RDPBCurrentStatus.SetQuantityInBox.ToString() }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[]{"Направление ленты",
                InterfaceDataExchange.RDPBCurrentStatus.TransporterSide == DoMCLib.Classes.RDPBTransporterSide.Stoped ? "Стоит" :
                InterfaceDataExchange.RDPBCurrentStatus.TransporterSide == DoMCLib.Classes.RDPBTransporterSide.Left ? "Влево" :
                InterfaceDataExchange.RDPBCurrentStatus.TransporterSide == DoMCLib.Classes.RDPBTransporterSide.Right ? "Вправо" :
                "Ошибка датчика"
            }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[]{"Ошибки",
                InterfaceDataExchange.RDPBCurrentStatus.Errors == DoMCLib.Classes.RDPBErrors.NoErrors ? "Ошибок нет" :
                InterfaceDataExchange.RDPBCurrentStatus.Errors == DoMCLib.Classes.RDPBErrors.TransporterDriveUnit ? "Авария привода конвейера" :
                InterfaceDataExchange.RDPBCurrentStatus.Errors == DoMCLib.Classes.RDPBErrors.SensorOfInitialState ? "Авария датчика исходного состояния" :
                "Неизвестная ошибка"
            }));
            txbTestRDPBCoolingBlocksStatus.Text = InterfaceDataExchange.RDPBCurrentStatus.CoolingBlocksQuantity.ToString();
        }

        private void btnTestDBLocal_Click(object sender, EventArgs e)
        {
            TestDB(InterfaceDataExchange.Configuration.LocalDataStorageConnectionString);
        }
        private void btnTestDBRemote_Click(object sender, EventArgs e)
        {
            TestDB(InterfaceDataExchange.Configuration.RemoteDataStorageConnectionString);
        }
        private bool TestDBCheckForConfiguration(string ConnectionString)
        {
            if (String.IsNullOrEmpty(ConnectionString))
            {
                MessageBox.Show("Не настроено подключение к базе данных");
                return false;
            }
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder(ConnectionString);
            if (String.IsNullOrWhiteSpace(csb.DataSource))
            {
                MessageBox.Show("Не указан сервер базы данных");
                return false;
            }
            if (String.IsNullOrWhiteSpace(csb.InitialCatalog))
            {
                MessageBox.Show("Не указано имя базы данных");
                return false;
            }
            return true;
        }
        private void TestDB(string ConnectionString)
        {
            /* if (!TestDBCheckForConfiguration(ConnectionString)) return;
             var db = new DoMCLib.DB.MSSQLDBStorage(ConnectionString);
             if (!db.CheckDB(false))
             {
                 if (MessageBox.Show("Ошибка в базе данных. Пересоздать?", "Запрос", MessageBoxButtons.YesNo) == DialogResult.Yes)
                 {
                     if (DoMCLib.Dialogs.DigitalInput.ShowIntegerDialog("Введите пинкод для продолжения", true) == AdminPinCode)
                     {
                         if (db.CheckDB(true))
                         {
                             MessageBox.Show("База данных создана");
                             return;
                         }
                         else
                         {
                             MessageBox.Show("Ошибка при создании базы данных");
                             return;
                         }
                     }
                     else
                     {
                         MessageBox.Show("Неверный пинкод");
                         return;

                     }
                 }
             }
             else
             {
                 MessageBox.Show("Проверка прошла успешно");
             }
            */
        }

        private void btnTestDBLocalRecreate_Click(object sender, EventArgs e)
        {
            RecreateDB(InterfaceDataExchange.Configuration.LocalDataStorageConnectionString);
        }

        private void btnTestDBRemoteRecreate_Click(object sender, EventArgs e)
        {
            RecreateDB(InterfaceDataExchange.Configuration.RemoteDataStorageConnectionString);
        }
        private void RecreateDB(string ConnectionString)
        {
            /*if (!TestDBCheckForConfiguration(ConnectionString)) return;
            if (DoMCLib.Dialogs.DigitalInput.ShowIntegerDialog("Введите пинкод для продолжения", true) == AdminPinCode)
            {
                try
                {
                    var db = new DoMCLib.DB.MSSQLDBStorage(ConnectionString);
                    db.DeleteDB();
                    db.CheckDB(true);
                    DisplayMessage.Show("База данных создана", "Завершено");
                }
                catch (Exception ex)
                {
                    DisplayMessage.Show("Ошибка базы данных: " + ex.Message, "Ошибка");

                }
            }
            else
            {
                DisplayMessage.Show("Неверный пинкод.", "Ошибка");
            }
            */
        }
        private void btnRestoreLocalDB_Click(object sender, EventArgs e)
        {
            RestoreDB(InterfaceDataExchange.Configuration.LocalDataStorageConnectionString);
        }

        private void btnRestoreRemoteDB_Click(object sender, EventArgs e)
        {
            RestoreDB(InterfaceDataExchange.Configuration.RemoteDataStorageConnectionString);
        }
        private void RestoreDB(string ConnectionString)
        {
            /*if (!TestDBCheckForConfiguration(ConnectionString)) return;
            if (DoMCLib.Dialogs.DigitalInput.ShowIntegerDialog("Введите пинкод для продолжения", true) == AdminPinCode)
            {
                try
                {
                    var db = new DoMCLib.DB.MSSQLDBStorage(ConnectionString);
                    db.RestoreDB();
                    DisplayMessage.Show("База данных восстановлена", "Завершено");
                }
                catch (Exception ex)
                {
                    DisplayMessage.Show("Ошибка базы данных: " + ex.Message, "Ошибка");

                }
            }
            else
            {
                DisplayMessage.Show("Неверный пинкод.", "Ошибка");
            }
            */
        }

        private void cbTestCCDMaxPointShow_CheckedChanged(object sender, EventArgs e)
        {
            showMaxDevPoint = cbTestCCDMaxPointShow.Checked;
            ShowSocket(TestSocketNumberSelected);
        }

        private void numFrame_DoubleClick(object sender, EventArgs e)
        {
            var value = DoMCLib.Dialogs.DigitalInput.ShowIntegerDialog("Введите номер линии", false, (int)numFrame.Value);
            if (value >= 0 && value < 512)
            {
                numFrame.Value = value;
            }
        }


        private bool StringToDouble(string str, out double value)
        {
            if (String.IsNullOrWhiteSpace(str))
            {
                value = 0;
                return true;
            }
            var currCulture = System.Globalization.CultureInfo.CurrentCulture;
            var invCulture = System.Globalization.CultureInfo.InvariantCulture;
            if (double.TryParse(str, System.Globalization.NumberStyles.Any, currCulture, out value)
                        ||
                        double.TryParse(str, System.Globalization.NumberStyles.Any, invCulture, out value)
                        )
            {
                return true;
            }
            return false;
        }

        private void SetImpulsesToTextBoxes(TextBox impTxb, TextBox mmTxb, double impulses)
        {
            if (mmTxb != null)
            {
                if ((InterfaceDataExchange?.Configuration?.LCBSettings.LCBKoefficient ?? 0) > 0)
                {
                    mmTxb.Text = (impulses / InterfaceDataExchange.Configuration.LCBSettings.LCBKoefficient).ToString("F4");
                }
            }
            if (impTxb != null)
            {
                impTxb.Text = ((int)impulses).ToString("F0");
            }

        }
        private void SetmmToTextBoxes(TextBox impTxb, TextBox mmTxb, double mm)
        {
            if (impTxb != null)
            {
                if ((InterfaceDataExchange?.Configuration?.LCBSettings.LCBKoefficient ?? 0) > 0)
                {
                    impTxb.Text = ((int)(mm * InterfaceDataExchange.Configuration.LCBSettings.LCBKoefficient)).ToString();
                }
            }
            if (mmTxb != null)
            {
                mmTxb.Text = mm.ToString("F4");
            }
        }
        private void txbTestLCBPreformLength_TextChanged(object sender, EventArgs e)
        {
            var ctrl = sender as TextBox;
            if (ctrl == null) return;
            if (ctrl.Focused || LCBSettingsPreformLengthGotFromConfig)
            {
                LCBSettingsPreformLengthGotFromConfig = false;
                if (StringToDouble(ctrl.Text, out double value))
                {
                    SetImpulsesToTextBoxes(null, txbTestLCBPreformLengthMm, value);
                }
            }
        }

        private void txbTestLCBPreformLengthMm_TextChanged(object sender, EventArgs e)
        {
            var ctrl = sender as TextBox;
            if (ctrl == null) return;
            if (ctrl.Focused)
            {
                if (StringToDouble(ctrl.Text, out double value))
                {
                    SetmmToTextBoxes(txbTestLCBPreformLength, null, value);
                }
            }

        }

        private void txbTestLCBDelayLength_TextChanged(object sender, EventArgs e)
        {
            var ctrl = sender as TextBox;
            if (ctrl == null) return;
            if (ctrl.Focused || LCBSettingsDelayLengthGotFromConfig)
            {
                LCBSettingsDelayLengthGotFromConfig = false;
                if (StringToDouble(ctrl.Text, out double value))
                {
                    SetImpulsesToTextBoxes(null, txbTestLCBDelayLengthMm, value);
                }
            }
        }

        private void txbTestLCBDelayLengthMm_TextChanged(object sender, EventArgs e)
        {
            var ctrl = sender as TextBox;
            if (ctrl == null) return;
            if (ctrl.Focused)
            {
                if (StringToDouble(ctrl.Text, out double value))
                {
                    SetmmToTextBoxes(txbTestLCBDelayLength, null, value);
                }
            }
        }

        private void соответствиеГнездToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var psf = new DoMCLib.Forms.PhysicalSocketsForm();
            if (InterfaceDataExchange.Configuration.DisplaySockets2PhysicalSockets == null || InterfaceDataExchange.Configuration.DisplaySockets2PhysicalSockets.GetSocketQuantity() != Context.Configuration.HardwareSettings.SocketQuantity)
            {
                InterfaceDataExchange.Configuration.DisplaySockets2PhysicalSockets = new DisplaySockets2PhysicalSockets();
                InterfaceDataExchange.Configuration.DisplaySockets2PhysicalSockets.SetDefaultMatrix(Context.Configuration.HardwareSettings.SocketQuantity);
            }
            psf.DisplaySockets2PhysicalSockets = InterfaceDataExchange.Configuration.DisplaySockets2PhysicalSockets;
            if (psf.ShowDialog() == DialogResult.OK)
            {
                InterfaceDataExchange.Configuration.DisplaySockets2PhysicalSockets = psf.DisplaySockets2PhysicalSockets;
                InterfaceDataExchange.Configuration.Save();
                FillSettingPage();
            }
        }

        string ConfigFileDialogExtentions = "Config file (*.DoMCConfig)|*.DoMCConfig";
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var saveDlg = new SaveFileDialog();
            saveDlg.Filter = ConfigFileDialogExtentions;
            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                InterfaceDataExchange.Configuration.Save(saveDlg.FileName);
            }
        }

        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var openDlg = new OpenFileDialog();
            openDlg.Filter = ConfigFileDialogExtentions;
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    InterfaceDataExchange.Configuration.Load(openDlg.FileName);
                    InterfaceDataExchange.Configuration.Save();
                    FillSettingPage();
                    //Refresh();
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void btnSettingsCheckCardStatus_Click(object sender, EventArgs e)
        {
            var socketsToCheckOld = InterfaceDataExchange.Configuration.SocketsToCheck;
            var timeout = InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout;
            InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout = 3000;
            InterfaceDataExchange.Configuration.SocketsToCheck = Enumerable.Repeat(true, 96).ToArray();
            LoadConfiguration(false, true, false, ShowMessages: false);
            InterfaceDataExchange.Configuration.SocketsToCheck = socketsToCheckOld;
            InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout = timeout;
            InterfaceDataExchange.SendCommand(ModuleCommand.CCDGetSocketStatus);
            SettingsPageSocketsStatusShow = SettingsPageSocketsStatus.SocketCardsIsWorking;
            StopCCDWork();
            FillSettingPage();
        }
        private enum SettingsPageSocketsStatus
        {
            IsSocketSettingsOk,
            SocketCardsIsWorking,
        }

        private void btnCheckSettings_Click(object sender, EventArgs e)
        {
            SettingsPageSocketsStatusShow = SettingsPageSocketsStatus.IsSocketSettingsOk;
            FillSettingPage();
        }

        private void txbInput_DoubleClick(object sender, EventArgs e)
        {
            var txb = (sender as TextBox);
            string title = "";
            if (txb == txbTestLCBCurrent) title = "тока";
            if (txb == txbTestLCBDelayLength) title = "длина задержка";
            if (txb == txbTestLCBDelayLengthMm) title = "длина задержки в мм";
            if (txb == txbTestLCBPreformLength) title = "длина преформы в имп";
            if (txb == txbTestLCBPreformLengthMm) title = "длина преформы в мм";
            int.TryParse(txb.Text, out int Current);
            var newvalue = DoMCLib.Dialogs.DigitalInput.ShowIntegerDialog($"Ввод значения {title}", false, Current);
            if (newvalue >= 0)
                txb.Text = newvalue.ToString();
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

        bool MovingCyclesToArchive = false;
        private void btnMoveToArchive_Click(object sender, EventArgs e)
        {
            if (MovingCyclesToArchive)
            {
                MovingCyclesToArchive = false;
                InterfaceDataExchange.SendCommand(ModuleCommand.ArchiveDBStop);
                btnMoveToArchive.BackColor = SystemColors.Control;

            }
            else
            {
                MovingCyclesToArchive = true;
                InterfaceDataExchange.SendCommand(ModuleCommand.ArchiveDBStart);
                btnMoveToArchive.BackColor = Color.DarkGreen;
            }
        }

        private void дополнительныеПараметрыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var af = new DoMCLib.Forms.DoMCAdditionalParametersForm();
            af.AverageToHaveImage = InterfaceDataExchange.Configuration.AverageToHaveImage;
            af.LogPackets = InterfaceDataExchange.Configuration.LogPackets;
            af.RegisterEmptyImages = InterfaceDataExchange.Configuration.RegisterEmptyImages;
            if (af.ShowDialog() == DialogResult.OK)
            {
                InterfaceDataExchange.Configuration.AverageToHaveImage = af.AverageToHaveImage;
                InterfaceDataExchange.Configuration.LogPackets = af.LogPackets;
                InterfaceDataExchange.Configuration.RegisterEmptyImages = af.RegisterEmptyImages;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tbArchive)
            {
                LoadArchiveTab();
            }
        }
        private void LoadArchiveTab()
        {
            if (archiveForm != null) return;
            tbArchive.Font = new Font(tbArchive.Font.FontFamily, 8f);
            tbArchive.Scale(new SizeF(1, 1));
            archiveForm = new DoMCArchiveForm(Controller, Context.Configuration.HardwareSettings.LocalDataStoragePath, Context.Configuration.HardwareSettings.RemoteDataStoragePath);
            archiveForm.Visible = true;
            archiveForm.TopLevel = false;
            archiveForm.Parent = tbArchive;
            archiveForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            archiveForm.StartPosition = FormStartPosition.Manual;
            archiveForm.Location = new Point(0, 0);
            var size = new Size(tbArchive.ClientSize.Width, tbArchive.ClientSize.Height);
            archiveForm.Size = size;
        }

        private void tsmiReadImageStatistics_Click(object sender, EventArgs e)
        {
            /*var statFrom = new ImageReadBytesStatiscticsForm(InterfaceDataExchange);
            statFrom.Show();*/
        }

        private void btnLCBSaveToConfig_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txbTestLCBPreformLength.Text, out int preformLength))
            {
                MessageBox.Show("Значение длины преформы в импульсах должно быть целым числом");
                txbTestLCBPreformLength.Focus();
                return;
            }
            if (!int.TryParse(txbTestLCBDelayLength.Text, out int delayLength))
            {
                MessageBox.Show("Значение расстояния задержки в импульсах должно быть целым числом");
                txbTestLCBDelayLength.Focus();
                return;
            }
            if (!int.TryParse(txbTestLCBCurrent.Text, out int current))
            {
                MessageBox.Show("Значение тока должно быть целым числом");
                txbTestLCBCurrent.Focus();
                return;
            }
            InterfaceDataExchange.Configuration.LCBSettings.LEDCurrent = current;
            InterfaceDataExchange.Configuration.LCBSettings.PreformLength = preformLength;
            InterfaceDataExchange.Configuration.LCBSettings.DelayLength = delayLength;
        }

        private void btnLCBLoadFromConfig_Click(object sender, EventArgs e)
        {

            LCBSettingsPreformLengthGotFromConfig = true;
            LCBSettingsDelayLengthGotFromConfig = true;
            txbTestLCBCurrent.Text = InterfaceDataExchange.Configuration.LCBSettings.LEDCurrent.ToString();
            txbTestLCBPreformLength.Text = InterfaceDataExchange.Configuration.LCBSettings.PreformLength.ToString();
            txbTestLCBDelayLength.Text = InterfaceDataExchange.Configuration.LCBSettings.DelayLength.ToString();

        }
    }


}
