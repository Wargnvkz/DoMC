using System.Drawing;
using DoMCLib.ProcessState;

namespace DoMCLib.Classes.Configuration
{
    public class ImageProcessParameters
    {

        public int TopBorder = 0;

        public int BottomBorder = 511;

        public int LeftBorder = 0;

        public int RightBorder = 511;

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
