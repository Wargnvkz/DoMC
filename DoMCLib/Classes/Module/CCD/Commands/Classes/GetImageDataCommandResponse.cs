namespace DoMCLib.Classes.Module.CCD.Commands.Classes
{
    public class GetImageDataCommandResponse : CCDCardDataCommandResponse
    {
        public bool[] completedSuccessfully = new bool[12];
        public bool[] error = new bool[12];
        public SocketReadData[] Data = new SocketReadData[96];
        public void Clear()
        {
            base.Clear();
            Array.Fill(completedSuccessfully, false);
            Array.Fill(error, false);
        }
        public void SetCardCompleteSuccessfully(int i) => completedSuccessfully[i] = true;
        public void SetCardError(int i) => error[i] = true;
        public void SetSocketReadData(int socket, SocketReadData data) => Data[socket] = data;

        public List<int> GetCardsNotStopped()
        {
            return Enumerable.Range(0, 12).Where(i => requested[i] && !answered[i] && !completedSuccessfully[i] && !error[i]).ToList();
        }
    }
}
