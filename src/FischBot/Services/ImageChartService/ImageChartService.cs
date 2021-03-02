using System;
using System.IO;
using System.Linq;
using ImageChartsLib;

namespace FischBot.Services.ImageChartService
{
    public class ImageChartService : IImageChartService
    {
        public Stream CreateLineChart(decimal[] data, string lineColor, int width, int height)
        {
            var serializedData = $"a:{string.Join(',', data)}";
            var dataRange = $"0,{Math.Floor(data.Min())},{Math.Ceiling(data.Max())}";

            var lineChart = new ImageCharts()
                .cht("ls")
                .chco(lineColor)
                .chls("2")
                .chf("bg,s,00000000")
                .chs($"{width}x{height}")
                .chxt("y")
                .chxs("0N*cUSD2sz*,666666")
                .chxr(dataRange)
                .chd(serializedData);

            var buffer = lineChart.toBuffer();
            var stream = new MemoryStream(buffer);

            return stream;
        }
    }
}