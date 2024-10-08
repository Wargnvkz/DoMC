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
        public DoMCLib.Classes.DoMCGeneralSettings Value
        {
            get {
                var res = new DoMCLib.Classes.DoMCGeneralSettings();
                int.TryParse(txbCycles.Text, out res.NCycle);
                double.TryParse(txbStandardPercent.Text, out res.StandardPercent);
                return res;
            }
            set {
                var gs = value;
                txbCycles.Text = gs.NCycle.ToString();
                txbStandardPercent.Text = gs.StandardPercent.ToString("F2");
            }
        }
        public DoMCGeneralSettingsForm()
        {
            InitializeComponent();
        }
    }
}
