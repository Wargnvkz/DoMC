using DoMCModuleControl.Logging;
using DoMCModuleControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoMC.Classes;
using DoMC.Tools;
using DoMCLib.Tools;
using DoMCLib.Classes.Module.CCD;
using System.Net.Sockets;
using DoMCLib.Classes;

namespace DoMC.UserControls
{
    public partial class GetCCDStandardInterface : UserControl, IDisposable
    {
        IMainController MainController;
        ILogger WorkingLog;
        IDoMCSettingsUpdatedProvider SettingsUpdateProvider;
        DoMCLib.Classes.DoMCApplicationContext CurrentContext;
        bool StandardIsReading = false;
        Panel[] StandardSettingsSocketsPanelList;

        int socketMax = 96;
        int ImagesToMakeStandard = 3;
        int? ChosenSocketNumber;
        short[][,] SingleSocketImages;
        Bitmap bmpCheckSign = new Bitmap(512, 512);
        bool NeedToRenewList = false;
        bool ExternalRead = false;
        DoMCApplicationContext.ErrorsReadingData errorReadingData = new DoMCApplicationContext.ErrorsReadingData();
        int ProgressbarStep = 0;
        int PrograssbarMaxStep = 9;

        public GetCCDStandardInterface(IMainController Controller, ILogger logger, DoMC.Classes.IDoMCSettingsUpdatedProvider settingsUpdateProvider)
        {
            InitializeComponent();
            MainController = Controller;
            WorkingLog = logger;
            SettingsUpdateProvider = settingsUpdateProvider;
            SettingsUpdateProvider.SettingsUpdated += SettingsUpdateProvider_SettingsUpdated;
            Disposed += OnDispose;
            var bmpGraphics = Graphics.FromImage(bmpCheckSign);
            bmpGraphics.DrawString("✓", new Font("Arial", 300), new SolidBrush(Color.LimeGreen), new PointF(0, 0));
            cbExternalSignalForStandard.Checked = ExternalRead;
            MainController.GetObserver().NotificationReceivers += GetCCDStandardInterface_NotificationReceivers;
        }

        private void GetCCDStandardInterface_NotificationReceivers(string eventName, object? data)
        {
            if (eventName.EndsWith(".GetImageDataFromSocketAsync.Error"))
            {
                if (data != null)
                {
                    var errorData = ((int CardNumber, int Socket, int ImageDataRead))data;
                    errorReadingData.AddReadingError(new DoMCApplicationContext.ErrorsReadingData.ErrorReadingData(errorData));
                }
            }
        }

        private void SettingsUpdateProvider_SettingsUpdated(object? sender, EventArgs e)
        {
            var context = SettingsUpdateProvider.GetContext();
            ApplyNewContext(context);
        }

        private void ApplyNewContext(DoMCLib.Classes.DoMCApplicationContext context)
        {
            CurrentContext = context;
            RenewList();
        }
        private void OnDispose(object? sender, EventArgs e)
        {
            if (SettingsUpdateProvider != null)
            {
                SettingsUpdateProvider.SettingsUpdated -= SettingsUpdateProvider_SettingsUpdated;
            }
            try
            {
                MainController.GetObserver().NotificationReceivers -= GetCCDStandardInterface_NotificationReceivers;
            }
            catch { }
        }

        private void FillPage()
        {
            StandardSettingsSocketsPanelList = UserInterfaceControls.CreateSocketStatusPanels(CurrentContext.Configuration.HardwareSettings.SocketQuantity, ref pnlGetStandardSockets, ChooseSocket_Click);

            UserInterfaceControls.SetSocketStatuses(
                StandardSettingsSocketsPanelList,
                Enumerable.Range(0, CurrentContext.Configuration.HardwareSettings.SocketQuantity).
                Select(
                    i =>
                        CurrentContext.Configuration.ProcessingDataSettings.CCDSocketStandardsImage != null &&
                        CurrentContext.Configuration.ProcessingDataSettings.CCDSocketStandardsImage.Length > i
                        ?
                            CurrentContext.Configuration.ProcessingDataSettings.CCDSocketStandardsImage[i].StandardImage != null
                        :
                            false
                ).ToArray(),
                Color.Green,
                Color.DarkGray
            );

        }


        private void btnReadImagesToGetStandardForOneSocket_Click(object sender, EventArgs e)
        {
            if (!StandardIsReading)
            {
                PrepareProgressBar();
                StandardIsReading = true;
                var tsk = new Task(new Action(() => ImagesForSingleSocketStandard()));
                tsk.Start();
            }
        }


        private void ImagesForSingleSocketStandard()
        {
            if (ChosenSocketNumber == null) return;
            PictureBox[] StandardPictures = new PictureBox[3] { pbStandard1, pbStandard2, pbStandard3 };
            errorReadingData.Clear();
            SingleSocketImages = new short[this.ImagesToMakeStandard][,];
            ProgressbarStep = 0;
            if (CurrentContext.StartCCD(MainController, WorkingLog, ChosenSocketNumber.Value - 1))
            {
                try
                {
                    ProgressbarStep++;
                    if (CurrentContext.LoadCCDConfiguration(MainController, WorkingLog, ChosenSocketNumber.Value - 1))
                    {
                        ProgressbarStep++;
                        if (CurrentContext.SetFastRead(MainController, WorkingLog, ChosenSocketNumber.Value - 1))
                        {
                            ProgressbarStep++;
                            for (int i = 0; i < ImagesToMakeStandard; i++)
                            {
                                if (CurrentContext.ReadSockets(MainController, WorkingLog, ExternalRead, ChosenSocketNumber.Value - 1))
                                {
                                    ProgressbarStep++;
                                    var si = CurrentContext.GetSocketsImages(MainController, WorkingLog, ChosenSocketNumber.Value - 1);
                                    ProgressbarStep++;
                                    if (si == null || si.Data == null) break;
                                    var errorCards = errorReadingData.ErrorCards();
                                    if (errorCards.Count > 0)
                                    {
                                        for (int c = 0; c < errorCards.Count; c++)
                                        {
                                            CurrentContext.Configuration.SetCheckCard(errorCards[c], false);
                                        }
                                    }
                                    if (si.Data[ChosenSocketNumber.Value - 1] == null)
                                    {
                                        //TODO: вывести ошибку чтения
                                        continue;
                                    }
                                    SingleSocketImages[i] = si.Data[ChosenSocketNumber.Value - 1].Image;
                                    var sbmp = ImageTools.DrawImage(SingleSocketImages[i], false);
                                    StandardPictures[i].Image = sbmp;
                                }
                            }
                            var existImages = SingleSocketImages.Where(ssi => ssi != null).ToArray();
                            var avgImg = ImageTools.CalculateAverage(existImages);
                            CurrentContext.Configuration.ProcessingDataSettings.CCDSocketStandardsImage[ChosenSocketNumber.Value - 1].StandardImage = avgImg;
                            CurrentContext.Configuration.SaveProcessingDataSettings();
                        }
                    }
                }
                finally
                {
                    CurrentContext.StopCCD(MainController, WorkingLog, ChosenSocketNumber.Value - 1);
                }
            }
            StandardIsReading = false;
            RenewList();
        }
        private void btnGetAllStandards_Click(object sender, EventArgs e)
        {
            if (!StandardIsReading)
            {
                PrepareProgressBar();
                StandardIsReading = true;
                var task = new Task(new Action(() => FullStandardGet()));
                task.Start();
            }
        }
        private void FullStandardGet()
        {
            PictureBox[] StandardPictures = new PictureBox[3] { pbStandard1, pbStandard2, pbStandard3 };
            short[][][,] img = new short[socketMax][][,];
            for (int i = 0; i < StandardPictures.Length; i++) StandardPictures[i].Image = null;
            pbAverage.Image = null;
            errorReadingData.Clear();
            ProgressbarStep = 0;
            if (CurrentContext.StartCCD(MainController, WorkingLog))
            {
                ProgressbarStep++;
                try
                {
                    if (CurrentContext.LoadCCDConfiguration(MainController, WorkingLog))
                    {
                        ProgressbarStep++;
                        if (CurrentContext.SetFastRead(MainController, WorkingLog))
                        {
                            ProgressbarStep++;
                            for (int socketNum = 0; socketNum < socketMax; socketNum++)
                            {
                                img[socketNum] = new short[ImagesToMakeStandard][,];
                            }
                            for (int repeat = 0; repeat < ImagesToMakeStandard; repeat++)
                            {
                                if (CurrentContext.ReadSockets(MainController, WorkingLog, ExternalRead))
                                {
                                    ProgressbarStep++;
                                    var si = CurrentContext.GetSocketsImages(MainController, WorkingLog);
                                    ProgressbarStep++;
                                    if (si == null) throw new Exception();
                                    if (si.Data == null) throw new Exception();
                                    if (si.Data.Length != socketMax) throw new Exception();
                                    for (int socketNum = 0; socketNum < socketMax; socketNum++)
                                    {
                                        if (si.Data[socketNum] != null)
                                            img[socketNum][repeat] = si.Data[socketNum].Image;
                                        var errorCards = errorReadingData.ErrorCards();
                                        if (errorCards.Count > 0)
                                        {
                                            for (int c = 0; c < errorCards.Count; c++)
                                            {
                                                CurrentContext.Configuration.SetCheckCard(errorCards[c], false);
                                            }
                                        }

                                    }
                                }
                                StandardPictures[repeat].Image = bmpCheckSign;
                            }
                            for (int socketNum = 0; socketNum < socketMax; socketNum++)
                            {
                                if (img[socketNum].Any(im => im == null))
                                {
                                    CurrentContext.Configuration.ProcessingDataSettings.CCDSocketStandardsImage[socketNum].StandardImage = null;
                                    continue;
                                }
                                var avgImg = ImageTools.CalculateAverage(img[socketNum]);
                                CurrentContext.Configuration.ProcessingDataSettings.CCDSocketStandardsImage[socketNum].StandardImage = avgImg;
                            }

                            CurrentContext.Configuration.SaveProcessingDataSettings();
                        }
                    }
                }
                finally
                {
                    CurrentContext.StopCCD(MainController, WorkingLog);
                }
                pbAverage.Image = bmpCheckSign;
                StandardIsReading = false;
                RenewList();
            }
        }
        private void btnMakeAverage_Click(object sender, EventArgs e)
        {
            if (ChosenSocketNumber == null || SingleSocketImages == null || SingleSocketImages.Any(i => i == null)) return;
            var cardSocket = CurrentContext.EquipmentSocket2CardSocket[ChosenSocketNumber.Value - 1];
            var workingCardSocket = new TCPCardSocket(cardSocket);
            var imagesForStandards = new short[0][,];

            var avgImg = ImageTools.CalculateAverage(SingleSocketImages);
            CurrentContext.Configuration.ProcessingDataSettings.CCDSocketStandardsImage[ChosenSocketNumber.Value - 1].StandardImage = avgImg;
            CurrentContext.Configuration.SaveProcessingDataSettings();
            var msbmp = ImageTools.DrawImage(avgImg, false);// invertColors);
            pbAverage.Image = msbmp;
            RenewList();

        }

        private void ChooseSocket_Click(object? sender, EventArgs e)
        {
            if (sender == null) return;
            //CheckForDoMCModule();
            var ctrl = (Control)sender;
            var socketnumber = ((int)ctrl.Tag) + 1;
            ChosenSocketNumber = socketnumber;
            lblStandardSocketNumber.Text = ChosenSocketNumber.ToString();

            PictureBox[] StandardPictures = [pbStandard1, pbStandard2, pbStandard3];
            for (int i = 0; i < StandardPictures.Length; i++) StandardPictures[i].Image = null;
            pbAverage.Image = null;

            var getSocketStandard = new DoMCLib.Classes.SimpleCommands.GetEquipmentSocketStandard();
            try
            {
                var socketImage = getSocketStandard.Execute((CurrentContext, socketnumber));

                if (socketImage == null)
                {
                    //DoMCSocketIsNotConfiguredErrorMessage();
                    return;
                }


                var msbmp = ImageTools.DrawImage(socketImage.StandardImage, false);
                pbAverage.Image = msbmp;
            }
            catch { return; }

        }

        private void RenewList()
        {
            NeedToRenewList = true;
        }

        private void tmrRenew_Tick(object sender, EventArgs e)
        {
            if (NeedToRenewList)
            {
                FillPage();
                NeedToRenewList = false;
            }
            if (StandardIsReading)
            {
                try
                {
                    SetPrograssbarStep(ProgressbarStep);
                }
                catch { }
            }
        }

        private void cbExternalSignalForStandard_CheckedChanged(object sender, EventArgs e)
        {
            ExternalRead = cbExternalSignalForStandard.Checked;
        }

        private void PrepareProgressBar()
        {
            pbGettingStandard.Value = 0;
            pbGettingStandard.Maximum = PrograssbarMaxStep;
        }

        private void SetPrograssbarStep(int step)
        {
            pbGettingStandard.Value = step;
        }

    }
}
