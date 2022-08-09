using System;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using FischBot.Services.AstronomyService;
using FischBot.Services.DiscordModuleService;

namespace FischBot.Modules
{
    [Group("astro", "Astronomy commands")]
    public class AstronomyModule : FischBotInteractionModuleBase<SocketInteractionContext>
    {
        private readonly IAstronomyService _astronomyService;

        public AstronomyModule(IDiscordModuleService moduleService, IAstronomyService astronomyService) : base(moduleService)
        {
            _astronomyService = astronomyService;
        }

        [SlashCommand("apod", "Displays the NASA astronomy picture of the day")]
        public async Task DisplayApod([Summary(description: "Date to query for (optional)")] DateTime? date = null)
        {
            var apod = await _astronomyService.GetPictureOfTheDay(date);

            var embed = new EmbedBuilder()
                .WithTitle($"{apod.Date.ToString("d")}: {apod.Name}")
                .WithDescription(apod.Caption)
                .WithFooter("NASA Astronomy Picture of the Day");
            
            if (apod.MediaType.ToLower() == "video")
            {
                var videoEmbed = embed
                    .WithUrl(apod.Url)
                    .AddField("Watch Video", apod.Url)
                    .WithImageUrl(apod.ThumbnailUrl)
                    .Build();
                
                await RespondAsync(embed: videoEmbed);
            }
            else 
            {
                var imageEmbed = embed
                    .WithImageUrl(apod.Url)
                    .Build();

                await RespondAsync(embed: imageEmbed);
            }
        }
    }
}