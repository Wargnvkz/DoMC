using DoMCModuleControl;
using DoMCModuleControl.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoMC
{
    public partial class SettingsInterface : Form, IMainUserInterface
    {
        IMainController Controller;
        public SettingsInterface()
        {
            InitializeComponent();
        }

        public void SetMainController(IMainController mainController)
        {
            Controller = mainController;
        }
    }
}
