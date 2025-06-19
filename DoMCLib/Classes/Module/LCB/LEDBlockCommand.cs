using DoMCLib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoMCLib.Classes.Module.LCB
{
    public class LEDBlockCommand
    {
        public byte Command;
        public byte Length;
        public byte[] Data;
        public byte CRC;

        public static LEDBlockCommand FromByteArray(ref byte[] buffer, byte startbyte)
        {
            try
            {
                if (buffer == null || buffer.Length == 0) return null;
                var lbc = new LEDBlockCommand();
                var startindex = Array.IndexOf(buffer, startbyte);
                if (startindex < 0) return null;
                lbc.Command = buffer[startindex + 1];
                lbc.Length = buffer[startindex + 2];
                if (lbc.Length + startindex + 3 > buffer.Length) return null;
                lbc.Data = new byte[lbc.Length];
                Array.Copy(buffer, startindex + 3, lbc.Data, 0, lbc.Length);
                lbc.CRC = buffer[startindex + lbc.Length + 3];
                var crc = CalcCRC(lbc.Data);
                if (crc != lbc.CRC)
                {
                    lbc = null;
                    Array.ConstrainedCopy(buffer, startindex + 1, buffer, 0, buffer.Length - startindex - 1);
                    Array.Resize(ref buffer, buffer.Length - startindex - 1);
                }
                else
                {
                    Array.ConstrainedCopy(buffer, startindex + lbc.Length + 4, buffer, 0, buffer.Length - startindex - (lbc.Length + 4));
                    Array.Resize(ref buffer, buffer.Length - startindex - (lbc.Length + 4));
                }
                return lbc;
            }
            catch (Exception)
            {
                return null;
            }
        }
        private static byte CalcCRC(byte[] data)
        {
            byte s = 0;
            for (int i = 0; i < data.Length; i++) s += data[i];
            return s;
        }
        public byte[] ToBytes()
        {
            if (Data == null) Data = new byte[0];
            if (Data.Length > 250) return null;

            var bytes = new byte[Data.Length + 4];
            bytes[0] = 0xff;
            bytes[1] = Command;
            bytes[2] = (byte)Data.Length;

            Array.Copy(Data, 0, bytes, 3, Data.Length);
            bytes[bytes.Length - 1] = CalcCRC(Data);
            return bytes;
        }
    }

    public enum LEDCommandType
    {
        SetLEDCurrentRequest = 0x01,
        SetLEDCurrentResponse = 0x81,

        GetLEDCurrentRequest = 0x02,
        GetLEDCurrentResponse = 0x82,

        LEDSynchrosignalResponse = 0x83,

        LEDStatusResponse = 0x84,

        SetLCBMovementParametersRequest = 0x05,
        SetLCBMovementParametersResponse = 0x85,

        GetLCBMovementParametersRequest = 0x06,
        GetLCBMovementParametersResponse = 0x86,

        SetLCBEquipmentStatusRequest = 0x07,
        SetLCBEquipmentStatusResponse = 0x87,

        GetLCBEquipmentStatusRequest = 0x08,
        GetLCBEquipmentStatusResponse = 0x88,

        SetLCBWorkModeRequest = 0x09,
        SetLCBWorkModeResponse = 0x89,

        GetLCBMaxHorizontalStrokeRequest = 0x0a,
        GetLCBMaxHorizontalStrokeResponse = 0x8a,

        GetLCBCurrentHorizontalStrokeRequest = 0x0b,
        GetLCBCurrentHorizontalStrokeResponse = 0x8b

    }
    public class LEDEquipmentStatus
    {
        public bool[] LEDStatuses = new bool[12];
        public bool[] Inputs = new bool[8];
        public bool[] Outputs = new bool[6];
        public bool Magnets;
        public bool Valve;
        public byte[] ToBytesFull()
        {
            var res = new byte[4];
            for (int i = 0; i < LEDStatuses.Length; i++)
            {
                BitOperations.Set(res, i, LEDStatuses[i]);
            }
            for (int i = 0; i < Inputs.Length; i++)
            {
                BitOperations.Set(res, i + 24, Inputs[i]);
            }
            for (int i = 0; i < Outputs.Length; i++)
            {
                BitOperations.Set(res, i + 16, Outputs[i]);
            }
            BitOperations.Set(res, 23, Magnets);
            BitOperations.Set(res, 22, Valve);
            return res;
        }
        public byte[] ToBytesLED()
        {
            var res = new byte[2];
            for (int i = 0; i < LEDStatuses.Length; i++)
            {
                BitOperations.Set(res, i, LEDStatuses[i]);
            }
            return res;
        }

        public static LEDEquipmentStatus FromBytes(byte[] bytes)
        {
            if (bytes.Length != 2 && bytes.Length != 4) return null;
            var lbc = new LEDEquipmentStatus();
            switch (bytes.Length)
            {
                case 2:
                    for (int i = 0; i < lbc.LEDStatuses.Length; i++)
                    {
                        lbc.LEDStatuses[i] = BitOperations.Get(bytes, i);
                    }
                    break;
                case 4:
                    for (int i = 0; i < lbc.LEDStatuses.Length; i++)
                    {
                        lbc.LEDStatuses[i] = BitOperations.Get(bytes, i);
                    }
                    for (int i = 0; i < lbc.Inputs.Length; i++)
                    {
                        var b = BitOperations.Get(bytes, i + 24);
                        if (i != 0) b = !b;
                        lbc.Inputs[i] = b;
                    }
                    for (int i = 0; i < lbc.Outputs.Length; i++)
                    {
                        lbc.Outputs[i] = BitOperations.Get(bytes, i + 16);
                    }
                    lbc.Magnets = BitOperations.Get(bytes, 23);
                    lbc.Valve = BitOperations.Get(bytes, 22);
                    break;
            }
            return lbc;
        }
    }

    public struct LEDMovementParameters
    {
        public int PreformLengthImpulses;
        public int DelayLengthImpulses;

        public byte[] ToBytes()
        {
            var res = new byte[4];
            res[0] = (byte)(PreformLengthImpulses & 255);
            res[1] = (byte)(PreformLengthImpulses >> 8 & 255);
            res[2] = (byte)(DelayLengthImpulses & 255);
            res[3] = (byte)(DelayLengthImpulses >> 8 & 255);
            return res;
        }

        public static LEDMovementParameters FromBytes(byte[] bytes)
        {
            var lmp = new LEDMovementParameters();
            lmp.PreformLengthImpulses = lmp.PreformLengthImpulses | bytes[0];
            lmp.PreformLengthImpulses = lmp.PreformLengthImpulses | bytes[1] << 8;
            lmp.DelayLengthImpulses = lmp.DelayLengthImpulses | bytes[2];
            lmp.DelayLengthImpulses = lmp.DelayLengthImpulses | bytes[3] << 8;
            return lmp;
        }
    }


}
