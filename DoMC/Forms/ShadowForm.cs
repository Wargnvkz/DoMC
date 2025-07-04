using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMC.Forms
{
    using System;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;
    using System.Drawing.Drawing2D;

    public class ShadowForm : Form
    {
        private const int CS_DROPSHADOW = 0x20000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        public ShadowForm()
        {
            FormBorderStyle = FormBorderStyle.None; // Без стандартной рамки
            BackColor = System.Drawing.Color.White;
        }
    }
        
}
