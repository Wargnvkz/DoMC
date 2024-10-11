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
using ApplicationContext = DoMCLib.Classes.ApplicationContext;

namespace DoMC.Classes
{
    public class ModelCommandProcessor
    {
        IMainController Controller;
        ApplicationContext Context;
        Observer observer;
        ILogger logger;
        public ModelCommandProcessor(IMainController controller, ApplicationContext applicationContext)
        {
            Controller = controller;
            Context = applicationContext;
            observer = Controller.GetObserver();
            observer.NotificationReceivers += Observer_NotificationReceived;
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
            var startCmd = Controller.CreateCommand(typeof(LCBModule.StartCommand));
            if (startCmd == null) return false;
            startCmd.ExecuteCommandAsync().Wait();
            return startCmd.IsCompleteSuccessfully;
        }
        public bool StartCCD()
        {
            var startCmd = Controller.CreateCommand(typeof(CCDCardDataModule.StartCommand));
            if (startCmd == null) return false;
            startCmd.ExecuteCommandAsync().Wait();
            return startCmd.IsCompleteSuccessfully;
        }
        public bool StartRDPB()
        {
            var startCmd = Controller.CreateCommand(typeof(RDPBModule.StartCommand));
            if (startCmd == null) return false;
            startCmd.ExecuteCommandAsync().Wait();
            return startCmd.IsCompleteSuccessfully;
        }

        public bool StopLCB()
        {
            var StopCmd = Controller.CreateCommand(typeof(LCBModule.StopCommand));
            if (StopCmd == null) return false;
            StopCmd.ExecuteCommandAsync().Wait();
            return StopCmd.IsCompleteSuccessfully;
        }
        public bool StopCCD()
        {
            var StopCmd = Controller.CreateCommand(typeof(CCDCardDataModule.StopCommand));
            if (StopCmd == null) return false;
            StopCmd.ExecuteCommandAsync().Wait();
            return StopCmd.IsCompleteSuccessfully;
        }
        public bool StopRDPB()
        {
            var StopCmd = Controller.CreateCommand(typeof(RDPBModule.StopCommand));
            if (StopCmd == null) return false;
            StopCmd.ExecuteCommandAsync().Wait();
            return StopCmd.IsCompleteSuccessfully;
        }

        public bool LoadConfigurationLCB()
        {
            var setLCBConfigCommand = Controller.CreateCommand(typeof(LCBModule.SetLCBParametersCommand));
            if (setLCBConfigCommand == null) return false;
            setLCBConfigCommand.ExecuteCommandAsync(Context.Configuration.CurrentSettings.LCBSettings).Wait();
            if (setLCBConfigCommand.IsError) return false;

            var setLCBMovementParametersCommand = Controller.CreateCommand(typeof(LCBModule.SetLCBMovementParametersCommand));
            if (setLCBMovementParametersCommand == null) return false;
            setLCBMovementParametersCommand.ExecuteCommandAsync().Wait();
            if (setLCBMovementParametersCommand.IsError) return false;

            var setLCBCurrentCommand = Controller.CreateCommand(typeof(LCBModule.SetLCBCurrentCommand));
            if (setLCBCurrentCommand == null) return false;
            setLCBCurrentCommand.ExecuteCommandAsync().Wait();
            if (setLCBCurrentCommand.IsError) return false;
            return true;
        }
        public bool LoadConfigurationCCD()
        {
            
            return true;
        }
        public bool LoadConfigurationRDPB()
        {
            var LoadConfigurationCmd = Controller.CreateCommand(typeof(RDPBModule.LoadConfigurationCommand));
            if (LoadConfigurationCmd == null) return false;
            LoadConfigurationCmd.ExecuteCommandAsync(Context.Configuration.HardwareSettings.RemoveDefectedPreformBlockConfig).Wait();
            return LoadConfigurationCmd.IsCompleteSuccessfully;
        }

        public bool SendCCDReadCommand(bool ExternalSignal)
        {
            if (ExternalSignal)
            {
                var setExpositionCommand = Controller.CreateCommand(typeof(CCDCardDataModule.SendReadSocketWithExternalSignalCommand));
                if (setExpositionCommand == null) return false;
                var resultExpositionCommand = setExpositionCommand.Wait(Context.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeout);
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
                var setExpositionCommand = Controller.CreateCommand(typeof(CCDCardDataModule.SendReadSocketCommand));
                if (setExpositionCommand == null) return false;
                var resultExpositionCommand = setExpositionCommand.Wait(Context.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeout);
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
            var setExpositionCommand = Controller.CreateCommand(typeof(CCDCardDataModule.GetSocketsImagesDataCommand));
            if (setExpositionCommand == null) return null;
            var resultCommand = setExpositionCommand.Wait(Context.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeout);
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
    }
}
