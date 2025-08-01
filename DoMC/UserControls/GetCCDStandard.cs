﻿using DoMCModuleControl.Logging;
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
using DoMCLib.Classes.Module.CCD.Commands.Classes;
using DoMCLib.Classes.Module.LCB;
using Microsoft.AspNetCore.Mvc;

namespace DoMC.UserControls
{
    public partial class GetCCDStandardInterface : UserControl, IDisposable
    {
        IMainController MainController;
        ILogger WorkingLog;
        IDoMCSettingsUpdatedProvider SettingsUpdateProvider;
        DoMCLib.Classes.DoMCApplicationContext CurrentContext;
        //bool StandardIsReading = false;
        Panel[] StandardSettingsSocketsPanelList;

        int socketMax = 96;
        int ImagesToMakeStandard = 3;
        int? ChosenSocketNumber;
        short[][,] SingleSocketImages;
        Bitmap bmpCheckSign = new Bitmap(512, 512);
        bool NeedToRenewList = false;
        bool ExternalRead = false;
        DoMCApplicationContext.ErrorsReadingData errorReadingData = new DoMCApplicationContext.ErrorsReadingData();
        volatile int ProgressbarStep = 0;
        int PrograssbarMaxStep;
        LongInterfaceOperation CurrentOperation = new LongInterfaceOperation();

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
            PrograssbarMaxStep = 2 + 2 * ImagesToMakeStandard;

        }

        private async Task GetCCDStandardInterface_NotificationReceivers(string eventName, object? data)
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

        private async Task SettingsUpdateProvider_SettingsUpdated(object? sender)
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


        private async void btnReadImagesToGetStandardForOneSocket_Click(object sender, EventArgs e)
        {
            if (!CurrentOperation.IsRunning)
            {
                PrepareProgressBar();
                await CurrentOperation.StartOperation(ImagesForSingleSocketStandard(), SetResultOK, (ex) =>
                {
                    SetResultError();
                    throw ex;
                },
                RenewList);
            }

        }


        private async Task ImagesForSingleSocketStandard()
        {
            if (ChosenSocketNumber == null) return;
            PictureBox[] StandardPictures = new PictureBox[3] { pbStandard1, pbStandard2, pbStandard3 };
            errorReadingData.Clear();
            SingleSocketImages = new short[this.ImagesToMakeStandard][,];
            ProgressbarStep = 0;
            await new DoMCLib.Classes.Module.CCD.Commands.StartSingleSocketCommand(MainController, MainController.GetModule(typeof(CCDCardDataModule))).ExecuteCommandAsync((ChosenSocketNumber.Value - 1, CurrentContext));

            if ((await DoMCEquipmentCommands.StartCCD(MainController, CurrentContext, WorkingLog, ChosenSocketNumber.Value - 1)).Item1)
            {
                try
                {
                    ProgressbarStep++;
                    if ((await DoMCEquipmentCommands.LoadCCDReadingParametersConfiguration(MainController, CurrentContext, WorkingLog, ChosenSocketNumber.Value - 1)).Item1)
                    {
                        if ((await DoMCEquipmentCommands.LoadCCDExpositionConfiguration(MainController, CurrentContext, WorkingLog, ChosenSocketNumber.Value - 1)).Item1)
                        {
                            ProgressbarStep++;
                            if ((await DoMCEquipmentCommands.SetFastRead(MainController, CurrentContext, WorkingLog, ChosenSocketNumber.Value - 1)).Item1)
                            {
                                ProgressbarStep++;
                                for (int i = 0; i < ImagesToMakeStandard; i++)
                                {
                                    if ((await DoMCEquipmentCommands.ReadSockets(MainController, CurrentContext, WorkingLog, ExternalRead, ChosenSocketNumber.Value - 1)).Item1)
                                    {
                                        ProgressbarStep++;
                                        var si = await DoMCEquipmentCommands.GetSingleSocketsImage(MainController, CurrentContext, WorkingLog, ChosenSocketNumber.Value - 1);
                                        ProgressbarStep++;
                                        if (si == null || si.Image == null)
                                        {
                                            WorkingLog.Add(LoggerLevel.Critical, $"Не удалось получить изображение гнезда матрицы {ChosenSocketNumber.Value}");
                                            break;
                                        }
                                        SingleSocketImages[i] = si.Image;
                                        var sbmp = ImageTools.DrawImage(SingleSocketImages[i], false);
                                        StandardPictures[i].Image = sbmp;
                                    }
                                }
                                await Task.Yield();
                                ProgressbarStep++;
                                var existImages = SingleSocketImages.Where(ssi => ssi != null).ToArray();
                                var avgImg = ImageTools.CalculateAverage(existImages, CurrentContext.Configuration.HardwareSettings.ThresholdAverageToHaveImage);
                                CurrentContext.Configuration.ProcessingDataSettings.CCDSocketStandardsImage[ChosenSocketNumber.Value - 1].StandardImage = avgImg;
                                CurrentContext.Configuration.SaveProcessingDataSettings();
                            }
                        }
                    }
                }
                finally
                {
                    await DoMCEquipmentCommands.StopCCD(MainController, CurrentContext, WorkingLog, ChosenSocketNumber.Value - 1);
                }
            }
        }
        private async void btnGetAllStandards_Click(object sender, EventArgs e)
        {
            if (!CurrentOperation.IsRunning)
            {
                PrepareProgressBar();
                await CurrentOperation.StartOperation(FullStandardGet(), SetResultOK, (ex) =>
                {
                    SetResultError();
                    throw ex;
                },
                RenewList);
            }

        }
        private async Task FullStandardGet()
        {
            PictureBox[] StandardPictures = new PictureBox[3] { pbStandard1, pbStandard2, pbStandard3 };
            short[][][,] img = new short[socketMax][][,];
            for (int i = 0; i < StandardPictures.Length; i++) StandardPictures[i].Image = null;
            pbAverage.Image = null;
            errorReadingData.Clear();
            ProgressbarStep = 0;
            List<string> TextResults = new List<string>();
            (bool, CCDCardDataCommandResponse) startResult;
            //CurrentOperation = DoMCOperation.StartCCD;
            if ((startResult = await DoMCEquipmentCommands.StartCCD(MainController, CurrentContext, WorkingLog)).Item1)
            {
                if (startResult.Item2.CardsNotAnswered().Count > 0)
                {

                    var errorCards = errorReadingData.ErrorCards();
                    if (errorCards.Count > 0)
                    {
                        for (int c = 0; c < errorCards.Count; c++)
                        {
                            CurrentContext.Configuration.SetCheckCard(errorCards[c], false);
                        }
                        var logtext = $"Не получается подключиться к платам {String.Join(",", errorCards)}.";
                        TextResults.Add(logtext);
                        WorkingLog.Add(LoggerLevel.Critical, logtext);
                    }
                }
                ProgressbarStep++;
                try
                {
                    //CurrentOperation = DoMCOperation.SettingReadingParameters;
                    if ((await DoMCEquipmentCommands.LoadCCDReadingParametersConfiguration(MainController, CurrentContext, WorkingLog)).Item1)
                    {
                        //CurrentOperation = DoMCOperation.SettingExposition;
                        if ((await DoMCEquipmentCommands.LoadCCDExpositionConfiguration(MainController, CurrentContext, WorkingLog)).Item1)
                        {
                            ProgressbarStep++;
                            //CurrentOperation = DoMCOperation.SetFastReading;
                            if ((await DoMCEquipmentCommands.SetFastRead(MainController, CurrentContext, WorkingLog)).Item1)
                            {
                                ProgressbarStep++;
                                for (int socketNum = 0; socketNum < socketMax; socketNum++)
                                {
                                    img[socketNum] = new short[ImagesToMakeStandard][,];
                                }
                                for (int repeat = 0; repeat < ImagesToMakeStandard; repeat++)
                                {
                                    //CurrentOperation = ExternalRead ? DoMCOperation.ReadingSocketsExternal : DoMCOperation.ReadingSockets;
                                    if ((await DoMCEquipmentCommands.ReadSockets(MainController, CurrentContext, WorkingLog, ExternalRead)).Item1)
                                    {
                                        ProgressbarStep++;
                                        //CurrentOperation = DoMCOperation.GettingImages;
                                        var si = await DoMCEquipmentCommands.GetSocketsImages(MainController, CurrentContext, WorkingLog);
                                        ProgressbarStep++;
                                        for (int socketNum = 0; socketNum < socketMax; socketNum++)
                                        {
                                            if (si.Item2[socketNum] != null)
                                                img[socketNum][repeat] = si.Item2[socketNum].Image;

                                        }
                                    }
                                    StandardPictures[repeat].Image = bmpCheckSign;
                                }
                                await Task.Yield();

                                ProgressbarStep++;
                                //CurrentOperation = DoMCOperation.CreatingStandard;

                                for (int socketNum = 0; socketNum < socketMax; socketNum++)
                                {
                                    if (img[socketNum].Any(im => im == null))
                                    {
                                        CurrentContext.Configuration.ProcessingDataSettings.CCDSocketStandardsImage[socketNum].StandardImage = null;
                                        continue;
                                    }
                                    var avgImg = ImageTools.CalculateAverage(img[socketNum], CurrentContext.Configuration.HardwareSettings.ThresholdAverageToHaveImage);
                                    CurrentContext.Configuration.ProcessingDataSettings.CCDSocketStandardsImage[socketNum].StandardImage = avgImg;
                                }
                                ProgressbarStep++;
                                //CurrentOperation = DoMCOperation.SavingConfiguration;
                                CurrentContext.Configuration.SaveProcessingDataSettings();
                                //CurrentOperation = DoMCOperation.CompleteError;
                                MainController.LastCommand = typeof(DoMC.Classes.Operation.OperationsCompleteWithoutErrors);
                            }
                        }
                    }
                }
                finally
                {
                    await DoMCEquipmentCommands.StopCCD(MainController, CurrentContext, WorkingLog);
                }
                pbAverage.Image = bmpCheckSign;


            }
        }
        private void btnMakeAverage_Click(object sender, EventArgs e)
        {
            if (ChosenSocketNumber == null || SingleSocketImages == null || SingleSocketImages.Any(i => i == null)) return;
            var cardSocket = CurrentContext.EquipmentSocket2CardSocket[ChosenSocketNumber.Value - 1];
            var workingCardSocket = new TCPCardSocket(cardSocket);
            var imagesForStandards = new short[0][,];

            var avgImg = ImageTools.CalculateAverage(SingleSocketImages, CurrentContext.Configuration.HardwareSettings.ThresholdAverageToHaveImage);
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
            if (CurrentOperation.IsRunning)
            {
                try
                {
                    SetPrograssbarPosition(ProgressbarStep);
                    var lastcmd = MainController.LastCommand;
                    if ((lastcmd.FullName?.Contains("CCD") ?? false))
                        lblWorkStatus.Text = MainController.LastCommand?.GetDescriptionOrFullName() ?? "";//CurrentOperation.GetDescriptionOrFullName();
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

            ProgressbarStep = 0;
            pbGettingStandard.Visible = true;
            pbGettingStandard.Value = 0;
            pbGettingStandard.Maximum = PrograssbarMaxStep;

        }

        private void SetPrograssbarPosition(int step)
        {

            pbGettingStandard.Value = step;

        }

        private void SetResultOK()
        {
            RunOnUI(this, () =>
            {
                PrepareProgressBar();
                lblWorkStatus.Text = "Завершено (без ошибок)";
            });
        }
        private void SetResultError()
        {
            RunOnUI(this, () =>
            {
                PrepareProgressBar();
                lblWorkStatus.Text = "Завершено (ошибка)";
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
