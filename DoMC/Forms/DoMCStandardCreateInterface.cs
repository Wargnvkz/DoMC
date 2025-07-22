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

        IMainController Controller;
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

        public DoMCStandardCreateInterface(IMainController controller, DoMCApplicationContext context, ILogger logger)
        {
            InitializeComponent();
            Controller = controller;
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
            WorkingLog?.Add(LoggerLevel.Critical, "Начало создания эталона");
            try
            {
                ResetCheckSigns();
                SocketReadRepeat = -1;
                SocketCreateCompleted = false;
                CheckSignSet = -1;
                btnCreateStandard.Enabled = false;
                var tsk = new Task(FullStandardGet);
                tsk.Start();
                tmCheckSignShow.Enabled = true;
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
                await DoMCEquipmentCommands.StopCCD(Controller, CurrentContext, WorkingLog);
            }
            catch { }
            try
            {
                await DoMCEquipmentCommands.StopLCB(Controller, WorkingLog);
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

        private async void FullStandardGet()
        {
            PictureBox[] StandardPictures = new PictureBox[3] { pbStandard1, pbStandard2, pbStandard3 };
            short[][][,] img = new short[CurrentContext.Configuration.HardwareSettings.SocketQuantity][][,];
            for (int i = 0; i < StandardPictures.Length; i++) StandardPictures[i].Image = null;
            pbStandardSum.Image = null;
            int socketMax = CurrentContext.Configuration.HardwareSettings.SocketQuantity;
            errorReadingData.Clear();

            var startResult = await DoMCEquipmentCommands.StartCCD(Controller, CurrentContext, WorkingLog);
            if (startResult.Item1)
            {
                try
                {
                    if (await DoMCEquipmentCommands.StartLCB(Controller, WorkingLog))
                    {
                        if (await DoMCEquipmentCommands.SetLCBWorkingMode(Controller, WorkingLog))
                        {
                            if ((await DoMCEquipmentCommands.LoadCCDReadingParametersConfiguration(Controller, CurrentContext, WorkingLog)).Item1)
                            {
                                if ((await DoMCEquipmentCommands.LoadCCDExpositionConfiguration(Controller, CurrentContext, WorkingLog)).Item1)
                                {
                                    if ((await DoMCEquipmentCommands.SetFastRead(Controller, CurrentContext, WorkingLog)).Item1)
                                    {
                                        for (int socketNum = 0; socketNum < socketMax; socketNum++)
                                        {
                                            img[socketNum] = new short[ImagesToMakeStandard][,];
                                        }
                                        int StandartImageNumReading = 0;
                                        for (int repeat = 0; repeat < MaxImagesReadToMakeStandard && StandartImageNumReading < ImagesToMakeStandard; repeat++)
                                        {
                                            if ((await DoMCEquipmentCommands.ReadSockets(Controller, CurrentContext, WorkingLog, true)).Item1)
                                            {
                                                var getImageResult = await DoMCEquipmentCommands.GetSocketsImages(Controller, CurrentContext, WorkingLog);
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
                                            var avgImg = ImageTools.CalculateAverage(img[socketNum],CurrentContext.Configuration.HardwareSettings.ThresholdAverageToHaveImage);
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
                                    var msg = "Не удалось загрузить конфигурацию в платы ПЗС";
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
        }

        private void ResetCheckSigns()
        {
            // for (int i = 0; i < StandardPictures.Length; i++) StandardPictures[i].Image = null;
            pbStandardSum.Image = null;
        }

        private void tmCheckSignShow_Tick(object sender, EventArgs e)
        {
            /*
            if (!(th?.IsAlive ?? false))
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
    }
}
