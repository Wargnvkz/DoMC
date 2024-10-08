using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace DoMC
{
    public class PressButton : Button
    {
        private bool _IsPressed { get; set; }
        public bool IsPressed { get { return _IsPressed; } set { _IsPressed = value; Invalidate(); } }

        public PressButton()
        {
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            //var dColor = ControlPaint.Dark(BackColor);
            //var lColor = ControlPaint.Light(BackColor);
            //var lColor = LighterDarkenColorYUV(BackColor, 1.5);
            //var dColor = LighterDarkenColorYUV(BackColor, 0.5);

            //var lColor = LightenDarkenColorM(BackColor, 1.5);
            //var dColor = LightenDarkenColorM(BackColor, 0.5);
            //if (IsSolidBorder)
            {
                var color = BackColor;
                if (color.A == 0)
                {
                    color = LightenDarkenColorYUV(SystemColors.Control,0.7);
                }

                var dColor = ControlPaint.Dark(color);
                var lColor = ControlPaint.Light(color);
                //var lColor = LightenDarkenColorYUV(color, 1.1);

                var ddColor = ControlPaint.DarkDark(color);
                var llColor = ControlPaint.LightLight(color);
                var InnerRect = pevent.ClipRectangle;
                InnerRect.Inflate(-2, -2);
                if (IsPressed)
                {
                    ControlPaint.DrawBorder(pevent.Graphics, pevent.ClipRectangle,
                            ddColor, 2, ButtonBorderStyle.Solid,
                            ddColor, 2, ButtonBorderStyle.Solid,
                            llColor, 2, ButtonBorderStyle.Solid,
                            llColor, 2, ButtonBorderStyle.Solid);
                    ControlPaint.DrawBorder(pevent.Graphics, InnerRect,
                            dColor, 2, ButtonBorderStyle.Solid,
                            dColor, 2, ButtonBorderStyle.Solid,
                            lColor, 2, ButtonBorderStyle.Solid,
                            lColor, 2, ButtonBorderStyle.Solid);

                }
                else
                {
                    ControlPaint.DrawBorder(pevent.Graphics, pevent.ClipRectangle,
                            llColor, 2, ButtonBorderStyle.Solid,
                            llColor, 2, ButtonBorderStyle.Solid,
                            ddColor, 2, ButtonBorderStyle.Solid,
                            ddColor, 2, ButtonBorderStyle.Solid);
                    ControlPaint.DrawBorder(pevent.Graphics, InnerRect,
                            lColor, 2, ButtonBorderStyle.Solid,
                            lColor, 2, ButtonBorderStyle.Solid,
                            dColor, 2, ButtonBorderStyle.Solid,
                            dColor, 2, ButtonBorderStyle.Solid);
                }
            }
            /*else
            {
                if (IsPressed)
                {
                    ControlPaint.DrawBorder3D(pevent.Graphics, pevent.ClipRectangle, Border3DStyle.Sunken);
                }
                else
                {
                    ControlPaint.DrawBorder3D(pevent.Graphics, pevent.ClipRectangle, Border3DStyle.Raised);
                }
            }*/

        }

        private Color LightenDarkenColor(Color col, int plus)
        {
            var lr = (int)(BackColor.R + plus);
            var lg = (int)(BackColor.G + plus);
            var lb = (int)(BackColor.B + plus);
            lr = lr > 255 ? 255 : lr;
            lg = lg > 255 ? 255 : lg;
            lb = lb > 255 ? 255 : lb;
            var res = Color.FromArgb(lr, lg, lb);
            return res;
        }
        private Color LightenDarkenColorM(Color col, double mult)
        {
            var lr = (int)(BackColor.R * mult);
            var lg = (int)(BackColor.G * mult);
            var lb = (int)(BackColor.B * mult);
            lr = lr > 255 ? 255 : lr;
            lg = lg > 255 ? 255 : lg;
            lb = lb > 255 ? 255 : lb;
            var res = Color.FromArgb(lr, lg, lb);
            return res;
        }

        protected override void OnClick(EventArgs e)
        {
            IsPressed = !IsPressed;
            base.OnClick(e);
        }

        private Color LightenDarkenColorYUV(Color col, double mult)
        {
            var y = 0.2126 * col.R + 0.7152 * col.G + 0.0722 * col.B;
            var u = -0.09991 * col.R - 0.33609 * col.G + 0.436 * col.B;
            var v = 0.615 * col.R - 0.55861 * col.G - 0.05639 * col.B;
            y = y * mult;
            var r = 1 * y + 0 * u + 1.28033 * v;
            var g = 1 * y - 0.21482 * u - 0.38059 * v;
            var b = 1 * y + 2.21798 * u + 0 * v;
            r = r < 0 ? 0 : r > 255 ? 255 : r;
            g = g < 0 ? 0 : g > 255 ? 255 : g;
            b = b < 0 ? 0 : b > 255 ? 255 : b;
            var res = Color.FromArgb((int)r, (int)g, (int)b);
            return res;
        }

    }
}
