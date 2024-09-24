using DoMCLib.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCLib.Classes.Configuration.CCD;
using DoMCLib.Tools;
using DoMCModuleControl.Commands;

/// <summary>
/// Управление получением данных из платы и передача данных в плату
/// </summary>
namespace DoMCLib.Classes.Module.CCD
{

    /// <summary>
    /// Управление получением данных из платы и передача данных в плату
    /// </summary>
    public partial class DoMCCardDataModule : ModuleBase
    {
        MainController Controller;
        DoMCCardTCPClient[] tcpClients;
        public DoMCCardDataModule(IMainController MainController) : base(MainController)
        {
            tcpClients = new DoMCCardTCPClient[12];
        }


        public void Start()
        {
            for (int i = 0; i < tcpClients.Length; i++)
            {
                if (tcpClients[i] == null) tcpClients[i] = new DoMCCardTCPClient(i + 1, Controller);
                tcpClients[i].Start();
            }
        }
        public void Stop()
        {
            for (int i = 0; i < tcpClients.Length; i++)
            {
                if (tcpClients[i] != null)
                    tcpClients[i].Stop();
            }
        }
        public void SetSocketConfiguration(SocketParameters[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                var cs = new TCPCardSocket();
            }
        }

        public void CCDCardsReset()
        {
            try
            {
                CardsConnection.StartSetSocketReadingParameters(false, false, null, true, true);
            }
            catch
            {

            }
        }


        public void StartRead()
        {

            CardsConnection.StartReadAllSockets();
        }

        public void StartReadExternal()
        {
            CardsConnection.StartSetSocketReadingParameters(false, true, null, true);
            System.Threading.Thread.Sleep(100);
        }

        public void StartSeveralSocketRead()
        {

            var sockets = CCDDataEchangeStatuses.SocketsToSave;
            CardsConnection.StartReadSeveralSockets(sockets);
        }

        public void StartSeveralSocketReadExternal()
        {
            System.Threading.Thread.Sleep(100);
            var sockets = CCDDataEchangeStatuses.SocketsToSave;
            CardsConnection.StartReadSeveralSocketsExternal(sockets, CCDDataEchangeStatuses.FastRead);
        }

        public void GetSocketImages()
        {
            if (CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.GetSocketImages && CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Processing) break;
            CardsConnection.StartGetAllSocketImages();

        }

        public void GetSeveralSocketImages()
        {
            System.Threading.Thread.Sleep(100);

            if (CCDDataEchangeStatuses.ModuleStatus == ModuleCommand.GetSeveralSocketImages && CCDDataEchangeStatuses.ModuleStep == ModuleCommandStep.Processing) break;
            var sockets = CCDDataEchangeStatuses.SocketsToSave;
            if (sockets == null)
            {
            }
            else
            {
                CardsConnection.StartGetSocketsImages(sockets);
            }
        }

        public void StopModuleWork()
        {

        }

        public void CCDGetSocketStatus()
        {
            var statuses = CardsConnection.GetCardsStatuses(CardsConnection.ReadTimeout);
            CardSocketWorkingStatus = statuses;
        }


        public void Dispose()
        {
            StopModuleWork();
        }

        private bool LoadConfiguration(FullDoMCConfiguration cfg)
        {
            if (!CCDDataEchangeStatuses.IsNetworkCardSet)
            {
                return false;
            }

            var ss = cfg.SocketToCardSocketConfigurations.Keys.ToArray();
            if (!CCDDataEchangeStatuses.IsConfigurationLoaded)
            {
                System.Threading.Thread.Sleep(100);
                CardsConnection.StartSetSocketProcessingParameters(ss);
                if (!CardsConnection.WaitForAnyCommandCompleteOrTimeout(CardsConnection.ReadTimeout)) //11
                {
                    return false;
                }
                System.Threading.Thread.Sleep(100);

                CardsConnection.StartSetSocketsExpositionParameters(ss);
                if (!CardsConnection.WaitForAnyCommandCompleteOrTimeout(CardsConnection.ReadTimeout)) //4
                {
                    return false;
                }
                CCDDataEchangeStatuses.IsConfigurationLoaded = true;
            }
            return true;
        }

        #region Send requests

        private void ResetSocketStatistics()
        {
            for (int i = 0; i < DoMCCardTCPClients.Length; i++)
            {
                DoMCCardTCPClients[i].ResetSocketsStatistics();
            }
        }

        /// <summary>
        /// Command = 4, Загрузка конфигурации чтения в гнезда по списку
        /// </summary>

        public void StartSetSocketsExpositionParameters(int[] socketNums)
        {
            if (!IsStarted) throw new System.Net.Sockets.SocketException((int)System.Net.Sockets.SocketError.NotConnected);
            ResetSocketStatistics();
            if (socketNums == null)
            {
                socketNums = Enumerable.Range(1, SocketQuantity).ToArray();
            }
            var DoMCCards = GetDoMCCardTCPConnectionBySockets(socketNums);
            foreach (DoMCCardTCPClient card in DoMCCards)
            {
                if (!card.IsCardInUseForChecking() || !card.IsStarted) continue;
                card.SendCommandSetSocketsExpositionParameters(1);
            }


            Thread.Sleep(10);

        }
        /// <summary>
        /// Command = 11, Загрузка основной конфигурации в гнезда по списку
        /// </summary>
        public void StartSetSocketProcessingParameters(int[] socketNums)
        {

            if (!IsStarted) throw new System.Net.Sockets.SocketException((int)System.Net.Sockets.SocketError.NotConnected);
            ResetSocketStatistics();
            if (socketNums == null)
            {
                socketNums = Enumerable.Range(1, SocketQuantity).ToArray();
            }
            var DoMCCards = GetDoMCCardTCPConnectionBySockets(socketNums);
            foreach (DoMCCardTCPClient card in DoMCCards)
            {
                if (!card.IsCardInUseForChecking() || !card.IsStarted) continue;
                card.SendCommandSetSocketProcessingParameters(1);
            }


            Thread.Sleep(10);

        }

        /// <summary>
        /// Command = 1, читать изображения со всех плат
        /// </summary>

        public void StartReadAllSockets()
        {

            if (!IsStarted) throw new System.Net.Sockets.SocketException((int)System.Net.Sockets.SocketError.NotConnected);
            ResetSocketStatistics();

            for (int card = 0; card < DoMCCardTCPClients.Length; card++)
            {
                if (!DoMCCardTCPClients[card].IsStarted) continue;
                if (!DoMCCardTCPClients[card].IsCardInUseForChecking()) continue;
                DoMCCardTCPClients[card].SendCommandReadAllSockets();

            }
            Thread.Sleep(10);
        }
        /// <summary>
        /// Command = 1, читать изображение одного гнезда
        /// </summary>

        public void StartReadSocket(int socket)
        {
            if (!IsStarted) throw new System.Net.Sockets.SocketException((int)System.Net.Sockets.SocketError.NotConnected);
            ResetSocketStatistics();

            if (socket > SocketQuantity || socket < 1) throw new ArgumentOutOfRangeException("Значение номера гнезда должно быть от 1 до " + SocketQuantity);
            var phSocket = DisplaySocket2PhysicalSocket[socket];
            var ccdsn = new CCDSocketNumber(phSocket);

            if (DoMCCardTCPClients[ccdsn.CCDCardNumber].IsCardInUseForChecking() && DoMCCardTCPClients[ccdsn.CCDCardNumber].IsStarted)
                DoMCCardTCPClients[ccdsn.CCDCardNumber].SendCommandReadAllSockets();
            Thread.Sleep(10);

        }

        /// <summary>
        /// Command = 1, запрос чтения по списку сокетов
        /// </summary>

        public void StartReadSeveralSockets(int[] sockets)
        {
            if (!IsStarted) throw new System.Net.Sockets.SocketException((int)System.Net.Sockets.SocketError.NotConnected);
            ResetSocketStatistics();
            if (sockets == null || sockets.Length == 0) throw new ArgumentException("Параметр sockets не может быть пустым");

            var DoMCCards = GetDoMCCardTCPConnectionBySockets(sockets);
            foreach (DoMCCardTCPClient card in DoMCCards)
            {
                if (!card.IsCardInUseForChecking() || !card.IsStarted) continue;
                card.SendCommandReadAllSockets();
            }

            Thread.Sleep(10);

        }

        /// <summary>
        /// Command = 5
        /// </summary>

        public void StartSetSocketReadingParameters(bool AnswerWithImage, bool ExternalStart, int[] sockets, bool FastRead, bool ResetReady = true)
        {
            if (!IsStarted) throw new System.Net.Sockets.SocketException((int)System.Net.Sockets.SocketError.NotConnected);
            ResetSocketStatistics();
            if (sockets == null || sockets.Length == 0) //throw new ArgumentException("Параметр sockets не может быть пустым");
            { sockets = Enumerable.Range(1, SocketQuantity).ToArray(); }

            var DoMCCards = GetDoMCCardTCPConnectionBySockets(sockets);
            foreach (DoMCCardTCPClient card in DoMCCards)
            {
                if (!card.IsCardInUseForChecking() || !card.IsStarted) continue;
                card.SendCommandSetSocketReadingParameters(AnswerWithImage, ExternalStart, FastRead, ResetReady);
            }

            Thread.Sleep(10);

        }
        /// <summary>
        /// Command = 5
        /// </summary>
        public void StartReadSeveralSocketsExternal(int[] socketNums, bool FastRead)
        {
            if (!IsStarted) throw new System.Net.Sockets.SocketException((int)System.Net.Sockets.SocketError.NotConnected);
            ResetSocketStatistics();
            if (socketNums == null || socketNums.Length == 0) throw new ArgumentException("Параметр socketNums не может быть пустым");

            var DoMCCards = GetDoMCCardTCPConnectionBySockets(socketNums);
            foreach (DoMCCardTCPClient card in DoMCCards)
            {
                if (!card.IsCardInUseForChecking() || !card.IsStarted) continue;
                card.SendCommandSetSocketReadingParameters(false, true, FastRead, true);
            }
            Thread.Sleep(10);
        }

        /// <summary>
        /// Command = 9, получить прочитанные изображения по всем гнездам
        /// </summary>

        public void StartGetAllSocketImages()
        {
            if (!IsStarted) throw new System.Net.Sockets.SocketException((int)System.Net.Sockets.SocketError.NotConnected);
            ResetSocketStatistics();

            for (int card = 0; card < DoMCCardTCPClients.Length; card++)
            {
                if (!DoMCCardTCPClients[card].IsStarted) continue;

                if (!DoMCCardTCPClients[card].IsCardInUseForChecking()) continue;

                //DoMCCardTCPClients[card].GetImageDataFromAllSockets(ReadTimeout);
                //DoMCCardTCPClients[card].SendCommandGetAllSocketImages();
                DoMCCardTCPClients[card].StartGetImageDataFromAllSocketsWithCommandThread(ReadTimeout);

            }
            Thread.Sleep(10);

        }
        /// <summary>
        /// Command = 9, получить прочитанные изображения по указанным гнездам
        /// </summary>

        public void StartGetSocketsImages(int[] socketNums)
        {

            if (!IsStarted) throw new System.Net.Sockets.SocketException((int)System.Net.Sockets.SocketError.NotConnected);
            ResetSocketStatistics();
            if (socketNums == null)
            {
                socketNums = Enumerable.Range(1, SocketQuantity).ToArray();
            }
            var phSockets = GetCCDSocketNumbersByDisplaySockets(socketNums);
            foreach (CCDSocketNumber ccdsn in phSockets)
            {
                if (!DoMCCardTCPClients[ccdsn.CCDCardNumber].IsStarted) continue;
                var inner = ccdsn.InnerSocketNumber;
                DoMCCardTCPClients[ccdsn.CCDCardNumber].SendCommandGetSocketsImages((byte)inner);
                DoMCCardTCPClients[ccdsn.CCDCardNumber].GetImageDataFromSocketSync(inner, ReadTimeout);
            }
            Thread.Sleep(10);

        }

        #endregion


        public class StopCommand : CommandBase
        {
            public StopCommand(IMainController mainController, ModuleBase module) : base(mainController, module, null, null) { }
            protected override void Executing() => ((DoMCCardDataModule)Module).Start();
        }

    }


}
