using DoMCLib.Classes.Module.CCD.Command;
using DoMCLib.Classes.Module.CCD.Commands.Classes;
using DoMCLib.Classes.Module.CCD;
using DoMCLib.Classes.Module.LCB;
using DoMCModuleControl.Logging;
using DoMCModuleControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DoMCLib.Classes.DoMCApplicationContext;
using DoMCLib.Classes.Module.CCD.Commands;

namespace DoMCLib.Classes
{
    public class DoMCEquipmentCommands
    {
        public async static Task<bool> StartCCD(IMainController Controller, ILogger WorkingLog, out CCDCardDataCommandResponse result, int? SocketNumber = null)
        {
            LastAction = LastCCDAction.Starting;
            if (SocketNumber == null)
            {
                result=await (new DoMCLib.Classes.Module.CCD.Commands.StartCommand(Controller, Controller.GetModule(typeof(CCDCardDataModule)))).;
                var res = Controller.CreateCommandInstance(typeof(CCDCardDataModule.StartCommand))
                    .Wait(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out result);

                if ((result?.CardsNotAnswered().Count ?? 0) > 0)
                {
                    WorkingLog.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", result.CardsNotAnswered())}) не отвечают");
                }
                return res;

            }
            else
            {
                var res = Controller.CreateCommandInstance(typeof(CCDCardDataModule.StartSingleSocketCommand))
                    .Wait((this, SocketNumber.Value), Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out result);

                if ((result?.CardsNotAnswered().Count ?? 0) > 0)
                {
                    WorkingLog.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", result.CardsNotAnswered())}) не отвечают");
                }
                return res;

            }
        }
        public async static Task<bool> StopCCD(IMainController Controller, ILogger WorkingLog, out CCDCardDataCommandResponse result, int? SocketNumber = null)
        {
            LastAction = LastCCDAction.Stopping;
            if (SocketNumber == null)
            {
                var res = Controller.CreateCommandInstance(typeof(CCDCardDataModule.StopCommand))
                    .Wait(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out result);
                if ((result?.CardsNotAnswered().Count ?? 0) > 0)
                {
                    WorkingLog.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", result.CardsNotAnswered())}) не отвечают");
                }
                return res;

            }
            else
            {
                var res = Controller.CreateCommandInstance(typeof(CCDCardDataModule.StopSingleSocketCommand))
                    .Wait((this, SocketNumber.Value), Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out result);
                if ((result?.CardsNotAnswered().Count ?? 0) > 0)
                {
                    WorkingLog.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", result.CardsNotAnswered())}) не отвечают");
                }
                return res;


            }
        }
        public async static Task<bool> LoadCCDConfiguration(IMainController Controller, ILogger WorkingLog, out CCDCardDataCommandResponse result, int? SocketNumber = null, CancellationTokenSource cancellationTokenSource = null)
        {
            LastAction = LastCCDAction.LocadConfig;

            if (SocketNumber == null)
            {
                {
                    WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Передача настроек экспозиции гнезд в модуль плат ПЗС");
                    var res = Controller.CreateCommandInstance(typeof(CCDCardDataModule.SetExpositionCommand))
                        .Wait(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out result);

                    if ((result?.CardsNotAnswered().Count ?? 0) > 0)
                    {
                        WorkingLog.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", result.CardsNotAnswered())}) не отвечают");
                    }
                    if (!res)
                    {
                        return res;
                    }
                }
                {
                    WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Передача настроек чтения гнезд в модуль плат ПЗС");
                    var res = Controller.CreateCommandInstance(typeof(CCDCardDataModule.SetReadingParametersCommand))
                        .Wait(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out result);

                    if ((result?.CardsNotAnswered().Count ?? 0) > 0)
                    {
                        WorkingLog.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", result.CardsNotAnswered())}) не отвечают");
                    }
                    return res;
                }

            }
            else
            {

                {
                    WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Передача настроек экспозиции гнезд в модуль плат ПЗС");
                    var res = Controller.CreateCommandInstance(typeof(SetExpositionToSingleSocketCommand))
                        .Wait((this, SocketNumber.Value), Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out result);
                    if ((result?.CardsNotAnswered().Count ?? 0) > 0)
                    {
                        WorkingLog.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", result.CardsNotAnswered())}) не отвечают");
                    }
                    if (!res)
                    {
                        return res;
                    }
                }

                {
                    WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Передача настроек чтения гнезд в модуль плат ПЗС");
                    var res = Controller.CreateCommandInstance(typeof(SetReadingParametersToSingleSocketCommand))
                        .Wait((this, SocketNumber.Value), Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out result);
                    if ((result?.CardsNotAnswered().Count ?? 0) > 0)
                    {
                        WorkingLog.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", result.CardsNotAnswered())}) не отвечают");
                    }
                    return res;
                }
            }
        }



        public async static Task<bool> StartLCB(IMainController Controller, ILogger WorkingLog)
        {
            WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Подключение к БУС");
            if (!Controller.CreateCommandInstance(typeof(LCBModule.LCBStartCommand))
                .Wait<object>(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out _))
            //if (!WaitForCommand<LCBModule.LCBStartCommand, SetReadingParametersCommandResult>(Controller, WorkingLog, this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, "Подключение к БУС", LoggerLevel.FullDetailedInformation, out SetReadingParametersCommandResult? result))
            {
                /*if (result != null)
                {
                    WorkingLog.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", result.CardsNotAnswered())}) не отвечают");
                }*/
            }
            return true;
        }
        public async static Task<bool> StopLCB(IMainController Controller, ILogger WorkingLog)
        {
            WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Отключение от БУС");
            if (!Controller.CreateCommandInstance(typeof(LCBModule.LCBStopCommand))
                .Wait<object>(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out _))
            //if (!WaitForCommand<LCBModule.LCBStopCommand, SetReadingParametersCommandResult>(Controller, WorkingLog, this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, "Отключение от БУС", LoggerLevel.FullDetailedInformation, out SetReadingParametersCommandResult? result))
            {
                /*if (result != null)
                {
                    WorkingLog.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", result.CardsNotAnswered())}) не отвечают");
                }*/
            }
            return true;
        }
        public async static Task<bool> SetLCBWorkingMode(IMainController Controller, ILogger WorkingLog)
        {
            WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Подключение к БУС");
            var res = Controller.CreateCommandInstance(typeof(LCBModule.SetLCBWorkModeCommand))
                .Wait<bool>(null, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out bool result);
            return res && result;
        }
        public async static bool SetLCBNonWorkingMode(IMainController Controller, ILogger WorkingLog)
        {
            WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Подключение к БУС");
            var res = Controller.CreateCommandInstance(typeof(LCBModule.SetLCBNonWorkModeCommand))
                .Wait<bool>(null, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out bool result);
            return res && result;
        }

        public async static Task<bool> ReadSockets(IMainController Controller, ILogger WorkingLog, bool IsExternalRead, out CCDCardDataCommandResponse result, int? socketNumber = null, CancellationTokenSource cancellationTokenSource = null)
        {
            LastAction = LastCCDAction.Reading;
            if (IsExternalRead)
            {
                if (!socketNumber.HasValue)
                {
                    result = await new SendReadSocketsWithExternalSignalCommand(Controller, Controller.GetModule(typeof(CCDCardDataModule))).ExecuteCommandAsync<CCDCardDataCommandResponse>();
                    return Controller.CreateCommandInstance(typeof(SendReadSocketsWithExternalSignalCommand), typeof(CCDCardDataModule))
                        .Wait(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out result, cancellationTokenSource);
                }
                else
                {
                    return Controller.CreateCommandInstance(typeof(SendReadSingleSocketWithExternalSignalCommand), typeof(CCDCardDataModule))
                        .Wait((this, socketNumber.Value), Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out result, cancellationTokenSource);

                }
            }
            else
            {
                if (!socketNumber.HasValue)
                {
                    result = await new SendReadSocketsCommand(Controller, Controller.GetModule(typeof(CCDCardDataModule))).ExecuteCommandAsync<CCDCardDataCommandResponse>();
                    return Controller.CreateCommandInstance(typeof(SendReadSocketCommand), typeof(CCDCardDataModule))
                        .Wait(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out result, cancellationTokenSource);
                }
                else
                {
                    return Controller.CreateCommandInstance(typeof(SendReadSingleSocketCommand), typeof(CCDCardDataModule))
                        .Wait((this, socketNumber.Value), Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out result, cancellationTokenSource);
                }
            }
        }

        public async static Task<bool> GetSocketsImages(IMainController Controller, ILogger WorkingLog, out GetImageDataCommandResponse result, int? EquipmentSocketNumber = null, CancellationTokenSource cancellationTokenSource = null)
        {
            LastAction = LastCCDAction.GettingImages;
            if (EquipmentSocketNumber == null)
            {
                return Controller.CreateCommandInstance(typeof(GetSocketsImagesDataCommand), typeof(CCDCardDataModule))
                        .Wait(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out result, cancellationTokenSource);
            }
            else
            {
                return Controller.CreateCommandInstance(typeof(GetSingleSocketImageDataCommand), typeof(CCDCardDataModule))
                        .Wait((this, EquipmentSocketNumber.Value), Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out result, cancellationTokenSource);
            }
        }
        public async static Task<bool> TestCards(IMainController Controller, ILogger WorkingLog, out CCDCardDataCommandResponse result)
        {

            var timeout = 5;
            var savedSocketsToCheck = Configuration.HardwareSettings.SocketsToCheck;
            try
            {
                Configuration.HardwareSettings.SocketsToCheck = Enumerable.Range(0, 96).Select(v => true).ToArray();
                var res = Controller.CreateCommandInstance(typeof(StartCommand), typeof(CCDCardDataModule))
                            .Wait(this, timeout, out result);
                var res1 = Controller.CreateCommandInstance(typeof(StopCommand), typeof(CCDCardDataModule))
                            .Wait(this, timeout, out CCDCardDataCommandResponse _);
                return res && res1;
            }
            finally
            {
                Configuration.HardwareSettings.SocketsToCheck = savedSocketsToCheck;
            }
        }
        public async static Task<bool> ResetCards(IMainController Controller, ILogger WorkingLog, int? EquipmentSocketNumber = null)
        {
            var timeout = Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds;
            if (EquipmentSocketNumber == null)
            {
                return Controller.CreateCommandInstance(typeof(GetSocketsImagesDataCommand), typeof(CCDCardDataModule))
                        .Wait(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out GetImageDataCommandResponse result);

                //return WaitForCommand<CCDCardDataModule.ResetCardsCommand, CCDCardDataModule, CCDCardDataCommandResponse>(Controller, WorkingLog, this, timeout, "", LoggerLevel.Critical, out CCDCardDataCommandResponse result);
            }
            else
            {
                return Controller.CreateCommandInstance(typeof(ResetCardsSingleSocketCommand), typeof(CCDCardDataModule))
                        .Wait((this, EquipmentSocketNumber.Value), Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out GetImageDataCommandResponse result);
                //return WaitForCommand<CCDCardDataModule.ResetCardsSingleSocketCommand, CCDCardDataModule, CCDCardDataCommandResponse>(Controller, WorkingLog, (this, EquipmentSocketNumber), timeout, "", LoggerLevel.Critical, out CCDCardDataCommandResponse result);

            }
        }
        public async static Task<bool> SetFastRead(IMainController Controller, ILogger WorkingLog, int? EquipmentSocketNumber = null)
        {
            var timeout = Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds;
            if (EquipmentSocketNumber == null)
            {
                return Controller.CreateCommandInstance(typeof(SetFastReadCommand), typeof(CCDCardDataModule))
                        .Wait(this, Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out CCDCardDataCommandResponse result);
                //return WaitForCommand<CCDCardDataModule.SetFastReadCommand, CCDCardDataModule, GetImageDataCommandResponse>(Controller, WorkingLog, this, timeout, "", LoggerLevel.Critical, out GetImageDataCommandResponse result);
            }
            else
            {
                return Controller.CreateCommandInstance(typeof(SetFastReadSingleSocketCommand), typeof(CCDCardDataModule))
                        .Wait((this, EquipmentSocketNumber.Value), Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, out CCDCardDataCommandResponse result);
                //return WaitForCommand<CCDCardDataModule.SetFastReadSingleSocketCommand, CCDCardDataModule, GetImageDataCommandResponse>(Controller, WorkingLog, (this, EquipmentSocketNumber.Value), timeout, "", LoggerLevel.Critical, out GetImageDataCommandResponse result);

            }
        }
    }
}
