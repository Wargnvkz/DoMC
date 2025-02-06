using DoMCLib.Classes.Configuration.CCD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DoMCLib.Forms
{
    public partial class DoMCSocketSettingsForm : Form
    {

        private SocketParameters _Configuration;
        public SocketParameters Configuration
        {
            get
            {
                var cfg = _Configuration.Clone();
                cfg.ImageCheckingParameters = _Configuration.ImageCheckingParameters.Clone();
                cfg.ReadingParameters.Exposition = Exposition;
                cfg.ReadingParameters.FilterModule = FilterModule;
                cfg.ReadingParameters.FrameDuration = FrameDuration;
                cfg.ReadingParameters.MeasureDelay = MeasureDelay;
                cfg.ReadingParameters.DataType = DataType;
                return cfg;
            }
            set
            {
                var cfg = value.Clone();
                _Configuration = cfg;
                _Configuration.ImageCheckingParameters = cfg.ImageCheckingParameters.Clone();
                Exposition = cfg.ReadingParameters.Exposition;
                FilterModule = cfg.ReadingParameters.FilterModule;
                FrameDuration = cfg.ReadingParameters.FrameDuration;
                MeasureDelay = cfg.ReadingParameters.MeasureDelay;
                DataType = cfg.ReadingParameters.DataType;
            }
        }

        private int FilterModule
        {
            get
            {
                return (int)nudFilterModule.Value;
            }
            set
            {
                nudFilterModule.Value = value;
            }
        }
        /*public int DeviationWindow
        {
            get { return int.Parse(nudDeviationWindow.Text); }
            set { nudDeviationWindow.Text = value.ToString(); }
        }
        public short MaxDeviation
        {
            get { return short.Parse(nudDeviationExcess.Text); }
            set { nudDeviationExcess.Text = value.ToString(); }
        }
        public short MaxAverage
        {
            get { return short.Parse(nudAverageExcess.Text); }
            set { nudAverageExcess.Text = value.ToString(); }
        }
        public int TopBorder
        {
            get { return (int)(nudTop.Value); }
            set { nudTop.Value = value; }
        }
        public int BottomBorder
        {
            get { return (int)(nudBottom.Value); }
            set { nudBottom.Value = value; }
        }
        public int LeftBorder
        {
            get { return (int)(nudLeft.Value); }
            set { nudLeft.Value = value; }
        }
        public int RightBorder
        {
            get { return (int)(nudRight.Value); }
            set { nudRight.Value = value; }
        }*/
        private int DataType
        {
            get
            {
                return (int)nudDataType.Value;
            }
            set
            {
                nudDataType.Value = value;
            }
        }
        private int Exposition
        {
            get
            {
                return (int)nudExposition.Value;
            }
            set
            {
                nudExposition.Value = value;
            }
        }
        private int FrameDuration
        {
            get
            {
                return (int)nudFrameDuration.Value;
            }
            set
            {
                nudFrameDuration.Value = value;
            }
        }
        private int MeasureDelay
        {
            get
            {
                return (int)nudMeasureDelay.Value;
            }
            set
            {
                nudMeasureDelay.Value = value;
            }
        }
        public DoMCSocketSettingsForm()
        {
            InitializeComponent();

        }
        public new DialogResult ShowDialog()
        {

            return base.ShowDialog();
        }

        private void numExposition_DoubleClick(object sender, EventArgs e)
        {
            var nud = (NumericUpDown)sender;
            var title = "";
            if (nud == nudExposition) title = "(Экспозиция)";
            if (nud == nudFrameDuration) title = "(Длина фрейма)";
            /*if (nud == nudDeviationWindow) title = "(Точек рассчета)";
            if (nud == nudDeviationExcess) title = "(Максимальное значение оклонения)";
            if (nud == nudAverageExcess) title = "(Максимальное значение среднего)";
            if (nud == nudTop) title = "(Верхняя граница области проверки)";
            if (nud == nudBottom) title = "(Нижняя граница области проверки)";
            if (nud == nudLeft) title = "(Левая граница области проверки)";
            if (nud == nudRight) title = "(Правая граница области проверки)";*/
            var newvalue = DoMCLib.Dialogs.DigitalInput.ShowIntegerDialog($"Ввод значения {title}", false, (int)nud.Value);
            if (newvalue >= 0)
                nud.Value = newvalue;
        }

        private void nudExposition_Validating(object sender, CancelEventArgs e)
        {
            if (nudExposition.Value <= 0)
            {
                epError.SetError(nudExposition, "Значение должно быть больше 0");
                e.Cancel = true;
            }
            else
            {
                epError.Clear();
                e.Cancel = false;
            }

        }

        /*private void nudLeftRight_Validating(object sender, CancelEventArgs e)
        {
            if (nudRight.Value < nudLeft.Value)
            {
                var errStr = "Значение справа должно быть не меньше, чем значение слева";
                epError.SetError(nudRight, errStr);
                epError.SetError(nudLeft, errStr);
                e.Cancel = true;
            }
            else
            {
                epError.Clear();
                e.Cancel = false;
            }
        }

        private void nudTopBottom_Validating(object sender, CancelEventArgs e)
        {
            if (nudTop.Value > nudBottom.Value)
            {
                var errStr = "Значение снизу должно быть не меньше, чем значение сверху";
                epError.SetError(nudBottom, errStr);
                epError.SetError(nudTop, errStr);
                e.Cancel = true;
            }
            else
            {
                epError.Clear();
                e.Cancel = false;
            }
        }*/

        private void btnImageParameters_Click(object sender, EventArgs e)
        {

            var form = new DoMCImageProcessSettingsForm();
            form.ImageProcessParameters = _Configuration.ImageCheckingParameters.Clone();
            if (form.ShowDialog() == DialogResult.OK)
            {
                _Configuration.ImageCheckingParameters = form.ImageProcessParameters.Clone();
            }

        }
    }
}
