using DoMCLib.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCLib.Classes.Configuration.CCD;
using DoMCLib.Tools;
using DoMCModuleControl.Commands;

/// <summary>
/// Управление получением данных из платы и передача данных в плату
/// </summary>
namespace DoMCLib.Classes.Module.CCD
{

    /// <summary>
    /// Управление получением данных из платы и передача данных в плату
    /// </summary>
    public partial class CCDCardDataModule : ModuleBase
    {
        CCDCardTCPClient[] tcpClients;
        public CCDCardDataModule(IMainController MainController) : base(MainController)
        {
            tcpClients = new CCDCardTCPClient[12];
        }


        public class SetReadingParametersCommand : WaitCommandBase
        {
            public SetReadingParametersCommand(IMainController mainController, ModuleBase module) : base(mainController, module, typeof(ApplicationContext), null) { }
            protected override void Executing()
            {
                var module = (CCDCardDataModule)Module;
                var context = (ApplicationContext)InputData;
                if (context != null)
                {
                    var workingCards = context.GetWorkingCards(context.GetWorkingPhysicalSocket());
                    var cardParameters = context.GetCardParametersByCardList(workingCards);
                    for (int i = 0; i < cardParameters.Count; i++)
                    {
                        module.tcpClients[cardParameters[i].Item1].SendCommandSetSocketReadingParameters(cardParameters[i].Item2);
                    }
                }
                else
                {

                }

            }

            protected override void NotificationReceived(string NotificationName, object? data)
            {
                if (NotificationName.Contains("ResponseSetReadingParametersConfiguration"))
                {
                    var card = (int)data;
                }
            }

            protected override bool MakeDecisionIsCommandCompleteFunc()
            {
                throw new NotImplementedException();
            }


        }
        public class SetExpositionParametersCommand : CommandBase
        {
            public SetExpositionParametersCommand(IMainController mainController, ModuleBase module) : base(mainController, module, typeof(ApplicationContext), null) { }
            protected override void Executing()
            {
                var module = (CCDCardDataModule)Module;
                var context = (ApplicationContext)InputData;
                if (context != null)
                {
                    var workingCards = context.GetWorkingCards(context.GetWorkingPhysicalSocket());
                    var cardParameters = context.GetCardParametersByCardList(workingCards);
                    for (int i = 0; i < cardParameters.Count; i++)
                    {
                        module.tcpClients[cardParameters[i].Item1].SendCommandSetSocketsExpositionParameters(cardParameters[i].Item2);
                    }

                }
                else
                {

                }

            }
        }

    }

}
