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
        public class SetFastReadCommand : WaitingCommandBase
        {
            CCDCardDataCommandResponse result = new CCDCardDataCommandResponse();
            public SetFastReadCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof(DoMCApplicationContext), null) { }
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
                        module.tcpClients[cardParameters[i].Item1].SendCommandSetSocketReadingParameters(CancelationTokenSourceToCancelCommandExecution.Token, false, false, true, true);
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
        public class SetFastReadSingleSocketCommand : WaitingCommandBase
        {
            CCDCardDataCommandResponse result = new CCDCardDataCommandResponse();
            public SetFastReadSingleSocketCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof((DoMCApplicationContext, int)), typeof(CCDCardDataCommandResponse)) { }
            protected override void Executing()
            {
                var module = (CCDCardDataModule)Module;
                var data = ((DoMCApplicationContext context, int EquipmentSocketNumber)?)InputData;
                if (data != null && data.Value.context != null)
                {
                    var cardParameters = data.Value.context.GetSocketParametersByEquipmentSockets(new List<int>() { data.Value.EquipmentSocketNumber });
                    if (cardParameters.Count > 0 && cardParameters[0].Item3 != null && cardParameters[0].Item3.ReadingParameters != null && cardParameters[0].Item3.ReadingParameters.Exposition > 0 && cardParameters[0].Item3.ReadingParameters.FrameDuration > 0)
                    {
                        var cardParameter = cardParameters[0];
                        result.SetCardRequested(cardParameter.Item2.CCDCardNumber);
                        module.tcpClients[cardParameter.Item2.CCDCardNumber].SendCommandSetSocketReadingParameters(CancelationTokenSourceToCancelCommandExecution.Token, false, false, true, true);
                    }
                    else
                    {
                        throw new DoMCSocketParametersNotSetException(-1, data.Value.EquipmentSocketNumber);
                    }
                }
                else
                {
                    throw new ArgumentNullException(nameof(InputData));
                }
            }
            protected override void NotificationReceived(string NotificationName, object? data)
            {
                if (NotificationName.Contains("ResponseSetConfiguration"))
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
