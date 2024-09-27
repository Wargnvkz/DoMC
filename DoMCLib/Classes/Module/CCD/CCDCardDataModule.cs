using DoMCLib.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCLib.Classes.Configuration.CCD;
using DoMCLib.Tools;
using DoMCModuleControl.Commands;
using DoMCLib.Classes.Module.CCD.Commands.Classes;

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

        public class SendGetSocketsImagesCommand : CommandBase
        {
            CCDCardDataCommandResponse result = new CCDCardDataCommandResponse();
            public SendGetSocketsImagesCommand(IMainController mainController, ModuleBase module) : base(mainController, module, typeof(ApplicationContext), null) { }
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
                        result.SetCardRequested(i);
                        module.tcpClients[cardParameters[i].Item1].SendCommandGetAllSocketImages();
                    }
                }
                else
                {

                }

            }
            
        }

        public class GetSocketsImagesDataCommand : WaitCommandBase
        {
            CCDCardDataCommandResponse result = new CCDCardDataCommandResponse();
            public GetSocketsImagesDataCommand(IMainController mainController, ModuleBase module) : base(mainController, module, typeof(ApplicationContext), null) { }
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
                        result.SetCardRequested(i);
                        module.tcpClients[cardParameters[i].Item1].SendCommandGetAllSocketImages();
                    }
                }
                else
                {

                }

            }
            protected override void NotificationReceived(string NotificationName, object? data)
            {
                if (NotificationName.Contains("ResponseGetSocketsImages"))
                {
                    var CardAnswerResults = (CCDCardAnswerResults)data;
                    if (CardAnswerResults == null) return;
                    result.SetCardAnswered(CardAnswerResults.CardNumber);
                }
            }
            protected List<int> CardsAnswered()
            {
                return Enumerable.Range(0, 12).Where(i => result.answered[i] && result.requested[i]).ToList();
            }

            protected override bool MakeDecisionIsCommandCompleteFunc()
            {
                return result.CardsNotAnswered().Count() > 0;
            }

            protected override void PrepareOutputData()
            {
                OutputData = result;

            }
        }



    }

}
