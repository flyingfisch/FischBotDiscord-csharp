using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using FischBot.Services.DiscordModuleService;
using FischBot.Services.FinanceService;

namespace FischBot.Modules
{
    [Group("crypto")]
    public class CryptocurrencyModule : FischBotModuleBase<SocketCommandContext>
    {
        private readonly IFinanceService _financeService;

        public CryptocurrencyModule(IDiscordModuleService moduleService, IFinanceService financeService) : base(moduleService)
        {
            _financeService = financeService;
        }

        [Command("price")]
        [Summary("Gets today's price information for the specified crypto symbol.")]
        public async Task DisplayRealtimeCryptoInfo([Summary("Crypto symbol to get information for")] string symbol)
        {
            try
            {
                var requestSymbol = $"{symbol}/USD";

                var price = await _financeService.GetRealTimeStockPrice(requestSymbol);

                var timeSeries = await _financeService.GetTimeSeries(requestSymbol, "1day");
                var latestTimeSeriesInfo = timeSeries.Values.First();

                var usageStats = await _financeService.GetApiUsageStats();

                var embed = new EmbedBuilder()
                    .WithTitle($"Price information for {symbol.ToUpper()}")
                    .WithColor(price > latestTimeSeriesInfo.Open ? Color.Green : Color.Red)

                    .AddField("Price", price)

                    .AddField("Trading info for last trading day", latestTimeSeriesInfo.Datetime)

                    .AddField("Open/Close", $"{latestTimeSeriesInfo.Open.ToString("C")}/{latestTimeSeriesInfo.Close.ToString("C")}")
                    .AddField("High/Low", $"{latestTimeSeriesInfo.High.ToString("C")}/{latestTimeSeriesInfo.Low.ToString("C")}")

                    .WithFooter($"Source: twelvedata | Daily usage: {usageStats.daily_usage}/{usageStats.plan_daily_limit}")
                    .Build();

                await ReplyAsync(embed: embed);
            }
            catch (Exception)
            {
                await ReplyAsync("Unable to get information for that cryptocurrency.");
            }
        }
    }
}