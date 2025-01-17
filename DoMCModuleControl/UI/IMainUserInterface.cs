using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl.UI
{
    public interface IMainUserInterface
    {
        public void SetMainController(IMainController mainController, object? Data);
        public void Show();
        public void Hide();
        public void Close();
    }
}
