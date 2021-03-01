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
                var quote = await _financeService.GetQuote(requestSymbol, "1day");
                var usageStats = await _financeService.GetApiUsageStats();

                var embed = new EmbedBuilder()
                    .WithTitle(quote.Name)
                    .WithColor(quote.Change > 0 ? Color.Green : Color.Red)

                    .AddField(
                        "Price",
                        $"{price.ToString("C")} {(quote.Change > 0 ? "▲" : "▼")}{quote.Change.ToString("C")} ({quote.PercentChange.ToString("F2")}%)"
                    )

                    .AddField("Trading info for last trading day", quote.Datetime.ToString("d"))

                    .AddField("Open/Close", $"{quote.Open.ToString("C")}/{quote.Close.ToString("C")}")
                    .AddField("High/Low", $"{quote.High.ToString("C")}/{quote.Low.ToString("C")}")

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