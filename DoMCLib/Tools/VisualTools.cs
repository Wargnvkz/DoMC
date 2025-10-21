using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Tools
{
    public class VisualTools
    {
        public static Bitmap DrawMatrix(Size rectSize, bool[] IsSocketGood, bool[] SocketsToSave, int[] ErrorSumBySocket, bool[] IsSocketChecking, Color goodColor, Color badColor, Color blockedSocketColor, Color forSavingColor, bool showErrors)
        {
            var indent = 5;
            var bmp = new Bitmap(rectSize.Width - 2 * indent, rectSize.Height - 2 * indent);
            var Y = 16;
            var X = 6;
            var middlespace = 50;
            var kx = (rectSize.Width - 2 * indent) / (double)X;
            var ky = (rectSize.Height - middlespace - 2 * indent - 10) / (double)Y;
            var diameter = Math.Min(kx, ky) * 0.9;

            var ToExcludeCoords = new Point[] { new Point(2, 7), new Point(3, 7), new Point(2, 8), new Point(3, 8) };
            var graph = Graphics.FromImage(bmp);

            var badbrush = new SolidBrush(badColor);
            var goodbrush = new SolidBrush(goodColor);
            var saveSocketBrush = new SolidBrush(forSavingColor);

            //if (k>5)//(kx > 5 && ky > 5)
            {
                bool addText = false;
                var fontsize = 0;
                Font font = null;
                if (diameter > 10)//(kx > 10 && ky > 10)
                {
                    fontsize = (int)diameter / 2;//Math.Min(kx, ky);
                    font = new Font("Arial", fontsize);

                    fontsize = FindSize((fs) =>
                    {
                        var sz = graph.MeasureString("99", new Font(font.FontFamily, fs));
                        return (int)Math.Max(sz.Width, sz.Height);

                    }, (int)diameter//Math.Min(kx, ky)*
                              , 20, 0);
                    if (fontsize >= 8)
                    {
                        addText = true;
                        font = new Font(font.FontFamily, fontsize);
                    }
                }
                SolidBrush textBrush = new SolidBrush(Color.Black);
                for (int y = 0; y < Y; y++)
                {
                    for (int x = 0; x < X; x++)
                    {
                        var n = x * 16 + y + 1;
                        var addy = 0;
                        if (y >= 8) addy = middlespace;
                        //ToExcludeCoords
                        SolidBrush brush;
                        //InterfaceDataExchange.CurrentCycleCCD.SocketsToSave
                        //InterfaceDataExchange.CurrentCycleCCD.ImageStatuses[n]==1

                        if (SocketsToSave[n - 1])
                        {
                            graph.FillRectangle(saveSocketBrush, (int)(x * kx + (kx - diameter) / 2) - 2 + indent, (int)(y * ky + (ky - diameter) / 2) + addy - 2 + indent, (int)diameter + 4, (int)diameter + 4);
                        }
                        if (IsSocketChecking[n - 1])
                        {
                            if (showErrors)
                            {
                                if (ErrorSumBySocket[n - 1] > 0)
                                {
                                    brush = badbrush;
                                }
                                else
                                {
                                    brush = goodbrush;
                                }
                            }
                            else
                            {
                                if (!IsSocketGood[n - 1])
                                    brush = badbrush;
                                else
                                    brush = goodbrush;
                            }
                        }
                        else
                        {
                            brush = new SolidBrush(blockedSocketColor);
                        }
                        //graph.FillEllipse(brush, (int)(x * kx), (int)(y * ky), (int)kx, (int)ky);
                        graph.FillEllipse(brush, (int)(x * kx + (kx - diameter) / 2) + indent, (int)(y * ky + (ky - diameter) / 2) + addy + indent, (int)diameter, (int)diameter);

                        if (addText && font != null)
                        {
                            string str;
                            if (showErrors)
                            {
                                str = ErrorSumBySocket[n - 1].ToString();
                            }
                            else
                            {
                                str = n.ToString();
                            }
                            var sz = graph.MeasureString(str, font);
                            //graph.DrawString(str,font,brush, (int)(x * kx), (int)(y * ky));
                            graph.DrawString(str, font, textBrush, (int)(x * kx + (kx - diameter) / 2 + (diameter / 2 - sz.Width / 2)) + indent, (int)(y * ky + (ky - diameter) / 2 + (diameter / 2 - sz.Height / 2)) + addy + indent);
                        }
                    }
                }

            }

            return bmp;
        }
        public static int FindSize(Func<int, int> function, int compareTo, int higherBound, int lowerBound)
        {
            int v = lowerBound, prv_v;
            do
            {
                prv_v = v;
                v = (higherBound + lowerBound) / 2;
                var res = function(v);
                if (res > compareTo)
                {
                    higherBound = v - 1;
                }
                else
                {
                    lowerBound = v + 1;
                }
            } while (higherBound != lowerBound && prv_v != v);
            return v;
        }


    }
}
