using DoMCLib.Classes.Module.CCD;
using DoMCLib.Classes.Module.LCB;
using DoMCLib.Classes.Module.RDPB;
using DoMCModuleControl;
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
        public ModelCommandProcessor(IMainController controller, ApplicationContext applicationContext)
        {
            Controller = controller;
            Context = applicationContext;
            observer = Controller.GetObserver();
            observer.NotificationReceivers += Observer_NotificationReceived;
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
                            if (Enum.TryParse(typeof(Operations), eventNameParts[1], out var op))
                            {
                                var ledOperationType = (Operations)op;
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
        }

        public bool StartRDPB()
        {
            var startCmd = Controller.CreateCommand(typeof(RDPBModule.StartCommand));
            if (startCmd == null) return false;
            startCmd.ExecuteCommandAsync().Wait();
        }

    }
}
