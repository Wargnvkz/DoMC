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
using DoMC.Classes;
using DoMC.Forms;
using DoMCModuleControl.UI;
using DoMC.UserControls;
using DoMCLib.Forms;
using DoMCLib.Tools;

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
            //CurrentContext = context;
            //contextProcessor = new ModelCommandProcessor(controller, CurrentContext);
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
            #endregion

            Application.ThreadException += Application_ThreadException;

            var bmpGraphics = Graphics.FromImage(bmpCheckSign);
            bmpGraphics.DrawString("✓", new Font("Arial", 300), new SolidBrush(Color.LimeGreen), new PointF(0, 0));

            TestCCDInterfaceView = new TestCCDControl(Controller, WorkingLog, this);
            tbCCDTest.Controls.Add(TestCCDInterfaceView);
            TestCCDInterfaceView.Size = tbCCDTest.ClientSize;
            TestCCDInterfaceView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            GetCCDStandardInterface = new GetCCDStandardInterface(Controller, WorkingLog, this);
            tbGetStandard.Controls.Add(GetCCDStandardInterface);
            GetCCDStandardInterface.Size = tbCCDTest.ClientSize;
            GetCCDStandardInterface.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            TestLCBInterface = new TestLCBInterface(Controller, WorkingLog, this);
            tbTestLCB.Controls.Add(TestLCBInterface);
            TestLCBInterface.Size = tbTestLCB.ClientSize;
            TestLCBInterface.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            CheckSettingsInterface = new CheckSettings(Controller, WorkingLog, this);
            tbSettingsCheck.Controls.Add(CheckSettingsInterface);
            CheckSettingsInterface.Size = tbSettingsCheck.ClientSize;
            CheckSettingsInterface.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            TestRDPBControlInterface = new TestRDPBControl(Controller, WorkingLog, this);
            tbTestRDPB_uc.Controls.Add(TestRDPBControlInterface);
            TestRDPBControlInterface.Size = tbTestRDPB_uc.ClientSize;
            TestRDPBControlInterface.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;


            FillSettingPage();
        }
        private void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            WorkingLog.Add(LoggerLevel.Critical, "Необработанная ошибка:", e.Exception);
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

        private void FillSettingPage()
        {
            SettingsUpdated?.Invoke(this, new EventArgs());

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
            var sheight = screen.WorkingArea.Height - tabControl1.Top - SystemInformation.CaptionHeight;
            WorkingLog.Add(LoggerLevel.FullDetailedInformation, $"WxH={swidth}x{sheight}");

            var pbtotalwidth = swidth - tabControl1.Left;// - 300;
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
            Application.DoEvents();



            #endregion
        }


        #endregion Settings


        private void DoMCMainInterface_FormClosed(object sender, FormClosedEventArgs e)
        {
            observer.NotificationReceivers -= Observer_NotificationReceivers;
            observer.Notify(DoMCApplicationContext.SettingsInterfaceClosedEventName, null);
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

            var dir = System.IO.Path.Combine(Application.StartupPath, "");// ApplicationCardParameters.StandardsPath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            var sd = new SaveFileDialog();
            sd.InitialDirectory = dir;
            sd.DefaultExt = "std";
            sd.AddExtension = true;
            sd.Filter = "Эталоны (*.std)|*.std|Все файлы (*.*)|*.*";
            if (sd.ShowDialog() == DialogResult.OK)
            {
                Context.Configuration.SaveStandardSettings(sd.FileName);
            }

        }

        private void miLoadStandard_Click(object sender, EventArgs e)
        {

            var dir = System.IO.Path.Combine(Application.StartupPath, "");
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            var od = new OpenFileDialog();
            od.Filter = "Эталоны (*.std)|*.std|Все файлы (*.*)|*.*";
            od.InitialDirectory = dir;
            od.DefaultExt = "std";
            od.AddExtension = true;
            if (od.ShowDialog() == DialogResult.OK)
            {
                Context.Configuration.LoadStandardSettings(od.FileName);
                FillSettingPage();
                NotifyConfigurationUpdated();
            }

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

        private void miSetCheckSockets_Click(object sender, EventArgs e)
        {

            if (Context.Configuration.HardwareSettings.SocketsToCheck != null)
            {
                //if (CurrentContext.Configuration.HardwareSettings.SocketsToCheck == null) CurrentContext.Configuration.HardwareSettings.SocketsToCheck = new bool[CurrentContext.Configuration.HardwareSettings.SocketQuantity];
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
        }

        private void miDBSettings_Click(object sender, EventArgs e)
        {
            if (Context.Configuration != null && Context.Configuration.HardwareSettings != null)
            {
                var dbsf = new Forms.Settings.DoMCDBSettingsForm();
                dbsf.LocalDBConnectionString = Context.Configuration.HardwareSettings.ArchiveDBConfig.LocalDBPath;
                dbsf.RemoteDBConnectionString = Context.Configuration.HardwareSettings.ArchiveDBConfig.ArchiveDBPath;
                dbsf.DelayBeforeMoveDataToArchive = Context.Configuration.HardwareSettings.Timeouts.DelayBeforeMoveDataToArchiveTimeInSeconds;
                if (dbsf.ShowDialog() == DialogResult.OK)
                {
                    Context.Configuration.HardwareSettings.ArchiveDBConfig.LocalDBPath = dbsf.LocalDBConnectionString;
                    Context.Configuration.HardwareSettings.ArchiveDBConfig.ArchiveDBPath = dbsf.RemoteDBConnectionString;
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
                Context.Configuration.SaveAllConfiguration(saveDlg.FileName);
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
                    Context.Configuration.LoadConfiguration(openDlg.FileName);
                    Context.Configuration.SaveAllConfiguration();
                    NotifyConfigurationUpdated();
                    FillSettingPage();
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }

        }


        private enum SettingsPageSocketsStatus
        {
            IsSocketSettingsOk,
            SocketCardsIsWorking,
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
            archiveForm = new DoMCArchiveForm(Controller, Context.Configuration.HardwareSettings.ArchiveDBConfig.LocalDBPath, Context.Configuration.HardwareSettings.ArchiveDBConfig.ArchiveDBPath);
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

        public DoMCLib.Classes.DoMCApplicationContext GetContext()
        {
            return Context;
        }

        private void tsmiLogsArchive_Click(object sender, EventArgs e)
        {
            var dir = System.IO.Path.Combine(Application.StartupPath, "Logs");
            FileAndDirectoryTools.OpenFolder(dir);
        }

    }


}
