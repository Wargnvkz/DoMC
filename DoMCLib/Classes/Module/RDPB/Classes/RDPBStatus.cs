using System.Diagnostics;
using System.Text;

namespace DoMCLib.Classes.Module.RDPB.Classes
{
    public class RDPBStatus
    {
        public int MachineNumber;
        public RDPBCommandType CommandType;
        public RDPBCommandType SentCommandType;
        public int CycleNumber;
        public int CoolingBlocksQuantity;
        public int CoolingBlocksQuantityToSet;
        //Ответ на хороший/плохой съем
        public RDPBTransporterSide TransporterSide = RDPBTransporterSide.Stoped;
        public RDPBErrors Errors = RDPBErrors.NoErrors;
        public BoxDirectionType BoxDirection = BoxDirectionType.Left;
        public bool BlockIsOn;
        public int BoxNumber;
        public int SetQuantityInBox;
        public int GoodSetQuantityInBox;
        public int BadSetQuantityInBox;

        public long TimeCurrentStatusGot;
        public long TimeCommandSent;
        public long TimeParametersGot;
        public long TimeLastSent;
        public long TimeLastReceived;
        private long RDPBTimeout;
        public string ManualCommand;
        public bool IsStarted;

        private Stopwatch Timer;

        #region Temporary RDPB data
        public int CurrentBoxDefectCycles;
        public int TotalDefectCycles;
        public RDPBTransporterSide CurrentTransporterSide;
        public RDPBTransporterSide PreviousDirection;
        public int ErrorCounter = 0;
        #endregion
        public RDPBStatus()
        {
            Timer = new Stopwatch();
            Timer.Start();
        }
        public bool ResponseGot
        {
            get
            {
                return TimeLastReceived > TimeLastSent;
            }
        }

        public bool IsTimeout
        {
            get
            {
                return TimeLastSent != 0 && !ResponseGot && (Timer.ElapsedTicks - TimeLastSent) < RDPBTimeout;

            }
        }

        public bool IsCurrentStatusActual()
        {
            return TimeCommandSent < TimeCurrentStatusGot;
        }
        public bool IsParametersActual()
        {
            return TimeCommandSent < TimeParametersGot;
        }

        public void SetTimeout(int Timeoutms)
        {
            RDPBTimeout = Timeoutms * 10000;
        }

        // Данные в сети в тексте по 4 байта в Hex с пробелами

        private string Data2Hex(ushort[] data)
        {
            string[] values = new string[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                var v = data[i];
                var lo = (byte)v;
                var hi = (byte)(v >> 8);
                values[i] = String.Format("{0:X2}{1:X2} ", hi, lo);
            }
            var res = string.Join("", values);
            return res;
        }

        private string CreateHeader(byte machine, RDPBCommandType command)
        {
            var v = (int)command;
            var lo = (char)((byte)v);
            var hi = (char)(v >> 8);
            var res = String.Format("N{0:X1}{1}{2} ", machine, hi, lo);
            return res;
        }

        public static string CalcLRC(string BaseString)
        {
            var sum = 0;
            for (int i = 0; i < BaseString.Length; i++)
            {
                sum += BaseString[i];
            }
            //byte lrc=(byte)(0xff - (byte)sum + 1);
            byte lrc = (byte)(-(byte)sum);
            var res = String.Format("{0:X2}\r\n", lrc);
            return res;
        }

        public new string ToString()
        {
            ushort[] data = new ushort[0];
            switch (CommandType)
            {
                case RDPBCommandType.SetIsOK:
                    {

                    }
                    break;
                case RDPBCommandType.SetIsBad:
                    {

                    }
                    break;
                case RDPBCommandType.On:
                    {

                    }
                    break;
                case RDPBCommandType.Off:
                    {

                    }
                    break;
                case RDPBCommandType.GetParameters:
                    {

                    }
                    break;
                case RDPBCommandType.SetCoolingBlocks:
                    {
                        data = new ushort[1];
                        data[0] = (ushort)CoolingBlocksQuantity;
                    }
                    break;
            }
            var hexdata = Data2Hex(data);
            var header = CreateHeader((byte)MachineNumber, CommandType);
            StringBuilder sb = new StringBuilder();
            sb.Append(header);
            sb.Append(hexdata);
            sb.Append(CalcLRC(sb.ToString()));
            return sb.ToString();
        }

        public string ToResponseString()
        {
            ushort[] data = new ushort[0];
            switch (CommandType)
            {
                case RDPBCommandType.On:
                case RDPBCommandType.Off:
                case RDPBCommandType.SetIsOK:
                case RDPBCommandType.SetIsBad:
                    {
                        data = new ushort[2];
                        data[0] = 1;
                        data[1] = (ushort)((CoolingBlocksQuantity << 12) | ((BlockIsOn ? 1 : 0) << 8) | ((int)(TransporterSide - 0x30) << 4) | ((int)(Errors - 0x30)));
                    }
                    break;
                case RDPBCommandType.GetParameters:
                    {
                        data = new ushort[4];
                        data[0] = 0x2001;
                        data[1] = 1;
                        data[2] = 1;
                        data[3] = 0;
                    }
                    break;
                case RDPBCommandType.SetCoolingBlocks:
                    {
                        data = new ushort[1];
                        data[0] = (ushort)CoolingBlocksQuantity;
                    }
                    break;
            }
            var hexdata = Data2Hex(data);
            var header = CreateHeader((byte)MachineNumber, CommandType);
            StringBuilder sb = new StringBuilder();
            sb.Append(header);
            sb.Append(hexdata);
            sb.Append(CalcLRC(sb.ToString()));
            return sb.ToString();
        }

        public StatusStringProccessResult ChangeFromString(string str)
        {
            if (str.StartsWith("N") && str.EndsWith("\r\n"))
            {
                var lrcindex = str.LastIndexOf(" ");
                if (lrcindex == -1 || lrcindex != str.Length - 5) return StatusStringProccessResult.WrongStringFormat;
                var lrcstr = str.Substring(lrcindex + 1, 4).ToUpper();
                var basestring = str.Substring(0, lrcindex + 1);
                var lrcbase = CalcLRC(basestring);
                if (lrcbase != lrcstr) return StatusStringProccessResult.BadCRC; // несовпадение контрольной суммы
                var doublespace = basestring.IndexOf("  ");
                if (doublespace != -1) return StatusStringProccessResult.WrongStringFormat; // неправильный формат
                var parts = basestring.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Any(p => !(p.Length == 4 || p.Length == 0))) return StatusStringProccessResult.WrongStringFormat; // неправильный формат
                int.TryParse(parts[0][1].ToString(), System.Globalization.NumberStyles.HexNumber, null, out this.MachineNumber);
                var cmd = (RDPBCommandType)BitConverter.ToUInt16(Encoding.ASCII.GetBytes(parts[0].Substring(2, 2)).Reverse().ToArray(), 0);
                this.CommandType = cmd;
                this.TimeCurrentStatusGot = Timer.ElapsedTicks;
                switch (cmd)
                {
                    case RDPBCommandType.SetIsOK:
                    case RDPBCommandType.SetIsBad:
                    case RDPBCommandType.On:
                    case RDPBCommandType.Off:
                        {
                            if (parts.Length != 3) return StatusStringProccessResult.WrongStringFormat;
                            int.TryParse(parts[1], System.Globalization.NumberStyles.HexNumber, null, out this.CycleNumber);
                            int.TryParse(parts[2][0].ToString(), out this.CoolingBlocksQuantity);
                            int.TryParse(parts[2][1].ToString(), out int iBlockIsOn);
                            this.BlockIsOn = Convert.ToBoolean(iBlockIsOn);
                            this.TransporterSide = (RDPBTransporterSide)(parts[2][2]);
                            this.Errors = (RDPBErrors)parts[2][3];
                            // DateTime.Now;

                        }
                        break;
                    case RDPBCommandType.GetParameters:
                        {
                            if (parts.Length != 5) return StatusStringProccessResult.WrongStringFormat;
                            switch (parts[1][0])
                            {
                                case '2':
                                    this.BoxDirection = BoxDirectionType.Left;
                                    break;
                                case '4':
                                    this.BoxDirection = BoxDirectionType.Right;
                                    break;
                                default:
                                    this.BoxDirection = BoxDirectionType.Unknown;
                                    break;
                            }
                            int.TryParse(parts[1].Substring(1, 3), System.Globalization.NumberStyles.HexNumber, null, out this.BoxNumber);
                            int.TryParse(parts[2], System.Globalization.NumberStyles.HexNumber, null, out this.SetQuantityInBox);
                            int.TryParse(parts[3], System.Globalization.NumberStyles.HexNumber, null, out this.GoodSetQuantityInBox);
                            int.TryParse(parts[4], System.Globalization.NumberStyles.HexNumber, null, out this.BadSetQuantityInBox);
                            SetTimeParametersGot();

                        }
                        break;
                    case RDPBCommandType.SetCoolingBlocks:
                        {
                            int.TryParse(parts[1].ToString(), out this.CoolingBlocksQuantity);

                        }
                        break;
                }
                return StatusStringProccessResult.OK;
            }
            else
            {
                return StatusStringProccessResult.WrongStringFormat;
            }
        }
        public void ChangeFromRequestString(string str)
        {
            if (str.StartsWith("N") && str.EndsWith("\r\n"))
            {
                var lrcindex = str.LastIndexOf(" ");
                if (lrcindex == -1 || lrcindex != str.Length - 5) return;
                var lrcstr = str.Substring(lrcindex + 1, 4).ToUpper();
                var basestring = str.Substring(0, lrcindex + 1);
                var lrcbase = CalcLRC(basestring);
                if (lrcbase != lrcstr) return; // несовпадение контрольной суммы
                var doublespace = basestring.IndexOf("  ");
                if (doublespace != -1) return; // неправильный формат
                var parts = basestring.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Any(p => !(p.Length == 4 || p.Length == 0))) return; // неправильный формат
                int.TryParse(parts[0][1].ToString(), System.Globalization.NumberStyles.HexNumber, null, out this.MachineNumber);
                var cmd = (RDPBCommandType)BitConverter.ToUInt16(Encoding.ASCII.GetBytes(parts[0].Substring(2, 2)).Reverse().ToArray(), 0);
                this.CommandType = cmd;
                switch (cmd)
                {
                    case RDPBCommandType.SetIsOK:
                    case RDPBCommandType.SetIsBad:
                    case RDPBCommandType.On:
                    case RDPBCommandType.Off:
                        {

                        }
                        break;
                    case RDPBCommandType.GetParameters:
                        {

                        }
                        break;
                    case RDPBCommandType.SetCoolingBlocks:
                        int.TryParse(parts[1], System.Globalization.NumberStyles.HexNumber, null, out CoolingBlocksQuantity);
                        break;
                }
                return;
            }
            else
            {
                return;
            }
        }

        /*public void SetFromConfiguration(FullDoMCConfiguration cfg)
        {
            CoolingBlocksQuantity = cfg.RemoveDefectedPreformBlockConfig?.CoolingBlocksQuantity ?? 4;
            CoolingBlocksQuantityToSet = cfg.RemoveDefectedPreformBlockConfig?.CoolingBlocksQuantity ?? 4;
        }*/

        public void SetTimeCurrentStatusGot() { TimeCurrentStatusGot = Timer.ElapsedTicks; }
        public void SetTimeCommandSent() { TimeCommandSent = Timer.ElapsedTicks; }
        public void SetTimeParametersGot() { TimeParametersGot = Timer.ElapsedTicks; }
        public void SetTimeLastSent() { TimeLastSent = Timer.ElapsedTicks; }
        public void SetTimeLastReceived() { TimeLastReceived = Timer.ElapsedTicks; }

    }

    public enum StatusStringProccessResult
    {
        OK,
        BadCRC,
        WrongStringFormat
    }
}
