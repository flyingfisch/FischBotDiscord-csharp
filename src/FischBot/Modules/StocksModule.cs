using FischBot.Services.DiscordModuleService;
using System.Threading.Tasks;
using Discord;
using System;
using FischBot.Services.FinanceService;
using System.Linq;
using FischBot.Services.ImageChartService;
using Discord.Interactions;

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
                        $"{price.ToString("C")} {(quote.Change > 0 ? "▲" : "▼")}{quote.Change.ToString("C")} ({quote.PercentChange.ToString("F2")}%)"
                    )

                    .AddField("Trading info for last trading day", quote.Datetime.ToString("d"))

                    .AddField("Open/Close", $"{quote.Open.ToString("C")}/{quote.Close.ToString("C")}")
                    .AddField("High/Low", $"{quote.High.ToString("C")}/{quote.Low.ToString("C")}")

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
        public async Task DisplayStockChart([Summary(description: "Stock symbol")] string symbol, [Summary(description: "Time period to display for (optional)")] TimePeriod period = TimePeriod.Week)
        {
            var dataset = await GetDataSet(symbol, period);
            var lineColor = dataset.First() < dataset.Last() ? "2ECC71" : "E74C3C";

            var chartImage = _imageChartService.CreateLineChart(
                dataset,
                lineColor,
                500,
                100);

            await RespondWithFileAsync(chartImage, "chart.png");
        }

        private async Task<decimal[]> GetDataSet(string symbol, TimePeriod period)
        {
            if (period == TimePeriod.Week)
            {
                var timeSeries = await _financeService.GetTimeSeries(symbol, "2h");

                return timeSeries.Values
                    .Where(value => value.Datetime > DateTime.Now.AddDays(-7))
                    .OrderBy(value => value.Datetime)
                    .Select(value => value.Open)
                    .Take(7)
                    .ToArray();
            }

            if (period == TimePeriod.Month)
            {
                var timeSeries = await _financeService.GetTimeSeries(symbol, "1day");

                return timeSeries.Values
                    .Where(value => value.Datetime > DateTime.Now.AddMonths(-1))
                    .OrderBy(value => value.Datetime)
                    .Select(value => value.Open)
                    .Take(7)
                    .ToArray();
            }

            if (period == TimePeriod.Year)
            {
                var timeSeries = await _financeService.GetTimeSeries(symbol, "1month");

                return timeSeries.Values
                    .Where(value => value.Datetime > DateTime.Now.AddYears(-1))
                    .OrderBy(value => value.Datetime)
                    .Select(value => value.Open)
                    .Take(7)
                    .ToArray();
            }

            throw new NotImplementedException();
        }
    }
}