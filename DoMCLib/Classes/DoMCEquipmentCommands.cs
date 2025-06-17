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
using Microsoft.AspNetCore.Mvc;

namespace DoMCLib.Classes
{
    public static class DoMCEquipmentCommands
    {
        static LastCCDAction LastAction;

        #region CCD
        public async static Task<(bool, CCDCardDataCommandResponse)> StartCCD(IMainController Controller, DoMCApplicationContext context, ILogger WorkingLog, int? SocketNumber = null)
        {
            LastAction = LastCCDAction.Starting;
            if (SocketNumber == null)
            {
                return await ExecuteCCDCommandForAllCards(WorkingLog, async () =>
                 await (new DoMCLib.Classes.Module.CCD.Commands.StartCommand(Controller, Controller.GetModule(typeof(CCDCardDataModule)))).ExecuteCommandAsync(context)
                );

            }
            else
            {
                return await ExecuteCCDCommandForSingleSocket(WorkingLog, SocketNumber.Value, async (socketNumber) =>
                 await (new DoMCLib.Classes.Module.CCD.Commands.StartSingleSocketCommand(Controller, Controller.GetModule(typeof(CCDCardDataModule)))).ExecuteCommandAsync((socketNumber, context))
                );
            }
        }

        public async static Task<(bool, CCDCardDataCommandResponse)> StopCCD(IMainController Controller, DoMCApplicationContext context, ILogger WorkingLog, int? SocketNumber = null)
        {
            LastAction = LastCCDAction.Stopping;
            if (SocketNumber == null)
            {
                return await ExecuteCCDCommandForAllCards(WorkingLog, async () =>
                 await (new DoMCLib.Classes.Module.CCD.Commands.StopCommand(Controller, Controller.GetModule(typeof(CCDCardDataModule)))).ExecuteCommandAsync(context)
                );

            }
            else
            {
                return await ExecuteCCDCommandForSingleSocket(WorkingLog, SocketNumber.Value, async (socketNumber) =>
                 await (new DoMCLib.Classes.Module.CCD.Commands.StopSingleSocketCommand(Controller, Controller.GetModule(typeof(CCDCardDataModule)))).ExecuteCommandAsync((socketNumber, context))
                );
            }

        }

        public async static Task<(bool, CCDCardDataCommandResponse)> LoadCCDConfiguration(IMainController Controller, DoMCApplicationContext context, ILogger WorkingLog, int? SocketNumber = null, CancellationTokenSource cancellationTokenSource = null)
        {
            LastAction = LastCCDAction.LoadConfig;

            if (SocketNumber == null)
            {
                WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Передача настроек экспозиции гнезд в модуль плат ПЗС");
                var res = await ExecuteCCDCommandForAllCards(WorkingLog, async () =>
                    await (new DoMCLib.Classes.Module.CCD.Commands.SendCommandSetSocketsExpositionParametersCommand(Controller, Controller.GetModule(typeof(CCDCardDataModule)))).ExecuteCommandAsync(context)
                );
                if (!res.Item1)
                {
                    return res;
                }
                WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Передача настроек чтения гнезд в модуль плат ПЗС");
                return await ExecuteCCDCommandForAllCards(WorkingLog, async () =>
                    await (new DoMCLib.Classes.Module.CCD.Commands.SendCommandSetSocketReadingParametersCommand(Controller, Controller.GetModule(typeof(CCDCardDataModule)))).ExecuteCommandAsync(context)
                );


            }
            else
            {

                WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Передача настроек экспозиции гнезд в модуль плат ПЗС");
                var res = await ExecuteCCDCommandForSingleSocket(WorkingLog, SocketNumber.Value, async (socketNumber) =>
                    await (new DoMCLib.Classes.Module.CCD.Commands.SetExpositionToSingleSocketCommand(Controller, Controller.GetModule(typeof(CCDCardDataModule)))).ExecuteCommandAsync((socketNumber, context))
                );
                if (!res.Item1)
                {
                    return res;
                }
                WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Передача настроек чтения гнезд в модуль плат ПЗС");
                return await ExecuteCCDCommandForSingleSocket(WorkingLog, SocketNumber.Value, async (socketNumber) =>
                    await (new DoMCLib.Classes.Module.CCD.Commands.SetReadingParametersToSingleSocketCommand(Controller, Controller.GetModule(typeof(CCDCardDataModule)))).ExecuteCommandAsync((socketNumber, context))
                );

            }
        }


        public async static Task<(bool, CCDCardDataCommandResponse)> ReadSockets(IMainController Controller, DoMCApplicationContext context, ILogger WorkingLog, bool IsExternalRead, int? SocketNumber = null)
        {
            LastAction = LastCCDAction.Reading;
            if (IsExternalRead)
            {
                if (SocketNumber == null)
                {
                    return await ExecuteCCDCommandForAllCards(WorkingLog, async () =>
                     await (new DoMCLib.Classes.Module.CCD.Commands.SendReadAllSocketsWithExternalSignalCommand(Controller, Controller.GetModule(typeof(CCDCardDataModule)))).ExecuteCommandAsync(context)
                    );

                }
                else
                {
                    return await ExecuteCCDCommandForSingleSocket(WorkingLog, SocketNumber.Value, async (socketNumber) =>
                     await (new DoMCLib.Classes.Module.CCD.Commands.SendReadSingleSocketWithExternalSignalCommand(Controller, Controller.GetModule(typeof(CCDCardDataModule)))).ExecuteCommandAsync((socketNumber, context))
                    );
                }
            }
            else
            {
                if (SocketNumber == null)
                {
                    return await ExecuteCCDCommandForAllCards(WorkingLog, async () =>
                     await (new DoMCLib.Classes.Module.CCD.Commands.SendReadAllSocketsCommand(Controller, Controller.GetModule(typeof(CCDCardDataModule)))).ExecuteCommandAsync(context)
                    );

                }
                else
                {
                    return await ExecuteCCDCommandForSingleSocket(WorkingLog, SocketNumber.Value, async (socketNumber) =>
                     await (new DoMCLib.Classes.Module.CCD.Commands.SendReadSingleSocketCommand(Controller, Controller.GetModule(typeof(CCDCardDataModule)))).ExecuteCommandAsync((socketNumber, context))
                    );
                }
            }
        }
        public async static Task<(bool, CCDCardDataCommandResponse)> ReadSockets(IMainController Controller, ILogger WorkingLog, bool IsExternalRead, out CCDCardDataCommandResponse result, int? socketNumber = null, CancellationTokenSource cancellationTokenSource = null)
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

        public async static Task<(bool, GetImageDataCommandResponse)> GetSocketsImages(IMainController Controller, ILogger WorkingLog, out GetImageDataCommandResponse result, int? EquipmentSocketNumber = null, CancellationTokenSource cancellationTokenSource = null)
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
        public async static Task<(bool, CCDCardDataCommandResponse)> TestCards(IMainController Controller, ILogger WorkingLog)
        {
            return await ExecuteCCDCommandForAllCards(WorkingLog, async () =>
                    await (new DoMCLib.Classes.Module.CCD.Commands.TestConnectionCardsCommand(Controller, Controller.GetModule(typeof(CCDCardDataModule)))).ExecuteCommandAsync()
                   );


        }
        public async static Task<(bool, CCDCardDataCommandResponse)> ResetCards(IMainController Controller, ILogger WorkingLog, int? SocketNumber = null)
        {
            LastAction = LastCCDAction.LoadConfig;
            if (SocketNumber == null)
            {
                return await ExecuteCCDCommandForAllCards(WorkingLog, async () =>
                 await (new DoMCLib.Classes.Module.CCD.Commands.ResetCardsCommand(Controller, Controller.GetModule(typeof(CCDCardDataModule)))).ExecuteCommandAsync(context)
                );

            }
            else
            {
                return await ExecuteCCDCommandForSingleSocket(WorkingLog, SocketNumber.Value, async (socketNumber) =>
                 await (new DoMCLib.Classes.Module.CCD.Commands.ResetCardsSingleSocketCommand(Controller, Controller.GetModule(typeof(CCDCardDataModule)))).ExecuteCommandAsync((socketNumber, context))
                );
            }

        }
        public async static Task<(bool, CCDCardDataCommandResponse)> SetFastRead(IMainController Controller, DoMCApplicationContext context, ILogger WorkingLog, int? SocketNumber = null)
        {
            LastAction = LastCCDAction.LoadConfig;
            if (SocketNumber == null)
            {
                return await ExecuteCCDCommandForAllCards(WorkingLog, async () =>
                 await (new DoMCLib.Classes.Module.CCD.Commands.SetFastReadCommand(Controller, Controller.GetModule(typeof(CCDCardDataModule)))).ExecuteCommandAsync(context)
                );

            }
            else
            {
                return await ExecuteCCDCommandForSingleSocket(WorkingLog, SocketNumber.Value, async (socketNumber) =>
                 await (new DoMCLib.Classes.Module.CCD.Commands.SetFastReadSingleSocketCommand(Controller, Controller.GetModule(typeof(CCDCardDataModule)))).ExecuteCommandAsync((socketNumber, context))
                );
            }

        }
        #endregion


        #region LCB

        public async static Task<bool> StartLCB(IMainController Controller, ILogger WorkingLog)
        {
            WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Подключение к БУС");
            try
            {
                await new DoMCLib.Classes.Module.LCB.Commands.LCBStartCommand(Controller, Controller.GetModule(typeof(DoMCLib.Classes.Module.LCB.LCBModule))).ExecuteCommandAsync();
                return true;
            }
            catch
            {
                return false;
            }

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


        #endregion
        #region Helpers
        public static async Task<(bool, CCDCardDataCommandResponse)> ExecuteCCDCommandForAllCards(ILogger WorkingLog, Func<Task<CCDCardDataCommandResponse>> func)
        {
            try
            {
                var result = await func();
                if ((result?.CardsNotAnswered().Count ?? 0) > 0)
                {
                    WorkingLog.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", result.CardsNotAnswered())}) не отвечают");
                }
                return (true, result);
            }
            catch { return (false, null); }
        }
        public static async Task<(bool, CCDCardDataCommandResponse)> ExecuteCCDCommandForSingleSocket(ILogger WorkingLog, int SocketNumber, Func<int, Task<bool>> func)
        {
            try
            {
                var result = await func(SocketNumber);
                if (!result)
                {
                    WorkingLog.Add(LoggerLevel.Critical, $"Не удалось выполнить команду для гнезда {SocketNumber}");
                    return (false, null);
                }
                return (true, null);
            }
            catch { return (false, null); }
        }
        #endregion
    }
}
