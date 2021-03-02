using System.IO;

namespace FischBot.Services.ImageChartService
{
    public interface IImageChartService
    {
        Stream CreateLineChart(decimal[] data, string lineColor, int width, int height);
    }
}