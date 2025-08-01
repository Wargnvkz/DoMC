﻿using DoMCLib.Tools;

namespace DoMCLib.Classes.Module.CCD.Commands.Classes
{
    public class GetImageDataCommandResponse : CCDCardDataCommandResponse
    {
        public bool[] completedSuccessfully = new bool[12];
        public bool[] error = new bool[12];
        public SocketReadData[][] CardsImageData = new SocketReadData[12][];
        public int[] EquipmentSocket2CardSocket = new int[96];
        public void Clear()
        {
            base.Clear();
            Array.Fill(completedSuccessfully, false);
            Array.Fill(error, false);
        }
        public void SetCardCompleteSuccessfully(int i) => completedSuccessfully[i] = true;
        public void SetCardError(int i) => error[i] = true;
        //public void SetSocketReadData(int socket, SocketReadData data) => Data[socket] = data;

        public List<int> GetCardsNotStopped()
        {
            //TODO: Понять как реагировать на ошибку при чтении картинки гнезда. Все отменять и выходить или ждать и дочитывать
            return Enumerable.Range(0, 12).Where(i => requested[i] && !answered[i] && !completedSuccessfully[i] && !error[i] || !FirstRequestSent).ToList();
        }
        public SocketReadData? this[int equipmentSocketNumber]
        {
            get
            {
                var cardSocket = new TCPCardSocket(EquipmentSocket2CardSocket[equipmentSocketNumber]);
                if (CardsImageData == null) return null;
                if (CardsImageData[cardSocket.CCDCardNumber] == null) return null;
                if (CardsImageData[cardSocket.CCDCardNumber].Length < cardSocket.InnerSocketNumber) return null;
                return CardsImageData[cardSocket.CCDCardNumber][cardSocket.InnerSocketNumber];
            }
        }
    }
}
