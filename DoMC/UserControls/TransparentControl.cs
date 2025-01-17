using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMC.UserControls
{
    public class TransparentControl : UserControl
    {
        public TransparentControl()
        {
            this.DoubleBuffered = true; // Для сглаживания
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true); // Включаем поддержку прозрачности
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.BackColor = Color.Transparent; // Устанавливаем прозрачный фон
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Отключаем закраску фона
            // base.OnPaintBackground(e); — не вызываем
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Пример: рисуем полупрозрачный круг
            using (Brush semiTransparentBrush = new SolidBrush(Color.FromArgb(32, Color.Blue)))
            {
                g.FillEllipse(semiTransparentBrush, 10, 10, this.Width - 20, this.Height - 20);
            }
           
            // Пример: обводка
            using (Pen borderPen = new Pen(Color.Black, 2))
            {
                g.DrawEllipse(borderPen, 10, 10, this.Width - 20, this.Height - 20);
            }
        }
    }
}
