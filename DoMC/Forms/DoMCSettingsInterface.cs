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
using DoMC.Forms;
using DoMCModuleControl.UI;
using DoMC.UserControls;
using DoMCLib.Forms;

namespace DoMC
{
    public partial class DoMCSettingsInterface : Form, IDoMCSettingsUpdatedProvider//, IMainUserInterface
    {

        DoMCLib.Forms.ShowPreformImages checkpreformalgorithmsForm;

        bool invertColors = false;
        //bool IsAbleToWork = false;
        bool TestRDPBConnected = false;
        int AdminPinCode = 1234;
        bool LCBSettingsPreformLengthGotFromConfig = false;
        bool LCBSettingsDelayLengthGotFromConfig = false;

        IMainController Controller;
        ILogger WorkingLog;
        Observer observer;
        DoMCLib.Classes.DoMCApplicationContext Context;

        SettingsPageSocketsStatus SettingsPageSocketsStatusShow = SettingsPageSocketsStatus.IsSocketSettingsOk;

        Bitmap bmpCheckSign = new Bitmap(512, 512);
        DoMCArchiveForm archiveForm = null;

        //ModelCommandProcessor contextProcessor;

        TestCCDControl TestCCDInterfaceView;
        GetCCDStandardInterface GetCCDStandardInterface;
        TestLCBInterface TestLCBInterface;
        CheckSettings CheckSettingsInterface;
        TestRDPBControl TestRDPBControlInterface;
        bool MovingCyclesToArchive = false;

        public event EventHandler SettingsUpdated;

        public DoMCSettingsInterface()
        {
            InitializeComponent();

        }

        public void SetMainController(IMainController controller, object? data)
        {
            Context = (DoMCLib.Classes.DoMCApplicationContext)data;
            Controller = controller;
            WorkingLog = controller.GetLogger("SettingInterface");
            observer = controller.GetObserver();
            //Context = context;
            //contextProcessor = new ModelCommandProcessor(controller, Context);
            WorkingLog.Add(LoggerLevel.Critical, "Запуск интерфейса настройки");
            InitControls();
            observer.NotificationReceivers += Observer_NotificationReceivers;
        }

        private void Observer_NotificationReceivers(string EventName, object? arg2)
        {
            if (EventName == DoMCApplicationContext.ConfigurationUpdateEventName)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => PreparationsAfterChangingConfig()));
                }
                else
                {
                    PreparationsAfterChangingConfig();
                }
            }
        }
        private void PreparationsAfterChangingConfig()
        {
            Context.FillEquipmentSocket2CardSocket();
            FillSettingPage();

        }
        private void NotifyConfigurationUpdated()
        {
            observer.Notify(DoMCApplicationContext.ConfigurationUpdateEventName, Context.Configuration);
        }

        private void InitControls()
        {
            WorkingLog.Add(LoggerLevel.Critical, "SetFormSchema");
            SetFormSchema();
            WorkingLog.Add(LoggerLevel.Critical, "FillPage");
            FillSettingPage();
            IsCycleStarted = false;

            try
            {
                checkpreformalgorithmsForm = new DoMCLib.Forms.ShowPreformImages();
                checkpreformalgorithmsForm.SetObserver(observer);
                //checkpreformalgorithmsForm.WindowState = FormWindowState.Maximized;
                checkpreformalgorithmsForm.TopLevel = false;
                checkpreformalgorithmsForm.Parent = tbShowPreformImages;
                checkpreformalgorithmsForm.Visible = true;
                checkpreformalgorithmsForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;


                var k = 0.8f;
                var screenpoint = tbShowPreformImages.PointToScreen(new Point(0, 0));
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

            TestCCDInterfaceView = new TestCCDControl(Controller, WorkingLog, this);
            tbCCDTest.Controls.Add(TestCCDInterfaceView);
            TestCCDInterfaceView.Size = tbCCDTest.ClientSize;

            GetCCDStandardInterface = new GetCCDStandardInterface(Controller, WorkingLog, this);
            tbGetStandard.Controls.Add(GetCCDStandardInterface);
            GetCCDStandardInterface.Size = tbCCDTest.ClientSize;

            TestLCBInterface = new TestLCBInterface(Controller, WorkingLog, this);
            tbTestLCB.Controls.Add(TestLCBInterface);
            TestLCBInterface.Size = tbTestLCB.ClientSize;


            CheckSettingsInterface = new CheckSettings(Controller, WorkingLog, this);
            tbSettingsCheck.Controls.Add(CheckSettingsInterface);
            CheckSettingsInterface.Size = tbSettingsCheck.ClientSize;

            TestRDPBControlInterface = new TestRDPBControl(Controller, WorkingLog, this);
            tbTestRDPB_uc.Controls.Add(TestRDPBControlInterface);
            TestRDPBControlInterface.Size = tbTestRDPB_uc.ClientSize;


            FillSettingPage();
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
            SettingsUpdated?.Invoke(this, new EventArgs());
            /*var q = Context.Configuration.HardwareSettings.SocketQuantity;
            //var q = Context.Configuration.HardwareSettings.SocketQuantity;
            lvDoMCCards.Items.Clear();
            var cardsWorking = Context.GetIsCardsWorking();
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
            }
            
            SettingsSocketsPanelList = UserInterfaceControls.CreateSocketStatusPanels(Context.Configuration.HardwareSettings.SocketQuantity, ref pnlSockets);
            //UserInterfaceControls.SetSocketStatuses(SettingsSocketsPanelList, UserInterfaceControls.GetListOfSetSocketConfiguration(Context.Configuration.HardwareSettings.SocketQuantity, Context.Configuration.SocketToCardSocketConfigurations), Color.Green, Color.DarkGray);
            switch (SettingsPageSocketsStatusShow)
            {
                case SettingsPageSocketsStatus.IsSocketSettingsOk:
                    UserInterfaceControls.SetSocketStatuses(SettingsSocketsPanelList, UserInterfaceControls.GetListOfSetSocketConfiguration(Configuration.SocketQuantity, Configuration.SocketToCardSocketConfigurations), Color.Green, Color.DarkGray);
                    break;
                case SettingsPageSocketsStatus.SocketCardsIsWorking:
                    UserInterfaceControls.SetSocketStatuses(SettingsSocketsPanelList, Context.GetIsCardWorking(), Color.Green, Color.OrangeRed);
                    break;
            }
            StandardSettingsSocketsPanelList = UserInterfaceControls.CreateSocketStatusPanels(Context.Configuration.HardwareSettings.SocketQuantity, ref pnlGetStandardSockets, ChooseSocket_Click);
            //UserInterfaceControls.SetSocketStatuses(StandardSettingsSocketsPanelList, Context.Configuration.SocketToCardSocketConfigurations.Select(s => s.Value.StandardImage != null).ToArray(), Color.Green, Color.DarkGray);

            WorkModeStandardSettingsSocketsPanelList = UserInterfaceControls.CreateSocketStatusPanels(Configuration.SocketQuantity, ref pnlWorkSockets, GetWorkModeStandard_Click);
            UserInterfaceControls.SetSocketStatuses(WorkModeStandardSettingsSocketsPanelList, UserInterfaceControls.GetListOfSetSocketConfiguration(Configuration.SocketQuantity, Configuration.SocketToCardSocketConfigurations), Color.Green, Color.DarkGray);

            //SetTestCCDSocketStatuses();
            CreateTestCCDPanelSocketStatuses();
            SetTestCCDPanelSocketStatuses();

            SetCheckedMenuItems();
            */
            miLEDSettings.Checked = Context.Configuration.ReadingSocketsSettings.IsLCBSettingsSet();
            miReadParameters.Checked = Context.Configuration.ReadingSocketsSettings.IsReadingParametersSet();
            miStandardRecalcSetting.Checked = Context.Configuration.HardwareSettings.IsStandardRecalculationSettingsSet();
            miSetCheckSockets.Checked = Context.Configuration.HardwareSettings.SocketsToCheck != null && Context.Configuration.HardwareSettings.SocketsToCheck.Any(s => s);
            miDBSettings.Checked = Context.Configuration.HardwareSettings.IsDBSettingsSet();
            miRDPSettings.Checked = Context.Configuration.HardwareSettings.IsRemoveDefectedPreformBlockConfigSet();
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


        /*private void StopCCDWork()
        {
            Context.SendCommand(ModuleCommand.CCDStop);
        }*/


        private void SetCheckedMenuItems()
        {
            /*if (Context.Configuration != null)
            {
                if (Context.Configuration.SocketToCardSocketConfigurations != null)
                {
                    if (Context.Configuration.SocketToCardSocketConfigurations.Keys.Count == Context.Configuration.HardwareSettings.SocketQuantity)
                        miReadParameters.Checked = true;
                    else
                        miReadParameters.Checked = false;
                }

                if (Context.Configuration.WorkModeSettings != null)
                {
                    var k = Context.Configuration.WorkModeSettings.Koefficient;
                    if (k == 0 || double.IsNaN(k) || double.IsInfinity(k))
                    {
                        miStandardRecalcSetting.Checked = false;
                    }
                    else
                    {
                        miStandardRecalcSetting.Checked = true;
                    }
                }
                else
                {
                    miStandardRecalcSetting.Checked = false;

                }
                if (Context.Configuration.CardSocket2EquipmentSocket != null)
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




        int TempStandardSocketNumber = -1;
        private void GetStandard_Click(object sender, EventArgs e)
        {
            /*CheckForDoMCModule();
            var ctrl = (Control)sender;
            var socketnumber = (int)ctrl.Tag;
            ChosenSocketNumber = socketnumber;
            lblStandardSocketNumber.Text = ChosenSocketNumber.ToString();

            if (!Context.Configuration.SocketToCardSocketConfigurations.ContainsKey(ChosenSocketNumber))
            {
                DoMCSocketIsNotConfiguredErrorMessage();
                return;
            }
            var socket = Context.Configuration.SocketToCardSocketConfigurations[ChosenSocketNumber];

            var img = socket.StandardImage;
            socket.TempAverageImage = img;

            var msbmp = ImageTools.DrawImage(img, invertColors);
            pbAverage.Image = msbmp;
            Context.Configuration.Save();
            */
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            /*if (InterfaceDataExchange == null) return;
            string text;
            switch (Context.CCDDataEchangeStatuses.ModuleStatus)
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
                default: text = Context.CCDDataEchangeStatuses.ModuleStatus.ToString(); break;
            }
            var stepText = "";
            switch (Context.CCDDataEchangeStatuses.ModuleStep)
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
            switch (Context.OperationOverallStatus)
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
                    overallText = Context.OperationOverallStatus.ToString();
                    break;
            }*/
            /*var fulltext = $"{text}: {stepText} ({overallText})";
            lblStatus.Text = fulltext;
            tslTestCCDCardReadStatus.Text = "Статус: " + fulltext;
            lblGetStandardWorkStatus.Text = fulltext;

            if ((DateTime.Now - Context.LEDStatus.TimeSyncSignalGot).TotalMilliseconds < 700)
                cbTestLCBSyncrosignal.CheckState = CheckState.Indeterminate;
            else
                cbTestLCBSyncrosignal.CheckState = CheckState.Unchecked;

            if (TestRDPBConnected)
            {
                if (Context.RDPBCurrentStatus.IsCurrentStatusActual())
                {
                    txbTestRDPBCoolingBlocksStatus.Text = Context.RDPBCurrentStatus.CoolingBlocksQuantity.ToString();
                }

                if (Context.RDPBCurrentStatus.IsParametersActual())
                {

                }
            }

            if (Context.DataStorage != null && Context.DataStorage.IsMovingReportWorking())
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

            }*/
        }

        private void DoMCMainInterface_FormClosed(object sender, FormClosedEventArgs e)
        {
            /*Context.SendCommand(ModuleCommand.SetLCBNonWorkModeRequest);
            Context.SendCommand(ModuleCommand.LCBStop);
            Context.SendCommand(ModuleCommand.StopModuleWork);*/
            observer.NotificationReceivers -= Observer_NotificationReceivers;
        }

        private void pbStandard1_DoubleClick(object sender, EventArgs e)
        {
            /*if (ChosenSocketNumber < 0) return;
             if (!Context.Configuration.SocketToCardSocketConfigurations.ContainsKey(ChosenSocketNumber))
             {
                 DoMCSocketIsNotConfiguredErrorMessage();
                 return;
             }
             var socket = Context.Configuration.SocketToCardSocketConfigurations[ChosenSocketNumber];
             if (socket.TempImages == null) return;
             var sf = new ShowFrameForm();
             sf.Image = socket.TempImages[0];
             sf.Show();
             //pbMainStandard.Image = pbStandard1.Image;
            */
        }

        private void pbStandard2_DoubleClick(object sender, EventArgs e)
        {
            /*if (ChosenSocketNumber < 0) return;
            if (!Context.Configuration.SocketToCardSocketConfigurations.ContainsKey(ChosenSocketNumber))
            {
                DoMCSocketIsNotConfiguredErrorMessage();
                return;
            }
            var socket = Context.Configuration.SocketToCardSocketConfigurations[ChosenSocketNumber];
            if (socket.TempImages == null) return;
            var sf = new ShowFrameForm();
            sf.Image = socket.TempImages[1];
            sf.Show();
            //pbMainStandard.Image = pbStandard2.Image;
            */
        }

        private void pbStandard3_DoubleClick(object sender, EventArgs e)
        {
            /*
            if (ChosenSocketNumber < 0) return;
            if (!Context.Configuration.SocketToCardSocketConfigurations.ContainsKey(ChosenSocketNumber))
            {
                DoMCSocketIsNotConfiguredErrorMessage();
                return;
            }
            var socket = Context.Configuration.SocketToCardSocketConfigurations[ChosenSocketNumber];
            if (socket.TempImages == null) return;
            var sf = new ShowFrameForm();
            sf.Image = socket.TempImages[2];
            sf.Show();
            //pbMainStandard.Image = pbStandard3.Image;
            */
        }

        private void pbAverage_DoubleClick(object sender, EventArgs e)
        {
            /*
            if (ChosenSocketNumber < 0) return;
            if (!Context.Configuration.SocketToCardSocketConfigurations.ContainsKey(ChosenSocketNumber))
            {
                DoMCSocketIsNotConfiguredErrorMessage();
                return;
            }
            var socket = Context.Configuration.SocketToCardSocketConfigurations[ChosenSocketNumber];
            if (socket.TempAverageImage == null) return;
            var sf = new ShowFrameForm();
            sf.Image = socket.TempAverageImage;
            sf.Show();
            //pbMainStandard.Image = pbAverage.Image;
            */
        }

        private void btnMakeAverage_Click(object sender, EventArgs e)
        {
            /*
            if (!Context.Configuration.SocketToCardSocketConfigurations.ContainsKey(ChosenSocketNumber))
            {
                DoMCSocketIsNotConfiguredErrorMessage();
                return;
            }
            var socket = Context.Configuration.SocketToCardSocketConfigurations[ChosenSocketNumber];
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
            Context.Configuration.Save();
            var msbmp = ImageTools.DrawImage(AvgIm, invertColors);
            pbAverage.Image = msbmp;
            FillPage();
            */
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
            /*
            try
            {
                Context.OperationOverallStatus = ModuleCommandStep.Start;
                Context.CardsConnection.PacketLogActive = Context.Configuration.LogPackets;

                CheckForDoMCModule();
                if (ChosenSocketNumber < 0) return;
                if (!Context.Configuration.SocketToCardSocketConfigurations.ContainsKey(ChosenSocketNumber))
                {
                    DoMCSocketIsNotConfiguredErrorMessage();
                    //Context.SendCommand(ModuleCommand.StartIdle);
                    return;
                }
                var socket = Context.Configuration.SocketToCardSocketConfigurations[ChosenSocketNumber];
                socket.DataType = 0;
                if (!LoadConfiguration(false))
                {
                    Context.OperationOverallStatus = ModuleCommandStep.Error;
                    return;
                }
                Context.OperationOverallStatus = ModuleCommandStep.Processing;
                Context.SendResetToCCDCards();
                Thread.Sleep(200);
                PictureBox[] StandardPictures = new PictureBox[3] { pbStandard1, pbStandard2, pbStandard3 };

                //short[][,] StandardImages = new short[3][,];
                socket.TempImages = new short[3][,];
                for (int i = 0; i < StandardPictures.Length; i++) StandardPictures[i].Image = null;
                pbAverage.Image = null;
                Application.DoEvents();


                Context.CCDDataEchangeStatuses.ExternalStart = cbExternalSignalForStandard.Checked;
                Context.CCDDataEchangeStatuses.SocketsToSave = new int[] { ChosenSocketNumber };
                for (int repeat = 0; repeat < 3; repeat++)
                {
                    DataExchangeKernel.Log.Add(new Guid(), $"Начало чтения гнезда", true);
                    bool rc = true;
                    if (Context.CCDDataEchangeStatuses.ExternalStart)
                    {
                        Context.SendCommand(ModuleCommand.StartSeveralSocketReadExternal);
                        rc = UserInterfaceControls.Wait(Context.Configuration.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, () =>
                        {
                            return Context.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartSeveralSocketReadExternal && Context.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Complete;
                        }, null);
                        if (!rc)
                        {
                            Context.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                            Context.OperationOverallStatus = ModuleCommandStep.Error;
                            DoMCNotAbleToReadSocketErrorMessage();
                            //Context.SendCommand(ModuleCommand.StartIdle);
                            return;
                        }

                    }
                    else
                    {
                        Context.SendCommand(ModuleCommand.StartSeveralSocketRead);
                        rc = UserInterfaceControls.Wait(Context.Configuration.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, () =>
                        {
                            return Context.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartSeveralSocketRead && Context.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Complete;
                        }, () => Context.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
                        if (!rc)
                        {
                            Context.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                            Context.OperationOverallStatus = ModuleCommandStep.Error;
                            DoMCNotAbleToReadSocketErrorMessage();
                            //Context.SendCommand(ModuleCommand.StartIdle);

                            return;
                        }
                    }
                    DataExchangeKernel.Log.Add(new Guid(), $"Начало чтения картинки", true);
                    Context.SendCommand(ModuleCommand.GetSeveralSocketImages);
                    var ri = UserInterfaceControls.Wait(1 * Context.Configuration.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, () =>
                    {
                        var mst = Context.CCDDataEchangeStatuses.ModuleStatus;
                        var stp = Context.CCDDataEchangeStatuses.ModuleStep;
                        return mst == ModuleCommand.GetSeveralSocketImages && (stp == ModuleCommandStep.Complete || stp == ModuleCommandStep.Error);
                    }, () => Context.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
                    if (!ri || Context.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Error)
                    {
                        Context.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                        Context.OperationOverallStatus = ModuleCommandStep.Error;
                        DoMCNotAbleToReadSocketErrorMessage();
                        //Context.SendCommand(ModuleCommand.StartIdle);
                        return;
                    }
                    DataExchangeKernel.Log.Add(new Guid(), $"Картинка получена", true);
                    var images = Context.CCDDataEchangeStatuses.Images;
                    if (images is null)
                    {
                        Context.OperationOverallStatus = ModuleCommandStep.Error;
                        DoMCNotAbleToReadSocketErrorMessage();
                        break;
                    }
                    socket.TempImages[repeat] = images[ChosenSocketNumber - 1];
                    var sbmp = ImageTools.DrawImage(images[ChosenSocketNumber - 1], invertColors);
                    StandardPictures[repeat].Image = sbmp;

                    //Application.DoEvents();
                }
                Context.CCDDataEchangeStatuses.ModuleStatus = ModuleCommand.StartIdle;


                var dev = ImageTools.CalculateDeviationFull(socket.TempImages, socket.ImageProcessParameters.GetRectangle());
                socket.TempDeviations = dev;
                this.Invoke((MethodInvoker)delegate { ImagesForStandardSetLabelValues(); });
                Context.OperationOverallStatus = ModuleCommandStep.Complete;

            }
            finally
            {
                StandardIsReading = false;
                StopCCDWork();
            }
            */
        }
        /*
        private void ImagesForStandardSetLabelValues()
        {
            try
            {
                var socket = Context.Configuration.SocketToCardSocketConfigurations[ChosenSocketNumber];
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
        */
        private void nudStandardLevel_ValueChanged(object sender, EventArgs e)
        {
            /*
            if (ChosenSocketNumber < 0) return;
            if (!Context.Configuration.SocketToCardSocketConfigurations.ContainsKey(ChosenSocketNumber))
            {
                DoMCSocketIsNotConfiguredErrorMessage();
                return;
            }
            var socket = Context.Configuration.SocketToCardSocketConfigurations[ChosenSocketNumber];
            var lvl = (double)nudStandardLevel.Value / 100.0d;
            var avg = socket.TempDeviations.TotalAverage;
            var s = socket.TempDeviations.TotalDeviation;
            lblRange.Text = $"{avg - s * lvl:F3} - {avg + s * lvl:F3}";
            */
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            /*
            var sf = new ShowFrameForm();
            sf.Image = testImage;
            sf.Show();
            */
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
            /*
            try
            {
                Context.OperationOverallStatus = ModuleCommandStep.Start;
                WorkingLog.Add(LoggerLevel.Information, "--------------- Получение эталонов по всем гнездам ---------------");
                Context.CardsConnection.PacketLogActive = Context.Configuration.LogPackets;

                CheckForDoMCModule();
                //var socket = Configuration.SocketToCardSocketConfigurations[ChosenSocketNumber];
                foreach (var socket in Context.Configuration.SocketToCardSocketConfigurations)
                {
                    socket.Value.DataType = 0;
                    socket.Value.TempImages = new short[3][,];
                }
                WorkingLog.Add(LoggerLevel.Information, "Начало загрузки конфигурации");
                if (!LoadConfiguration(false, true, false)) return;
                Context.OperationOverallStatus = ModuleCommandStep.Processing;
                WorkingLog.Add(LoggerLevel.Information, "Сброс гнезд");
                Context.SendResetToCCDCards();
                Thread.Sleep(200);

                PictureBox[] StandardPictures = new PictureBox[3] { pbStandard1, pbStandard2, pbStandard3 };

                //short[][,] StandardImages = new short[3][,];
                for (int i = 0; i < StandardPictures.Length; i++) StandardPictures[i].Image = null;
                pbAverage.Image = null;


                Context.CCDDataEchangeStatuses.ExternalStart = cbExternalSignalForStandard.Checked;
                //Context.SocketsToSave = new int[] { ChosenSocketNumber };
                for (int repeat = 0; repeat < 3; repeat++)
                {
                    DataExchangeKernel.Log.Add(new Guid(), $"Начало чтения гнезда", true);
                    WorkingLog.Add(LoggerLevel.Information, "Начало чтения гнезда");
                    bool rc = true;
                    if (Context.CCDDataEchangeStatuses.ExternalStart)
                    {
                        WorkingLog.Add(LoggerLevel.Information, "Запуск чтения по внешнему сигналу");
                        Context.SendCommand(ModuleCommand.StartReadExternal);
                        rc = UserInterfaceControls.Wait(Context.Configuration.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, () =>
                        {
                            return Context.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartReadExternal && Context.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Complete;
                        }, () => Context.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
                        if (!rc)
                        {
                            Context.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                            Context.OperationOverallStatus = ModuleCommandStep.Error;
                            DoMCNotAbleToReadSocketErrorMessage();
                            return;
                        }
                    }
                    else
                    {
                        WorkingLog.Add(LoggerLevel.Information, "Запуск чтения");
                        Context.SendCommand(ModuleCommand.StartRead);
                        rc = UserInterfaceControls.Wait(Context.Configuration.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, () =>
                        {
                            return Context.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartRead && Context.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Complete;
                        }, () => Context.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
                        if (!rc)
                        {
                            Context.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                            Context.OperationOverallStatus = ModuleCommandStep.Error;
                            DoMCNotAbleToReadSocketErrorMessage();
                            return;
                        }
                    }
                    DataExchangeKernel.Log.Add(new Guid(), $"Начало чтения картинки", true);
                    WorkingLog.Add("Получение изображения");
                    Context.SendCommand(ModuleCommand.GetSocketImages);
                    var ri = UserInterfaceControls.Wait(2 * Context.Configuration.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, () =>
                    {
                        var mst = Context.CCDDataEchangeStatuses.ModuleStatus;
                        var stp = Context.CCDDataEchangeStatuses.ModuleStep;
                        return mst == ModuleCommand.GetSocketImages && (stp == ModuleCommandStep.Complete || stp == ModuleCommandStep.Error);
                    }, () => Context.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
                    if (!ri || Context.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Error)
                    {
                        Context.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                        Context.OperationOverallStatus = ModuleCommandStep.Error;
                        DoMCNotAbleToReadSocketErrorMessage();
                        return;
                    }
                    DataExchangeKernel.Log.Add(new Guid(), $"Изображение получено", true);
                    WorkingLog.Add("Изображение получено");


                    var images = Context.CCDDataEchangeStatuses.Images;
                    if (images is null)
                    {
                        Context.OperationOverallStatus = ModuleCommandStep.Error;
                        DoMCNotAbleToReadSocketErrorMessage();
                        break;
                    }
                    for (int i = 0; i < Context.Configuration.HardwareSettings.SocketQuantity; i++)
                    {
                        var socket = Context.Configuration.SocketToCardSocketConfigurations[i + 1];
                        socket.TempImages[repeat] = images[i];
                    }
                    StandardPictures[repeat].Image = bmpCheckSign;
                }
                for (int s = 0; s < Context.Configuration.HardwareSettings.SocketQuantity; s++)
                {
                    var socket = Context.Configuration.SocketToCardSocketConfigurations[s + 1];

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
                Context.Configuration.Save();
                WorkingLog.Add("Отключение от плат ПЗС");
                StopCCDWork();
                pbAverage.Image = bmpCheckSign;
                FillPage();
                DisplayMessage.Show("Эталоны по всем гнездам созданы.", "Завершено");
                Context.OperationOverallStatus = ModuleCommandStep.Complete;
            }
            finally
            {
                Context.SendCommand(ModuleCommand.CCDStop);
                StandardIsReading = false;
            }
            */
        }

        #region Menu

        private void miReadParameters_Click(object sender, EventArgs e)
        {
            var ssf = new DoMCLib.Forms.DoMCSocketSettingsListForm();
            ssf.SocketQuantity = Context.Configuration.HardwareSettings.SocketQuantity;
            ssf.SocketConfigurations = Context.Configuration.ReadingSocketsSettings.CCDSocketParameters;
            if (ssf.ShowDialog() == DialogResult.OK)
            {
                Context.Configuration.HardwareSettings.SocketQuantity = ssf.SocketQuantity;
                Context.Configuration.ReadingSocketsSettings.CCDSocketParameters = ssf.SocketConfigurations;
                if (Context.Configuration.HardwareSettings.SocketsToCheck == null || Context.Configuration.HardwareSettings.SocketsToCheck.Length != ssf.SocketQuantity)
                {
                    Context.Configuration.HardwareSettings.SocketsToCheck = new bool[ssf.SocketQuantity];
                    for (int i = 0; i < ssf.SocketQuantity; i++) Context.Configuration.HardwareSettings.SocketsToCheck[i] = true;
                }
                Context.Configuration.SaveHardwareSettings();
                Context.Configuration.SaveReadingSocketsSettings();
            }
            //FillSettingPage();
            NotifyConfigurationUpdated();
        }

        private void miSaveStandard_Click(object sender, EventArgs e)
        {

            var dir = System.IO.Path.Combine(Application.StartupPath, ".");// ApplicationCardParameters.StandardsPath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            var sd = new SaveFileDialog();
            sd.InitialDirectory = dir;
            sd.DefaultExt = "std";
            sd.AddExtension = true;
            sd.Filter = "Эталоны (*.std)|*.std|Все файлы (*.*)|*.*";
            if (sd.ShowDialog() == DialogResult.OK)
            {
                Context.Configuration.SaveAll();
            }

        }

        private void miLoadStandard_Click(object sender, EventArgs e)
        {
            /*
            var dir = System.IO.Path.Combine(Application.StartupPath, ApplicationCardParameters.StandardsPath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            var od = new OpenFileDialog();
            od.Filter = "Эталоны (*.std)|*.std|Все файлы (*.*)|*.*";
            od.InitialDirectory = dir;
            od.DefaultExt = "std";
            od.AddExtension = true;
            if (od.ShowDialog() == DialogResult.OK)
            {
                Context.Configuration.LoadStandard(od.FileName);
                FillPage();
            }
            */
        }

        private void miWorkModeSettings_Click(object sender, EventArgs e)
        {

            var gsf = new DoMCLib.Forms.DoMCGeneralSettingsForm();
            gsf.Value = Context.Configuration.HardwareSettings.StandardRecalculationParameters;
            if (gsf.ShowDialog() == DialogResult.OK)
            {
                Context.Configuration.HardwareSettings.StandardRecalculationParameters = gsf.Value;
                Context.Configuration.SaveHardwareSettings();
                NotifyConfigurationUpdated();
                //FillSettingPage();
            }

        }

        private void miLEDSettings_Click(object sender, EventArgs e)
        {

            var ls = new DoMCLib.Forms.LEDSettingsForm();
            ls.Current = Context.Configuration.ReadingSocketsSettings.LCBSettings.LEDCurrent;
            ls.PreformLength = Context.Configuration.ReadingSocketsSettings.LCBSettings.PreformLength;
            ls.DelayLength = Context.Configuration.ReadingSocketsSettings.LCBSettings.DelayLength;
            ls.LCBKoefficient = Context.Configuration.ReadingSocketsSettings.LCBSettings.LCBKoefficient;
            if (ls.ShowDialog() == DialogResult.OK)
            {
                Context.Configuration.ReadingSocketsSettings.LCBSettings.LEDCurrent = ls.Current;
                Context.Configuration.ReadingSocketsSettings.LCBSettings.PreformLength = ls.PreformLength;
                Context.Configuration.ReadingSocketsSettings.LCBSettings.DelayLength = ls.DelayLength;
                Context.Configuration.ReadingSocketsSettings.LCBSettings.LCBKoefficient = ls.LCBKoefficient;
                Context.Configuration.SaveReadingSocketsSettings();
                NotifyConfigurationUpdated();
                //FillSettingPage();
            }
        }

        #endregion


        private void DoMCMainInterface_Load(object sender, EventArgs e)
        {
            SetFormSchema();

        }

        #region Опрос в цикле
        bool IsCycleStarted;
        private void AbleForCycle(bool InCycle)
        {
            btnCycleStart.Enabled = !InCycle;
            btnCycleStop.Enabled = InCycle;
        }

        private void btnCycleStartStop_Click(object sender, EventArgs e)
        {
            /*
            if (TestCCDIsReading) return;
            TestCCDIsReading = true;
            IsCycleStarted = true;
            AbleForCycle(true);
            AbleForRead(true);
            Context.CCDDataEchangeStatuses.ExternalStart = cbTest_ExternalStart.Checked;
            Context.CCDDataEchangeStatuses.FastRead = true;
            CycleReadingProc();
            IsCycleStarted = false;
            TestCCDIsReading = false;
            */
        }

        private void btnCycleStop_Click(object sender, EventArgs e)
        {
            IsCycleStarted = false;
            //TestCCDIsReading = false;
        }

        private void CycleReadingProc()
        {
            /*
            try
            {
                Context.OperationOverallStatus = ModuleCommandStep.Start;

                if (TestSocketNumberSelected < 1)
                {
                    MessageBox.Show("Нужно выбрать гнездо");
                    TestCCDIsReading = false;
                    return;
                }
                Context.CCDDataEchangeStatuses.SocketsToSave = new int[1] { TestSocketNumberSelected };
                CheckForDoMCModule();
                if (!LoadConfiguration(false, true, false)) //------ загрузить конфигурацию для одного гнезда
                {
                    DoMCNotAbleLoadConfigurationErrorMessage();
                    return;
                }
                //IsAbleToWork = true;

                Context.CurrentCycleCCD = new CycleImagesCCD();
                Context.CurrentCycleCCD.Differences = new short[Context.Configuration.HardwareSettings.SocketQuantity][,];
                Context.CurrentCycleCCD.WorkModeImages = new short[Context.Configuration.HardwareSettings.SocketQuantity][,];
                Context.CurrentCycleCCD.StandardImage = new short[Context.Configuration.HardwareSettings.SocketQuantity][,];
                Context.CurrentCycleCCD.IsSocketGood = new bool[Context.Configuration.HardwareSettings.SocketQuantity];

                //UserInterfaceControls.SetSocketStatuses(WorkModeStandardSettingsSocketsPanelList, new bool[WorkModeStandardSettingsSocketsPanelList.Length], Color.Green, Color.DarkGray);
                //if (!IsAbleToWork) return;


                Context.OperationOverallStatus = ModuleCommandStep.Processing;

                while (IsCycleStarted)
                {
                    Application.DoEvents();
                    bool rc = true;
                    if (Context.CCDDataEchangeStatuses.ExternalStart)
                    {
                        Context.SendCommand(ModuleCommand.StartSeveralSocketReadExternal);
                        rc = UserInterfaceControls.Wait(Context.Configuration.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, () =>
                        {
                            Application.DoEvents();
                            return Context.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartSeveralSocketReadExternal && Context.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Complete;
                        }, () => Context.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
                        if (!rc)
                        {
                            //DoMCNotAbleToReadSocketErrorMessage();
                            //IsWorkingModeStarted = false;
                            Context.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                            Context.OperationOverallStatus = ModuleCommandStep.Error;
                            return;
                        }
                    }
                    else
                    {
                        Context.SendCommand(ModuleCommand.CCDCardsReset);
                        Thread.Sleep(10);
                        Context.SendCommand(ModuleCommand.StartSeveralSocketRead);
                        rc = UserInterfaceControls.Wait(Context.Configuration.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, () =>
                        {
                            Application.DoEvents();
                            return Context.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartSeveralSocketRead && Context.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Complete;
                        }, () => Context.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
                        if (!rc)
                        {
                            //DoMCNotAbleToReadSocketErrorMessage();
                            //IsWorkingModeStarted = false;
                            Context.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                            Context.OperationOverallStatus = ModuleCommandStep.Error;
                            return;
                        }
                    }
                    Context.CurrentCycleCCD.CycleCCDDateTime = DateTime.Now;
                    Context.SendCommand(ModuleCommand.GetSeveralSocketImages);
                    var ri = UserInterfaceControls.Wait(2 * Context.Configuration.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, () =>
                    {
                        Application.DoEvents();

                        var mst = Context.CCDDataEchangeStatuses.ModuleStatus;
                        var stp = Context.CCDDataEchangeStatuses.ModuleStep;
                        return mst == ModuleCommand.GetSeveralSocketImages && (stp == ModuleCommandStep.Complete || stp == ModuleCommandStep.Error);
                    }, () => Context.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
                    if (!ri || Context.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Error)
                    {
                        //DoMCNotAbleToReadSocketErrorMessage();
                        //IsWorkingModeStarted = false;
                        Context.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                        Context.OperationOverallStatus = ModuleCommandStep.Error;
                        return;
                    }
                    var images = Context.CCDDataEchangeStatuses.Images;
                    if (images is null)
                    {
                        //DoMCNotAbleToReadSocketErrorMessage();
                        //IsWorkingModeStarted = false;
                        Context.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                        Context.OperationOverallStatus = ModuleCommandStep.Error;
                        return;
                    }
                    Context.CurrentCycleCCD.WorkModeImages = images;

                    Application.DoEvents();

                    for (int i = 0; i < Context.CurrentCycleCCD.WorkModeImages.Length; i++)
                    {
                        var socketnumber = i + 1;
                        if (Context.Configuration.SocketToCardSocketConfigurations.ContainsKey(socketnumber))
                        {
                            var socket = Context.Configuration.SocketToCardSocketConfigurations[socketnumber];
                            Context.CurrentCycleCCD.Differences[i] = ImageTools.GetDifference(Context.CurrentCycleCCD.WorkModeImages[i], Context.Configuration.SocketToCardSocketConfigurations[socketnumber].StandardImage);
                            //var dev = ImageTools.CalculateDeviationFull(new short[][,] { Context.CurrentCycle.Differences[i] }, socket.StartLine, socket.EndLine);
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
            Context.OperationOverallStatus = ModuleCommandStep.Complete;
            */
        }

        #endregion

        private void cbInvertColors_CheckedChanged(object sender, EventArgs e)
        {
            invertColors = cbInvertColors.Checked;
            //ShowSocket(TestSocketNumberSelected);
        }

        private void miSetCheckSockets_Click(object sender, EventArgs e)
        {

            if (Context.Configuration.HardwareSettings.SocketsToCheck != null)
            {
                //if (Context.Configuration.HardwareSettings.SocketsToCheck == null) Context.Configuration.HardwareSettings.SocketsToCheck = new bool[Context.Configuration.HardwareSettings.SocketQuantity];
                var scf = new DoMCSocketOnOffForm(Context);
                scf.SocketIsOn = Context.Configuration.HardwareSettings.SocketsToCheck;
                scf.Text = "Включение проверки данных по гнездам";
                if (scf.ShowDialog() == DialogResult.OK)
                {
                    Context.Configuration.HardwareSettings.SocketsToCheck = scf.SocketIsOn;
                    Context.Configuration.SaveHardwareSettings();
                }
            }

        }

        private void miWorkInterfaceStart_Click(object sender, EventArgs e)
        {
            /*
            TestLCBStop();
            var wmf = new DoMCWorkModeInterface();
            wmf.SetMemoryReference(globalMemory);
            this.Hide();
            wmf.ShowDialog();
            this.Show();
            */
        }

        private void miDBSettings_Click(object sender, EventArgs e)
        {
            if (Context.Configuration != null && Context.Configuration.HardwareSettings != null)
            {
                var dbsf = new Forms.Settings.DoMCDBSettingsForm();
                dbsf.LocalDBConnectionString = Context.Configuration.HardwareSettings.LocalDataStoragePath;
                dbsf.RemoteDBConnectionString = Context.Configuration.HardwareSettings.RemoteDataStoragePath;
                dbsf.DelayBeforeMoveDataToArchive = Context.Configuration.HardwareSettings.Timeouts.DelayBeforeMoveDataToArchiveTimeInSeconds;
                if (dbsf.ShowDialog() == DialogResult.OK)
                {
                    Context.Configuration.HardwareSettings.LocalDataStoragePath = dbsf.LocalDBConnectionString;
                    Context.Configuration.HardwareSettings.RemoteDataStoragePath = dbsf.RemoteDBConnectionString;
                    Context.Configuration.HardwareSettings.Timeouts.DelayBeforeMoveDataToArchiveTimeInSeconds = dbsf.DelayBeforeMoveDataToArchive;
                    Context.Configuration.SaveHardwareSettings();
                    NotifyConfigurationUpdated();
                    //FillSettingPage();
                }
            }

        }


        private void miRDPSettings_Click(object sender, EventArgs e)
        {

            if (Context != null && Context.Configuration != null)
            {
                var rdpsf = new RDPSettingsForm();
                rdpsf.IPPort = Context.Configuration.HardwareSettings.RemoveDefectedPreformBlockConfig.Port;
                rdpsf.IPAddress = IPAddress.Parse(String.IsNullOrWhiteSpace(Context.Configuration.HardwareSettings.RemoveDefectedPreformBlockConfig.IP) ? "0.0.0.0" : Context.Configuration.HardwareSettings.RemoveDefectedPreformBlockConfig.IP);
                rdpsf.CoolingBlocks = Context.Configuration.HardwareSettings.RemoveDefectedPreformBlockConfig.CoolingBlocksQuantity;
                rdpsf.MachineNumber = Context.Configuration.HardwareSettings.RemoveDefectedPreformBlockConfig.MachineNumber;
                rdpsf.SendBadCycleToRDPB = Context.Configuration.HardwareSettings.RemoveDefectedPreformBlockConfig.SendBadCycleToRDPB;
                rdpsf.Text = "Параметры бракера";
                if (rdpsf.ShowDialog() == DialogResult.OK)
                {
                    Context.Configuration.HardwareSettings.RemoveDefectedPreformBlockConfig.Port = rdpsf.IPPort;
                    Context.Configuration.HardwareSettings.RemoveDefectedPreformBlockConfig.IP = rdpsf.IPAddress.ToString();
                    Context.Configuration.HardwareSettings.RemoveDefectedPreformBlockConfig.CoolingBlocksQuantity = rdpsf.CoolingBlocks;
                    Context.Configuration.HardwareSettings.RemoveDefectedPreformBlockConfig.MachineNumber = rdpsf.MachineNumber;
                    Context.Configuration.HardwareSettings.RemoveDefectedPreformBlockConfig.SendBadCycleToRDPB = rdpsf.SendBadCycleToRDPB;
                    Context.Configuration.SaveHardwareSettings();
                    NotifyConfigurationUpdated();
                    //FillSettingPage();
                }
            }

        }

        private void TestRDPBStop()
        {
            /*
            Context.SendCommand(ModuleCommand.RDPBStop);
            if (!Context.RDPBCurrentStatus.IsStarted)
            {
                TestRDPBConnected = false;
                btnRDPBTestConnect.BackColor = SystemColors.Control;
                btnRDPBTestConnect.Text = "Подключить";
            }
            */
        }
        private void TestRDPBStart()
        {
            /*
            Context.SendCommand(ModuleCommand.RDPBStart);
            if (Context.RDPBCurrentStatus.IsStarted)
            {
                TestRDPBConnected = true;
                btnRDPBTestConnect.BackColor = Color.Green;
                btnRDPBTestConnect.Text = "Отключить";
            }
            */
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
            /*
            if (!TestRDPBConnected) return;
            this.Enabled = false;
            Context.SendCommand(ModuleCommand.RDPBSetIsOK);
            var res = UserInterfaceControls.Wait(Context.Configuration.Timeouts.WaitForRDPBCardAnswerTimeoutInSeconds, () => Context.RDPBCurrentStatus.SentCommandType == DoMCLib.Classes.RDPBCommandType.SetIsOK && Context.RDPBCurrentStatus.IsCurrentStatusActual());
            if (!res)
            {
                btnRDPBTestConnect.BackColor = Color.Red;
                TestRDPBConnected = false;
                TestRDPBStop();
            }
            TestRDPBStatusFill();
            this.Enabled = true;
            */
        }

        private void btnTestRDPBN81_Click(object sender, EventArgs e)
        {
            /*
            //if (!TestRDPBConnected) return;
            this.Enabled = false;
            Context.SendCommand(ModuleCommand.RDPBSetIsBad);
            var res = UserInterfaceControls.Wait(Context.Configuration.Timeouts.WaitForRDPBCardAnswerTimeoutInSeconds, () => Context.RDPBCurrentStatus.SentCommandType == DoMCLib.Classes.RDPBCommandType.SetIsBad && Context.RDPBCurrentStatus.IsCurrentStatusActual());
            if (!res)
            {
                btnRDPBTestConnect.BackColor = Color.Red;
                TestRDPBConnected = false;
                TestRDPBStop();
            }
            TestRDPBStatusFill();
            this.Enabled = true;
            */
        }

        private void btnTestRDPBN82_Click(object sender, EventArgs e)
        {
            /*
            if (!TestRDPBConnected) return;
            Context.SendCommand(ModuleCommand.RDPBOn);
            var res = UserInterfaceControls.Wait(Context.Configuration.Timeouts.WaitForRDPBCardAnswerTimeoutInSeconds, () => Context.RDPBCurrentStatus.SentCommandType == DoMCLib.Classes.RDPBCommandType.On);
            if (!res)
            {
                btnRDPBTestConnect.BackColor = Color.Red;
                TestRDPBConnected = false;
                TestRDPBStop();
            }
            TestRDPBStatusFill();
            this.Enabled = true;
            */
        }

        private void btnTestRDPBN83_Click(object sender, EventArgs e)
        {
            /*
            if (!TestRDPBConnected) return;
            this.Enabled = false;
            Context.SendCommand(ModuleCommand.RDPBOff);
            var res = UserInterfaceControls.Wait(Context.Configuration.Timeouts.WaitForRDPBCardAnswerTimeoutInSeconds, () => Context.RDPBCurrentStatus.SentCommandType == DoMCLib.Classes.RDPBCommandType.Off);
            if (!res)
            {
                btnRDPBTestConnect.BackColor = Color.Red;
                TestRDPBConnected = false;
                TestRDPBStop();
            }
            TestRDPBStatusFill();
            this.Enabled = true;
            */
        }

        private void btnTestRDPBN90_Click(object sender, EventArgs e)
        {
            /*
            if (!TestRDPBConnected) return;
            this.Enabled = false;
            Context.SendCommand(ModuleCommand.RDPBGetParameters);
            var res = UserInterfaceControls.Wait(Context.Configuration.Timeouts.WaitForRDPBCardAnswerTimeoutInSeconds, () => Context.RDPBCurrentStatus.SentCommandType == DoMCLib.Classes.RDPBCommandType.GetParameters && Context.RDPBCurrentStatus.IsParametersActual());
            if (!res)
            {
                btnRDPBTestConnect.BackColor = Color.Red;
                TestRDPBConnected = false;
                TestRDPBStop();
            }
            TestRDPBStatusFill();
            this.Enabled = true;
            */
        }

        private void btnTestRDPBSendCommand_Click(object sender, EventArgs e)
        {
            /*
            if (!TestRDPBConnected) return;
            Context.RDPBCurrentStatus.ManualCommand = txbTestRDPBManualCommand.Text;
            Context.SendCommand(ModuleCommand.RDPBSendManualCommand);
            Thread.Sleep(Context.Configuration.Timeouts.WaitForRDPBCardAnswerTimeoutInSeconds / 10);
            TestRDPBStatusFill();
            */
        }

        private void cbTestRDPBCoolingBlocksQuantity_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            if (!TestRDPBConnected) return;
            Context.RDPBCurrentStatus.CoolingBlocksQuantityToSet = int.Parse(cbTestRDPBCoolingBlocksQuantity.SelectedItem?.ToString() ?? "0");
            Context.SendCommand(ModuleCommand.RDPBSetCoolingBlockQuantity);
            UserInterfaceControls.Wait(Context.Configuration.Timeouts.WaitForRDPBCardAnswerTimeoutInSeconds, () => Context.RDPBCurrentStatus.SentCommandType == DoMCLib.Classes.RDPBCommandType.SetCoolingBlocks && Context.RDPBCurrentStatus.IsParametersActual());
            TestRDPBStatusFill();
            */
        }

        private void TestRDPBStatusFill()
        {
            /*
            lvTestRDPBStatuses.Items.Clear();
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[]{"Короб",
                Context.RDPBCurrentStatus.BoxDirection == DoMCLib.Classes.BoxDirectionType.Left ? "Левый" :
                Context.RDPBCurrentStatus.BoxDirection == DoMCLib.Classes.BoxDirectionType.Right ? "Правый" :
                "Неизвестно"
            }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[] { "Номер короба", Context.RDPBCurrentStatus.BoxNumber.ToString() }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[] { "Бракер ", (Context.RDPBCurrentStatus.BlockIsOn ? "Включен" : "Выключен") }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[] { "Охлаждающих блоков", Context.RDPBCurrentStatus.CoolingBlocksQuantity.ToString() }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[] { "Номер цикла", Context.RDPBCurrentStatus.CycleNumber.ToString() }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[] { "Забракованных съемов", Context.RDPBCurrentStatus.BadSetQuantityInBox.ToString() }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[] { "Хороших съемов", Context.RDPBCurrentStatus.GoodSetQuantityInBox.ToString() }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[] { "Съемов в коробе", Context.RDPBCurrentStatus.SetQuantityInBox.ToString() }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[]{"Направление ленты",
                Context.RDPBCurrentStatus.TransporterSide == DoMCLib.Classes.RDPBTransporterSide.Stoped ? "Стоит" :
                Context.RDPBCurrentStatus.TransporterSide == DoMCLib.Classes.RDPBTransporterSide.Left ? "Влево" :
                Context.RDPBCurrentStatus.TransporterSide == DoMCLib.Classes.RDPBTransporterSide.Right ? "Вправо" :
                "Ошибка датчика"
            }));
            lvTestRDPBStatuses.Items.Add(new ListViewItem(new string[]{"Ошибки",
                Context.RDPBCurrentStatus.Errors == DoMCLib.Classes.RDPBErrors.NoErrors ? "Ошибок нет" :
                Context.RDPBCurrentStatus.Errors == DoMCLib.Classes.RDPBErrors.TransporterDriveUnit ? "Авария привода конвейера" :
                Context.RDPBCurrentStatus.Errors == DoMCLib.Classes.RDPBErrors.SensorOfInitialState ? "Авария датчика исходного состояния" :
                "Неизвестная ошибка"
            }));
            txbTestRDPBCoolingBlocksStatus.Text = Context.RDPBCurrentStatus.CoolingBlocksQuantity.ToString();
            */
        }

        private void btnTestDBLocal_Click(object sender, EventArgs e)
        {
            //TestDB(Context.Configuration.LocalDataStorageConnectionString);
        }
        private void btnTestDBRemote_Click(object sender, EventArgs e)
        {
            //TestDB(Context.Configuration.RemoteDataStorageConnectionString);
        }
        private bool TestDBCheckForConfiguration(string ConnectionString)
        {
            /*
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
            */
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
            //RecreateDB(Context.Configuration.LocalDataStorageConnectionString);
        }

        private void btnTestDBRemoteRecreate_Click(object sender, EventArgs e)
        {
            //RecreateDB(Context.Configuration.RemoteDataStorageConnectionString);
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
            //RestoreDB(Context.Configuration.LocalDataStorageConnectionString);
        }

        private void btnRestoreRemoteDB_Click(object sender, EventArgs e)
        {
            //RestoreDB(Context.Configuration.RemoteDataStorageConnectionString);
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
            //showMaxDevPoint = cbTestCCDMaxPointShow.Checked;
            //ShowSocket(TestSocketNumberSelected);
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

            /*if (mmTxb != null)
            {
                if ((InterfaceDataExchange?.Configuration?.LCBSettings.LCBKoefficient ?? 0) > 0)
                {
                    mmTxb.Text = (impulses / Context.Configuration.LCBSettings.LCBKoefficient).ToString("F4");
                }
            }
            if (impTxb != null)
            {
                impTxb.Text = ((int)impulses).ToString("F0");
            }
            */
        }
        private void SetmmToTextBoxes(TextBox impTxb, TextBox mmTxb, double mm)
        {
            /*
            if (impTxb != null)
            {
                if ((InterfaceDataExchange?.Configuration?.LCBSettings.LCBKoefficient ?? 0) > 0)
                {
                    impTxb.Text = ((int)(mm * Context.Configuration.LCBSettings.LCBKoefficient)).ToString();
                }
            }
            if (mmTxb != null)
            {
                mmTxb.Text = mm.ToString("F4");
            }
            */
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
            if (Context.Configuration.HardwareSettings.CardSocket2EquipmentSocket == null || Context.Configuration.HardwareSettings.CardSocket2EquipmentSocket.Length != Context.Configuration.HardwareSettings.SocketQuantity)
            {
                Context.Configuration.HardwareSettings.CardSocket2EquipmentSocket = Enumerable.Range(1, 96).ToArray();
            }
            psf.CardSocket2EquipmentSocket = Context.Configuration.HardwareSettings.CardSocket2EquipmentSocket;
            if (psf.ShowDialog() == DialogResult.OK)
            {
                Context.Configuration.HardwareSettings.CardSocket2EquipmentSocket = psf.CardSocket2EquipmentSocket;
                Context.Configuration.SaveHardwareSettings();
                NotifyConfigurationUpdated();
                //FillSettingPage(); 
            }

        }

        string ConfigFileDialogExtentions = "Config file (*.DoMCConfig)|*.DoMCConfig";
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var saveDlg = new SaveFileDialog();
            saveDlg.Filter = ConfigFileDialogExtentions;
            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                Context.Configuration.SaveAll(saveDlg.FileName);
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
                    Context.Configuration.Load(openDlg.FileName);
                    Context.Configuration.SaveAll();
                    NotifyConfigurationUpdated();
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }

        }

        private void btnSettingsCheckCardStatus_Click(object sender, EventArgs e)
        {
            /*
            var socketsToCheckOld = Context.Configuration.SocketsToCheck;
            var timeout = Context.Configuration.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds;
            Context.Configuration.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds = 3000;
            Context.Configuration.SocketsToCheck = Enumerable.Repeat(true, 96).ToArray();
            LoadConfiguration(false, true, false, ShowMessages: false);
            Context.Configuration.SocketsToCheck = socketsToCheckOld;
            Context.Configuration.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds = timeout;
            Context.SendCommand(ModuleCommand.CCDGetSocketStatus);
            SettingsPageSocketsStatusShow = SettingsPageSocketsStatus.SocketCardsIsWorking;
            StopCCDWork();
            FillPage();
            */
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
            //FileAndDirectoryTools.OpenNotepad(Log.GetLogFileName(Log.LogModules.MainSystem, Log.GetCurrentShiftDate()));
        }

        private void miLCBLogs_Click(object sender, EventArgs e)
        {
            //FileAndDirectoryTools.OpenNotepad(Log.GetLogFileName(Log.LogModules.LCB, Log.GetCurrentShiftDate()));

        }

        private void miRDPBLogs_Click(object sender, EventArgs e)
        {
            //FileAndDirectoryTools.OpenNotepad(Log.GetLogFileName(Log.LogModules.RDPB, Log.GetCurrentShiftDate()));

        }

        private void miDBLogs_Click(object sender, EventArgs e)
        {
            //FileAndDirectoryTools.OpenNotepad(Log.GetLogFileName(Log.LogModules.DB, Log.GetCurrentShiftDate()));

        }

        private void miMainInterfaceLogsArchive_Click(object sender, EventArgs e)
        {
            // FileAndDirectoryTools.OpenFolder(Log.GetPath(Log.LogModules.MainSystem));
        }

        private void miLCBLogsArchive_Click(object sender, EventArgs e)
        {
            //  FileAndDirectoryTools.OpenFolder(Log.GetPath(Log.LogModules.LCB));
        }

        private void miRDPBLogsArchive_Click(object sender, EventArgs e)
        {
            // FileAndDirectoryTools.OpenFolder(Log.GetPath(Log.LogModules.RDPB));
        }

        private void miDBLogsArchive_Click(object sender, EventArgs e)
        {
            //   FileAndDirectoryTools.OpenFolder(Log.GetPath(Log.LogModules.DB));
        }


        private void btnMoveToArchive_Click(object sender, EventArgs e)
        {

            if (MovingCyclesToArchive)
            {
                MovingCyclesToArchive = false;
                Controller.CreateCommandInstance(typeof(DoMCLib.Classes.Module.ArchiveDB.ArchiveDBModule.StartCommand))?.ExecuteCommand();
                btnMoveToArchive.BackColor = SystemColors.Control;

            }
            else
            {
                MovingCyclesToArchive = true;
                Controller.CreateCommandInstance(typeof(DoMCLib.Classes.Module.ArchiveDB.ArchiveDBModule.StopCommand))?.ExecuteCommand();
                btnMoveToArchive.BackColor = Color.DarkGreen;
            }

        }

        private void дополнительныеПараметрыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var af = new DoMCLib.Forms.DoMCAdditionalParametersForm();
            af.AverageToHaveImage = Context.Configuration.HardwareSettings.AverageToHaveImage;
            af.LogPackets = Context.Configuration.HardwareSettings.LogPackets;
            af.RegisterEmptyImages = Context.Configuration.HardwareSettings.RegisterEmptyImages;
            if (af.ShowDialog() == DialogResult.OK)
            {
                Context.Configuration.HardwareSettings.AverageToHaveImage = af.AverageToHaveImage;
                Context.Configuration.HardwareSettings.LogPackets = af.LogPackets;
                Context.Configuration.HardwareSettings.RegisterEmptyImages = af.RegisterEmptyImages;
                NotifyConfigurationUpdated();
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
            //var statFrom = new ImageReadBytesStatiscticsForm(InterfaceDataExchange);
            //statFrom.Show();
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
            //Context.Configuration.LCBSettings.LEDCurrent = current;
            //Context.Configuration.LCBSettings.PreformLength = preformLength;
            //Context.Configuration.LCBSettings.DelayLength = delayLength;
        }

        private void btnLCBLoadFromConfig_Click(object sender, EventArgs e)
        {
            /*
            LCBSettingsPreformLengthGotFromConfig = true;
            LCBSettingsDelayLengthGotFromConfig = true;
            txbTestLCBCurrent.Text = Context.Configuration.LCBSettings.LEDCurrent.ToString();
            txbTestLCBPreformLength.Text = Context.Configuration.LCBSettings.PreformLength.ToString();
            txbTestLCBDelayLength.Text = Context.Configuration.LCBSettings.DelayLength.ToString();
            */
        }

        public DoMCLib.Classes.DoMCApplicationContext GetContext()
        {
            return Context;
        }

    }


}
