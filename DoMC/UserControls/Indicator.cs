using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace DoMC.UserControls
{
    public partial class Indicator : UserControl
    {
        /*private string[] _TextLines;
        public string[] TextLines
        {
            get
            {
                var ret = new string[_TextLines.Length];
                Array.Copy(_TextLines, ret, ret.Length);
                return ret;
            }
            set
            {
                _TextLines = new string[value.Length];
                Array.Copy(_TextLines, value, _TextLines.Length);
            }
        }*/
        //private string _TextLines { get; set; } = "Test";
        //private Font _Font { get; set; } = new Font("Arial", 12);
        private Color _TextColor { get; set; } = Color.Black;
        private Color _IndicatorColorOff { get; set; } = Color.Gray;
        private Color _IndicatorColorOn { get; set; } = Color.LimeGreen;
        private bool _IsIndicatorOn { get; set; } = false;
        //public string TextLines { get=>_TextLines; set { _TextLines = value;Invalidate(); } }
        //public Font Font { get => _Font; set { _Font = value; Invalidate(); } }
        public Color TextColor { get => _TextColor; set { _TextColor = value; Invalidate(); } }
        public Color IndicatorColorOff { get => _IndicatorColorOff; set { _IndicatorColorOff = value; Invalidate(); } }
        public Color IndicatorColorOn { get => _IndicatorColorOn; set { _IndicatorColorOn = value; Invalidate(); } }
        public bool IsIndicatorOn { get => _IsIndicatorOn; set { _IsIndicatorOn = value; Invalidate(); } }
        public Indicator()
        {
            InitializeComponent();
        }
        private void DrawLamp(Graphics g, Rectangle bounds, Color color, int boundWidth)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Градиентная заливка (радиальный градиент)
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(bounds);
                using (PathGradientBrush brush = new PathGradientBrush(path))
                {
                    brush.CenterColor = color; // Центр лампочки
                    brush.SurroundColors = new Color[] { ControlPaint.Dark(color) }; // Тёмный край
                    brush.FocusScales = new PointF(0.5f, 0.5f); // Размер центрального цвета

                    g.FillEllipse(brush, bounds);
                }
            }

            // Нарисуем отражение (блик)
            Rectangle highlight = new Rectangle(bounds.X + bounds.Width / 4, bounds.Y + bounds.Height / 4, bounds.Width / 3, bounds.Height / 3);
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(highlight);
                using (PathGradientBrush brush = new PathGradientBrush(path))
                {
                    brush.CenterColor = Color.White;
                    brush.SurroundColors = new Color[] { Color.FromArgb(0, Color.White) }; // Прозрачный край
                    g.FillEllipse(brush, highlight);
                }
            }

            // Обводка лампочки
            using (Pen pen = new Pen(Color.Black, boundWidth))
            {
                g.DrawEllipse(pen, bounds);
            }
        }

        private void PrintText(Graphics g, Font font, Brush brush, out float TextHeight)
        {
            var lineheight = font.GetHeight(g);
            /*TextHeight = _TextLines.Length * lineheight;
            var heightStart = Height - TextHeight;
            for (int line = 0; line < _TextLines.Length; line++)
            {
                var lineTop = heightStart + line * lineheight;
                g.DrawString(_TextLines[line], font, brush, new PointF(0, lineTop));
            }*/
            TextHeight = 1 * lineheight;
            var heightStart = Height - TextHeight;
            var lineTop = heightStart;
            g.DrawString(Text, font, brush, new PointF(0, lineTop));
        }

        private void Indicator_Paint(object sender, PaintEventArgs e)
        {
            if (Width <= 0 || Height <= 0) return;
            //base.OnPaint(e);
            e.Graphics.SetClip(ClientRectangle);
            //PrintText(e.Graphics, Font, new SolidBrush(TextColor), out float textHeight);
            var squareSize = Height;// - textHeight;
            if (Width < squareSize) { squareSize = Width; }
            var borderWidth = (int)(Math.Log(squareSize));
            squareSize = squareSize - borderWidth - 2;
            if (squareSize < 0) return;
            var left = (Width - squareSize) / 2;
            var top = (Height - squareSize) / 2;
            if (IsIndicatorOn)
            {
                DrawLamp(e.Graphics, new Rectangle((int)left, (int)top, (int)squareSize, (int)squareSize), IndicatorColorOn, borderWidth);

            }
            else
            {
                DrawLamp(e.Graphics, new Rectangle((int)left, (int)top, (int)squareSize, (int)squareSize), IndicatorColorOff, borderWidth);
            }
        }
    }
}
