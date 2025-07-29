using DoMCLib.Configuration;
using DoMCLib.Tools;
using System.Runtime.Serialization;

namespace DoMCLib.Classes.Configuration.CCD
{
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
                default: return img;
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
}
