using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCLib.Classes.Module.LCB;

/// <summary>
/// Управление получением данных из платы и передача данных в плату
/// </summary>
namespace DoMCLib.Classes.Module.CCD
{

    public partial class CCDCardDataModule
    {
        public class SendReadSocketCommand : WaitCommandBase
        {
            Commands.Classes.SetReadingParametersCommandResult result = new Commands.Classes.SetReadingParametersCommandResult();
            public SendReadSocketCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof(ApplicationContext), typeof(Commands.Classes.SetReadingParametersCommandResult)) { }
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
                        module.tcpClients[cardParameters[i].Item1].SendCommandSetSocketReadingParameters(cardParameters[i].Item2, CancellationTokenSourceBase.Token);
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
                    var CardAnswerResults = (CCDCardAnswerResults)data;
                    if (CardAnswerResults == null) return;
                    result.SetCardAnswered(CardAnswerResults.CardNumber);
                    result.SetReadResult(CardAnswerResults.CardNumber, CardAnswerResults.ReadingSocketsResult == 0);
                    result.SetAnswerTime(CardAnswerResults.CardNumber, CardAnswerResults.ReadingSocketsTime);
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
