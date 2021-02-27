using Discord.Commands;
using FischBot.Services.DiscordModuleService;
using FischBot.Services.AlphaVantageService;
using System.Threading.Tasks;
using Discord;
using System;

namespace FischBot.Modules
{
    [Group("stocks")]
    public class StocksModule : FischBotModuleBase<SocketCommandContext>
    {
        private readonly IAlphaVantageService _alphaVantageService;
        private readonly string _nasdaqBaseUrl = "https://www.nasdaq.com/";

        public StocksModule(IDiscordModuleService moduleService, IAlphaVantageService alphaVantageService) : base(moduleService)
        {
            _alphaVantageService = alphaVantageService;
        }

        [Command("price")]
        [Summary("Gets today's price information for the specified stock symbol.")]
        public async Task DisplayTodayStockPrice([Summary("Stock symbol to get information for")] string symbol)
        {
            try
            {
                var stockQuote = await _alphaVantageService.GetCurrentStockQuote(symbol);

                var embed = new EmbedBuilder()
                    .WithTitle($"Price information for {stockQuote.Symbol}")
                    .WithUrl($"{_nasdaqBaseUrl}market-activity/stocks/{symbol}/real-time")
                    .WithColor(stockQuote.ChangePercent > 0 ? Color.Green : Color.Red)

                    .AddField("Price", stockQuote.Price.ToString("C"), inline: true)
                    .AddField("High", stockQuote.HighestPrice.ToString("C"), inline: true)
                    .AddField("Low", stockQuote.LowestPrice.ToString("C"), inline: true)

                    .AddField("% change", $"{stockQuote.ChangePercent}%", inline: true)
                    .AddField("Volume", stockQuote.Volume, inline: true)

                    .WithFooter("Source: Alpha Vantage")
                    .Build();

                await ReplyAsync(embed: embed);
            }
            catch (Exception)
            {
                await ReplyAsync("Unable to find that stock.");
            }
        }
    }
}