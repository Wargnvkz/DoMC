using System.Drawing;
using System.Runtime.Serialization;
using DoMCLib.Classes;
using DoMCLib.Classes.Configuration.CCD;
using static DoMCLib.Classes.Configuration.CCD.SocketParameters;

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
        public MakeDecision[] Decisions = [new MakeDecision(), new MakeDecision()];
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
            ipp.Decisions = CloneDecisions();
            return ipp;
        }
        private MakeDecision[] CloneDecisions()
        {
            var decisions = new MakeDecision[2];
            decisions[0] = Decisions?[0]?.Clone() ?? new MakeDecision();
            decisions[1] = Decisions?[1]?.Clone() ?? new MakeDecision();
            return decisions;

        }
        public void FillTarget(ref ImageProcessParameters target, CopyImageProcessParameters copyImageProcessParameters)
        {
            if (!copyImageProcessParameters.HasAnySelection()) return;
            if (target == null) target = new ImageProcessParameters();
            if (copyImageProcessParameters.TopBorder) target.TopBorder = TopBorder;
            if (copyImageProcessParameters.BottomBorder) target.BottomBorder = BottomBorder;
            if (copyImageProcessParameters.LeftBorder) target.LeftBorder = LeftBorder;
            if (copyImageProcessParameters.RightBorder) target.RightBorder = RightBorder;
            if (copyImageProcessParameters.Decisions) target.Decisions = CloneDecisions();
        }
        public class CopyImageProcessParameters
        {
            public bool TopBorder;
            public bool BottomBorder;
            public bool LeftBorder;
            public bool RightBorder;
            public bool Decisions;
            public bool HasAnySelection()
            {
                return TopBorder || BottomBorder || LeftBorder || RightBorder || Decisions;
            }
        }
    }
}
