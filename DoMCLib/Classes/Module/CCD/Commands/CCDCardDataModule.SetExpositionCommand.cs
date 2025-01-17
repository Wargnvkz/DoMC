using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCLib.Classes.Module.CCD.Commands.Classes;
using DoMCLib.Exceptions;

/// <summary>
/// Управление получением данных из платы и передача данных в плату
/// </summary>
namespace DoMCLib.Classes.Module.CCD
{

    public partial class CCDCardDataModule
    {
        public class SetExpositionCommand : WaitingCommandBase
        {
            CCDCardDataCommandResponse result = new CCDCardDataCommandResponse();
            public SetExpositionCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof(DoMCApplicationContext), typeof(CCDCardDataCommandResponse)) { }
            protected override void Executing()
            {
                var module = (CCDCardDataModule)Module;
                var context = (DoMCApplicationContext)InputData;
                if (context != null)
                {
                    var workingCards = context.GetWorkingCards(context.GetWorkingPhysicalSocket());
                    var cardParameters = context.GetCardParametersByCardList(workingCards);
                    if (cardParameters.Count == 0) throw new DoMCNoSocketsAllowedToReadException();
                    for (int i = 0; i < cardParameters.Count; i++)
                    {
                        result.SetCardRequested(cardParameters[i].Item1);
                        module.tcpClients[cardParameters[i].Item1].SendCommandSetSocketsExpositionParameters(cardParameters[i].Item2, CancelationTokenSourceToCancelCommandExecution.Token);
                    }
                }
                else
                {

                }

            }

            protected override void NotificationReceived(string NotificationName, object? data)
            {
                if (NotificationName.Contains("ResponseSetSocketsExpositionParameters"))
                {
                    var CardAnswerResults = (CCDCardAnswerResults)data;
                    if (CardAnswerResults == null) return;
                    result.SetCardAnswered(CardAnswerResults.CardNumber - 1);
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
