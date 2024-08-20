using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl
{
    public class ProcessingData
    {
        // Пример данных для текущей обработки
        public byte[] ProcessedData { get; set; } = new byte[50 * 1024 * 1024]; // 50 МБ
                                                                                // Добавьте другие параметры текущей обработки
    }

}
