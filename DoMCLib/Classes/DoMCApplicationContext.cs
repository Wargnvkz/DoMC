using DoMCLib.Classes.Configuration.CCD;
using DoMCLib.Classes.Module.CCD.Commands.Classes;
using DoMCLib.Classes.Module.CCD;
using DoMCLib.Classes.Module.LCB;
using DoMCLib.Configuration;
using DoMCLib.Tools;
using DoMCModuleControl.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Modules;
using DoMCModuleControl;
using static DoMCLib.Classes.Module.CCD.CCDCardDataModule;

namespace DoMCLib.Classes
{
    public class DoMCApplicationContext
    {
        public static string ConfigurationUpdateEventName = "ConfigurationUpdate";
        public static string SettingsInterfaceClosedEventName = "SettingsInterfaceClosed";
        public const string FileName = "DoMC.cfg";
        public static string StandardFolder = "Standards";
        public ApplicationConfiguration Configuration { get; set; }
        /// <summary>
        /// Индекс 0 - реальное гнездо 1, то есть при получении физического номера для платы нужно использовать логический номер гнезда(нумерацию с 1 сверху вниз слева направо минус 1)
        /// </summary>
        public int[] EquipmentSocket2CardSocket;
        public bool IsInWorkingMode;
        public LastCCDAction LastAction;
        public DoMCApplicationContext()
        {
            Configuration = new ApplicationConfiguration(FileName);
            Configuration.LoadConfiguration();
            FillEquipmentSocket2CardSocket();
        }

        public List<TCPCardSocket> GetWorkingPhysicalSocket()
        {
            var result = new List<TCPCardSocket>();
            if (Configuration == null) return result;
            FillEquipmentSocket2CardSocket();
            for (int i = 0; i < EquipmentSocket2CardSocket.Length; i++)
            {
                if (Configuration.HardwareSettings.SocketsToCheck[i])
                {
                    var physicalSocket = EquipmentSocket2CardSocket[i];
                    var socket = new TCPCardSocket(physicalSocket);
                    result.Add(socket);
                }
            }
            return result;
        }
        public TCPCardSocket GetWorkingPhysicalSocket(int EquipmentSocket)
        {
            if (Configuration == null) throw new NullReferenceException(nameof(Configuration));
            FillEquipmentSocket2CardSocket();

            var physicalSocket = EquipmentSocket2CardSocket[EquipmentSocket];
            var socket = new TCPCardSocket(physicalSocket);
            return socket;
        }
        public List<int> GetWorkingCards(List<TCPCardSocket> WorkingPhysicalSocket)
        {
            var cards = new HashSet<int>();
            foreach (var socket in WorkingPhysicalSocket)
            {
                cards.Add(socket.CCDCardNumber);
            }
            return cards.ToList();
        }

        public void FillEquipmentSocket2CardSocket()
        {
            var maxSockets = Configuration.HardwareSettings.CardSocket2EquipmentSocket.Max();
            if (maxSockets == 0)
            {
                foreach (int i in Enumerable.Range(1, 96))
                {
                    Configuration.HardwareSettings.CardSocket2EquipmentSocket[i - 1] = i;
                }
            }
            maxSockets = Configuration.HardwareSettings.CardSocket2EquipmentSocket.Max();
            EquipmentSocket2CardSocket = new int[maxSockets];
            for (int i = 0; i < Configuration.HardwareSettings.CardSocket2EquipmentSocket.Length; i++)
            {
                EquipmentSocket2CardSocket[Configuration.HardwareSettings.CardSocket2EquipmentSocket[i] - 1] = i;
            }
        }

        public List<(int, SocketParameters)> GetCardParametersByCardList(List<int> CCDCardList)
        {
            var result = new List<(int, SocketParameters)>();
            for (int card = 0; card < CCDCardList.Count; card++)
            {
                var CardSocket = new TCPCardSocket(CCDCardList[card], 0);
                var EquipmentSocket = Configuration.HardwareSettings.CardSocket2EquipmentSocket[CardSocket.CardSocketNumber()];
                result.Add((CCDCardList[card], Configuration.ReadingSocketsSettings.CCDSocketParameters[EquipmentSocket - 1]));
            }
            return result;
        }
        /// <summary>
        /// Получает список гнезщд на плате и их параметров из физических гнезд
        /// </summary>
        /// <param name="EquipmentSockets">Список номеров физических гнезд матрицы начиная с 0</param>
        /// <returns>(номер гнезда матрицы, плата и гнездо, параметры гнезда</returns>
        public List<(int EquipmentSocketNumber, TCPCardSocket CardSocket, SocketParameters SocketParameters)> GetSocketParametersByEquipmentSockets(List<int> EquipmentSockets)
        {
            FillEquipmentSocket2CardSocket();
            var result = new List<(int, TCPCardSocket, SocketParameters)>();
            for (int eqSocket = 0; eqSocket < EquipmentSockets.Count; eqSocket++)
            {
                var cSocket = EquipmentSocket2CardSocket[EquipmentSockets[eqSocket]];
                var CardSocket = new TCPCardSocket(cSocket);
                result.Add((EquipmentSockets[eqSocket], CardSocket, Configuration.ReadingSocketsSettings.CCDSocketParameters[EquipmentSockets[eqSocket]]));
            }
            return result;
        }



        public class ErrorsReadingData
        {
            List<ErrorReadingData> errors = new List<ErrorReadingData>();
            public void Clear()
            {
                errors.Clear();
            }
            public void AddReadingError(ErrorReadingData data)
            {
                errors.Add(data);
            }

            public List<int> ErrorCards()
            {
                var errcards = errors.Select(e => e.CardNumber).Distinct().ToList();
                return errcards;
            }

            public class ErrorReadingData
            {
                public int CardNumber;
                public int SocketNumber;
                public int ReadBytes;
                public ErrorReadingData((int CardNumber, int SocketNumber, int ReadBytes) data)
                {
                    CardNumber = data.CardNumber;
                    SocketNumber = data.SocketNumber;
                    ReadBytes = data.ReadBytes;
                }
            }
        }
        public enum LastCCDAction
        {
            Starting,
            LoadConfig,
            Reading,
            GettingImages,
            Stopping
        }
    }

}
