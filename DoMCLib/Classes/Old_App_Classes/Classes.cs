using DoMCLib.Configuration;
using DoMCLib.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using DoMCLib.Classes.Module.LCB;
using DoMCLib.Classes.Module.RDPB.Classes;

namespace DoMCLib.Classes
{
    public enum ModuleCommandStep
    {
        None,
        Start,
        Processing,
        Complete,
        Error,
    }

    public class CycleImagesCCD
    {
        public DateTime CycleCCDDateTime;
        public DateTime TimeLCBSyncSignalGot;
        public short[][,] WorkModeImages;
        public short[][,] Differences;
        public short[][,] StandardImage;
        public bool[] IsSocketGood; //Хорошая ли преформа
        public bool[] IsSocketHasImage; //Есть ли изображение
        public bool[] SocketsToCheck; //Нужно ли проверять гнездо
        public bool[] LEDStatuses; // горит ли светодиод
        public bool LEDStatusesAdded = false; // прочитаны ли статусы светодиодов
        public bool[] SocketsToSave; // Гнезда, которые надо сохранить. Берется из конфигурации
        private static int DefaultLEDQnt = 12;
        public short MaxDeviation;
        public Point MaxDeviationPoint;
        public short Average;
        public ImageProcessParameters[] ImageProcessParameters;
        public ImageErrorType SocketErrorType;
        public RDPBTransporterSide TransporterSide;

        public void SetLEDStatuses(bool[] LEDStatuses, DateTime TimeLCBSincSignal)
        {
            if (LEDStatuses == null)
            {
                this.LEDStatuses = new bool[DefaultLEDQnt];
                return;
            }
            this.LEDStatuses = new bool[LEDStatuses.Length];
            for (int i = 0; i < LEDStatuses.Length; i++)
            {
                this.LEDStatuses[i] = (LEDStatuses[i]);
            }
            this.TimeLCBSyncSignalGot = TimeLCBSincSignal;
            this.LEDStatusesAdded = true;
        }

        public void SetImageProcessParameters(ImageProcessParameters[] parameters)
        {
            ImageProcessParameters = parameters;
        }
    }

    public class Box
    {
        public int BoxID;
        public DateTime CompletedTime;
        public int BadCyclesCount;
        public RDPBTransporterSide TransporterSide;

    }




    [DataContract]
    public class DoMCGeneralSettings
    {
        [DataMember]
        public int NCycle;
        [DataMember]
        public double StandardPercent;
        public double Koefficient
        {
            get
            {
                return Math.Exp(Math.Log(StandardPercent / 100) / NCycle);
            }
        }
    }
    public class RemoveDefectedPreformBlock
    {

    }

    public class TimeoutOfActions
    {
        /// <summary>
        /// Время ожидания ответа от плат ПЗС в миллисекундах
        /// </summary>
        public int WaitForCCDCardAnswerTimeout = 30000;
        public int WaitForLCBCardAnswerTimeout = 30000;
        public int WaitForRDPBCardAnswerTimeout = 1000;
        public int DelayBeforeMoveDataToArchiveTimeInSeconds = 3600;
        public int WaitForSynchrosignalTimoutAfterCCDReadingFailed = 10000;
    }

    public enum ImageErrorType
    {
        None = 0,
        Average = 1,
        Defect = 2
    }

    public class DoMCTimings
    {
        public DateTime CCDStart;
        public DateTime CCDEnd;
        public DateTime CCDGetImagesStarted;
        public DateTime CCDGetImagesEnded;
        public DateTime CCDImagesProcessStarted;
        public DateTime CCDImagesProcessEnded;
        public DateTime CCDEtalonsRecalculateStarted;
        public DateTime CCDEtalonsRecalculateEnded;

    }
    [DataContract]
    public class MakeDecision
    {
        [DataMember]
        public List<DecisionOperation> Operations = new List<DecisionOperation>();
        [DataMember]
        public MakeDecisionAction DecisionAction;
        [DataMember]
        public DecisionActionResult Result;
        [DataMember]
        public short ParameterCompareGoodIfLess;

        public void AddNextOperation(DecisionOperation operation)
        {
            Operations.Add(operation);
        }
        public void InsertOperation(int index, DecisionOperation operation)
        {
            Operations.Insert(index, operation);
        }
        public DecisionOperation GetOperation(int index)
        {
            return Operations[index];
        }
        public void RemoveOperation(int index)
        {
            Operations.RemoveAt(index);
        }
        public bool IsImageGood(short[,] std, short[,] img, ImageProcessParameters ipp, out short[,] ResultImg, out Point MaxCoord)
        {
            short[][,] res;
            if (Operations != null)
            {
                res = new short[][,] { std, img };
                foreach (var op in Operations)
                {
                    res = op.Operation(ipp, res);
                }
            }
            else
            {
                var op = new DecisionOperation() { OperationType = DecisionOperationType.Difference };
                res = new short[][,] { std, img };
                res = op.Operation(ipp, res);
            }
            ResultImg = res[0];
            MaxCoord = new Point(0, 0);
            switch (DecisionAction)
            {
                case MakeDecisionAction.Average:
                    var avg = ImageTools.Average(res[0]);
                    return avg < ParameterCompareGoodIfLess;
                case MakeDecisionAction.Max:
                    var imgarr = res[0].Cast<short>().ToArray();
                    var max = Math.Max(Math.Abs(imgarr.Max()), Math.Abs(imgarr.Min()));
                    var index = Array.FindIndex<short>(imgarr, e => Math.Abs(e) == max);
                    MaxCoord = new Point(index % 512, index / 512);
                    return max < ParameterCompareGoodIfLess;
                default: return true;
            }
        }

        public static List<Tuple<DecisionOperationType, string>> GetDecisionOperationTypeList()
        {
            return new List<Tuple<DecisionOperationType, string>>()
            {
                new Tuple<DecisionOperationType, string>(DecisionOperationType.None,"-"),
                new Tuple<DecisionOperationType, string>(DecisionOperationType.Normalize,"Нормализация"),
                new Tuple<DecisionOperationType, string>(DecisionOperationType.Difference,"Разница"),
                new Tuple<DecisionOperationType, string>(DecisionOperationType.Dispersion,"Дисперсия")
            };
        }
        public static List<Tuple<MakeDecisionAction, string>> GetMakeDecisionActionList()
        {
            return new List<Tuple<MakeDecisionAction, string>>()
            {
                new Tuple<MakeDecisionAction, string>(MakeDecisionAction.Average,"Среднее"),
                new Tuple<MakeDecisionAction, string>(MakeDecisionAction.Max,"Максимум")
            };
        }

        public MakeDecision Clone()
        {
            var md = new MakeDecision();
            md.DecisionAction = DecisionAction;
            md.Result = Result;
            md.ParameterCompareGoodIfLess = ParameterCompareGoodIfLess;
            md.Operations = new List<DecisionOperation>();
            md.Operations.AddRange(Operations.Select(o => o.Clone()));
            return md;
        }
    }
    [DataContract]
    public class DecisionOperation
    {
        [DataMember]
        public DecisionOperationType OperationType;
        [DataMember]
        public short Parameter;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipp"></param>
        /// <param name="img">index 0 - standard, if there are more than one image is passed</param>
        /// <returns></returns>
        public short[][,] Operation(ImageProcessParameters ipp, params short[][,] img)
        {
            switch (OperationType)
            {
                case DecisionOperationType.Dispersion:
                    return new short[][,] { ImageTools.DeviationByLine(img[0], Parameter) };
                case DecisionOperationType.Normalize:
                    var stdarr = img[0].Cast<short>();
                    var max = stdarr.Max();
                    var min = stdarr.Min();
                    var k = (double)Parameter / Math.Max(Math.Abs(max), Math.Abs(min));
                    var res = new short[img.Length][,];
                    for (int i = 0; i < res.Length; i++)
                    {
                        res[i] = ImageTools.Multiply(img[i], k);
                    }
                    return res;

                case DecisionOperationType.Difference:
                    return new short[][,] { ImageTools.GetDifference(img[0], img[1], ipp.GetRectangle()) };
                default: return new short[0][,];
            }
        }
        public DecisionOperation Clone()
        {
            var op = new DecisionOperation();
            op.OperationType = OperationType;
            op.Parameter = Parameter;
            return op;
        }
    }
    public enum DecisionOperationType
    {
        None,
        Normalize,
        Difference,
        Dispersion,
    }
    public enum MakeDecisionAction
    {
        Average,
        Max
    }
    public enum DecisionActionResult
    {
        Defect,
        Color
    }
}
