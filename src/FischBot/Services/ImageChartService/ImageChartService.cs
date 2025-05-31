using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FischBot.Models.Finance;
using ScottPlot;

namespace FischBot.Services.ImageChartService
{
    public class ImageChartService : IImageChartService
    {
        public MemoryStream CreateStockChart(IEnumerable<TimeSeriesValue> timeSeriesValues, TimeSpan span, bool showYearInXAxis)
        {
            var ohlcs = timeSeriesValues
                    .Select(price => new OHLC(
                        (double)price.Open,
                        (double)price.High,
                        (double)price.Low,
                        (double)price.Close,
                        price.Datetime.DateTime,
                        span))
                    .ToList();

            var plot = new Plot();

            var candlePlot = plot.Add.Candlestick(ohlcs);

            // enable sequential mode to place candles at X = 0, 1, 2, ...
            candlePlot.Sequential = true;

            // Since we are using sequential mode, we have to set the X axis manually.
            ConfigureXAxisLabels(plot, ohlcs, showYearInXAxis);

            // Putting the Y axis on the right side and formatting it as currency
            candlePlot.Axes.YAxis = plot.Axes.Right;
            plot.Axes.Right.TickGenerator = new ScottPlot.TickGenerators.NumericAutomatic()
            {
                LabelFormatter = (double value) => value.ToString("C")
            };

            var chartImage = plot.GetImageBytes(800, 400, ImageFormat.Png);
            return new MemoryStream(chartImage);
        }

        private static void ConfigureXAxisLabels(Plot plot, List<OHLC> ohlcs, bool showYearInXAxis)
        {
            // determine a few candles to display ticks for
            int tickCount = 5;
            int tickDelta = ohlcs.Count / tickCount;
            DateTime[] tickDates = ohlcs
                .Where((x, i) => i % tickDelta == 0)
                .Select(x => x.DateTime)
                .ToArray();

            // By default, horizontal tick labels will be numbers (1, 2, 3...)
            // We can use a manual tick generator to display dates on the horizontal axis
            double[] tickPositions = Generate.Consecutive(tickDates.Length, tickDelta);

            var dateFormat = showYearInXAxis ? "MM/dd/yyyy" : "MM/dd";
            string[] tickLabels = tickDates.Select(x => x.ToString(dateFormat)).ToArray();

            ScottPlot.TickGenerators.NumericManual tickGen = new(tickPositions, tickLabels);
            plot.Axes.Bottom.TickGenerator = tickGen;
        }
    }
}