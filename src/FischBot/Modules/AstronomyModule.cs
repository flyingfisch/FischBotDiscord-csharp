using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using FischBot.Services.AstronomyService;
using FischBot.Services.DiscordModuleService;

namespace FischBot.Modules
{
    [Group("astro")]
    public class AstronomyModule : FischBotModuleBase<SocketCommandContext>
    {
        private readonly IAstronomyService _astronomyService;

        public AstronomyModule(IDiscordModuleService moduleService, IAstronomyService astronomyService) : base(moduleService)
        {
            _astronomyService = astronomyService;
        }

        [Command("apod")]
        [Summary("Displays the NASA astronomy picture of the day.")]
        public async Task DisplayApod([Summary("Date to query for.")] DateTime? date = null)
        {
            var apod = await _astronomyService.GetPictureOfTheDay(date ?? DateTime.Now);

            var embed = new EmbedBuilder()
                .WithTitle($"{apod.Date.ToString("d")}: {apod.Name}")
                .WithImageUrl(apod.Url)
                .WithDescription(apod.Caption)
                .WithFooter("NASA Astronomy Picture of the Day")
                .Build();

            await ReplyAsync(embed: embed);
        }
    }
}