using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DoMC
{
    public partial class DisplayMessage : Form
    {
        public DisplayMessage()
        {
            InitializeComponent();
        }

        public static void Show(string text, string caption, Size? size=null)
        {
            var frm = new DisplayMessage();
            frm.lblErrorText.Text = text;
            frm.Text = caption;
            if (size != null)
            {
                frm.Size = size.Value;
            }
            else{
                Rectangle screenRectangle = frm.RectangleToScreen(frm.ClientRectangle);

                int titleHeight = screenRectangle.Top - frm.Top;
                frm.Size = new Size(frm.lblErrorText.Size.Width+ frm.lblErrorText.Left + 20, frm.lblErrorText.Size.Height+ frm.lblErrorText.Top + 50+frm.btnOk.Height+titleHeight);
            }
            frm.ShowDialog();
        }
    }
}
