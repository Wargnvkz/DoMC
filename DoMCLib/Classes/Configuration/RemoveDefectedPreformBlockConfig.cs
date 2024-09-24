namespace DoMCLib.Classes.Configuration
{
    public class RemoveDefectedPreformBlockConfig
    {

        public string IP;

        public int Port = 4001;

        public int CoolingBlocksQuantity;

        public int MachineNumber = 11;

        public bool SendBadCycleToRDPB = false;
        public bool IsConfigReady
        {
            get
            {
                return !string.IsNullOrWhiteSpace(IP) && Port > 0 && Port < 65536 && CoolingBlocksQuantity > 2 && CoolingBlocksQuantity < 5;
            }
        }
    }
}
