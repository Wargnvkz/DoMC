using DoMCLib.Classes;
using DoMCLib.Classes.Configuration.CCD;
using Microsoft.Extensions.DependencyInjection;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.DataContracts;
using static DoMCLib.Classes.Configuration.CCD.SocketParameters;

namespace DoMCLib.Configuration
{
    [DataContract]
    public class ImageProcessParameters
    {
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
        [DataMember]
        public AveragePercentageLimits AveragePercentageLimits = new AveragePercentageLimits();
        public Rectangle GetRectangle()
        {
            return new Rectangle(LeftBorder, TopBorder, RightBorder - LeftBorder, BottomBorder - TopBorder);
        }

        public bool IsImageProcessParametersMakeDecisionSet()
        {
            return
                Decisions[0].IsMakeDecisionSet()
                &&
                Decisions[1].IsMakeDecisionSet();
        }
        public bool IsImageProcessParametersWindowSet()
        {
            return
                TopBorder != 0
                &&
                BottomBorder != 511
                &&
                LeftBorder != 0
                &&
                RightBorder != 511;
        }

        public ImageProcessParameters Clone()
        {
            var ipp = new ImageProcessParameters()
            {
                TopBorder = TopBorder,
                BottomBorder = BottomBorder,
                LeftBorder = LeftBorder,
                RightBorder = RightBorder
            };
            ipp.Decisions = CloneDecisions();
            if (AveragePercentageLimits != null)
                ipp.AveragePercentageLimits = AveragePercentageLimits.Clone();
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
            if (copyImageProcessParameters.Decisions)
            {
                target.Decisions = CloneDecisions();
                target.AveragePercentageLimits = AveragePercentageLimits.Clone();
            }
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
