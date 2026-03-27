using DoMCLib.Classes;
using DoMCLib.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.RateLimiting;
using System.Threading.Tasks;

namespace DoMCLib.Tools
{
    public class DisplayAverageCalculationTools
    {
        public static short?[] GetAverageStandards(ApplicationConfiguration configuration)
        {
            var average = new short?[configuration.HardwareSettings.SocketQuantity];
            for (int i = 0; i < average.Length; i++)
            {
                if (configuration.ProcessingDataSettings.CCDSocketStandardsImage[i].StandardImage != null)
                {
                    average[i] = ImageTools.Average(configuration.ProcessingDataSettings.CCDSocketStandardsImage[i].StandardImage, configuration.ReadingSocketsSettings.CCDSocketParameters[i].ImageCheckingParameters.GetRectangle());
                }
            }
            return average;
        }
        public static double? DifferenceFromStandard(short? standartAverage, short? imageAverage)
        {
            return (standartAverage != null && imageAverage != null) ? (standartAverage - imageAverage) / standartAverage : null;
        }
        public static short? DifferenceFromStandard(short? standartAverage, double diffRate)
        {
            return standartAverage != null ? (short)(standartAverage * (1 + diffRate)) : null;
        }
        public static (short? hi, short? lo) GetLimitValues(short std, double precentLimit)
        {
            return (DifferenceFromStandard(std, precentLimit / 100d),
                DifferenceFromStandard(std, -precentLimit / 100d));

        }

        public static (double min, double max) GetMinMaxForGraph(short avgMin, short avgMax, short? stdAverage, short? maxlimit)
        {
            double Min, Max;
            double limit = maxlimit != null ? (maxlimit.Value) / 100d : 0.15;
            double avg = stdAverage != null ? stdAverage.Value : (avgMax - avgMin) / 2;

            var diff = (avgMax - avgMin);
            var padding = 0.1;

            Min = Math.Min(avg * (1 - limit * (1 + padding)), avgMin - diff * padding);
            Max = Math.Max(avg * (1 + limit * (1 + padding)), avgMax + diff * padding);
            if (Min == Max)
            {
                Max = Min + 1;
            }
            return (Min, Max);

        }
    }
}
