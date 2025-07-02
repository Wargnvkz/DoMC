using DoMC.Tools;
using DoMCLib.Classes.Configuration.CCD;
using DoMC.Classes;
using DoMCModuleControl;
using DoMCModuleControl.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoMCLib.Tools;
using static DoMCLib.Classes.DoMCApplicationContext.ErrorsReadingData;
using DoMCLib.Classes;
using DoMCLib.Configuration;
using DoMCLib.Classes.Module.CCD;
using DoMCLib.Classes.Module.CCD.Commands.Classes;

namespace DoMC.Forms
{
    public partial class TestCCDControl : UserControl, IDisposable
    {
        bool IsCycleStarted = false;
        IMainController MainController;
        ILogger WorkingLog;

        short[,] testImage;
        int TestSocketNumberSelected;
        bool showMaxDevPoint = false;


        short[,] TestDiffImage;
        short[,] TestReadImage;
        short[,] TestStandardImage;
        Bitmap TestBmpReadImage;
        Bitmap TestBmpDiffImage;
        Bitmap TestBmpStandardImage;

        int[] IsSocketsReadAndGood;
        int SocketQuantity;

        Panel[] SettingsSocketsPanelList;
        Panel[] WorkModeStandardSettingsSocketsPanelList;
        Panel[] TestSocketsSettingsSocketsPanelList;

        bool TestCCDIsReading;
        IDoMCSettingsUpdatedProvider SettingsUpdateProvider;
        DoMCLib.Classes.DoMCApplicationContext CurrentContext;
        short[][,] AllImages;
        bool ExternalStart = false;
        int socketMax = 96;
        DoMCApplicationContext.ErrorsReadingData errorReadingData = new DoMCApplicationContext.ErrorsReadingData();
        ImageProcessResult[] SocketCheckResults;
        bool invertColors;
        SynchronizationContext UIContext;
        volatile int ProgressbarStep = 0;
        int PrograssbarMaxStep = 8;

        public TestCCDControl(IMainController Controller, ILogger logger, DoMC.Classes.IDoMCSettingsUpdatedProvider settingsUpdateProvider)
        {
            InitializeComponent();
            MainController = Controller;
            WorkingLog = logger;
            SettingsUpdateProvider = settingsUpdateProvider;
            SettingsUpdateProvider.SettingsUpdated += SettingsUpdateProvider_SettingsUpdated;
            UIContext = SynchronizationContext.Current;
        }

        private async Task SettingsUpdateProvider_SettingsUpdated(object? sender)
        {
            var context = SettingsUpdateProvider.GetContext();
            ApplyNewContext(context);
        }

        private void ApplyNewContext(DoMCLib.Classes.DoMCApplicationContext context)
        {
            if (context == null || context.Configuration == null || context.Configuration.HardwareSettings == null) return;
            CurrentContext = context;
            SocketQuantity = CurrentContext.Configuration.HardwareSettings.SocketQuantity;
            FillSettingPage();
        }

        private void FillSettingPage()
        {
            CreateTestCCDPanelSocketStatuses();
            SetTestCCDPanelSocketStatuses();
        }


        private void AbleForRead(bool IsReading)
        {
            btnTest_ReadAllSocket.Enabled = !IsReading;
            btnTest_ReadSelectedSocket.Enabled = !IsReading;
            btnCycleStart.Enabled = !IsReading;

        }

        private void SendNotificationImagesSelected(short[,] Image, short[,] Standard, ImageProcessParameters ipp)
        {
            MainController.GetObserver().Notify(DoMCLib.EventNamesList.InterfaceImagesSelected, (Image, Standard, ipp));
        }

        private async void btnTest_ReadAllSockets_Click(object sender, EventArgs e)
        {

            if (TestCCDIsReading) return;
            TestCCDIsReading = true;
            AbleForRead(true);
            long Start = 0;
            try
            {
                TestSocketNumberSelected = 0;
                lblTestSelectedSocket.Text = "";
                ExternalStart = cbTest_ExternalStart.Checked;
                PrepareProgressBar();
                await TestReadAllSockets();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source);
            }
            finally
            {
                TestCCDIsReading = false;
            }
            SetTestCCDPanelSocketStatuses();
            AbleForRead(false);

        }
        private async void btnReadSelectedSocket_Click(object sender, EventArgs e)
        {

            if (TestCCDIsReading) return;
            if (TestSocketNumberSelected < 0 || TestSocketNumberSelected >= CurrentContext.Configuration.HardwareSettings.SocketQuantity) return;
            AbleForRead(true);
            TestCCDIsReading = true;
            try
            {
                ExternalStart = cbTest_ExternalStart.Checked;
                PrepareProgressBar();
                await TestReadOneSocket(TestSocketNumberSelected);

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

            SetTestCCDPanelSocketStatuses();
            AbleForRead(false);

        }

        private async void btnCycleStartStop_Click(object sender, EventArgs e)
        {

            if (TestCCDIsReading) return;
            TestCCDIsReading = true;
            IsCycleStarted = true;
            AbleForRead(true);
            ExternalStart = cbTest_ExternalStart.Checked;
            PrepareProgressBar();
            await CycleReadingProc(TestSocketNumberSelected);
            IsCycleStarted = false;
            TestCCDIsReading = false;


        }

        private void btnCycleStop_Click(object sender, EventArgs e)
        {
            IsCycleStarted = false;
            TestCCDIsReading = false;
            AbleForRead(false);
        }
        private async Task TestReadAllSockets()
        {
            AllImages = new short[socketMax][,];
            errorReadingData.Clear();
            try
            {
                ProgressbarStep++;
                (bool, CCDCardDataCommandResponse) operationResult;
                if ((operationResult = await DoMCEquipmentCommands.StartCCD(MainController, CurrentContext, WorkingLog)).Item1)
                {
                    if (operationResult.Item2.CardsNotAnswered().Count > 0)
                    {
                        var errorCards = errorReadingData.ErrorCards();
                        var logtext = $"Не удалось подключиться к платам {String.Join(",", errorCards)}.";
                        WorkingLog.Add(LoggerLevel.Critical, logtext);
                        if (MessageBox.Show(logtext + " Отключить их и продолжить?", "Продолжить?", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return;
                        }
                        if (errorCards.Count > 0)
                        {
                            for (int c = 0; c < errorCards.Count; c++)
                            {
                                CurrentContext.Configuration.SetCheckCard(errorCards[c], false);
                            }
                        }
                    }
                    ProgressbarStep++;
                    if ((operationResult = await DoMCEquipmentCommands.LoadCCDReadingParametersConfiguration(MainController, CurrentContext, WorkingLog)).Item1)
                    {
                        ProgressbarStep++;
                        if ((operationResult = await DoMCEquipmentCommands.LoadCCDExpositionConfiguration(MainController, CurrentContext, WorkingLog)).Item1)
                        {
                            ProgressbarStep++;
                            if ((operationResult = await DoMCEquipmentCommands.SetFastRead(MainController, CurrentContext, WorkingLog)).Item1)
                            {
                                for (int socketNum = 0; socketNum < socketMax; socketNum++)
                                {
                                    AllImages[socketNum] = null;
                                }

                                ProgressbarStep++;
                                if ((operationResult = await DoMCEquipmentCommands.ReadSockets(MainController, CurrentContext, WorkingLog, ExternalStart)).Item1)
                                {
                                    ProgressbarStep++;
                                    var ImageData = await DoMCEquipmentCommands.GetSocketsImages(MainController, CurrentContext, WorkingLog);
                                    ProgressbarStep++;
                                    for (int socketNum = 0; socketNum < socketMax; socketNum++)
                                    {
                                        try
                                        {
                                            if ((ImageData.Item2?[socketNum] ?? null) != null)
                                                AllImages[socketNum] = ImageData.Item2[socketNum]?.Image ?? null;
                                        }
                                        catch (Exception ex)
                                        {

                                        }

                                    }
                                    ProgressbarStep++;
                                }
                                else
                                {
                                    ShowCardResultError(operationResult.Item2);
                                }
                            }
                            else
                            {
                                ShowCardResultError(operationResult.Item2);
                            }

                        }
                        else
                        {
                            ShowCardResultError(operationResult.Item2);
                        }
                    }
                    else
                    {
                        ShowCardResultError(operationResult.Item2);
                    }
                }
                else
                {
                    ShowCardResultError(operationResult.Item2);
                }
            }
            finally
            {
                await DoMCEquipmentCommands.StopCCD(MainController, CurrentContext, WorkingLog);
            }
            SetTestCCDSocketStatuses();
            SetTestCCDPanelSocketStatuses();
            HideProgressBar();
        }

        private void ShowCardResultError(CCDCardDataCommandResponse res)
        {
            var errorCards = res?.CardsNotAnswered() ?? new List<int>(); ;
            var logtext = $"Не удалось подключиться к платам {String.Join(",", errorCards.Select(card => card + 1))}.";
            MessageBox.Show(logtext, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private async Task TestReadOneSocket(int SelectedSocket)
        {
            if (SelectedSocket == -1) return;
            if (AllImages == null)
                AllImages = new short[socketMax][,];
            errorReadingData.Clear();
            (bool, CCDCardDataCommandResponse) startResult;
            ProgressbarStep++;
            if ((startResult = await DoMCEquipmentCommands.StartCCD(MainController, CurrentContext, WorkingLog, SelectedSocket)).Item1)
            {
                try
                {
                    var notAnsweredCards = startResult.Item1;
                    if (!notAnsweredCards)
                    {
                        var logText = $"Плата гнезда {SelectedSocket} не отвечает.";
                        DisplayMessage.Show(logText, "Ошибка");
                        return;
                    }
                    ProgressbarStep++;
                    if ((await DoMCEquipmentCommands.LoadCCDReadingParametersConfiguration(MainController, CurrentContext, WorkingLog, SelectedSocket)).Item1)
                    {
                        ProgressbarStep++;
                        if ((await DoMCEquipmentCommands.LoadCCDExpositionConfiguration(MainController, CurrentContext, WorkingLog, SelectedSocket)).Item1)
                        {
                            ProgressbarStep++;
                            if ((await DoMCEquipmentCommands.SetFastRead(MainController, CurrentContext, WorkingLog, SelectedSocket)).Item1)
                            {
                                ProgressbarStep++;
                                if ((await DoMCEquipmentCommands.ReadSockets(MainController, CurrentContext, WorkingLog, ExternalStart, SelectedSocket)).Item1)
                                {
                                    ProgressbarStep++;
                                    var ImageData = await DoMCEquipmentCommands.GetSingleSocketsImage(MainController, CurrentContext, WorkingLog, SelectedSocket);

                                    if (ImageData != null)
                                        AllImages[SelectedSocket] = ImageData.Image;

                                }
                            }

                        }
                    }
                }
                finally
                {
                    await DoMCEquipmentCommands.StopCCD(MainController, CurrentContext, WorkingLog, SelectedSocket);
                }
            }

            SetTestCCDSocketStatuses();

            SetTestCCDPanelSocketStatuses();
            HideProgressBar();


        }
        private async Task CycleReadingProc(int SelectedSocket)
        {
            if (SelectedSocket == -1) return;
            if (AllImages == null)
                AllImages = new short[socketMax][,];
            errorReadingData.Clear();

            (bool, CCDCardDataCommandResponse) startResult;
            ProgressbarStep++;
            if ((startResult = await DoMCEquipmentCommands.StartCCD(MainController, CurrentContext, WorkingLog, SelectedSocket)).Item1)
            {
                try
                {
                    var notAnsweredCards = startResult.Item2.CardsNotAnswered();
                    if (notAnsweredCards.Count > 0)
                    {
                        var logText = $"Плата {notAnsweredCards[0]} не отвечает.";
                        DisplayMessage.Show(logText, "Ошибка");
                        return;
                    }
                    ProgressbarStep++;
                    if ((await DoMCEquipmentCommands.LoadCCDReadingParametersConfiguration(MainController, CurrentContext, WorkingLog, SelectedSocket)).Item1)
                    {
                        ProgressbarStep++;
                        if ((await DoMCEquipmentCommands.LoadCCDExpositionConfiguration(MainController, CurrentContext, WorkingLog, SelectedSocket)).Item1)
                        {
                            ProgressbarStep++;
                            if ((await DoMCEquipmentCommands.SetFastRead(MainController, CurrentContext, WorkingLog, SelectedSocket)).Item1)
                            {
                                while (IsCycleStarted)
                                {

                                    ProgressbarStep = 5;
                                    if ((await DoMCEquipmentCommands.ReadSockets(MainController, CurrentContext, WorkingLog, ExternalStart, SelectedSocket)).Item1)
                                    {
                                        ProgressbarStep = 6;
                                        var ImageData = await DoMCEquipmentCommands.GetSingleSocketsImage(MainController, CurrentContext, WorkingLog, SelectedSocket);

                                        if (ImageData != null)
                                            AllImages[SelectedSocket] = ImageData.Image;
                                        Task.Run(() => { }).FireAndForgetWithResult(() => { ShowSocket(SelectedSocket); }, context: UIContext);
                                        //ShowSocket(SelectedSocket);
                                        //Application.DoEvents();
                                    }
                                }
                            }

                        }
                    }
                }
                finally
                {
                    await DoMCEquipmentCommands.StopCCD(MainController, CurrentContext, WorkingLog);
                }
            }
            HideProgressBar();

        }

        private void SetControlSize(int Width, int Height)
        {
            WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Maximize window");

            #region Test tab
            WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Screen.FromControl");
            var swidth = Width;
            var sheight = Height;
            WorkingLog.Add(LoggerLevel.FullDetailedInformation, $"WxH={swidth}x{sheight}");

            var pbtotalwidth = swidth - btnCycleStop.Width;// - 300;
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
            var wd = (int)(Width - 520 * kwidth * 3 - 50);
            WorkingLog.Add(LoggerLevel.FullDetailedInformation, $"wd={wd}");
            pnlSockets.Size = new Size(wd, socketButtonsHeight);//new Size(wd, (int)(512*kheight));

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

            btnCycleStart.Location = new Point(pnlSockets.Location.X, pnlSockets.Location.Y + pnlSockets.Size.Height + 10);
            //btnCycleStop.Location = new Point(pnlTestSockets.Location.X + pnlTestSockets.Width - btnCycleStop.Width, pnlTestSockets.Location.Y + pnlTestSockets.Size.Height + 10);
            btnCycleStop.Location = new Point(pnlSockets.Location.X, btnCycleStart.Location.Y + btnCycleStart.Size.Height + 10);
            #endregion
        }

        private void TestCCDInterface_Resize(object sender, EventArgs e)
        {
            SetControlSize(this.Width, this.Height);
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
            var ctrl = sender as Control;
            if (ctrl != null)
            {
                var socketnumber = (int)(ctrl?.Tag ?? -1);
                TestSocketNumberSelected = socketnumber;
                ShowSocket(socketnumber);
            }

        }

        private void ShowSocket(int socketnumber)
        {

            TestSocketNumberSelected = socketnumber;
            lblTestSelectedSocket.Text = (TestSocketNumberSelected + 1).ToString();
            try
            {
                if (AllImages == null)
                {
                    //MessageBox.Show("Изображения для гнезда еще не получены");
                    TestBmpReadImage = null;
                    TestBmpDiffImage = null;
                    TestBmpStandardImage = null;
                    TestRedrawBitmap();
                    TestTabDrawGraphLine((int)numFrame.Value);
                    return;
                }
                TestStandardImage = CurrentContext.Configuration.ProcessingDataSettings.CCDSocketStandardsImage[TestSocketNumberSelected].StandardImage;
                TestReadImage = AllImages[TestSocketNumberSelected];
                if (TestReadImage != null && TestReadImage != null)
                {
                    TestDiffImage = ImageTools.GetDifference(TestStandardImage, TestReadImage);
                    var ipp = CurrentContext.Configuration.ReadingSocketsSettings.CCDSocketParameters[TestSocketNumberSelected].ImageCheckingParameters;

                    try
                    {
                        var SocketCheckResults = ImageTools.CheckIfSocketGood(TestReadImage, TestStandardImage, ipp);
                        if (!SocketCheckResults.IsSocketGood || showMaxDevPoint)
                            TestBmpDiffImage = ImageTools.DrawImage(TestDiffImage, invertColors, SocketCheckResults.MaxDeviationPoint);
                        else
                            TestBmpDiffImage = ImageTools.DrawImage(TestDiffImage, invertColors);
                    }
                    catch
                    {

                    }
                    SendNotificationImagesSelected(TestReadImage, TestStandardImage, ipp);

                }
                else
                {
                    TestDiffImage = null;
                    TestBmpDiffImage = null;
                }


                TestBmpReadImage = ImageTools.DrawImage(TestReadImage, invertColors);
                TestBmpStandardImage = ImageTools.DrawImage(TestStandardImage, invertColors);



                //var sp = InterfaceDataExchange.CCDDataEchangeStatuses.StartProcessImages;
                //var ep = InterfaceDataExchange.CCDDataEchangeStatuses.StopProcessImages;

                //lblTimeImageProcess.Text = $"Обработка изображений и принятие решения:{(ep - sp) * 1e-4:F3} мс";

                //numFrame.Value = 0;
                TestRedrawBitmap();
                TestTabDrawGraphLine((int)numFrame.Value);

                /*if (checkpreformalgorithmsForm != null)
                {
                    checkpreformalgorithmsForm.SetStandardImage(TestStandardImage);
                    checkpreformalgorithmsForm.SetImage(TestReadImage);
                    checkpreformalgorithmsForm.SetImageProcessParameters(InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations[socketnumber].ImageCheckingParameters);
                    checkpreformalgorithmsForm.RecalcAndRedrawImages();

                }*/
            }
            catch
            {
                TestBmpReadImage = null;
                TestBmpDiffImage = null;
                TestBmpStandardImage = null;
                TestRedrawBitmap();
            }
        }

        private void TestTabDrawGraphLine(int linen)
        {
            short[]? ReadLine = null;
            short[]? StandardLine = null;
            short[]? DiffLine = null;
            if (cbFullMax.Checked)
            {
                ReadLine = ImageTools.GetRow(TestReadImage, linen);
                StandardLine = ImageTools.GetRow(TestStandardImage, linen);
                DiffLine = ImageTools.GetRow(TestDiffImage, linen);
                for (int y = 0; y < 512; y++) for (int x = 0; x < 512; x++) if (ReadLine[x] < TestReadImage[y, x]) ReadLine[x] = TestReadImage[y, x]; ;
                for (int y = 0; y < 512; y++) for (int x = 0; x < 512; x++) if (StandardLine[x] < TestStandardImage[y, x]) StandardLine[x] = TestStandardImage[y, x]; ;
                for (int y = 0; y < 512; y++) for (int x = 0; x < 512; x++) if (DiffLine[x] < TestDiffImage[y, x]) DiffLine[x] = TestDiffImage[y, x]; ;
            }
            else
            {
                if (TestReadImage != null) ReadLine = ImageTools.GetRow(TestReadImage, linen);
                if (TestStandardImage != null) StandardLine = ImageTools.GetRow(TestStandardImage, linen);
                if (TestDiffImage != null) DiffLine = ImageTools.GetRow(TestDiffImage, linen);
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

        private void pbTestReadImage_Paint(object sender, PaintEventArgs e)
        {

            if (TestBmpReadImage == null)
            {
                //pbTestReadImage.Image = null; 
                return;
            }
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

            if (TestBmpDiffImage == null)
            {
                //pbTestDifference.Image = null;
                return;
            }
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
            if (TestBmpStandardImage == null)
            {
                //pbTestStandard.Image = null; 
                return;
            }
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
            //TestTabDrawGraphLine((int)numFrame.Value);
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


        private void SetTestCCDSocketStatuses()
        {
            IsSocketsReadAndGood = new int[SocketQuantity];
            SocketCheckResults = new ImageProcessResult[SocketQuantity];
            Parallel.For(0, SocketQuantity, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, i =>
            {
                try
                {
                    if (AllImages[i] == null)
                    {
                        IsSocketsReadAndGood[i] = 0;
                    }
                    else
                    {
                        var avg = ImageTools.Average(AllImages[i]);
                        if (avg < Math.Abs(CurrentContext.Configuration.HardwareSettings.AverageToHaveImage))
                        {
                            IsSocketsReadAndGood[i] = 3;
                        }
                        else
                        {
                            SocketCheckResults[i] =
                                    ImageTools.CheckIfSocketGood(AllImages[i], CurrentContext.Configuration.ProcessingDataSettings.CCDSocketStandardsImage[i].StandardImage, CurrentContext.Configuration.ReadingSocketsSettings.CCDSocketParameters[i].ImageCheckingParameters);
                            if (SocketCheckResults[i].IsSocketGood)
                            {
                                IsSocketsReadAndGood[i] = 1;
                            }
                            else
                            {
                                IsSocketsReadAndGood[i] = 2;
                            }
                        }
                    }
                }
                catch
                {
                    IsSocketsReadAndGood[i] = 3;
                }
            });
        }
        private void cbInvertColors_CheckedChanged(object sender, EventArgs e)
        {
            invertColors = cbInvertColors.Checked;
            ShowSocket(TestSocketNumberSelected);
        }

        private void CreateTestCCDPanelSocketStatuses()
        {
            TestSocketsSettingsSocketsPanelList = UserInterfaceControls.CreateSocketStatusPanels(SocketQuantity, ref pnlSockets, TestShowSocketImages_Click);
        }
        private void SetTestCCDPanelSocketStatuses()
        {
            UserInterfaceControls.SetSocketStatuses(TestSocketsSettingsSocketsPanelList, IsSocketsReadAndGood, Color.DarkGray, Color.Green, Color.Red, Color.Yellow);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (TestCCDIsReading)
            {
                try
                {
                    SetPrograssbarPosition(ProgressbarStep);
                    var lastcmd = MainController.LastCommand;
                    if (lastcmd.FullName?.Contains("CCD") ?? false)
                        lblGetStandardWorkStatus.Text = MainController.LastCommand?.GetDescription() ?? "";//CurrentOperation.GetDescription();
                }
                catch { }
            }
        }
        private void PrepareProgressBar()
        {
            RunOnUI(pbGettingStandard, () =>
            {
                ProgressbarStep = 0;
                pbGettingStandard.Visible = true;
                pbGettingStandard.Value = 0;
                pbGettingStandard.Maximum = PrograssbarMaxStep;
            });
        }
        private void SetPrograssbarPosition(int step)
        {
            RunOnUI(pbGettingStandard, () =>
            {
                pbGettingStandard.Value = step;
            });
        }
        private void HideProgressBar()
        {
            RunOnUI(pbGettingStandard, () =>
            {

                pbGettingStandard.Visible = false;
                pbGettingStandard.Value = 0;
            });
        }

        public static void RunOnUI(Control control, Action action)
        {
            if (control.InvokeRequired)
                control.Invoke(action);
            else
                action();
        }
    }
}
