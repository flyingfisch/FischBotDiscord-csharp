using System;
using System.Collections.Generic;
using System.IO;
using FischBot.Models.Finance;

namespace FischBot.Services.ImageChartService
{
    public interface IImageChartService
    {
        MemoryStream CreateStockChart(IEnumerable<TimeSeriesValue> timeSeriesValues, TimeSpan span, bool showYearInXAxis);
    }
}