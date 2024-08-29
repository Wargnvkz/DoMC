using DoMCModuleControl;
using DoMCModuleControl.Commands;
using DoMCModuleControl.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes.Model.RDPB
{
    public class RDPBModule : ModuleBase
    {
        public Configuration.RemoveDefectedPreformBlockConfig config;
        private IMainController mainController;
        public RDPBModule(IMainController MainController) : base(MainController)
        {
            mainController = MainController;
        }

        public override void Initialize()
        {

        }

        public override void Shutdown()
        {

        }

        public override void Start()
        {

        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        #region
        public class LoadConfiguration : CommandBase
        {
            public LoadConfiguration(ModuleBase module) : base(module, typeof(RemoveDefectedPreformBlockConfig), null)
            {
            }

            protected override void Executing()
            {
                if (InputData is Configuration.RemoveDefectedPreformBlockConfig)
                {
                    ((RDPBModule)Module).config = InputData as Configuration.RemoveDefectedPreformBlockConfig;
                }
            }
        }
        #endregion
    }
}
