using DoMCModuleControl.Modules;
using DoMCModuleControl.Commands;
using DoMCModuleControl;

namespace DoMCTestingTools.ClassesForTests
{
    public partial class TestModule
    {
        public class TestCommand : AbstractCommandBase
        {
            public TestCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof(TestInputData), typeof(TestOutputData))
            {
            }

            protected override void Executing()
            {
                var outputData = new TestOutputData();
                outputData.Value = ((TestInputData)InputData).Value.ToString();
                OutputData = outputData;
            }
        }
    }
}