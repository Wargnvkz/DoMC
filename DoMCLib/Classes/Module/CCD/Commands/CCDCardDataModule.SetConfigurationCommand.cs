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
        public class SetConfigurationCommand : WaitingCommandBase
        {
            CCDCardDataCommandResponse result = new CCDCardDataCommandResponse();
            public SetConfigurationCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof(DoMCApplicationContext), null) { }
            protected override void Executing()
            {
                var module = (CCDCardDataModule)Module;
                var context = (DoMCApplicationContext)InputData;
                if (context != null)
                {
                    var workingCards = context.GetWorkingCards(context.GetWorkingPhysicalSocket());
                    var cardParameters = context.GetCardParametersByCardList(workingCards);
                    for (int i = 0; i < cardParameters.Count; i++)
                    {
                        result.SetCardRequested(cardParameters[i].Item1);
                        module.tcpClients[cardParameters[i].Item1].SendCommandSetSocketReadingParameters(CancelationTokenSourceToCancelCommandExecution.Token, false, false, false, true);
                    }
                }
                else
                {

                }

            }
            protected override void NotificationReceived(string NotificationName, object? data)
            {
                if (NotificationName.Contains("ResponseSetConfiguration"))
                {
                    var CardAnswerResults = (CCDCardAnswerResults)data;
                    if (CardAnswerResults == null) return;
                    result.SetCardAnswered(CardAnswerResults.CardNumber-1);
                }
            }

            protected override bool MakeDecisionIsCommandCompleteFunc()
            {
                return result.CardsNotAnswered().Count() == 0;
            }

            protected override void PrepareOutputData()
            {
                OutputData = result;

            }
        }

    }

}
