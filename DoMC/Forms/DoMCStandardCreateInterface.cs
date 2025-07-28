using DoMC;
using DoMCLib.Classes;
using DoMCLib.Classes.Module.CCD.Commands.Classes;
using DoMCLib.Configuration;
using DoMCLib.Tools;
using DoMCModuleControl;
using DoMCModuleControl.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static DoMCLib.Classes.DoMCApplicationContext.ErrorsReadingData;

namespace DoMC
{
    public partial class DoMCStandardCreateInterface : Form
    {

        IMainController MainController;
        DoMCApplicationContext CurrentContext;
        Bitmap bmpCheckSign;
        PictureBox[] StandardPictures;
        int SocketReadRepeat = 0;
        bool SocketCreateCompleted = false;
        int CheckSignSet = -1;
        ILogger WorkingLog;
        List<Form> badforms = new List<Form>();
        int ImagesToMakeStandard = 3;
        int MaxImagesReadToMakeStandard = 10;
        DoMCApplicationContext.ErrorsReadingData errorReadingData = new DoMCApplicationContext.ErrorsReadingData();

        volatile int ProgressbarStep = 0;
        int PrograssbarMaxStep = 9;
        bool IsGettingStandard = false;

        public DoMCStandardCreateInterface(IMainController controller, DoMCApplicationContext context, ILogger logger)
        {
            InitializeComponent();
            MainController = controller;
            CurrentContext = context;
            WorkingLog = logger;
            bmpCheckSign = new Bitmap(100, 100);
            var bmpGraphics = Graphics.FromImage(bmpCheckSign);
            bmpGraphics.DrawString("✓", new Font("Arial", 30), new SolidBrush(Color.LimeGreen), new PointF(0, 0));
            StandardPictures = new PictureBox[3] { pbStandard1, pbStandard2, pbStandard3 };

        }

        private void DoMCNotFoundErrorMessage()
        {
            DisplayMessage.Show("Не найдено устройство управления платами ПМК", "Ошибка");
            this.Close();
        }

        private void btnCreateStandard_Click(object sender, EventArgs e)
        {
            if (IsGettingStandard) return;
            WorkingLog?.Add(LoggerLevel.Critical, "Начало создания эталона");
            try
            {
                ResetCheckSigns();
                SocketCreateCompleted = false;
                btnCreateStandard.Enabled = false;
                Task.Run(FullStandardGet);
                tmCheckSignShow.Enabled = true;
                IsGettingStandard = true;
            }
            catch
            {
                ResetInterface();
            }

        }

        private async Task StopReading()
        {
            try
            {
                await DoMCEquipmentCommands.StopCCD(MainController, CurrentContext, WorkingLog);
            }
            catch { }
            try
            {
                await DoMCEquipmentCommands.StopLCB(MainController, WorkingLog);
            }
            catch { }
            ResetInterface();
            //InterfaceDataExchange.StopCCD();
        }
        private void ResetInterface()
        {
            this.Invoke(new Action(() =>
            {
                tmCheckSignShow.Enabled = false;
                btnCreateStandard.Enabled = true;
            }));
        }
        private async Task FullStandardGet()
        {
            PictureBox[] StandardPictures = new PictureBox[3] { pbStandard1, pbStandard2, pbStandard3 };
            int SocketQuantity = CurrentContext.Configuration.HardwareSettings.SocketQuantity;
            short[][][,] img = new short[SocketQuantity][][,];
            List<string> TextResults = new List<string>();
            (bool, CCDCardDataCommandResponse) startResult;
            ProgressbarStep = 0;
            //CurrentOperation = DoMCOperation.StartCCD;
            if ((startResult = await DoMCEquipmentCommands.StartCCD(MainController, CurrentContext, WorkingLog)).Item1)
            {
                ProgressbarStep++;
                try
                {
                    if (await DoMCEquipmentCommands.StartLCB(MainController, WorkingLog))
                    {
                        try
                        {
                            if (await DoMCEquipmentCommands.SetLCBWorkingMode(MainController, WorkingLog))
                            {
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
                                                for (int socketNum = 0; socketNum < SocketQuantity; socketNum++)
                                                {
                                                    img[socketNum] = new short[ImagesToMakeStandard][,];
                                                }
                                                for (int repeat = 0; repeat < ImagesToMakeStandard; repeat++)
                                                {
                                                    if ((await DoMCEquipmentCommands.ReadSockets(MainController, CurrentContext, WorkingLog, true)).Item1)
                                                    {
                                                        ProgressbarStep++;
                                                        //CurrentOperation = DoMCOperation.GettingImages;
                                                        var si = await DoMCEquipmentCommands.GetSocketsImages(MainController, CurrentContext, WorkingLog);
                                                        ProgressbarStep++;
                                                        for (int socketNum = 0; socketNum < SocketQuantity; socketNum++)
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

                                                for (int socketNum = 0; socketNum < SocketQuantity; socketNum++)
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
                                                pbStandardSum.Image = bmpCheckSign;
                                            }
                                            else
                                            {
                                                var msg = "Не удалось установить режим быстрого чтения плат ПЗС";
                                                WorkingLog.Add(LoggerLevel.Critical, msg);
                                                MessageBox.Show(msg);
                                            }
                                        }
                                        else
                                        {
                                            var msg = "Не удалось загрузить параметры экспозиции в платы ПЗС";
                                            WorkingLog.Add(LoggerLevel.Critical, msg);
                                            MessageBox.Show(msg);
                                        }
                                    }
                                    else
                                    {
                                        var msg = "Не удалось загрузить параметры чтения в платы ПЗС";
                                        WorkingLog.Add(LoggerLevel.Critical, msg);
                                        MessageBox.Show(msg);
                                    }
                                }
                                finally
                                {
                                    await DoMCEquipmentCommands.SetLCBNonWorkingMode(MainController, WorkingLog);
                                }
                            }
                            else
                            {
                                var msg = "Не удалось установить рабочий режим БУС";
                                WorkingLog.Add(LoggerLevel.Critical, msg);
                                MessageBox.Show(msg);
                            }
                        }
                        finally
                        {
                            await DoMCEquipmentCommands.StopLCB(MainController, WorkingLog);
                        }
                    }
                    else
                    {
                        var msg = "Не удалось запустить модуль работы с БУС";
                        WorkingLog.Add(LoggerLevel.Critical, msg);
                        MessageBox.Show(msg);
                    }
                }
                finally
                {
                    await DoMCEquipmentCommands.StopCCD(MainController, CurrentContext, WorkingLog);
                    IsGettingStandard = false;
                }

            }
        }

        /*private async Task FullStandardGet()
        {
            PictureBox[] StandardPictures = new PictureBox[3] { pbStandard1, pbStandard2, pbStandard3 };
            short[][][,] img = new short[CurrentContext.Configuration.HardwareSettings.SocketQuantity][][,];
            for (int i = 0; i < StandardPictures.Length; i++) StandardPictures[i].Image = null;
            pbStandardSum.Image = null;
            int socketMax = CurrentContext.Configuration.HardwareSettings.SocketQuantity;
            errorReadingData.Clear();

            var startResult = await DoMCEquipmentCommands.StartCCD(MainController, CurrentContext, WorkingLog);
            if (startResult.Item1)
            {
                try
                {
                    if (await DoMCEquipmentCommands.StartLCB(MainController, WorkingLog))
                    {
                        if (await DoMCEquipmentCommands.SetLCBWorkingMode(MainController, WorkingLog))
                        {
                            if ((await DoMCEquipmentCommands.LoadCCDReadingParametersConfiguration(MainController, CurrentContext, WorkingLog)).Item1)
                            {
                                if ((await DoMCEquipmentCommands.LoadCCDExpositionConfiguration(MainController, CurrentContext, WorkingLog)).Item1)
                                {
                                    if ((await DoMCEquipmentCommands.SetFastRead(MainController, CurrentContext, WorkingLog)).Item1)
                                    {
                                        for (int socketNum = 0; socketNum < socketMax; socketNum++)
                                        {
                                            img[socketNum] = new short[ImagesToMakeStandard][,];
                                        }
                                        int StandartImageNumReading = 0;
                                        for (int repeat = 0; repeat < MaxImagesReadToMakeStandard && StandartImageNumReading < ImagesToMakeStandard; repeat++)
                                        {
                                            if ((await DoMCEquipmentCommands.ReadSockets(MainController, CurrentContext, WorkingLog, true)).Item1)
                                            {
                                                var getImageResult = await DoMCEquipmentCommands.GetSocketsImages(MainController, CurrentContext, WorkingLog);
                                                for (int socketNum = 0; socketNum < socketMax; socketNum++)
                                                {
                                                    if (getImageResult.Item2[socketNum] != null)
                                                        img[socketNum][StandartImageNumReading] = getImageResult.Item2[socketNum].Image;
                                                    var errorCards = errorReadingData.ErrorCards();
                                                    if (errorCards.Count > 0)
                                                    {
                                                        for (int c = 0; c < errorCards.Count; c++)
                                                        {
                                                            CurrentContext.Configuration.SetCheckCard(errorCards[c], false);
                                                        }
                                                    }

                                                }
                                                StandardPictures[StandartImageNumReading].Image = bmpCheckSign;
                                                StandartImageNumReading++;
                                            }
                                            else
                                            {

                                            }
                                        }
                                        Parallel.For(0, socketMax, socketNum =>
                                        //for (int socketNum = 0; socketNum < socketMax; socketNum++)
                                        {
                                            if (img[socketNum].Any(im => im == null))
                                            {
                                                CurrentContext.Configuration.ProcessingDataSettings.CCDSocketStandardsImage[socketNum].StandardImage = null;
                                                //continue;
                                            }
                                            var avgImg = ImageTools.CalculateAverage(img[socketNum], CurrentContext.Configuration.HardwareSettings.ThresholdAverageToHaveImage);
                                            CurrentContext.Configuration.ProcessingDataSettings.CCDSocketStandardsImage[socketNum].StandardImage = avgImg;
                                        });

                                        CurrentContext.Configuration.SaveProcessingDataSettings();
                                        pbStandardSum.Image = bmpCheckSign;
                                    }
                                    else
                                    {
                                        var msg = "Не удалось установить режим быстрого чтения плат ПЗС";
                                        WorkingLog.Add(LoggerLevel.Critical, msg);
                                        MessageBox.Show(msg);
                                    }
                                }
                                else
                                {
                                    var msg = "Не удалось загрузить параметры экспозиции в платы ПЗС";
                                    WorkingLog.Add(LoggerLevel.Critical, msg);
                                    MessageBox.Show(msg);
                                }
                            }
                            else
                            {
                                var msg = "Не удалось загрузить параметры чтения в платы ПЗС";
                                WorkingLog.Add(LoggerLevel.Critical, msg);
                                MessageBox.Show(msg);
                            }

                        }
                        else
                        {
                            var msg = "Не удалось установить рабочий режим БУС";
                            WorkingLog.Add(LoggerLevel.Critical, msg);
                            MessageBox.Show(msg);
                        }

                    }
                    else
                    {
                        var msg = "Не удалось запустить модуль работы с БУС";
                        WorkingLog.Add(LoggerLevel.Critical, msg);
                        MessageBox.Show(msg);
                    }
                }
                finally
                {
                    await StopReading();
                }
            }
            else
            {
                var msg = "Не удалось запустить модуль работы с платами ПЗС";
                WorkingLog.Add(LoggerLevel.Critical, msg);
                MessageBox.Show(msg);
            }
        }

        private void DoMCNotAbleLoadConfigurationErrorMessage(string ErrorMessage)
        {
            DisplayMessage.Show($"Не могу загрузить конфигурацию в плату ({ErrorMessage})", "Ошибка");
            return;
        }

        private void DoMCNotAbleToReadSocketErrorMessage()
        {
            DisplayMessage.Show("Не удалось прочитать гнездо", "Ошибка");
            return;
        }*/

        private void ResetCheckSigns()
        {
            // for (int i = 0; i < StandardPictures.Length; i++) StandardPictures[i].Image = null;
            pbStandardSum.Image = null;
        }

        private void tmCheckSignShow_Tick(object sender, EventArgs e)
        {
            if (IsGettingStandard)
            {
                try
                {
                    SetPrograssbarPosition(ProgressbarStep);
                    var lastcmd = MainController.LastCommand;
                    if (lastcmd.FullName?.Contains("CCD") ?? false)
                        lblWorkStatus.Text = MainController.LastCommand?.GetDescriptionOrFullName() ?? "";//CurrentOperation.GetDescriptionOrFullName();
                }
                catch { }
            }



           /* if (!(th?.IsAlive ?? false))
            {
                tmCheckSignShow.Enabled = false;
                btnCreateStandard.Enabled = true;
            }
            if (SocketReadRepeat != CheckSignSet)
            {
                for (int i = 0; i <= SocketReadRepeat; i++)
                    StandardPictures[i].Image = bmpCheckSign;
                CheckSignSet = SocketReadRepeat;
            }
            if (SocketCreateCompleted)
            {
                pbStandardSum.Image = bmpCheckSign;
                //tmCheckSignShow.Enabled = false;
                //btnCreateStandard.Enabled = true;

            }
           */
        }

        private void DoMCStandardCreateInterface_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Firebrick, ButtonBorderStyle.Solid);
        }

        private void DoMCStandardCreateInterface_FormClosed(object sender, FormClosedEventArgs e)
        {
            /*
            if (th != null && th.ThreadState == ThreadState.Running)
            {
                th.Abort();
                StopReadingBecauseOfError();
            }
            */
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
