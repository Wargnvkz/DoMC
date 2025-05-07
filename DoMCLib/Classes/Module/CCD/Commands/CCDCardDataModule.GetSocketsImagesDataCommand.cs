using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCLib.Tools;
using DoMCLib.Classes.Module.CCD.Commands.Classes;
using DoMCLib.Classes.Configuration.CCD;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Logging;
using System.Text;

/// <summary>
/// Управление получением данных из платы и передача данных в плату
/// </summary>
namespace DoMCLib.Classes.Module.CCD
{

    public partial class CCDCardDataModule
    {
        /// <summary>
        /// послыка команды плате на передачу данных об изображении и подключение к плате для получения данных изображения гнезд
        /// OutputData - GetImageDataCommandResponse - массив гнезд соответствующих гнездам на машине (не платам)
        /// </summary>
        public class GetSocketsImagesDataCommand : WaitingCommandBase
        {
            GetImageDataCommandResponse result = new GetImageDataCommandResponse();
            CancellationTokenSource[] cancellationTokenSources = new CancellationTokenSource[12];
            ILogger WorkingLog;
            public GetSocketsImagesDataCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof(DoMCApplicationContext), typeof(GetImageDataCommandResponse)) { }
            protected override void Executing()
            {
                WorkingLog = Controller.GetLogger(this.GetType().Name);
                var module = (CCDCardDataModule)Module;

                var context = (DoMCApplicationContext)InputData;
                if (context != null)
                {
                    var workingCards = context.GetWorkingCards(context.GetWorkingPhysicalSocket());
                    var cardParameters = context.GetCardParametersByCardList(workingCards);
                    for (int i = 0; i < cardParameters.Count; i++)
                    {
                        result.SetCardRequested(cardParameters[i].Item1);
                        cancellationTokenSources[cardParameters[i].Item1] = new CancellationTokenSource();
                        module.tcpClients[cardParameters[i].Item1].SendCommandGetAllSocketImages(CancelationTokenSourceToCancelCommandExecution.Token);
                        StartReadAllSockets(cardParameters[i].Item1, context, module);
                    }
                }
                else
                {

                }

            }
            private void StartReadAllSockets(int cardnumber, DoMCApplicationContext context, CCDCardDataModule module)
            {
                WorkingLog.Add(LoggerLevel.Information, $"Запуск чтения для платы {cardnumber}.");
                var th = new Thread((object o) =>
                {
                    bool error = false;
                    StringBuilder sb = new StringBuilder();
                    try
                    {
                        for (int socket = 0; socket < 8 && (!cancellationTokenSources[cardnumber].IsCancellationRequested); socket++)
                        {
                            WorkingLog.Add(LoggerLevel.Information, $"Запуск чтения для платы {cardnumber}.");
                            StringBuilder socketSb = new StringBuilder();
                            WorkingLog.Add(LoggerLevel.Information, $"Card:{cardnumber}; Socket: {socket}; ");
                            socketSb.Append($"Card:{cardnumber}; Socket: {socket}; ");
                            var ReadResult = module.tcpClients[cardnumber].GetImageDataFromSocketAsync(socket, context.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds * 1000, cancellationTokenSources[cardnumber].Token, out SocketReadData data);
                            WorkingLog.Add(LoggerLevel.Information, $"ReadResult: {ReadResult}; Data: {data.ImageDataRead}; Ticks: {data.ImageTicksRead}; ");
                            socketSb.Append($"ReadResult: {ReadResult}; Data: {data.ImageDataRead}; Ticks: {data.ImageTicksRead}; ");
                            if (ReadResult)
                            {
                                TCPCardSocket cardSocket = new TCPCardSocket(cardnumber, socket);
                                var equipmentSocket = context.Configuration.HardwareSettings.CardSocket2EquipmentSocket[cardSocket.CardSocketNumber()];
                                result.SetSocketReadData(equipmentSocket - 1, data);
                                WorkingLog.Add(LoggerLevel.Information, $"EquipmentSocket:{equipmentSocket};");
                                socketSb.Append($"EquipmentSocket:{equipmentSocket};");

                            }
                            else
                            {
                                error = true;
                            }
                            sb.AppendLine(socketSb.ToString());
                            if (Environment.HasShutdownStarted)
                            {
                                break;
                            }
                        }
                        if (error)
                        {
                            result.SetCardError(cardnumber);
                        }
                        else
                        {
                            result.SetCardCompleteSuccessfully(cardnumber);
                        }
                    }
                    catch
                    {

                    }
                    WorkingLog.Add(LoggerLevel.Information, $"Получение изображений завершено. Результаты:");
                    WorkingLog.Add(LoggerLevel.FullDetailedInformation, sb.ToString());
                });
                th.Start();
            }


            protected override void NotificationReceived(string NotificationName, object? data)
            {
                if (NotificationName.Contains("ResponseGetSocketsImages"))
                {
                    var CardAnswerResults = (CCDCardAnswerResults)data;
                    if (CardAnswerResults == null) return;
                    result.SetCardAnswered(CardAnswerResults.CardNumber - 1);
                    result.SetCardError(CardAnswerResults.CardNumber - 1);
                    cancellationTokenSources[CardAnswerResults.CardNumber - 1].Cancel();
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

        public class GetSingleSocketImageDataCommand : WaitingCommandBase
        {
            GetImageDataCommandResponse result = new GetImageDataCommandResponse();
            CancellationTokenSource[] cancellationTokenSources = new CancellationTokenSource[12];
            public GetSingleSocketImageDataCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof((DoMCApplicationContext, int)), typeof(GetImageDataCommandResponse)) { }
            protected override void Executing()
            {
                var module = (CCDCardDataModule)Module;
                var contextSocket = ((DoMCApplicationContext, int)?)InputData;
                if (contextSocket != null)
                {
                    var context = contextSocket.Value.Item1;
                    var equipmentSocketNumber = contextSocket.Value.Item2;
                    var workingCard = context.GetWorkingPhysicalSocket(equipmentSocketNumber);
                    result.SetCardRequested(workingCard.CCDCardNumber);
                    cancellationTokenSources[workingCard.CCDCardNumber] = new CancellationTokenSource();
                    module.tcpClients[workingCard.CCDCardNumber].SendCommandGetAllSocketImages(CancelationTokenSourceToCancelCommandExecution.Token);
                    StartReadAllSockets(workingCard.CCDCardNumber, context, module);
                }
                else
                {

                }

            }
            private void StartReadAllSockets(int cardnumber, DoMCApplicationContext context, CCDCardDataModule module)
            {
                var task = new Task(() =>
                {
                    for (int socket = 0; socket < 8 && (!cancellationTokenSources[cardnumber].IsCancellationRequested); socket++)
                    {
                        var ReadResult = module.tcpClients[cardnumber].GetImageDataFromSocketAsync(socket, context.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds * 1000, cancellationTokenSources[cardnumber].Token, out SocketReadData data);
                        if (ReadResult)
                        {
                            TCPCardSocket cardSocket = new TCPCardSocket(cardnumber, socket);
                            var equipmentSocket = context.Configuration.HardwareSettings.CardSocket2EquipmentSocket[cardSocket.CardSocketNumber()];
                            result.SetSocketReadData(equipmentSocket - 1, data);
                        }
                        else
                        {
                            result.SetCardError(cardnumber);
                        }
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
