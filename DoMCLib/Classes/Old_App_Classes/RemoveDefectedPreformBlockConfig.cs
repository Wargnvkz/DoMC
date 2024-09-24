using System.Runtime.Serialization;

namespace DoMCLib.Classes
{
    [DataContract]
    public class RemoveDefectedPreformBlockConfig
    {
        [DataMember]
        public string IP;
        [DataMember]
        public int Port = 4001;
        [DataMember]
        public int CoolingBlocksQuantity;
        [DataMember]
        public int MachineNumber = 11;
        [DataMember]
        public bool SendBadCycleToRDPB = false;
        public bool IsConfigReady
        {
            get
            {
                return !String.IsNullOrWhiteSpace(IP) && (Port > 0 && Port < 65536) && (CoolingBlocksQuantity > 2 && CoolingBlocksQuantity < 5);
            }
        }
    }
}
