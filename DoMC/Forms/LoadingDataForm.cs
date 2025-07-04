using DoMC.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WorkshopEquipmentData
{
    public partial class LoadingDataForm : ShadowForm
    {
        public LoadingDataForm()
        {
            InitializeComponent();
        }

        private void StartingForm_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, Border3DStyle.Flat);
        }
    }
}
