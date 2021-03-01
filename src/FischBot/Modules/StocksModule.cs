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

                await ReplyAsync(embed: embed);
            }
            catch (Exception)
            {
                await ReplyAsync("Unable to get information for that stock.");
            }
        }
    }
}