using System.Drawing;
using System.Runtime.Serialization;
using DoMCLib.Classes;

namespace DoMCLib.Configuration
{
    [DataContract]
    public class ImageProcessParameters
    {
        /*[DataMember]
        public int DeviationWindow = 10;
        [DataMember]
        public short MaxDeviation = 1000;
        [DataMember]
        public short MaxAverage = 1000;*/
        [DataMember]
        public int TopBorder = 0;
        [DataMember]
        public int BottomBorder = 511;
        [DataMember]
        public int LeftBorder = 0;
        [DataMember]
        public int RightBorder = 511;
        [DataMember]
        public MakeDecision[] Decisions = new MakeDecision[2];
        public Rectangle GetRectangle()
        {
            return new Rectangle(LeftBorder, TopBorder, RightBorder - LeftBorder, BottomBorder - TopBorder);
        }

        public ImageProcessParameters Clone()
        {
            var ipp = new ImageProcessParameters()
            {
                /*DeviationWindow = this.DeviationWindow,
                MaxDeviation = this.MaxDeviation,
                MaxAverage = this.MaxAverage,*/
                TopBorder = TopBorder,
                BottomBorder = BottomBorder,
                LeftBorder = LeftBorder,
                RightBorder = RightBorder
            };
            ipp.Decisions = new MakeDecision[2];
            ipp.Decisions[0] = Decisions?[0]?.Clone() ?? new MakeDecision();
            ipp.Decisions[1] = Decisions?[1]?.Clone() ?? new MakeDecision();
            return ipp;
        }
    }
}
