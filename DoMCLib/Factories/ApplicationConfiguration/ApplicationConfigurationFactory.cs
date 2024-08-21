using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoMCLib.Configuration;

namespace DoMCLib.Factories.ApplicationConfiguration
{
    /// <summary>
    /// абстрактная фабрика для разных видов конфигураций
    /// В приложении будет реализована свои классы для них и своя фабрика
    /// </summary>
    public class ApplicationConfigurationFactory
    {
        public HardwareSettings CreateHardwareSettings() => new HardwareSettings();
        public CurrentSettings CreateCurrentSettings() => new CurrentSettings();
        public ProcessingData CreateProcessingData() => new ProcessingData();
    }
}
