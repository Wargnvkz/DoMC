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
using System.ComponentModel;

namespace DoMCLib.Classes.Module.CCD.Commands
{
    #region Start/stop
    [Description("Запуск модуля работы с платами ПЗС")]
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
    [Description("Остановка модуля работы с платами ПЗС")]
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
    [Description("Запуск модуля работы с платами ПЗС для одной платы")]
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
    [Description("Остановка модуля работы с платами ПЗС для одной платы")]
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
    [Description("Чтение всех гнезд работающих плат ПЗС")]
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
    [Description("Чтение всех гнезд работающих плат ПЗС по внешнему сигналу")]
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
    [Description("Сброс плат ПЗС")]
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

    [Description("Установка параметров чтения плат ПЗС")]
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
    [Description("Установка экспозиции плат ПЗС")]
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
    [Description("Установка режима быстрого чтения плат ПЗС")]
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

    [Description("Проверка возможности подключения к платам ПЗС")]
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
    [Description("Чтение одной платы ПЗС")]
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

    [Description("Чтение одной платы ПЗС по внешнему сигналу")]
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
    [Description("Сброс платы ПЗС")]
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
    [Description("Установка режима быстрого чтения платы ПЗС")]
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
    [Description("Установка параметров чтения платы ПЗС")]
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
    [Description("Установка экспозиции платы ПЗС")]
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
    #endregion

    #region Images Commands
    [Description("Получение изображений всех гнезд плат ПЗС")]
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
    [Description("Получение изображений одного гнезд плат ПЗС")]
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


}

