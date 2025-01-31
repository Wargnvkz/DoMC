using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib
{
    public static class EventNamesList
    {
        /// <summary>
        /// Событие, когда в интерфейсе TestCCD выбрано гнездо и нужно передать эти изображения в другие элементы управления
        /// data=(short[,] Image, short[,] Standard, ImageProcessParameters ipp)
        /// </summary>
        public static string InterfaceImagesSelected = "Interface.Images.Selected";
    }
}
