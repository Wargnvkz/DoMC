using DataExchangeKernel.ACS_Core;
using DataExchangeKernel.Interface;
using DoMC;
using DoMCLib.Classes;
using DoMCLib.Configuration;
using DoMCLib.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DoMC
{
    public partial class DoMCStandardCreateInterface : Form, IUserKernelInterface
    {
        DoMCInterfaceDataExchange InterfaceDataExchange = null;
        Bitmap bmpCheckSign;
        PictureBox[] StandardPictures;
        int SocketReadRepeat = 0;
        bool SocketCreateCompleted = false;
        int CheckSignSet = -1;
        Thread th;
        Log WorkingLog;
        List<Form> badforms = new List<Form>();
        public DoMCStandardCreateInterface()
        {
            InitializeComponent();
            bmpCheckSign = new Bitmap(100, 100);
            var bmpGraphics = Graphics.FromImage(bmpCheckSign);
            bmpGraphics.DrawString("✓", new Font("Arial", 30), new SolidBrush(Color.LimeGreen), new PointF(0, 0));
            StandardPictures = new PictureBox[3] { pbStandard1, pbStandard2, pbStandard3 };
            WorkingLog = new Log(Log.LogModules.MainSystem);
        }
        public void SetMemoryReference(GlobalMemory memory)
        {
            InterfaceDataExchange = memory?.OverallMemory[ApplicationCardParameters.DoMCCardControlInstance] as DoMCInterfaceDataExchange;
            if (InterfaceDataExchange == null)
            {
                DoMCNotFoundErrorMessage();
                btnCreateStandard.Enabled = false;
                return;
            }
            else
            {
                btnCreateStandard.Enabled = true;
            }

        }

        private void DoMCNotFoundErrorMessage()
        {
            DisplayMessage.Show("Не найдено устройство управления платами ПМК", "Ошибка");
            this.Close();
        }

        private void btnCreateStandard_Click(object sender, EventArgs e)
        {
            if (InterfaceDataExchange == null) return;
            if (th != null && th.ThreadState == ThreadState.Running) return;
            if (InterfaceDataExchange.IsWorkingModeStarted)
            {
                DisplayMessage.Show("Работа ПМК не остановлена", "Ошибка");
                return;
            }
            WorkingLog?.Add("Начало создания эталона");
            try
            {
                ResetCheckSigns();
                SocketReadRepeat = -1;
                SocketCreateCompleted = false;
                CheckSignSet = -1;
                btnCreateStandard.Enabled = false;
                th = new Thread(ReadImages);
                th.Start();
                tmCheckSignShow.Enabled = true;
            }
            catch
            {
                tmCheckSignShow.Enabled = false;
                btnCreateStandard.Enabled = true;
            }
        }

        private void StopReading()
        {
            StopLCBWorking();
            InterfaceDataExchange.StopCCD();
        }

        private void ReadImages(object o)
        {
            try
            {
                if (InterfaceDataExchange == null) return;
                var ImageNumberToCalcStandard = 3;

                InterfaceDataExchange.CardsConnection.PacketLogActive = InterfaceDataExchange.Configuration.LogPackets;

                //var socket = Configuration.SocketToCardSocketConfigurations[TempStandardSocketNumber];
                foreach (var socket in InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations)
                {
                    socket.Value.DataType = 0;
                    socket.Value.TempImages = new short[ImageNumberToCalcStandard][,];
                }


                var loadresult = InterfaceDataExchange.LoadCCDConfigurationAndStart();
                if (loadresult != DoMCInterfaceDataExchange.LoadError.None)
                {
                    WorkingLog.Add("Ошибка загрузки конфигураций гнезд. Остановка работы");
                    InterfaceDataExchange.Errors.CCDNotRespond = true;
                    DoMCNotAbleLoadConfigurationErrorMessage(loadresult);
                    return;
                }

                WorkLCBStart();
                InterfaceDataExchange.SendResetToCCDCards();
                Thread.Sleep(200);


                //Application.DoEvents();


                InterfaceDataExchange.CCDDataEchangeStatuses.ExternalStart = true;
                //InterfaceDataExchange.SocketsToSave = new int[] { TempStandardSocketNumber };
                for (int repeat = 0; repeat < ImageNumberToCalcStandard; repeat++)
                {
                    WorkingLog?.Add($"Запрос на чтение гнезд: {repeat}");
                    DataExchangeKernel.Log.Add(new Guid(), $"Начало чтения гнезда", true);
                    bool rc = true;

                    InterfaceDataExchange.SendCommand(ModuleCommand.StartReadExternal);
                    rc = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () =>
                    {
                        return InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartReadExternal && InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Complete;
                    }, () => InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
                    if (!rc)
                    {
                        InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                        DoMCNotAbleToReadSocketErrorMessage();
                        return;
                    }

                    WorkingLog?.Add($"Запрос изображения");
                    DataExchangeKernel.Log.Add(new Guid(), $"Начало чтения картинки", true);
                    InterfaceDataExchange.SendCommand(ModuleCommand.GetSocketImages);
                    var ri = UserInterfaceControls.Wait(5 * InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () =>
                    {
                        var mst = InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus;
                        var stp = InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep;
                        return mst == ModuleCommand.GetSocketImages && (stp == ModuleCommandStep.Complete || stp == ModuleCommandStep.Error);
                    }, () => InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
                    if (!ri || InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Error)
                    {
                        InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                        DoMCNotAbleToReadSocketErrorMessage();
                        return;
                    }
                    DataExchangeKernel.Log.Add(new Guid(), $"Картинка получена", true);


                    var images = InterfaceDataExchange.CCDDataEchangeStatuses.Images;
                    if (images is null)
                    {
                        DoMCNotAbleToReadSocketErrorMessage();
                        break;
                    }
                    for (int i = 0; i < InterfaceDataExchange.Configuration.SocketQuantity; i++)
                    {
                        var socket = InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations[i + 1];
                        socket.TempImages[repeat] = images[i];
                    }
                    SocketReadRepeat = repeat;
                    //Application.DoEvents();
                }

                // Здесь пытаюсь убрать те эталоны, которые слишком отличаются от всех остальных
                // Для этого строю таблицу соответствий между всеми изображениями и отбрасываю те, которые не соответствуют большинству
                bool CreateStandardError = false;
                for (int s = 0; s < InterfaceDataExchange.Configuration.SocketQuantity; s++)
                {
                    if (!InterfaceDataExchange.CardsConnection[s].SocketIsInUseForCheck) continue;
                    var socket = InterfaceDataExchange.Configuration.SocketToCardSocketConfigurations[s + 1];

                    #region with operations
                    var badCounter = new int[ImageNumberToCalcStandard];

                    var ipp = socket.ImageProcessParameters;
                    if (ipp != null && ipp.Decisions == null)
                    {
                        ipp.Decisions[0].ParameterCompareGoodIfLess *= 2;
                        ipp.Decisions[1].ParameterCompareGoodIfLess *= 2;
                    }
                    else
                    {
                        ipp.Decisions = new MakeDecision[2];
                        ipp.Decisions[0] = new MakeDecision();
                        ipp.Decisions[0].Operations = new List<DecisionOperation>();
                        ipp.Decisions[0].Operations.Add(new DecisionOperation() { OperationType = DecisionOperationType.Difference });
                        ipp.Decisions[0].Result = DecisionActionResult.Color;
                        ipp.Decisions[0].ParameterCompareGoodIfLess = 2000;
                        ipp.Decisions[1].Operations.Add(new DecisionOperation() { OperationType = DecisionOperationType.Difference });
                        ipp.Decisions[1].Result = DecisionActionResult.Defect;
                        ipp.Decisions[1].ParameterCompareGoodIfLess = 2000;
                    }
                    #endregion

                    for (int imgX = 0; imgX < ImageNumberToCalcStandard; imgX++)
                    {
                        for (int img_Y = imgX + 1; img_Y < ImageNumberToCalcStandard; img_Y++)
                        {
                            var imgres=ImageTools.CheckIfSocketGood(socket.TempImages[imgX], socket.TempImages[img_Y], ipp);
                            if (!imgres.IsSocketGood)
                            {
                                badCounter[imgX]++;
                                badCounter[img_Y]++;
                            }
                        }
                    }
                    /*
                    #region Old compare
                    List<short[,]> DeltaImages = new List<short[,]>(); // Разницы картинок
                    List<Tuple<int, int>> ImagesXY = new List<Tuple<int, int>>(); // Какие гнезда сравнивались, индекс соответствует DeltaIndex
                    for (int imgX = 0; imgX < ImageNumberToCalcStandard; imgX++)
                    {
                        for (int img_Y = imgX + 1; img_Y < ImageNumberToCalcStandard; img_Y++)
                        {
                            var dXY = ImageTools.GetDifference(socket.TempImages[imgX], socket.TempImages[img_Y]);
                            ImagesXY.Add(new Tuple<int, int>(imgX, img_Y));
                            DeltaImages.Add(dXY);

                        }
                    }



                    var imgdev = ImageTools.CalculateDeviationFull(DeltaImages.ToArray(), socket.ImageProcessParameters.GetRectangle());
                    var badCounter = new int[ImageNumberToCalcStandard];
                    for (int imgN = 0; imgN < DeltaImages.Count; imgN++)
                    {
                        var XY = ImagesXY[imgN];
                        if (imgdev.SocketAverage[imgN] > socket.ImageProcessParameters.MaxAverage * 2)//|| Math.Max(Math.Abs(imgdev.Max[imgN]), Math.Abs(imgdev.Min[imgN])) > 3000)
                        {
                            badCounter[XY.Item1]++;
                            badCounter[XY.Item2]++;
                        }
                    }
                    #endregion
                    */
                    List<short[,]> ImagesForStandards = new List<short[,]>();
                    for (int imgN = 0; imgN < ImageNumberToCalcStandard; imgN++)
                    {
                        if (badCounter[imgN] < ImageNumberToCalcStandard - 1)
                            ImagesForStandards.Add(socket.TempImages[imgN]);
                    }
                    if (ImagesForStandards.Count < 2)
                    {
                        for (int imgN = 0; imgN < ImageNumberToCalcStandard; imgN++)
                        {
                            var form1 = new ShowFrameForm();
                            form1.Image = socket.TempImages[imgN];
                            form1.Text = $"Гнездо: {s + 1}; Проход: {imgN}";
                            form1.Show();
                            badforms.Add(form1);
                        }
                        CreateStandardError = true;
                        break;
                    }
                    var AvgIm = ImageTools.CalculateAverage(ImagesForStandards.ToArray());
                    socket.TempAverageImage = AvgIm;
                    socket.StandardImage = AvgIm;
                }
                if (!CreateStandardError)
                {
                    InterfaceDataExchange.Configuration.Save();
                    SocketCreateCompleted = true;
                    DisplayMessage.Show("Эталоны по всем гнездам созданы.", "Завершено");
                }
                else
                {

                    DisplayMessage.Show("Невозможно создать эталон, потому что изображения слишком сильно различаются", "Ошибка");
                }
            }
            catch (ThreadAbortException tae)
            {

            }
            catch (Exception ex)
            {
                DisplayMessage.Show(ex.Message, "Ошибка");
                WorkingLog?.Add("Создание эталонов. ", ex);
            }
            finally
            {
                StopReading();
            }
        }
        /*
        private bool LoadConfiguration()
        {
            bool res = false;
            //InterfaceDataExchange.Configuration = InterfaceDataExchange.Configuration;
            InterfaceDataExchange.CCDDataEchangeStatuses.IsConfigurationLoaded = false;
            InterfaceDataExchange.CCDDataEchangeStatuses.IsNetworkCardSet = false;
            InterfaceDataExchange.LEDStatus.LСBInitialized = false;
            InterfaceDataExchange.LEDStatus.LCBConfigurationLoaded = false;

            if (!InterfaceDataExchange.IsCCDConfigurationFull) { DoMCNotAbleLoadConfigurationErrorMessage("Платы ПЗС не полностью сконфигурированы"); return false; }

            if ((InterfaceDataExchange?.CardsConnection ?? null) != null)
            {
                InterfaceDataExchange.CardsConnection.PacketLogActive = InterfaceDataExchange.Configuration.LogPackets;
            }


            InterfaceDataExchange.SendCommand(ModuleCommand.SetAllCardsAndSocketsConfiguration);
            res = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () => InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.SetAllCardsAndSocketsConfiguration && InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Complete, () => InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
            if (!res)
            {
                InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                DoMCNotAbleLoadConfigurationErrorMessage("Платы ПЗС не ответили");
                return false;
            }
            InterfaceDataExchange.CCDDataEchangeStatuses.IsNetworkCardSet = true;

            InterfaceDataExchange.SendCommand(ModuleCommand.LoadConfiguration);
            res = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () => InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.LoadConfiguration && InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Complete || InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Error, () => InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
            if (!res || InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Error)
            {
                InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                DoMCNotAbleLoadConfigurationErrorMessage("Платы ПЗС не ответили");
                return false;
            }
            InterfaceDataExchange.CCDDataEchangeStatuses.IsConfigurationLoaded = true;



            //InterfaceDataExchange.CardsConnection.PacketLogActive = false;
            InterfaceDataExchange.SendCommand(ModuleCommand.InitLCB);
            res = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () => InterfaceDataExchange.InitLCBStatus == 2);
            if (!res)
            {
                InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                DoMCNotAbleLoadConfigurationErrorMessage("БУС не ответил");
                return false;
            }
            InterfaceDataExchange.LEDStatus.LСBInitialized = true;


            if (!InterfaceDataExchange.IsLEDConfiguartionFull) return false;
            // Загрузить в БУС параметры работы и перевести в рабочий режим

            InterfaceDataExchange.SendCommand(ModuleCommand.SetLCBCurrentRequest);
            res = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () => InterfaceDataExchange.LEDStatus.NumberOfLastCommandSent == 1 && InterfaceDataExchange.LEDStatus.LastCommandReceivedStatusIsOK);
            if (!res)
            {
                InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                DoMCNotAbleLoadConfigurationErrorMessage("БУС не отвечает");
                return false;
            }


            InterfaceDataExchange.SendCommand(ModuleCommand.SetLCBMovementParametersRequest);
            res = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () => InterfaceDataExchange.InitLCBStatus == 2, () => InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.StartIdle);
            if (!res)
            {
                InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                DoMCNotAbleLoadConfigurationErrorMessage("БУС не отвечает");
                return false;
            }

            return res;
        }
        */
        private void DoMCNotAbleLoadConfigurationErrorMessage(string ErrorMessage)
        {
            DisplayMessage.Show($"Не могу загрузить конфигурацию в плату ({ErrorMessage})", "Ошибка");
            return;
        }
        private void DoMCNotAbleLoadConfigurationErrorMessage(DoMCInterfaceDataExchange.LoadError error)
        {
            MessageBox.Show($"Не могу загрузить конфигурацию в плату ({error})", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        private void StopLCBWorking()
        {
            var start = DateTime.Now;
            InterfaceDataExchange.SendCommand(ModuleCommand.SetLCBNonWorkModeRequest);
            var res = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForLCBCardAnswerTimeout, () => InterfaceDataExchange.LEDStatus.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.SetLCBWorkModeResponse && InterfaceDataExchange.LEDStatus.LastCommandResponseReceived > start);
            if (!res)
            {
                DisplayMessage.Show("Не удалось остановить БУС", "Ошибка");
                return;
            }
            start = DateTime.Now;
            InterfaceDataExchange.SendCommand(ModuleCommand.LCBStop);
            //InterfaceDataExchange.SendCommand(ModuleCommand.StartIdle);
            //InterfaceDataExchange.SendResetToCCDCards();
            Thread.Sleep(200);
        }
        private void WorkLCBStart()
        {
            var start = DateTime.Now;
            InterfaceDataExchange.SendCommand(ModuleCommand.InitLCB);
            var res = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForCCDCardAnswerTimeout, () => InterfaceDataExchange.InitLCBStatus == 2);
            if (!res)
            {
                InterfaceDataExchange.CCDDataEchangeStatuses.ModuleStep = ModuleCommandStep.Error;
                throw new Exception("Не удалось подключиться к БУС");
            }
            InterfaceDataExchange.SendCommand(ModuleCommand.SetLCBWorkModeRequest);
            res = UserInterfaceControls.Wait(InterfaceDataExchange.Configuration.Timeouts.WaitForLCBCardAnswerTimeout, () => InterfaceDataExchange.LEDStatus.NumberOfLastCommandReceived == (int)DoMCLib.Classes.LEDCommandType.SetLCBWorkModeResponse && InterfaceDataExchange.LEDStatus.LastCommandResponseReceived > start);
            if (!res)
            {
                DisplayMessage.Show("Не удалось запустить БУС", "Ошибка");
                return;
            }

        }
        private void DoMCNotAbleToReadSocketErrorMessage()
        {
            DisplayMessage.Show("Не удалось прочитать гнездо", "Ошибка");
            return;
        }

        private void ResetCheckSigns()
        {
            for (int i = 0; i < StandardPictures.Length; i++) StandardPictures[i].Image = null;
            pbStandardSum.Image = null;
        }

        private void tmCheckSignShow_Tick(object sender, EventArgs e)
        {
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
        }

        private void DoMCStandardCreateInterface_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Firebrick, ButtonBorderStyle.Solid);
        }

        private void DoMCStandardCreateInterface_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (th != null && th.ThreadState == ThreadState.Running)
            {
                th.Abort();
                StopReading();
            }
        }
    }
}
