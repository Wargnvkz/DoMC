using DoMCModuleControl.Modules;
using DoMCModuleControl;

namespace DoMCModuleControlTests.ClassesForTests
{
    public partial class TestModule : AbstractModuleBase
    {
        public TestModule(IMainController MainController) : base(MainController)
        {
        }
    }
}