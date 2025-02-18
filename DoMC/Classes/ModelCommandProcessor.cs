using DoMCLib.Classes.Module.CCD;
using DoMCLib.Classes.Module.CCD.Commands.Classes;
using DoMCLib.Classes.Module.LCB;
using DoMCLib.Classes.Module.RDPB;
using DoMCModuleControl;
using DoMCModuleControl.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DoMCLib.Classes.Module.LCB.LCBModule;
using DoMCApplicationContext = DoMCLib.Classes.DoMCApplicationContext;

namespace DoMC.Classes
{
   /* public class ModelCommandProcessor
    {
        IMainController Controller;
        DoMCApplicationContext CurrentContext;
        Observer observer;
        ILogger logger;
        public ModelCommandProcessor(IMainController controller, DoMCApplicationContext applicationContext)
        {
            Controller = controller;
            CurrentContext = applicationContext;
            observer = Controller.GetObserver();
            //observer.NotificationReceivers += Observer_NotificationReceived;
            logger = Controller.GetLogger("Operations");
        }

        private void Observer_NotificationReceived(string eventName, object? data)
        {
            var eventNameParts = eventName.Split('.');
            if (eventNameParts[0] == typeof(LCBModule).Name)
            {
                if (Enum.TryParse(typeof(LCBModule.EventType), eventNameParts[2], out var et))
                {
                    var eventType = (LCBModule.EventType)et;
                    switch (eventType)
                    {
                        case LCBModule.EventType.Success:
                        case LCBModule.EventType.Error:
                            if (Enum.TryParse(typeof(DoMCLib.Classes.Module.LCB.LCBModule.Operations), eventNameParts[1], out var op))
                            {
                                var ledOperationType = (DoMCLib.Classes.Module.LCB.LCBModule.Operations)op;
                            }
                            break;
                        case LCBModule.EventType.Received:
                            if (Enum.TryParse(typeof(LEDCommandType), eventNameParts[1], out var lct))
                            {
                                var ledCommantType = (LEDCommandType)lct;
                            }
                            break;
                    }
                }
            }
            else
            {
                if (eventNameParts[0] == typeof(RDPBModule).Name)
                {
                }
                else
                {
                    if (eventNameParts[0] == typeof(CCDCardDataModule).Name)
                    {
                    }
                }
            }
        }

        public bool StartLCB()
        {
            var startCmd = Controller.CreateCommandInstance(typeof(LCBModule.LCBStartCommand));
            if (startCmd == null) return false;
            startCmd.ExecuteCommandAsync().Wait();
            return startCmd.WasCompletedSuccessfully();
        }
        public bool StartCCD()
        {
            var startCmd = Controller.CreateCommandInstance(typeof(CCDCardDataModule.StartCommand));
            if (startCmd == null) return false;
            startCmd.ExecuteCommandAsync().Wait();
            return startCmd.WasCompletedSuccessfully();
        }
        public bool StartRDPB()
        {
            var startCmd = Controller.CreateCommandInstance(typeof(RDPBModule.StartCommand));
            if (startCmd == null) return false;
            startCmd.ExecuteCommandAsync().Wait();
            return startCmd.WasCompletedSuccessfully();
        }

        public bool SetNonWorkingModeLCB()
        {
            var StopCmd = Controller.CreateCommandInstance(typeof(LCBModule.LCBStopCommand));
            if (StopCmd == null) return false;
            StopCmd.ExecuteCommandAsync().Wait();
            return StopCmd.WasCompletedSuccessfully();
        }
        public bool StopCCD()
        {
            var StopCmd = Controller.CreateCommandInstance(typeof(CCDCardDataModule.StopCommand));
            if (StopCmd == null) return false;
            StopCmd.ExecuteCommandAsync().Wait();
            return StopCmd.WasCompletedSuccessfully();
        }
        public bool StopRDPB()
        {
            var StopCmd = Controller.CreateCommandInstance(typeof(RDPBModule.StopCommand));
            if (StopCmd == null) return false;
            StopCmd.ExecuteCommandAsync().Wait();
            return StopCmd.WasCompletedSuccessfully();
        }

        public bool LoadConfigurationLCB()
        {
            var setLCBConfigCommand = Controller.CreateCommandInstance(typeof(LCBModule.SetLCBParametersForWorkingModeCommand));
            if (setLCBConfigCommand == null) return false;
            setLCBConfigCommand.ExecuteCommandAsync(CurrentContext.Configuration.ReadingSocketsSettings.LCBSettings).Wait();
            if (!setLCBConfigCommand.WasCompletedSuccessfully()) return false;

            var setLCBMovementParametersCommand = Controller.CreateCommandInstance(typeof(LCBModule.SetLCBMovementParametersCommand));
            if (setLCBMovementParametersCommand == null) return false;
            setLCBMovementParametersCommand.ExecuteCommandAsync().Wait();
            if (!setLCBMovementParametersCommand.WasCompletedSuccessfully()) return false;

            var setLCBCurrentCommand = Controller.CreateCommandInstance(typeof(LCBModule.SetLCBCurrentCommand));
            if (setLCBCurrentCommand == null) return false;
            setLCBCurrentCommand.ExecuteCommandAsync().Wait();
            if (!setLCBCurrentCommand.WasCompletedSuccessfully()) return false;
            return true;
        }
        public bool LoadConfigurationCCD()
        {
            
            return true;
        }
        public bool LoadConfigurationRDPB()
        {
            var LoadConfigurationCmd = Controller.CreateCommandInstance(typeof(RDPBModule.LoadConfigurationToModuleCommand));
            if (LoadConfigurationCmd == null) return false;
            LoadConfigurationCmd.ExecuteCommandAsync(CurrentContext.Configuration.HardwareSettings.RemoveDefectedPreformBlockConfig).Wait();
            return LoadConfigurationCmd.WasCompletedSuccessfully();
        }

        public bool SendCCDReadCommand(bool ExternalSignal)
        {
            if (ExternalSignal)
            {
                var setExpositionCommand = Controller.CreateCommandInstance(typeof(CCDCardDataModule.SendReadSocketsWithExternalSignalCommand));
                if (setExpositionCommand == null) return false;
                var resultExpositionCommand = setExpositionCommand.Wait(CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds);
                if (resultExpositionCommand is SetReadingParametersCommandResult resultExposition)
                {
                    var lst = resultExposition.CardsNotAnswered();
                    if (lst.Count > 0)
                    {
                        //TODO: Нужно ли отлючать платы, которые не работают и продолжать?
                        logger.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", lst)}) не отвечают");
                    }
                }
            }
            else
            {
                var setExpositionCommand = Controller.CreateCommandInstance(typeof(CCDCardDataModule.SendReadSocketCommand));
                if (setExpositionCommand == null) return false;
                var resultExpositionCommand = setExpositionCommand.Wait(CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds);
                if (resultExpositionCommand is SetReadingParametersCommandResult resultExposition)
                {
                    var lst = resultExposition.CardsNotAnswered();
                    if (lst.Count > 0)
                    {
                        //TODO: Нужно ли отлючать платы, которые не работают и продолжать?
                        logger.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", lst)}) не отвечают");
                    }
                }
            }


            return true;
        }

        public GetImageDataCommandResponse? GetReadImagesCommand()
        {
            var setExpositionCommand = Controller.CreateCommandInstance(typeof(CCDCardDataModule.GetSocketsImagesDataCommand));
            if (setExpositionCommand == null) return null;
            var resultCommand = setExpositionCommand.Wait(CurrentContext.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds);
            if (resultCommand is GetImageDataCommandResponse resultExposition)
            {
                var lst = resultExposition.CardsNotAnswered();
                if (lst.Count > 0)
                {
                    //TODO: Нужно ли отлючать платы, которые не работают и продолжать?
                    logger.Add(LoggerLevel.Critical, $"Платы ({String.Join(", ", lst)}) не отвечают");
                }
                return resultExposition;
            }
            return null;
        }
    }*/
}
