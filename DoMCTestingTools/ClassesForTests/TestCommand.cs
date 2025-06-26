using DoMCModuleControl.Modules;
using DoMCModuleControl.Commands;
using DoMCModuleControl;

namespace DoMCTestingTools.ClassesForTests
{
    public partial class TestModule
    {
        public class TestCommand : AbstractCommandBase, IExecuteCommandAsync
        {
            public TestCommand(IMainController mainController, AbstractModuleBase module) : base(mainController, module, typeof(TestInputData), typeof(TestOutputData))
            {
            }

            protected override async Task Executing()
            {
                var outputData = new TestOutputData();
                outputData.Value = ((TestInputData)InputData).Value.ToString();
                OutputData = outputData;
            }
            public new async Task ExecuteCommandAsync()
            {
                await base.ExecuteCommandAsync();
            }
        }
    }
}