﻿using DoMCLib.Classes.Module.CCD.Commands.Classes;
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

        public async static Task<(bool, CCDCardDataCommandResponse)> LoadCCDExpositionConfiguration(IMainController Controller, DoMCApplicationContext context, ILogger WorkingLog, int? SocketNumber = null, CancellationTokenSource cancellationTokenSource = null)
        {
            LastAction = LastCCDAction.LoadConfigExposition;

            if (SocketNumber == null)
            {
                WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Передача настроек экспозиции гнезд в модуль плат ПЗС");
                return await ExecuteCCDCommandForAllCards(WorkingLog, async () =>
                    await (new DoMCLib.Classes.Module.CCD.Commands.SendCommandSetSocketsExpositionParametersCommand(Controller, Controller.GetModule(typeof(CCDCardDataModule)))).ExecuteCommandAsync(context)
                );
            }
            else
            {

                WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Передача настроек экспозиции гнезд в модуль плат ПЗС");
                return await ExecuteCCDCommandForSingleSocket(WorkingLog, SocketNumber.Value, async (socketNumber) =>
                    await (new DoMCLib.Classes.Module.CCD.Commands.SetExpositionToSingleSocketCommand(Controller, Controller.GetModule(typeof(CCDCardDataModule)))).ExecuteCommandAsync((socketNumber, context))
                );
            }
        }

        public async static Task<(bool, CCDCardDataCommandResponse)> LoadCCDReadingParametersConfiguration(IMainController Controller, DoMCApplicationContext context, ILogger WorkingLog, int? SocketNumber = null, CancellationTokenSource cancellationTokenSource = null)
        {
            LastAction = LastCCDAction.LoadConfigReadingParameters;

            if (SocketNumber == null)
            {

                WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Передача настроек чтения гнезд в модуль плат ПЗС");
                return await ExecuteCCDCommandForAllCards(WorkingLog, async () =>
                    await (new DoMCLib.Classes.Module.CCD.Commands.SendCommandSetSocketReadingParametersCommand(Controller, Controller.GetModule(typeof(CCDCardDataModule)))).ExecuteCommandAsync(context)
                );


            }
            else
            {

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


        public async static Task<(bool, GetImageDataCommandResponse)> GetSocketsImages(IMainController Controller, DoMCApplicationContext context, ILogger WorkingLog)
        {
            LastAction = LastCCDAction.GettingImages;
            GetImageDataCommandResponse result = new GetImageDataCommandResponse();
            try
            {
                result = await new CCDAllSocketsImagesCommand(Controller, Controller.GetModule(typeof(CCDCardDataModule))).ExecuteCommandAsync(context);
                if ((result?.CardsAnswered().Count ?? 0) > 0)
                {
                    WorkingLog.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", result.CardsAnswered())}) не отвечают");
                }
                return (true, result);
            }
            catch { return (false, result); }


        }
        public async static Task<SocketReadData?> GetSingleSocketsImage(IMainController Controller, DoMCApplicationContext context, ILogger WorkingLog, int EquipmentSocketNumber, CancellationTokenSource cancellationTokenSource = null)
        {
            LastAction = LastCCDAction.GettingImages;
            try
            {
                var result = await new CCDGetSingleSocketImageCommand(Controller, Controller.GetModule(typeof(CCDCardDataModule))).ExecuteCommandAsync((EquipmentSocketNumber, context));
                return result;
            }
            catch
            {
                WorkingLog.Add(LoggerLevel.Critical, $"Не удалось получить изображение для {EquipmentSocketNumber}");
                return null;
            }
        }
        public async static Task<(bool, CCDCardDataCommandResponse)> TestCards(IMainController Controller, DoMCApplicationContext context, ILogger WorkingLog)
        {
            return await ExecuteCCDCommandForAllCards(WorkingLog, async () =>
                    await (new DoMCLib.Classes.Module.CCD.Commands.TestConnectionCardsCommand(Controller, Controller.GetModule(typeof(CCDCardDataModule)))).ExecuteCommandAsync(context)
                   );


        }
        public async static Task<(bool, CCDCardDataCommandResponse)> ResetCards(IMainController Controller, DoMCApplicationContext context, ILogger WorkingLog, int? SocketNumber = null)
        {
            LastAction = LastCCDAction.Reset;
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
            LastAction = LastCCDAction.SetFastRead;
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
            try
            {
                await new DoMCLib.Classes.Module.LCB.Commands.LCBStopCommand(Controller, Controller.GetModule(typeof(DoMCLib.Classes.Module.LCB.LCBModule))).ExecuteCommandAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async static Task<bool> SetLCBWorkingMode(IMainController Controller, ILogger WorkingLog)
        {
            WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Подключение к БУС");
            try
            {
                await new DoMCLib.Classes.Module.LCB.Commands.SetLCBWorkModeCommand(Controller, Controller.GetModule(typeof(DoMCLib.Classes.Module.LCB.LCBModule))).ExecuteCommandAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async static Task<bool> SetLCBNonWorkingMode(IMainController Controller, ILogger WorkingLog)
        {
            WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Подключение к БУС");
            try
            {
                await new DoMCLib.Classes.Module.LCB.Commands.SetLCBNonWorkModeCommand(Controller, Controller.GetModule(typeof(DoMCLib.Classes.Module.LCB.LCBModule))).ExecuteCommandAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async static Task<bool> SetLCBMovementConfiguration(IMainController Controller, DoMCApplicationContext context, ILogger WorkingLog)
        {
            WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Установка параметров движения БУС");
            try
            {
                await new DoMCLib.Classes.Module.LCB.Commands.SetLCBMovementParametersCommand(Controller, Controller.GetModule(typeof(DoMCLib.Classes.Module.LCB.LCBModule))).ExecuteCommandAsync((context.Configuration.ReadingSocketsSettings.LCBSettings.PreformLength, context.Configuration.ReadingSocketsSettings.LCBSettings.DelayLength));
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async static Task<bool> SetLCBCurrentConfiguration(IMainController Controller, DoMCApplicationContext context, ILogger WorkingLog)
        {
            WorkingLog.Add(LoggerLevel.FullDetailedInformation, "Установка токов светодиодов БУС");
            try
            {
                await new DoMCLib.Classes.Module.LCB.Commands.SetLCBCurrentCommand(Controller, Controller.GetModule(typeof(DoMCLib.Classes.Module.LCB.LCBModule))).ExecuteCommandAsync(context.Configuration.ReadingSocketsSettings.LCBSettings.LEDCurrent);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
        #region Helpers
        public static async Task<(bool, CCDCardDataCommandResponse)> ExecuteCCDCommandForAllCards(ILogger WorkingLog, Func<Task<CCDCardDataCommandResponse>> func)
        {
            CCDCardDataCommandResponse result = new CCDCardDataCommandResponse();
            try
            {
                result = await func();
                if (result == null) return (false, null);
                if ((result?.CardsNotAnswered().Count ?? 0) > 0)
                {
                    WorkingLog.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", result.CardsNotAnswered())}) не отвечают");
                }
                return (result.CardsNotAnswered().Count == 0, result);
            }
            catch (Exception ex)
            {
                WorkingLog.Add(LoggerLevel.Critical, $"Ошибка при выполнении команды", ex);
                return (false, result);
            }
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
            catch (Exception ex)
            {
                WorkingLog.Add(LoggerLevel.Critical, "Ошибка при выполнении команды", ex);

                return (false, null);
            }
        }


        #endregion
    }
}
