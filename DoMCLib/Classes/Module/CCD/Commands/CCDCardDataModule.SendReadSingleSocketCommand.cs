using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCLib.Classes.Module.CCD.Commands.Classes;
using DoMCLib.Tools;
using Newtonsoft.Json.Linq;

/// <summary>
/// Управление получением данных из платы и передача данных в плату
/// </summary>
namespace DoMCLib.Classes.Module.CCD
{

    public partial class CCDCardDataModule
    {
        public class SendReadSingleSocketCommand : WaitingCommandBase
        {
            CCDCardDataCommandResponse result = new CCDCardDataCommandResponse();
            int cardNumber = -1;
            public SendReadSingleSocketCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof((DoMCApplicationContext Context, int EquipmentSocketNumber)), typeof(CCDCardDataCommandResponse)) { }
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
                    cardNumber = workingCardSocket.CCDCardNumber;
                    result.SetCardRequested(cardNumber);
                    module.tcpClients[cardNumber].SendCommandReadSocket(1, CancelationTokenSourceToCancelCommandExecution.Token);
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


    }

}
