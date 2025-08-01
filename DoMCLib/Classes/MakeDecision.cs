using DoMCLib.Classes.Configuration.CCD;
using DoMCLib.Configuration;
using DoMCLib.Tools;
using System.Drawing;
using System.Runtime.Serialization;

namespace DoMCLib.Classes
{
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
        public bool IsImageGood(short[,] std, short[,] img, ImageProcessParameters ipp, out short[,] ResultImg, out Point? MaxCoord)
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
            MaxCoord = null;
            switch (DecisionAction)
            {
                case MakeDecisionAction.Average:
                    var avg = ImageTools.Average(res[0], ipp.GetRectangle());

                    return avg < ParameterCompareGoodIfLess;
                case MakeDecisionAction.Max:
                    var resultImage = ImageTools.ClearOutsideRect(res[0], ipp.GetRectangle());
                    ResultImg = resultImage;
                    var imgarr = resultImage.Cast<short>().ToArray();
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
}
