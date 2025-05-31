using FischBot.Services.DiscordModuleService;
using System.Threading.Tasks;
using Discord;
using System;
using FischBot.Services.FinanceService;
using System.Linq;
using FischBot.Services.ImageChartService;
using Discord.Interactions;
using System.Collections.Generic;
using FischBot.Models.Finance;

namespace FischBot.Modules
{
    [Group("stocks", "Stock market commands")]
    public class StocksModule : FischBotInteractionModuleBase<SocketInteractionContext>
    {
        private readonly IFinanceService _financeService;
        private readonly IImageChartService _imageChartService;

        public enum TimePeriod
        {
            Week,
            Month,
            SixMonths,
            Year
        }


        public StocksModule(IDiscordModuleService moduleService, IFinanceService financeService, IImageChartService imageChartService) : base(moduleService)
        {
            _financeService = financeService;
            _imageChartService = imageChartService;
        }

        [SlashCommand("price", "Gets realtime information for the specified stock.")]
        public async Task DisplayRealtimeStockInfo([Summary(description: "Stock to get information for")] string symbol)
        {
            try
            {
                var price = await _financeService.GetRealTimeStockPrice(symbol);
                var quote = await _financeService.GetQuote(symbol, "1day");
                var usageStats = await _financeService.GetApiUsageStats();

                var embed = new EmbedBuilder()
                    .WithTitle($"{quote.Symbol}: {quote.Name}")
                    .WithColor(quote.Change > 0 ? Color.Green : Color.Red)

                    .AddField(
                        "Price",
                        $"{price:C} {(quote.Change > 0 ? "▲" : "▼")}{quote.Change:C} ({quote.PercentChange:F2}%)"
                    )

                    .AddField("Trading info for last trading day", quote.Datetime.ToString("d"))

                    .AddField("Open/Close", $"{quote.Open:C}/{quote.Close:C}")
                    .AddField("High/Low", $"{quote.High:C}/{quote.Low:C}")

                    .AddField("Volume", quote.Volume, inline: false)

                    .WithFooter($"Source: twelvedata | Daily usage: {usageStats.daily_usage}/{usageStats.plan_daily_limit}")
                    .Build();

                await RespondAsync(embed: embed);
            }
            catch (Exception)
            {
                await RespondAsync("Unable to get information for that stock.", ephemeral: true);
            }
        }

        [SlashCommand("chart", "Displays a chart for the specified stock.")]
        public async Task DisplayStockChart([Summary(description: "Stock symbol")] string symbol,
                                            [Summary(description: "Time period to display for (optional)")] TimePeriod period = TimePeriod.Month)
        {
            var timeSeriesValues = await GetTimeSeriesValues(symbol, period);

            var span = period switch
            {
                TimePeriod.Week => TimeSpan.FromDays(7),
                TimePeriod.Month => TimeSpan.FromDays(30),
                TimePeriod.SixMonths => TimeSpan.FromDays(180),
                TimePeriod.Year => TimeSpan.FromDays(365),
                _ => throw new NotImplementedException()
            };

            var showYearInXAxis = period == TimePeriod.Year || period == TimePeriod.SixMonths;

            var chartImageStream = _imageChartService.CreateStockChart(
                timeSeriesValues,
                span,
                showYearInXAxis);

            await RespondWithFileAsync(chartImageStream, "chart.png");
        }


        private async Task<List<TimeSeriesValue>> GetTimeSeriesValues(string symbol, TimePeriod period)
        {
            switch (period)
            {
                case TimePeriod.Week:
                    {
                        var timeSeries = await _financeService.GetTimeSeries(symbol, "1h");

                        var values = timeSeries.Values
                            .Where(value => value.Datetime > DateTime.Now.AddDays(-7))
                            .OrderBy(value => value.Datetime.DateTime)
                            .ToList();

                        return values;
                    }

                case TimePeriod.Month:
                    {
                        var timeSeries = await _financeService.GetTimeSeries(symbol, "1day");

                        var values = timeSeries.Values
                            .Where(value => value.Datetime > DateTime.Now.AddDays(-30))
                            .OrderBy(value => value.Datetime.DateTime)
                            .ToList();

                        return values;
                    }

                case TimePeriod.SixMonths:
                    {
                        var timeSeries = await _financeService.GetTimeSeries(symbol, "1week");

                        var values = timeSeries.Values
                            .Where(value => value.Datetime > DateTime.Now.AddYears(-1))
                            .OrderBy(value => value.Datetime.DateTime)
                            .ToList();

                        return values;
                    }

                case TimePeriod.Year:
                    {
                        var timeSeries = await _financeService.GetTimeSeries(symbol, "1month");

                        var values = timeSeries.Values
                            .Where(value => value.Datetime > DateTime.Now.AddYears(-1))
                            .OrderBy(value => value.Datetime.DateTime)
                            .ToList();

                        return values;
                    }

                default:
                    throw new NotImplementedException();
            }
        }
    }
}