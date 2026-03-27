using DoMCLib.Classes;
using DoMCLib.Tools;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace DoMCForms.Classes
{
    public class ChartTools
    {

        #region Series
        private static string[] LimitsLinesNames = new[] { "DefectMinLine", "WarningMinLine", "WarningMaxLine", "DefectMaxLine" };
        public static void RemoveSeriesLimitsLines(Chart chart)
        {
            for (int i = 0; i < LimitsLinesNames.Length; i++)
            {
                var serie = chart.Series.FindByName(LimitsLinesNames[i]);
                if (serie != null)
                {
                    chart.Series.Remove(serie);
                }
            }
        }
        public static void AddLimitsSeriesLines(Chart chart, short std, AveragePercentageLimits limits, double minX, double maxX)
        {
            var lineWidth = 2;
            CreateSerieLine(chart, 3, minX, maxX, DisplayAverageCalculationTools.DifferenceFromStandard(std, limits.DefectPercentage / 100d) ?? 0, Color.Red, lineWidth);
            CreateSerieLine(chart, 2, minX, maxX, DisplayAverageCalculationTools.DifferenceFromStandard(std, limits.WarningPercentage / 100d) ?? 0, Color.Orange, lineWidth);
            CreateSerieLine(chart, 1, minX, maxX, DisplayAverageCalculationTools.DifferenceFromStandard(std, -limits.WarningPercentage / 100d) ?? 0, Color.Orange, lineWidth);
            CreateSerieLine(chart, 0, minX, maxX, DisplayAverageCalculationTools.DifferenceFromStandard(std, -limits.DefectPercentage / 100d) ?? 0, Color.Red, lineWidth);

        }

        public static void CreateSerieLine(Chart chart, int serieNumber, double minX, double maxX, double lineValue, Color color, int width)
        {
            var series = new Series(LimitsLinesNames[serieNumber]);
            series.ChartType = SeriesChartType.Line;
            series.Color = color;
            series.BorderWidth = width;
            series.IsVisibleInLegend = false;

            // две точки — линия на весь график
            series.Points.AddXY(minX, lineValue);
            series.Points.AddXY(maxX, lineValue);

            chart.Series.Add(series);
        }
        #endregion Series

        #region StripLines
        public static void AddLimitsStripLines(Chart chart, short std, AveragePercentageLimits limits)
        {
            // chart.Series
            ElementPosition pos = chart.ChartAreas[0].InnerPlotPosition;

            float chartHeightPx = chart.Height;

            float plotHeightPx = chartHeightPx * pos.Height / 100f;

            double valuePerPixel = (chart.ChartAreas[0].AxisY.Maximum - chart.ChartAreas[0].AxisY.Minimum) / plotHeightPx;


            double lineWidth = 2 * valuePerPixel;
            var hWarnLine = CreateStripLine(std, limits.WarningPercentage / 100d, Color.Orange, lineWidth);
            var lWarnLine = CreateStripLine(std, -limits.WarningPercentage / 100d, Color.Orange, lineWidth);
            var hDefectLine = CreateStripLine(std, limits.DefectPercentage / 100d, Color.Red, lineWidth);
            var lDefectLine = CreateStripLine(std, -limits.DefectPercentage / 100d, Color.Red, lineWidth);

            try
            {
                //chart.ChartAreas[0].AxisY.StripLines.SuspendUpdates();
                chart.BeginInvoke(() =>
                {
                    var axis = chart.ChartAreas[0].AxisY;

                    axis.StripLines.Clear();

                    var lines = new[]
                    {
                          hWarnLine,
                          lWarnLine,
                          hDefectLine,
                          lDefectLine
                    };

                    foreach (var line in lines)
                    {
                        if (line != null)
                            axis.StripLines.Add(line);
                    }
                });
            }
            finally
            {
                //chart.ChartAreas[0].AxisY.StripLines.ResumeUpdates();
            }
        }

        public static StripLine CreateStripLine(short std, double diffRate, Color color, double width)
        {
            var value = DisplayAverageCalculationTools.DifferenceFromStandard(std, diffRate);
            if (value == null) return null;
            StripLine limitLine = new StripLine();
            limitLine.Interval = 0;
            limitLine.IntervalOffset = value.Value; // Значение на оси Y
            limitLine.StripWidth = width;// width;// width;    // Толщина линии
            limitLine.BackColor = color; // Цвет
            limitLine.BorderDashStyle = ChartDashStyle.Dash;
            return limitLine;
        }
        #endregion StripLines

    }
}
