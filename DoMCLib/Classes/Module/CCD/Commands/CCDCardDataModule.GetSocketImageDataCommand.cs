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
        /// <summary>
        /// Получить изображение одного гнезда
        /// </summary>
       /* public class GetSingleSocketImageDataCommand : WaitingCommandBase
        {
            SocketReadData? result = null;
            public GetSingleSocketImageDataCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof((DoMCApplicationContext, int)), typeof(SocketReadData)) { }
            protected override void Executing()
            {
                var module = (CCDCardDataModule)Module;
                var Data = ((DoMCApplicationContext Context, int EquipmentSocket))InputData;
                if (Data.Context != null)
                {
                    var cardSocketParameters = Data.Context.GetSocketParametersByEquipmentSockets(new List<int>() { Data.EquipmentSocket });
                    var cardSocket = cardSocketParameters[0].Item2;
                    var SocketParameters = cardSocketParameters[0].Item3;

                    module.tcpClients[cardSocket.CCDCardNumber].SendCommandGetSocketImage(cardSocket.InnerSocketNumber, CancelationTokenSourceToCancelCommandExecution.Token);
                    //var task = new Task(new Action<object?>((i) => { }), cardSocket.InnerSocketNumber,);
                    var task = new Task(new Action<object?>((i) =>
                    {
                        var cardnumber = (int)i;
                        var ReadResult = module.tcpClients[cardSocket.CCDCardNumber].GetImageDataFromSocketAsync(cardSocket.InnerSocketNumber, Data.Context.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, CancelationTokenSourceToCancelCommandExecution.Token, out SocketReadData data);
                        if (ReadResult)
                        {
                            var equipmentSocket = Data.Context.Configuration.HardwareSettings.CardSocket2EquipmentSocket[cardSocket.CardSocketNumber()];
                            result = data;
                        }
                        else
                        {
                            result = null;
                        }

                    }), cardSocket.InnerSocketNumber, CancelationTokenSourceToCancelCommandExecution.Token);
                    task.Start();
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
                    CancelationTokenSourceToCancelCommandExecution.Cancel();
                }
            }

            protected override bool MakeDecisionIsCommandCompleteFunc()
            {
                return result != null;
            }

            protected override void PrepareOutputData()
            {
                OutputData = result;

            }
        }

        */
        public class GetSingleSocketImageDataCommand : WaitingCommandBase
        {
            GetImageDataCommandResponse result = new GetImageDataCommandResponse();
            CancellationTokenSource[] cancellationTokenSources = new CancellationTokenSource[12];
            public GetSingleSocketImageDataCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof((DoMCApplicationContext, int)), typeof(GetImageDataCommandResponse)) { }
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
                    cancellationTokenSources[cardNumber] = new CancellationTokenSource();
                    module.tcpClients[cardNumber].SendCommandGetAllSocketImages(CancelationTokenSourceToCancelCommandExecution.Token);
                    StartReadSockets(cardNumber, workingCardSocket.InnerSocketNumber, context, module);

                }

            }
            private void StartReadSockets(int cardnumber, int socketNumber, DoMCApplicationContext context, CCDCardDataModule module)
            {
                var task = new Task(() =>
                {
                    var ReadResult = module.tcpClients[cardnumber].GetImageDataFromSocketAsync(socketNumber, context.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds, cancellationTokenSources[cardnumber].Token, out SocketReadData data);
                    if (ReadResult)
                    {
                        TCPCardSocket cardSocket = new TCPCardSocket(cardnumber, socketNumber);
                        var equipmentSocket = context.Configuration.HardwareSettings.CardSocket2EquipmentSocket[cardSocket.CardSocketNumber()];
                        result.SetSocketReadData(equipmentSocket - 1, data);
                    }
                    else
                    {
                        result.SetCardError(cardnumber);
                    }
                    result.SetCardCompleteSuccessfully(cardnumber);
                }, cancellationTokenSources[cardnumber].Token);
                task.Start();
            }


            protected override void NotificationReceived(string NotificationName, object? data)
            {
                if (NotificationName.Contains("ResponseGetSocketsImages"))
                {
                    var CardAnswerResults = (CCDCardAnswerResults)data;
                    if (CardAnswerResults == null) return;
                    result.SetCardAnswered(CardAnswerResults.CardNumber - 1);
                    result.SetCardError(CardAnswerResults.CardNumber - 1);
                    cancellationTokenSources[CardAnswerResults.CardNumber].Cancel();
                }
            }

            protected override bool MakeDecisionIsCommandCompleteFunc()
            {
                return result.GetCardsNotStopped().Count() == 0;
            }

            protected override void PrepareOutputData()
            {
                OutputData = result;

            }

            public override void Cancel()
            {
                for (int i = 0; i < 12; i++)
                {
                    cancellationTokenSources[i]?.Cancel();
                }
                base.Cancel();
            }
        }

    }

}
