using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCLib.Classes.Module.CCD.Commands.Classes;
using DoMCLib.Tools;

/// <summary>
/// Управление получением данных из платы и передача данных в плату
/// </summary>
namespace DoMCLib.Classes.Module.CCD
{

    public partial class CCDCardDataModule
    {
        public class ResetCardsSingleSocketCommand : WaitingCommandBase
        {
            CCDCardDataCommandResponse result = new CCDCardDataCommandResponse();
            public ResetCardsSingleSocketCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof((DoMCApplicationContext, int)), null) { }
            protected override void Executing()
            {
                var module = (CCDCardDataModule)Module;
                var contextSocket = InputData as (DoMCApplicationContext Context, int EquipmentSocketNumber)?;
                if (contextSocket != null && contextSocket.Value.Context != null)
                {
                    var context = contextSocket.Value.Context;
                    var EquipmentSocketNumber = contextSocket.Value.EquipmentSocketNumber;
                    var cardSocket = context.EquipmentSocket2CardSocket[EquipmentSocketNumber];
                    var workingCardSocket = new TCPCardSocket(cardSocket);
                    var cardNumber = workingCardSocket.CCDCardNumber;
                    result.SetCardRequested(cardNumber);
                    module.tcpClients[cardNumber].SendCommandSetSocketReadingParameters(CancelationTokenSourceToCancelCommandExecution.Token, false, false, false, true);
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

    }

}
