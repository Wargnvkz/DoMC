using DoMCLib.Classes.Module.CCD.Commands.Classes;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Modules;
using DoMCModuleControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using DoMCLib.Tools;
using DoMCModuleControl.Logging;
using Microsoft.AspNetCore.Mvc;
using DoMCLib.Exceptions;
using DoMCLib.Classes.Configuration.CCD;

namespace DoMCLib.Classes.Module.CCD.Commands
{
    #region Start/stop
    public class StartCommand : CCDAllSocketsCommand
    {
        public StartCommand(IMainController controller, AbstractModuleBase module) : base(controller, module)
        {
        }

        protected override async Task<CCDCardAnswerResults> ExecuteCCDFunction(CCDCardDataModule module, int cardnumber, SocketParameters cardParameters, CancellationToken token, int TimeoutMs)
        {
            await module[cardnumber].Start();
            return new CCDCardAnswerResults() { CardNumber = cardnumber, CommandSucceed = true, ReadingSocketsResult = 0 };
        }
    }
    public class StopCommand : CCDAllSocketsCommand
    {
        public StopCommand(IMainController controller, AbstractModuleBase module) : base(controller, module)
        {
        }

        protected override async Task<CCDCardAnswerResults> ExecuteCCDFunction(CCDCardDataModule module, int cardnumber, SocketParameters cardParameters, CancellationToken token, int TimeoutMs)
        {
            await module[cardnumber].Stop();
            return new CCDCardAnswerResults() { CardNumber = cardnumber, CommandSucceed = true, ReadingSocketsResult = 0 };
        }
    }
    public class StartSingleSocketCommand : CCDSingleSocketCommand
    {
        public StartSingleSocketCommand(IMainController controller, AbstractModuleBase module) : base(controller, module)
        {
        }

        protected override async Task<CCDCardAnswerResults> ExecuteCCDFunction(CCDCardDataModule module, TCPCardSocket cardSocket, SocketParameters cardParameters, CancellationToken token, int TimeoutMs)
        {
            await module[cardSocket.CCDCardNumber].Start();
            return new CCDCardAnswerResults() { CardNumber = cardSocket.CCDCardNumber, CommandSucceed = true, ReadingSocketsResult = 0 };
        }
    }
    public class StopSingleSocketCommand : CCDSingleSocketCommand
    {
        public StopSingleSocketCommand(IMainController controller, AbstractModuleBase module) : base(controller, module)
        {
        }

        protected override async Task<CCDCardAnswerResults> ExecuteCCDFunction(CCDCardDataModule module, TCPCardSocket cardSocket, SocketParameters cardParameters, CancellationToken token, int TimeoutMs)
        {
            await module[cardSocket.CCDCardNumber].Stop();
            return new CCDCardAnswerResults() { CardNumber = cardSocket.CCDCardNumber, CommandSucceed = true, ReadingSocketsResult = 0 };
        }
    }

    #endregion


    #region AllCards
    public class SendReadAllSocketsCommand : CCDAllSocketsCommand
    {
        public SendReadAllSocketsCommand(IMainController controller, AbstractModuleBase module) : base(controller, module)
        {
        }

        protected override async Task<CCDCardAnswerResults> ExecuteCCDFunction(CCDCardDataModule module, int cardnumber, SocketParameters cardParameters, CancellationToken token, int TimeoutMs)
        {
            return await module[cardnumber].SendCommandReadAllSocketsAsync(TimeoutMs, token);
        }
    }
    public class SendReadAllSocketsWithExternalSignalCommand : CCDAllSocketsCommand
    {
        public SendReadAllSocketsWithExternalSignalCommand(IMainController controller, AbstractModuleBase module) : base(controller, module)
        {
        }

        protected override async Task<CCDCardAnswerResults> ExecuteCCDFunction(CCDCardDataModule module, int cardnumber, SocketParameters cardParameters, CancellationToken token, int TimeoutMs)
        {
            return await module[cardnumber].SendCommandReadSeveralSocketsExternalAsync(TimeoutMs, token);
        }
    }
    public class ResetCardsCommand : CCDAllSocketsCommand
    {
        public ResetCardsCommand(IMainController controller, AbstractModuleBase module) : base(controller, module)
        {
        }

        protected override async Task<CCDCardAnswerResults> ExecuteCCDFunction(CCDCardDataModule module, int cardnumber, SocketParameters cardParameters, CancellationToken token, int TimeoutMs)
        {
            return await module[cardnumber].SendCommandSetSocketReadingStateAsync(token, TimeoutMs, false, false, false, true);
        }
    }

    public class SendCommandSetSocketReadingParametersCommand : CCDAllSocketsCommand
    {
        public SendCommandSetSocketReadingParametersCommand(IMainController controller, AbstractModuleBase module) : base(controller, module)
        {
        }

        protected override async Task<CCDCardAnswerResults> ExecuteCCDFunction(CCDCardDataModule module, int cardnumber, SocketParameters cardParameters, CancellationToken token, int TimeoutMs)
        {
            return await module[cardnumber].SendCommandSetSocketReadingParametersAsync(cardParameters, TimeoutMs, token);
        }
    }
    public class SendCommandSetSocketsExpositionParametersCommand : CCDAllSocketsCommand
    {
        public SendCommandSetSocketsExpositionParametersCommand(IMainController controller, AbstractModuleBase module) : base(controller, module)
        {
        }

        protected override async Task<CCDCardAnswerResults> ExecuteCCDFunction(CCDCardDataModule module, int cardnumber, SocketParameters cardParameters, CancellationToken token, int TimeoutMs)
        {
            return await module[cardnumber].SendCommandSetSocketsExpositionParametersAsync(cardParameters, TimeoutMs, token);
        }
    }
    public class SetFastReadCommand : CCDAllSocketsCommand
    {
        public SetFastReadCommand(IMainController controller, AbstractModuleBase module) : base(controller, module)
        {
        }

        protected override async Task<CCDCardAnswerResults> ExecuteCCDFunction(CCDCardDataModule module, int cardnumber, SocketParameters cardParameters, CancellationToken token, int TimeoutMs)
        {
            return await module[cardnumber].SendCommandSetSocketReadingStateAsync(token, TimeoutMs, false, false, true, true);
        }
    }

    public class TestConnectionCardsCommand : CCDAllSocketsCommand
    {
        public TestConnectionCardsCommand(IMainController controller, AbstractModuleBase module) : base(controller, module)
        {
        }

        protected override async Task<CCDCardAnswerResults> ExecuteCCDFunction(CCDCardDataModule module, int cardnumber, SocketParameters cardParameters, CancellationToken token, int TimeoutMs)
        {
            var result = await module[cardnumber].TestConntectivity();
            if (result)
                return new CCDCardAnswerResults() { CardNumber = cardnumber, CommandSucceed = true, ReadingSocketsResult = 0 };
            else
                return new CCDCardAnswerResults() { CardNumber = cardnumber, CommandSucceed = false, ReadingSocketsResult = 1 };
        }
    }


    #endregion

    #region Single Socket

    public class SendReadSingleSocketCommand : CCDSingleSocketCommand
    {
        public SendReadSingleSocketCommand(IMainController controller, AbstractModuleBase module) : base(controller, module)
        {
        }

        protected override async Task<CCDCardAnswerResults> ExecuteCCDFunction(CCDCardDataModule module, TCPCardSocket cardSocket, SocketParameters cardParameters, CancellationToken token, int TimeoutMs)
        {
            return await module[cardSocket.CCDCardNumber].SendCommandReadSocketAsync(0, TimeoutMs, token);
        }
    }

    public class SendReadSingleSocketWithExternalSignalCommand : CCDSingleSocketCommand
    {
        public SendReadSingleSocketWithExternalSignalCommand(IMainController controller, AbstractModuleBase module) : base(controller, module)
        {
        }

        protected override async Task<CCDCardAnswerResults> ExecuteCCDFunction(CCDCardDataModule module, TCPCardSocket cardSocket, SocketParameters cardParameters, CancellationToken token, int TimeoutMs)
        {
            return await module[cardSocket.CCDCardNumber].SendCommandReadSeveralSocketsExternalAsync(TimeoutMs, token);
        }
    }

    public class ResetCardsSingleSocketCommand : CCDSingleSocketCommand
    {
        public ResetCardsSingleSocketCommand(IMainController controller, AbstractModuleBase module) : base(controller, module)
        {
        }

        protected override async Task<CCDCardAnswerResults> ExecuteCCDFunction(CCDCardDataModule module, TCPCardSocket cardSocket, SocketParameters cardParameters, CancellationToken token, int TimeoutMs)
        {
            return await module[cardSocket.CCDCardNumber].SendCommandSetSocketReadingStateAsync(token, TimeoutMs, false, false, false, true);
        }
    }
    public class SetFastReadSingleSocketCommand : CCDSingleSocketCommand
    {
        public SetFastReadSingleSocketCommand(IMainController controller, AbstractModuleBase module) : base(controller, module)
        {
        }

        protected override async Task<CCDCardAnswerResults> ExecuteCCDFunction(CCDCardDataModule module, TCPCardSocket cardSocket, SocketParameters cardParameters, CancellationToken token, int TimeoutMs)
        {
            return await module[cardSocket.CCDCardNumber].SendCommandSetSocketReadingStateAsync(token, TimeoutMs, false, false, true, true);

        }
    }
    public class SetReadingParametersToSingleSocketCommand : CCDSingleSocketCommand
    {
        public SetReadingParametersToSingleSocketCommand(IMainController controller, AbstractModuleBase module) : base(controller, module)
        {
        }

        protected override async Task<CCDCardAnswerResults> ExecuteCCDFunction(CCDCardDataModule module, TCPCardSocket cardSocket, SocketParameters cardParameters, CancellationToken token, int TimeoutMs)
        {
            return await module[cardSocket.CCDCardNumber].SendCommandSetSocketReadingParametersAsync(cardParameters, TimeoutMs, token);
        }
    }

    public class SetExpositionToSingleSocketCommand : CCDSingleSocketCommand
    {
        public SetExpositionToSingleSocketCommand(IMainController controller, AbstractModuleBase module) : base(controller, module)
        {
        }

        protected override async Task<CCDCardAnswerResults> ExecuteCCDFunction(CCDCardDataModule module, TCPCardSocket cardSocket, SocketParameters cardParameters, CancellationToken token, int TimeoutMs)
        {
            return await module[cardSocket.CCDCardNumber].SendCommandSetSocketsExpositionParametersAsync(cardParameters, TimeoutMs, token);
        }
    }


    #endregion

    #region GetImageData
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

    #endregion




    public abstract class CCDAllSocketsCommand : GenericCommandBase<DoMCApplicationContext, CCDCardDataCommandResponse>
    {
        public CCDAllSocketsCommand(IMainController controller, AbstractModuleBase module)
        : base(controller, module) { }
        protected abstract Task<CCDCardAnswerResults> ExecuteCCDFunction(
            CCDCardDataModule module,
            int cardnumber,
            SocketParameters cardParameters,
            CancellationToken token,
            int TimeoutMs
        );
        /// <summary>
        /// Подготавливает и вызывает ExecuteCCDFunction для всех рабающих плат
        /// </summary>
        /// <returns></returns>
        protected override async Task Executing()
        {
            var module = (CCDCardDataModule)Module;
            var context = InputData;
            var result = new CCDCardDataCommandResponse();

            var workingCards = context.GetWorkingCards(context.GetWorkingPhysicalSocket());
            var cardParameters = context.GetCardParametersByCardList(workingCards);
            var tasks = new List<Task>();

            foreach (var (cardNumber, cardWorkingParameters) in cardParameters)
            {
                result.SetCardRequested(cardNumber);

                var task = ExecuteCCDFunction(module, cardNumber, cardWorkingParameters, CancelationTokenSourceToCancelCommandExecution.Token, context.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds * 1000)
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
        
    }
    public abstract class CCDSingleSocketCommand : GenericCommandBase<(int SocketNumber, DoMCApplicationContext Context), bool>
    {
        public CCDSingleSocketCommand(IMainController controller, AbstractModuleBase module)
        : base(controller, module) { }
        protected abstract Task<CCDCardAnswerResults> ExecuteCCDFunction(
            CCDCardDataModule module,
            TCPCardSocket tcpCardSocket,
            SocketParameters socketParameters,
            CancellationToken token,
            int TimeoutMs
            );

        protected override async Task Executing()
        {
            var module = (CCDCardDataModule)Module;
            var context = InputData.Context;
            var socketNumber = InputData.SocketNumber;
            var socketNumberAndParameters = context.GetSocketParametersByEquipmentSockets(new List<int>() { socketNumber });
            var cardSocket = socketNumberAndParameters[0].CardSocket;
            var SocketParameters = socketNumberAndParameters[0].SocketParameters;

            var token = CancelationTokenSourceToCancelCommandExecution.Token;
            var timeout = context.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds * 1000;

            var answer = await ExecuteCCDFunction(module, cardSocket, SocketParameters, token, timeout);

            SetOutput(answer.ReadingSocketsResult == 0);

        }
    }

}

