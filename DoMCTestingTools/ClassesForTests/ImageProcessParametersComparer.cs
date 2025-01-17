using DoMCLib.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCTestingTools.ClassesForTests
{
    public class ImageProcessParametersComparer : System.Collections.IComparer
    {
        public int Compare(object? x, object? y)
        {
            var X = (ImageProcessParameters)x;
            var Y = (ImageProcessParameters)y;
            var t = X.TopBorder - Y.TopBorder;
            var l = X.LeftBorder - Y.LeftBorder;
            var r = X.RightBorder - Y.RightBorder;
            var b = X.BottomBorder - Y.BottomBorder;
            var xrect = X.GetRectangle();
            var xsq = xrect.Width * xrect.Height;
            var yrect = Y.GetRectangle();
            var ysq = xrect.Width * yrect.Height;
            if (t == 0 && l == 0 && r == 0 && b == 0) return 0;
            if (xsq < ysq) return -1;
            return 1;
        }
    }
}
