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
    public partial class LEDSettingsForm : Form
    {
        public int Current
        {
            get
            {
                int.TryParse(txbCurrent.Text, out int current);
                return current;
            }
            set
            {
                txbCurrent.Text = value.ToString();
            }
        }
        public int PreformLength
        {
            get
            {
                int.TryParse(txbPreformLength.Text, out int PreformLength);
                return PreformLength;
            }
            set
            {
                txbPreformLength.Text = value.ToString();
            }
        }
        public int DelayLength
        {
            get
            {
                int.TryParse(txbDelayLength.Text, out int DelayLength);
                return DelayLength;
            }
            set
            {
                txbDelayLength.Text = value.ToString();
            }
        }
        public double LCBKoefficient
        {
            get
            {
                double.TryParse(txbLCBKoefficient.Text, out double LCBKoefficient);
                return LCBKoefficient;
            }
            set
            {
                txbLCBKoefficient.Text = value.ToString();
            }
        }
        public LEDSettingsForm()
        {
            InitializeComponent();
        }

        private void txb_intValidating(object sender, CancelEventArgs e)
        {
            var txb = sender as TextBox;
            if (txb == null) return;
            if (!int.TryParse(txb.Text, out int _))
            {
                epError.SetError(txb, "Должно быть целое число");
                e.Cancel = true;
            }
            else
            {
                epError.Clear();
                e.Cancel = false;
            }
        }
        private void txb_doubleValidating(object sender, CancelEventArgs e)
        {
            var txb = sender as TextBox;
            if (txb == null) return;
            if (!double.TryParse(txb.Text, out double _))
            {
                epError.SetError(txb, "Должно быть дробное число");
                e.Cancel = true;
            }
            else
            {
                epError.Clear();
                e.Cancel = false;
            }
        }

        private void txbInput_DoubleClick(object sender, EventArgs e)
        {
            var txb = (sender as TextBox);
            string title = "";
            if (txb == txbCurrent) title = "тока";
            if (txb == txbDelayLength) title = "длина задержка";
            if (txb == txbPreformLength) title = "длина преформы";
            if (txb == txbLCBKoefficient) title = "Коэффициент";
            int.TryParse(txb.Text, out int Current);
            var newvalue = DoMCLib.Dialogs.DigitalInput.ShowIntegerDialog($"Ввод значения {title}", false, Current);
            if (newvalue >= 0)
                txb.Text = newvalue.ToString();
        }

    }
}
