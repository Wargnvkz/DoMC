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
    public class CCDStopCommand : CCDAllSocketsCommand
    {
        public CCDStopCommand(IMainController controller, AbstractModuleBase module) : base(controller, module)
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

    #region Helpers
    #region Common Commands
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
            //var tasks = new List<Task>();

            var tasks = cardParameters.Select(async card =>
            {
                result.SetCardRequested(card.CardNumber);
                try
                {
                    var answer = await ExecuteCCDFunction(
                        module,
                        card.CardNumber,
                        card.CardWorkingParameters,
                        CancelationTokenSourceToCancelCommandExecution.Token,
                        context.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds * 1000
                    );

                    if (answer.ReadingSocketsResult == 0)
                        result.SetCardAnswered(card.CardNumber);
                }
                catch
                {
                }
            });
            await Task.WhenAll(tasks.ToArray());
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
    #endregion
    #region Images Commands
    public class CCDAllSocketsImagesCommand : GenericCommandBase<DoMCApplicationContext, GetImageDataCommandResponse>
    {
        public CCDAllSocketsImagesCommand(IMainController controller, AbstractModuleBase module)
        : base(controller, module) { }

        /// <summary>
        /// Подготавливает и вызывает ExecuteCCDFunction для всех рабающих плат
        /// </summary>
        /// <returns></returns>
        protected override async Task Executing()
        {
            var module = (CCDCardDataModule)Module;
            var context = InputData;
            var result = new GetImageDataCommandResponse()
            {
                EquipmentSocket2CardSocket = (int[])context.EquipmentSocket2CardSocket.Clone()
            };

            var workingCards = context.GetWorkingCards(context.GetWorkingPhysicalSocket());
            var cardParameters = context.GetCardParametersByCardList(workingCards);
            //var tasks = new List<Task>();
            var tasks = cardParameters.Select(async (card) =>
            {
                result.SetCardRequested(card.CardNumber);

                var imageData = await module[card.CardNumber]
                        .GetAllImagesDataAsync(
                            context.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds * 1000,
                            CancelationTokenSourceToCancelCommandExecution.Token);

                result.SetCardAnswered(card.CardNumber);
                result.CardsImageData[card.CardNumber] = imageData.Select(r => r.Item1).ToArray();
            }
            );

            await Task.WhenAll(tasks);
            SetOutput(result);
        }

    }
    public class CCDGetSingleSocketImageCommand : GenericCommandBase<(int SocketNumber, DoMCApplicationContext Context), SocketReadData>
    {
        public CCDGetSingleSocketImageCommand(IMainController controller, AbstractModuleBase module)
        : base(controller, module) { }


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

            var answer = await module[cardSocket.CCDCardNumber].GetAllImagesDataAsync(context.Configuration.HardwareSettings.Timeouts.WaitForCCDCardAnswerTimeoutInSeconds * 1000, CancelationTokenSourceToCancelCommandExecution.Token);

            SetOutput(answer[cardSocket.InnerSocketNumber].Item1);

        }
    }
    #endregion
    #endregion

}

