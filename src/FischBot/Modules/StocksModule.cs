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
            var interval = period switch
            {
                TimePeriod.Week => "1h",
                TimePeriod.Month => "1day",
                TimePeriod.SixMonths => "1week",
                TimePeriod.Year => "1month",
                _ => throw new NotImplementedException()
            };

            var span = period switch
            {
                TimePeriod.Week => TimeSpan.FromDays(7),
                TimePeriod.Month => TimeSpan.FromDays(30),
                TimePeriod.SixMonths => TimeSpan.FromDays(180),
                TimePeriod.Year => TimeSpan.FromDays(365),
                _ => throw new NotImplementedException()
            };

            var timeSeries = await _financeService.GetTimeSeries(symbol, interval);
            var timeSeriesValues = timeSeries.Values
                .OrderBy(value => value.Datetime.DateTime)
                .ToList();

            var showYearInXAxis = period == TimePeriod.Year || period == TimePeriod.SixMonths;

            var chartImageStream = _imageChartService.CreateStockChart(
                timeSeriesValues,
                span,
                showYearInXAxis);

            var usageStats = await _financeService.GetApiUsageStats();

            var embed = new EmbedBuilder()
                .WithTitle($"{timeSeries.Symbol} Stock Chart")
                .WithFooter($"Source: twelvedata | Daily usage: {usageStats.daily_usage}/{usageStats.plan_daily_limit}")
                .Build();

            await RespondWithFileAsync(chartImageStream,
                "chart.png",
                embed: embed);
        }
    }
}