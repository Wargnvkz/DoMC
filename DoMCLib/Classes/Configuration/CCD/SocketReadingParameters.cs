using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using DoMCLib.Classes.Module.CCD.CCDCardDataExchangeCommandClasses;
using static DoMCLib.Classes.Configuration.CCD.SocketParameters;

namespace DoMCLib.Classes.Configuration.CCD
{

    public class SocketReadingParameters
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


        public CCDCardConfigRequestB GetReadingParametersConfiguration()
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

        public SocketReadingParameters Clone()
        {
            var cfg = new SocketReadingParameters
            {
                FilterModule = FilterModule,
                CompareThreshold = CompareThreshold,

                DataType = DataType,
                Exposition = Exposition,
                FrameDuration = FrameDuration,
                MeasureDelay = MeasureDelay,

                AnswerAfterScanAutomaticly = AnswerAfterScanAutomaticly,
            };
            return cfg;

        }
        public void FillTarget(ref SocketReadingParameters target, CopySocketReadingParameters copySocketReadingParameters)
        {
            if (!copySocketReadingParameters.HasAnySelection()) return;
            if (target == null) target = new SocketReadingParameters();
            if (copySocketReadingParameters.FilterModule) target.FilterModule = FilterModule;
            if (copySocketReadingParameters.CompareThreshold) target.CompareThreshold = CompareThreshold;
            if (copySocketReadingParameters.DataType) target.DataType = DataType;
            if (copySocketReadingParameters.Exposition) target.Exposition = Exposition;
            if (copySocketReadingParameters.FrameDuration) target.FrameDuration = FrameDuration;
            if (copySocketReadingParameters.MeasureDelay) target.MeasureDelay = MeasureDelay;
            if (copySocketReadingParameters.AnswerAfterScanAutomaticly) target.AnswerAfterScanAutomaticly = AnswerAfterScanAutomaticly;
        }
        public class CopySocketReadingParameters
        {
            public bool FilterModule;
            public bool CompareThreshold;
            public bool DataType;
            public bool Exposition;
            public bool FrameDuration;
            public bool MeasureDelay;
            public bool AnswerAfterScanAutomaticly;
            public bool HasAnySelection()
            {
                return FilterModule || CompareThreshold || DataType || Exposition || FrameDuration || MeasureDelay || AnswerAfterScanAutomaticly;
            }
        }
    }
}
