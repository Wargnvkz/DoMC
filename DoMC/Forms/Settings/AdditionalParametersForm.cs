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
    public partial class DoMCAdditionalParametersForm : Form
    {
        public short AverageToHaveImage
        {
            get
            {
                if (short.TryParse(txbAverageImage.Text, out short res))
                {
                    return res;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                txbAverageImage.Text = value.ToString();
            }
        }

        
        public bool LogPackets
        {
            get { return cbLogPackets.Checked; }
            set { cbLogPackets.Checked = value; }
        }

        public bool RegisterEmptyImages
        {
            get { return cbRegisterEmptyImages.Checked; }
            set { cbRegisterEmptyImages.Checked = value; }
        }
        public DoMCAdditionalParametersForm()
        {
            InitializeComponent();
        }
    }
}
