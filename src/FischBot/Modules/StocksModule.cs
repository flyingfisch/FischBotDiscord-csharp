using Discord.Commands;
using FischBot.Services.DiscordModuleService;
using System.Threading.Tasks;
using Discord;
using System;
using FischBot.Services.FinanceService;
using System.Linq;

namespace FischBot.Modules
{
    [Group("stocks")]
    public class StocksModule : FischBotModuleBase<SocketCommandContext>
    {
        private readonly IFinanceService _financeService;

        public StocksModule(IDiscordModuleService moduleService, IFinanceService financeService) : base(moduleService)
        {
            _financeService = financeService;
        }

        [Command("price")]
        [Summary("Gets realtime information for the specified stock.")]
        public async Task DisplayRealtimeStockInfo([Summary("Stock to get information for")] string symbol)
        {
            try
            {
                var price = await _financeService.GetRealTimeStockPrice(symbol);

                var timeSeries = await _financeService.GetTimeSeries(symbol, "1day");
                var latestTimeSeriesInfo = timeSeries.Values.First();

                var usageStats = await _financeService.GetApiUsageStats();

                var embed = new EmbedBuilder()
                    .WithTitle($"Price information for {timeSeries.Symbol}")
                    .WithColor(price > latestTimeSeriesInfo.Open ? Color.Green : Color.Red)

                    .AddField("Price", price, inline: false)

                    .AddField("Trading info for last trading day", latestTimeSeriesInfo.Datetime, inline: false)

                    .AddField("Open/Close", $"{latestTimeSeriesInfo.Open.ToString("C")}/{latestTimeSeriesInfo.Close.ToString("C")}")
                    .AddField("High/Low", $"{latestTimeSeriesInfo.High.ToString("C")}/{latestTimeSeriesInfo.Low.ToString("C")}")

                    .AddField("Volume", latestTimeSeriesInfo.Volume, inline: false)

                    .WithFooter($"Source: twelvedata | Daily usage: {usageStats.daily_usage}/{usageStats.plan_daily_limit}")
                    .Build();

                await ReplyAsync(embed: embed);
            }
            catch (Exception)
            {
                await ReplyAsync("Unable to get information for that stock.");
            }
        }
    }
}