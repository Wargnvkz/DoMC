using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using DoMCModuleControl.Factories.ApplicationConfiguration;

namespace DoMCModuleControl.Configuration
{
    /// <summary>
    /// Данные всего приложения и всех модулей - конфигурация, состояния, временные данные
    /// </summary>
    public class ApplicationContext
    {
        public ApplicationConfiguration Configuration { get; private set; }

        public ApplicationContext(ApplicationConfigurationFactory configurationFactory, string configurationFilePath)
        {
            Configuration = new ApplicationConfiguration(configurationFactory, configurationFilePath);

            // Загрузка данных из файла
            Configuration.Load();
        }


    }

}
