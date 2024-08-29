using DoMCModuleControl;

namespace DoMCUserInterface
{
    public partial class WorkingForm : Form, DoMCModuleControl.UI.IMainUserInterface
    {
        IMainController? controller;
        public WorkingForm()
        {
            InitializeComponent();
        }

        public void SetMainController(IMainController mainController)
        {
            controller = mainController;
        }
    }
}
