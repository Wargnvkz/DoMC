namespace DoMCLib.Classes.Module.LCB
{
    public class LEDDataExchangeStatus
    {
        public bool LСBInitialized;
        public bool LCBConfigurationLoaded;
        public DateTime TimePreviousSyncSignalGot;
        public DateTime TimeSyncSignalGot;
        public DateTime TimeLEDStatusGot;
        public DateTime InOutStatusGot;
        public bool[] LEDStatuses;
        public int LEDCurrent;
        public int PreformLength;
        public int DelayLength;
        public int MaximumHorizontalStroke;
        public int CurrentHorizontalStroke;

        public bool[] Inputs = new bool[8];
        public bool[] Outputs = new bool[6];
        public bool Magnets;
        public bool Valve;

        public DateTime LastCommandSent;
        public int NumberOfLastCommandSent;
        public DateTime LastCommandResponseReceived;
        public int NumberOfLastCommandReceived;
        public bool LastCommandReceivedStatusIsOK;
        public DateTime LastMovementParametersReceived;

        public DateTime UDPReceived;

        public TimeSpan CycleDuration()
        {
            var _0 = new TimeSpan(0);
            if (TimeSyncSignalGot == DateTime.MinValue || TimePreviousSyncSignalGot == DateTime.MinValue) return _0;
            return TimeSyncSignalGot - TimePreviousSyncSignalGot;

        }
        public void ResetCycleDuration()
        {
            TimeSyncSignalGot = DateTime.MinValue;
            TimePreviousSyncSignalGot = DateTime.MinValue;

        }

        public LEDDataExchangeStatus Clone()
        {
            var CopyStatus = new LEDDataExchangeStatus();
            CopyStatus.LСBInitialized = LСBInitialized;
            CopyStatus.LCBConfigurationLoaded = LCBConfigurationLoaded;
            CopyStatus.TimePreviousSyncSignalGot = TimePreviousSyncSignalGot;
            CopyStatus.TimeSyncSignalGot = TimeSyncSignalGot;
            CopyStatus.TimeLEDStatusGot = TimeLEDStatusGot;
            CopyStatus.InOutStatusGot = InOutStatusGot;
            CopyStatus.LEDStatuses = LEDStatuses;
            CopyStatus.LEDCurrent = LEDCurrent;
            CopyStatus.PreformLength = PreformLength;
            CopyStatus.DelayLength = DelayLength;
            CopyStatus.MaximumHorizontalStroke = MaximumHorizontalStroke;
            CopyStatus.CurrentHorizontalStroke = CurrentHorizontalStroke;
            CopyStatus.Inputs = new bool[8];
            Array.Copy(CopyStatus.Inputs, Inputs, Inputs.Length);
            CopyStatus.Outputs = new bool[6];
            Array.Copy(CopyStatus.Outputs, Outputs, Outputs.Length);
            CopyStatus.Magnets = Magnets;
            CopyStatus.Valve = Valve;
            CopyStatus.LastCommandSent = LastCommandSent;
            CopyStatus.NumberOfLastCommandSent = NumberOfLastCommandSent;
            CopyStatus.LastCommandResponseReceived = LastCommandResponseReceived;
            CopyStatus.NumberOfLastCommandReceived = NumberOfLastCommandReceived;
            CopyStatus.LastCommandReceivedStatusIsOK = LastCommandReceivedStatusIsOK;
            CopyStatus.LastMovementParametersReceived = LastMovementParametersReceived;
            CopyStatus.UDPReceived = UDPReceived;
            return CopyStatus;
        }
    }
}
