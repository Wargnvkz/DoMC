using DoMCModuleControl.Modules;
using DoMCModuleControl;
using DoMCModuleControl.Commands;

/// <summary>
/// Управление получением данных из платы и передача данных в плату
/// </summary>
namespace DoMCLib.Classes.Module.CCD
{

    public partial class CCDCardDataModule
    {
        public class StartCommand : CommandBase
        {
            public StartCommand(IMainController mainController, ModuleBase module) : base(mainController, module, null, null) { }
            protected override void Executing() => ((CCDCardDataModule)Module).Start();
        }

    }


}
