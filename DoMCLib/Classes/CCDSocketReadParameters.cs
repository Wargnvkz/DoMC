using DoMCLib.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using ToolLibrary;

namespace DoMCLib.Classes
{
     
    public class CCDSocketReadParameters
    {
         
        public int FilterModule;
         
        public int CompareThreshold;
        /// <summary>
        /// 0 – Измеренные данные;
        /// 1 – Фильтрованые данные;
        /// 2 – Результат сравнения;
        /// 3 – Сохраненный эталон;
        /// </summary>
         
        public int DataType;
         
        public int Exposition;
         
        public int FrameDuration;
         
        public int MeasureDelay;
         
        public bool AnswerAfterScanAutomaticly;
        /// <summary>
        /// Параметры сравнения изображения с эталоном
        /// </summary>
         
        public ImageProcessParameters ImageProcessParameters = new ImageProcessParameters();

        [IgnoreDataMember]
        public short[,] StandardImage;
         
        public string ImageText
        {
            get
            {
                if (StandardImage == null) return "";
                return Tools.ImageTools.ToBase64(Tools.ImageTools.Compress(Tools.ImageTools.ImageToArray(StandardImage)));
            }
            set
            {
                if (String.IsNullOrEmpty(value)) StandardImage = new short[512, 512];
                else
                    StandardImage = Tools.ImageTools.ArrayToImage(Tools.ImageTools.Decompress(Tools.ImageTools.FromBase64(value)));
            }
        }

        [IgnoreDataMember]
        public short[][,] TempImages;
        [IgnoreDataMember]
        public Tools.Deviation TempDeviations;
        [IgnoreDataMember]
        public short[,] TempAverageImage;
        [IgnoreDataMember]
        public int DeviationLevel;


        public CCDCardConfigRequestB GetMainConfiguration()
        {
            var conf = new CCDCardConfigRequestB()
            {
                Command = 0x0b,
                Address = 1,
                First = 0,//(ushort)StartLine,
                End = 511,//(ushort)EndLine,
                FirstMask = 0,//(ushort)MaskStart,
                EndMask = 0,//(ushort)MaskEnd,
                FilterModule = (ushort)FilterModule,
                MeasureDelay = (ushort)MeasureDelay,
                Mode = (CCDCardConfigRequestBMode)DataType,
                Threshold = (ushort)CompareThreshold,
            };
            return conf;
        }
        public CCDCardFrameParamsRequest4 GetFrameExpositionConfiguration()
        {
            var conf = new CCDCardFrameParamsRequest4()
            {
                Command = 0x04,
                Address = 1,
                FrameDuration = (ushort)FrameDuration,
                Exposition = (ushort)Exposition
            };
            return conf;
        }

        public CCDCardConfigRequest5 GetStartConfiguration(bool ExternalStart, bool ResetReady = false, bool AnswerWithNoRequest = false)
        {
            //бит 0 - внешний старт, 1 - нет выдачи готовности без запроса, 2 - сброс флага готовности
            var cfg = new CCDCardConfigRequest5() { Config = (byte)((ResetReady ? 1 : 0) << 2 | (AnswerWithNoRequest ? 1 : 0) << 1 | (ExternalStart ? 1 : 0)) };
            return cfg;

        }

        public CCDSocketReadParameters Clone()
        {
            var cfg = new CCDSocketReadParameters
            {
                FilterModule = FilterModule,
                CompareThreshold = CompareThreshold,

                DataType = DataType,
                Exposition = Exposition,
                FrameDuration = FrameDuration,
                MeasureDelay = MeasureDelay,

                AnswerAfterScanAutomaticly = AnswerAfterScanAutomaticly,
                /*ImageProcessParameters = new ImageProcessParameters()
                {
                    BottomBorder = ImageProcessParameters.BottomBorder,
                    TopBorder = ImageProcessParameters.TopBorder,
                    LeftBorder = ImageProcessParameters.LeftBorder,
                    RightBorder = ImageProcessParameters.RightBorder,
                    DeviationWindow = ImageProcessParameters.DeviationWindow,
                    MaxAverage = ImageProcessParameters.MaxAverage,
                    MaxDeviation=ImageProcessParameters.MaxDeviation
                }*/
            };
            cfg.ImageProcessParameters = ImageProcessParameters.Clone();
            return cfg;

        }
    }
    public class CCDCardReadRequest1
    {
        [BinaryConverter.Byte]
        public byte Address = 1;
        [BinaryConverter.Byte]
        public byte Command = 1;
    }
    public class CCDCardReadResponse1
    {
        [BinaryConverter.Byte]
        public byte Address = 1;
        [BinaryConverter.Byte]
        public byte Command = 1;
        [BinaryConverter.OneDimensionalArray(typeof(byte), 84)]
        public byte[] Data;
    }
    public class CCDCardConfigRequest5
    {
        [BinaryConverter.Byte]
        public byte Address = 1;
        [BinaryConverter.Byte]
        public byte Command = 5;
        [BinaryConverter.UInt16]
        public ushort Config = 4; //бит 0 - внешний старт, 1 - нет выдачи готовности без запроса, 2 - сброс флага готовности
        public static CCDCardConfigRequest5 GetConfiguration(bool ResetReady, bool AnswerWithoutRequest, bool ExternalStart, bool FastRead)
        {
            //бит 0 - внешний старт, 1 - нет выдачи готовности без запроса, 2 - сброс флага готовности
            var cfg = new CCDCardConfigRequest5() { Config = (byte)((FastRead ? 1 : 0) << 3 | (ResetReady ? 1 : 0) << 2 | (AnswerWithoutRequest ? 1 : 0) << 1 | (ExternalStart ? 1 : 0)) };
            return cfg;

        }
    }
    public class CCDCardConfigResponse5
    {
        [BinaryConverter.Byte]
        public byte Address = 1;
        [BinaryConverter.Byte]
        public byte Command = 5;
        [BinaryConverter.OneDimensionalArray(typeof(byte), 84)]
        public byte[] Data;
    }
    public class CCDCardFrameParamsRequest4
    {
        [BinaryConverter.Byte]
        public byte Address = 1;
        [BinaryConverter.Byte]
        public byte Command = 4;
        [BinaryConverter.UInt16]
        public ushort FrameDuration = 1300;
        [BinaryConverter.UInt16]
        public ushort Exposition = 400;
    }
    public class CCDCardFrameParamsResponse4
    {
        [BinaryConverter.Byte]
        public byte Address = 1;
        [BinaryConverter.Byte]
        public byte Command = 4;
        [BinaryConverter.OneDimensionalArray(typeof(byte), 84)]
        public byte[] Data;
    }
    public class CCDCardArrayRequest9
    {
        [BinaryConverter.Byte]
        public byte Address = 1;
        [BinaryConverter.Byte]
        public byte Command = 9;
        [BinaryConverter.UInt16]
        public ushort X = 0;
        [BinaryConverter.UInt16]
        public ushort Y = 0;
    }
    public class CCDCardArrayResponse9
    {
        [BinaryConverter.Byte]
        public byte Address;
        [BinaryConverter.Byte]
        public byte Command;
        [BinaryConverter.UInt16]
        public ushort X;
        [BinaryConverter.UInt16]
        public ushort Y;
        [BinaryConverter.OneDimensionalArray(typeof(short), 512)]
        public short[] Data;
    }
    public class CCDCardArrayResponse9PosOnly
    {
        [BinaryConverter.Byte]
        public byte Address;
        [BinaryConverter.Byte]
        public byte Command;
        [BinaryConverter.UInt16]
        public ushort X;
        [BinaryConverter.UInt16]
        public ushort Y;
        [BinaryConverter.OneDimensionalArray(typeof(ushort), 512)]
        public ushort[] Data;
    }

    public class CCDCardConfigRequestB
    {
        [BinaryConverter.Byte]
        public byte Address = 1;
        [BinaryConverter.Byte]
        public byte Command = 11;
        [BinaryConverter.UInt16]
        public ushort FilterModule = 5;
        [BinaryConverter.UInt16]
        public ushort Threshold = 20;
        [BinaryConverter.UInt16]
        public CCDCardConfigRequestBMode Mode = CCDCardConfigRequestBMode.Filtered;
        [BinaryConverter.UInt16]
        public ushort First = 10;
        [BinaryConverter.UInt16]
        public ushort End = 450;
        [BinaryConverter.UInt16]
        public ushort FirstMask = 140;
        [BinaryConverter.UInt16]
        public ushort EndMask = 160;
        [BinaryConverter.UInt16]
        public ushort MeasureDelay = 160;
        /*[BinaryConverter.OneDimensionalArray(typeof(short), 512)]
        public short[] Data;*/
    }
    public enum CCDCardConfigRequestBMode
    {
        Measure,
        Filtered,
        Comparison,
        Standard
    }

    public class CCDCardConfigResponseB
    {
        [BinaryConverter.Byte]
        public byte Address = 1;
        [BinaryConverter.Byte]
        public byte Command = 4;
        [BinaryConverter.OneDimensionalArray(typeof(byte), 84)]
        public byte[] Data;
    }
    public class CCDCardDataRequest7
    {
        [BinaryConverter.Byte]
        public byte Address = 1;
        [BinaryConverter.Byte]
        public byte Command = 7;
    }
    public class CCDCardDataResponse7
    {
        [BinaryConverter.Byte]
        public byte Address = 1;
        [BinaryConverter.Byte]
        public byte Command = 7;
        [BinaryConverter.Byte]
        public byte X;
        [BinaryConverter.Byte]
        public byte Y;
        [BinaryConverter.OneDimensionalArray(typeof(byte), 82)]
        public byte[] Data;
    }
}
