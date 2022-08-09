using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using FischBot.Services.DiscordModuleService;

namespace FischBot.Modules
{
    public class PollModule : FischBotInteractionModuleBase<SocketInteractionContext>
    {
        private static readonly Int32[] _letterPollEmoji =
        { 
            // regional letter indicators
            0x1F1E6, 0x1F1E7, 0x1F1E8, 0x1F1E9, 0x1F1EA, 0x1F1EB, 0x1F1EC, 0x1F1ED, 0x1F1EE, 0x1F1EF,
            0x1F1F0, 0x1F1F1, 0x1F1F2, 0x1F1F3, 0x1F1F4, 0x1F1F5, 0x1F1F6, 0x1F1F7, 0x1F1F8, 0x1F1F9,
            0x1F1FA, 0x1F1FB, 0x1F1FC, 0x1F1FD, 0x1F1FE, 0x1F1FF
        };

        public PollModule(IDiscordModuleService moduleService) : base(moduleService)
        {
        }

        [SlashCommand("poll", "Up to 10 options")]
        public async Task CreatePoll(
            [Summary(description: "Poll Message")] string message,
            string choice1 = null,
            string choice2 = null,
            string choice3 = null,
            string choice4 = null,
            string choice5 = null,
            string choice6 = null,
            string choice7 = null,
            string choice8 = null,
            string choice9 = null,
            string choice10 = null)
        {
            var choices = new List<string>() { choice1, choice2, choice3, choice4, choice5, choice6, choice7, choice8, choice9, choice10 }
                .Where(choice => !string.IsNullOrEmpty(choice));

            var choicesWithLetterEmoji = choices.Select((choice, index) => $"{char.ConvertFromUtf32(_letterPollEmoji[index])}: {choice}");
            var choicesDescription = string.Join("\n", choicesWithLetterEmoji);

            // build embed for poll message and options
            var embed = new EmbedBuilder()
                .WithTitle($"POLL: {message}")
                .WithDescription(choicesDescription)
                .Build();


            await RespondAsync(embed: embed);

            // react to the embed we just sent with letters for each option
            var originalResponse = await GetOriginalResponseAsync();
            for (int i = 0; i < choices.Count(); i++)
            {
                var emoji = new Emoji(char.ConvertFromUtf32(_letterPollEmoji[i]));

                await originalResponse.AddReactionAsync(emoji);
            }
        }
    }
}