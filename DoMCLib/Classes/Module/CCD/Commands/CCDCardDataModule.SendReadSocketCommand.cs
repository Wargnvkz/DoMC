using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCLib.Classes.Module.LCB;
using DoMCLib.Classes.Module.CCD.Commands.Classes;

/// <summary>
/// Управление получением данных из платы и передача данных в плату
/// </summary>
namespace DoMCLib.Classes.Module.CCD
{

    public partial class CCDCardDataModule
    {
        public class SendReadSocketCommand : WaitingCommandBase
        {
            //API_Commands.Classes.SetReadingParametersCommandResult result = new API_Commands.Classes.SetReadingParametersCommandResult();
            CCDCardDataCommandResponse result = new CCDCardDataCommandResponse();
            public SendReadSocketCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof(DoMCApplicationContext), typeof(Commands.Classes.CCDCardDataCommandResponse)) { }
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
                        module.tcpClients[cardParameters[i].Item1].SendCommandReadSocket(1, CancelationTokenSourceToCancelCommandExecution.Token);
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
                    if (CardAnswerResults.ReadingSocketsResult == 0)
                    {
                        CancelationTokenSourceToCancelCommandExecution.Cancel();
                        return;
                    }
                    result.SetCardAnswered(CardAnswerResults.CardNumber - 1);
                    /*
                    var CardAnswerResults = (CCDCardAnswerResults)data;
                    if (CardAnswerResults == null) return;
                    result.SetCardAnswered(CardAnswerResults.CardNumber - 1);
                    result.SetReadResult(CardAnswerResults.CardNumber - 1, CardAnswerResults.ReadingSocketsResult == 0);
                    result.SetAnswerTime(CardAnswerResults.CardNumber - 1, CardAnswerResults.ReadingSocketsTime);*/
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
