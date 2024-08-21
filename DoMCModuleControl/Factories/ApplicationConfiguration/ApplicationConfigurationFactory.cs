using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl.Factories.ApplicationConfiguration
{
    /// <summary>
    /// абстрактная фабрика для разных видов конфигураций
    /// В приложении будет реализована свои классы для них и своя фабрика
    /// </summary>
    public abstract class ApplicationConfigurationFactory
    {
        public abstract HardwareSettings CreateHardwareSettings();
        public abstract CurrentSettings CreateCurrentSettings();
        public abstract ProcessingData CreateProcessingData();
    }
}
