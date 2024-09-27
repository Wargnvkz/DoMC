using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCLib.Classes.Module.CCD.Commands.Classes;

/// <summary>
/// Управление получением данных из платы и передача данных в плату
/// </summary>
namespace DoMCLib.Classes.Module.CCD
{

    public partial class CCDCardDataModule
    {
        public class SendReadSocketWithExternalSignalCommand : WaitCommandBase
        {
            CCDCardDataCommandResponse result = new CCDCardDataCommandResponse();
            public SendReadSocketWithExternalSignalCommand(IMainController mainController, ModuleBase module) : base(mainController, module, typeof(ApplicationContext), null) { }
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
                        module.tcpClients[cardParameters[i].Item1].SendCommandReadSeveralSocketsExternal(true);
                    }
                }
                else
                {
                }
            }
            protected override void NotificationReceived(string NotificationName, object? data)
            {
                if (NotificationName.Contains("ResponseReadSockets"))
                {
                    var CardAnswerResults = (CCDCardAnswerResults)data;
                    if (CardAnswerResults == null) return;
                    result.SetCardAnswered(CardAnswerResults.CardNumber);
                }
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
