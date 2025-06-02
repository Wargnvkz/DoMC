using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCLib.Classes.Module.LCB;
using DoMCLib.Classes.Module.CCD.Commands.Classes;
using DoMCModuleControl.Commands;

/// <summary>
/// Управление получением данных из платы и передача данных в плату
/// </summary>
namespace DoMCLib.Classes.Module.CCD
{

    public partial class CCDCardDataModule
    {
        /*
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
        */
        /*public class SendReadSocketCommand : GenericCommandBase<DoMCApplicationContext, CCDCardDataCommandResponse>
        {
            public SendReadSocketCommand(IMainController controller, AbstractModuleBase module)
                : base(controller, module) { }

            protected override void Executing()
            {
                var module = (CCDCardDataModule)Module;
                var context = InputData;
                var result = new CCDCardDataCommandResponse();

                var workingCards = context.GetWorkingCards(context.GetWorkingPhysicalSocket());
                var cardParameters = context.GetCardParametersByCardList(workingCards);
                var tasks = new List<Task>();

                foreach (var (cardNumber, _) in cardParameters)
                {
                    result.SetCardRequested(cardNumber);

                    var task = module.tcpClients[cardNumber]
                        .SendCommandReadSocketAsync(1, CancelationTokenSourceToCancelCommandExecution.Token)
                        .ContinueWith(t =>
                        {
                            if (t.Status == TaskStatus.RanToCompletion && t.Result.ReadingSocketsResult == 0)
                                result.SetCardAnswered(cardNumber);
                        });

                    tasks.Add(task);
                }

                Task.WaitAll(tasks.ToArray(), CancelationTokenSourceToCancelCommandExecution.Token); // блокируем здесь, потому что это sync Executing()
                SetOutput(result);
            }
        }*/

    }

}
