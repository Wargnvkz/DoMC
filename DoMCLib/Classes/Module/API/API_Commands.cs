using DoMCLib.Classes.Module.RDPB;
using DoMCLib.Configuration;
using DoMCModuleControl.Modules;
using DoMCModuleControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes.Module.API.Commands
{

    public class StartRESTAPIServerCommand : WaitingCommandBase// AbstractCommandBase
    {
        bool responseReceived = false;
        public StartRESTAPIServerCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, typeof(bool)) { }
        protected override void Executing()
        {
            var apiModule = (API)Module;
            apiModule.StartServer();

        }
        protected override bool MakeDecisionIsCommandCompleteFunc()
        {
            return responseReceived;
        }

        protected override void NotificationReceived(string NotificationName, object? data)
        {
            if (NotificationName == "REST.API.Started") responseReceived = true;
        }
        protected override void PrepareOutputData()
        {
            OutputData = responseReceived;
        }
    }
    public class StopRESTAPIServerCommand : WaitingCommandBase// AbstractCommandBase
    {
        bool responseReceived = false;
        public StopRESTAPIServerCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, null, typeof(bool)) { }
        protected override void Executing()
        {
            var apiModule = (API)Module;
            apiModule.StopServer();

        }
        protected override bool MakeDecisionIsCommandCompleteFunc()
        {
            return responseReceived;
        }

        protected override void NotificationReceived(string NotificationName, object? data)
        {
            if (NotificationName == "REST.API.Stopped") responseReceived = true;
        }
        protected override void PrepareOutputData()
        {
            OutputData = responseReceived;
        }
    }

}
