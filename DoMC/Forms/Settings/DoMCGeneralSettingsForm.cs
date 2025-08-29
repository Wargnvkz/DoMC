using DoMCLib.Classes.Configuration;
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
    public partial class DoMCGeneralSettingsForm : Form
    {
        public DoMCStandardRecalculationSettings Value
        {
            get
            {
                var res = new DoMCStandardRecalculationSettings();
                res.NCycle = (int)nudCycles.Value;
                res.StandardPercent = (int)nudStandardPercent.Value;
                return res;
            }
            set
            {
                var gs = value;
                nudCycles.Value = gs.NCycle;
                nudStandardPercent.Value = (int)gs.StandardPercent;
            }
        }

        public DoMCGeneralSettingsForm()
        {
            InitializeComponent();
        }
        private void num_DoubleClick(object sender, EventArgs e)
        {
            if (sender is NumericUpDown nud)
            {
                var title = "";
                if (nud == nudCycles) title = "(Количество расчетных циклов)";
                if (nud == nudStandardPercent) title = "(Процентов остающихся от изначального эталона)";
                var newvalue = DoMCLib.Dialogs.DigitalInput.ShowIntegerDialog($"Ввод значения {title}", false, (int)nud.Value);
                if (newvalue >= 0)
                    nud.Value = newvalue;
            }
        }
    }
}
