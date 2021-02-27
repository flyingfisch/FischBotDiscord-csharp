using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using FischBot.Services.AlphaVantageService;
using FischBot.Services.DiscordModuleService;

namespace FischBot.Modules
{
    [Group("crypto")]
    public class CryptoModule : FischBotModuleBase<SocketCommandContext>
    {
        private readonly IAlphaVantageService _alphaVantageService;

        public CryptoModule(IDiscordModuleService moduleService, IAlphaVantageService alphaVantageService) : base(moduleService)
        {
            _alphaVantageService = alphaVantageService;
        }

        [Command("price")]
        [Summary("Gets today's price information for the specified crypto symbol.")]
        public async Task DisplayTodayCryptoPrice([Summary("Crypto symbol to get information for")] string symbol)
        {
            try
            {
                var cryptoExchangeRate = await _alphaVantageService.GetCurrentCryptoExchangeRate(symbol, "USD");

                var embed = new EmbedBuilder()
                    .WithTitle($"Price information for {cryptoExchangeRate.FromCurrencyCode}")
                    .WithColor(Color.Blue)

                    .AddField("Price", cryptoExchangeRate.ExchangeRate.ToString("C"))

                    .WithFooter("Source: Alpha Vantage")
                    .Build();

                await ReplyAsync(embed: embed);
            }
            catch (Exception)
            {
                await ReplyAsync("Unable to find that cryptocurrency.");
            }
        }
    }
}