using DoMCLib.Classes.Module.CCD.Commands.Classes;
using DoMCLib.Classes;
using DoMCLib.Tools;
using DoMCModuleControl.Logging;
using DoMCModuleControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DoMCLib.Classes.DoMCApplicationContext.ErrorsReadingData;
using Microsoft.AspNetCore.Mvc;

namespace DoMC.Tools
{
    internal class FullStandardGet
    {
        public async static Task Execute(IMainController MainController, DoMCApplicationContext CurrentContext, ILogger WorkingLog, int SocketQuantity, int ImagesToMakeStandard, Action NextInternalStep, Action<int> NextImageGot, Action OnSuccess)
        {
            short[][][,] img = new short[SocketQuantity][][,];
            List<string> TextResults = new List<string>();
            (bool, CCDCardDataCommandResponse) startResult;
            //CurrentOperation = DoMCOperation.StartCCD;
            if ((startResult = await DoMCEquipmentCommands.StartCCD(MainController, CurrentContext, WorkingLog)).Item1)
            {
                NextInternalStep?.Invoke();
                try
                {
                    if (await DoMCEquipmentCommands.StartLCB(MainController, WorkingLog))
                    {
                        if (await DoMCEquipmentCommands.SetLCBWorkingMode(MainController, WorkingLog))
                        {
                            //CurrentOperation = DoMCOperation.SettingReadingParameters;
                            if ((await DoMCEquipmentCommands.LoadCCDReadingParametersConfiguration(MainController, CurrentContext, WorkingLog)).Item1)
                            {
                                //CurrentOperation = DoMCOperation.SettingExposition;
                                if ((await DoMCEquipmentCommands.LoadCCDExpositionConfiguration(MainController, CurrentContext, WorkingLog)).Item1)
                                {
                                    NextInternalStep?.Invoke();
                                    //CurrentOperation = DoMCOperation.SetFastReading;
                                    if ((await DoMCEquipmentCommands.SetFastRead(MainController, CurrentContext, WorkingLog)).Item1)
                                    {
                                        NextInternalStep?.Invoke();
                                        for (int socketNum = 0; socketNum < SocketQuantity; socketNum++)
                                        {
                                            img[socketNum] = new short[ImagesToMakeStandard][,];
                                        }
                                        for (int repeat = 0; repeat < ImagesToMakeStandard; repeat++)
                                        {
                                            if ((await DoMCEquipmentCommands.ReadSockets(MainController, CurrentContext, WorkingLog, true)).Item1)
                                            {
                                                NextInternalStep?.Invoke();
                                                //CurrentOperation = DoMCOperation.GettingImages;
                                                var si = await DoMCEquipmentCommands.GetSocketsImages(MainController, CurrentContext, WorkingLog);
                                                NextInternalStep?.Invoke();
                                                for (int socketNum = 0; socketNum < SocketQuantity; socketNum++)
                                                {
                                                    if (si.Item2[socketNum] != null)
                                                        img[socketNum][repeat] = si.Item2[socketNum].Image;

                                                }
                                            }
                                            NextImageGot?.Invoke(repeat);
                                        }
                                        await Task.Yield();

                                        NextInternalStep?.Invoke();
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
                                        NextInternalStep?.Invoke();
                                        //CurrentOperation = DoMCOperation.SavingConfiguration;
                                        CurrentContext.Configuration.SaveProcessingDataSettings();
                                        //CurrentOperation = DoMCOperation.CompleteError;
                                        MainController.LastCommand = typeof(DoMC.Classes.Operation.OperationsCompleteWithoutErrors);
                                        OnSuccess?.Invoke();
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
                    await DoMCEquipmentCommands.StopCCD(MainController, CurrentContext, WorkingLog);
                }

            }
        }
    }
}
