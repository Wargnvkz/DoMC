using DoMCLib.Classes.Configuration.CCD;
using DoMCLib.Configuration;
using DoMCLib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes
{
    public class ApplicationContext
    {
        public ApplicationConfiguration Configuration { get; set; }

        /// <summary>
        /// Индекс 0 - реальное гнездо 1
        /// </summary>
        public int[] EquipmentSocket2CardSocket;

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
                var CardSocket = new TCPCardSocket() { CCDCardNumber = CCDCardList[card], InnerSocketNumber = 0 };
                var EquipmentSocket = Configuration.HardwareSettings.CardSocket2EquipmentSocket[CardSocket.CardSocketNumber()];
                result.Add((CCDCardList[card], Configuration.CurrentSettings.CCDSocketParameters[EquipmentSocket - 1]));
            }
            return result;
        }
        /// <summary>
        /// Получает список гнезщд на плате и их параметров из физических гнезд
        /// </summary>
        /// <param name="EquipmentSockets">Список номеров физических гнезд матрицы начиная с 1</param>
        /// <returns>(номер гнезда матрицы, плата и гнездо, параметры гнезда</returns>
        public List<(int,TCPCardSocket, SocketParameters)> GetSocketParametersByEquipmentSockets(List<int> EquipmentSockets)
        {
            FillEquipmentSocket2CardSocket();
            var result = new List<(int,TCPCardSocket, SocketParameters)>();
            for (int eqSocket = 0; eqSocket < EquipmentSockets.Count; eqSocket++)
            {
                var cSocket = EquipmentSocket2CardSocket[eqSocket - 1];
                var CardSocket = new TCPCardSocket(cSocket);
                result.Add((eqSocket,CardSocket, Configuration.CurrentSettings.CCDSocketParameters[eqSocket - 1]));
            }
            return result;
        }
    }
}
