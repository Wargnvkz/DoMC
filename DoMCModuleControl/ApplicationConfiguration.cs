using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl
{
    /// <summary>
    /// Данные конфигурации
    /// </summary>
    public class ApplicationConfiguration
    {
        public HardwareSettings HardwareSettings { get; set; }
        public CurrentSettings CurrentSettings { get; set; }
        public ProcessingData ProcessingData { get; set; }

        public ApplicationConfiguration()
        {
            // Инициализация с значениями по умолчанию или загрузка из файла
            HardwareSettings = new HardwareSettings();
            CurrentSettings = new CurrentSettings();
            ProcessingData = new ProcessingData();
        }
    }
}
