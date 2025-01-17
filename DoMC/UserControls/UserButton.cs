using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoMC.UserControls
{
    public partial class UserButton : UserControl
    {
        private Color _buttonColor = Color.CornflowerBlue;
        private Color _indicatorColor = Color.Red;
        private bool _isPressed = false;
        [Category("Appearance")]
        public Color ButtonColor
        {
            get => _buttonColor;
            set { _buttonColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        public Color IndicatorColor
        {
            get => _indicatorColor;
            set { _indicatorColor = value; Invalidate(); }
        }
        public bool IsPressed
        {
            get { return _isPressed; }
            set { _isPressed = value; Invalidate(); }
        }

        public UserButton()
        {
            InitializeComponent();
            this.DoubleBuffered = true; // Для сглаживания
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true); // Включаем поддержку прозрачности
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.BackColor = Color.Transparent; // Устанавливаем прозрачный фон

            this.DoubleBuffered = true; // Для сглаживания
            this.Size = new Size(100, 100); // Стандартный размер
            this.MouseDown += (s, e) => { _isPressed = true; Invalidate(); };
            this.MouseUp += (s, e) => { _isPressed = false; Invalidate(); };
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

            // Размеры
            int shadowOffset = 10;
            Rectangle buttonRect = new Rectangle(0, 0, Width - shadowOffset, Height - shadowOffset);
            Rectangle shadowRect = new Rectangle(shadowOffset, shadowOffset, Width - shadowOffset, Height - shadowOffset);

            // Тень
            if (!_isPressed)
            {
                using (Brush shadowBrush = new SolidBrush(Color.FromArgb(127, Color.Gray)))
                    g.FillEllipse(shadowBrush, shadowRect);
            }

            // Кнопка
            using (Brush buttonBrush = new SolidBrush(_buttonColor))
                g.FillEllipse(buttonBrush, buttonRect);

            // Индикатор
            int indicatorSize = Width / 5;
            Rectangle indicatorRect = new Rectangle((Width - indicatorSize) / 2, (Height - indicatorSize) / 2, indicatorSize, indicatorSize);
            using (Brush indicatorBrush = new SolidBrush(_indicatorColor))
                g.FillEllipse(indicatorBrush, indicatorRect);

            // Обводка
            using (Pen borderPen = new Pen(Color.Black, 2))
                g.DrawEllipse(borderPen, buttonRect);
        }
    }
}
