using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using FischBot.Helpers;
using FischBot.Services.DiscordModuleService;
using FischBot.Services.EightBallService;

namespace FischBot.Modules
{
    [Group("games")]
    public class GameModule : FischBotModuleBase<SocketCommandContext>
    {
        private readonly IEightBallService _eightBallService;

        public GameModule(IDiscordModuleService moduleService, IEightBallService eightBallService) : base(moduleService)
        {
            _eightBallService = eightBallService;
        }

        /// <summary>
        /// Return a response to the users question, randomly choosing from a scrambled list of possible responses.
        /// </summary>
        /// <param name="question">Users question for the Magic 8-Ball.</param>
        /// <returns></returns>
        [Command("8ball")]
        [Summary("Asks the Magic 8-Ball for the answer that you seek.")]
        public async Task QuestionThe8Ball([Remainder][Summary("The question for which you seek an answer.")] string question)
        {
            // Jumble our possible results.
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"You shake the Magic 8-Ball and ask it, \"{question}?\"");

            // Build the initial embed
            var embedBuilder = new EmbedBuilder
            {
                Color = Color.DarkGrey,
                Title = "Hey, Magic 8-Ball...",
                Description = stringBuilder.ToString()
            };

            // Get an answer
            var (phrase, level) = _eightBallService.GetEightBallResult();

            await Task.Run(async () =>
            {
                // Output the initial message for the user, and wait 1 second.
                var message = await ReplyAsync(embed: embedBuilder.Build()).ConfigureAwait(false);
                await Task.Delay(1000).ConfigureAwait(false);

                // Append the next step in the process for the user, building up to the answer. Wait 1 more second.
                stringBuilder.AppendLine("Slowly turning it over, the blue liquid fades away as your answer appears.");
                embedBuilder.Description = stringBuilder.ToString();
                await message.ModifyAsync(msg => msg.Embed = embedBuilder.Build()).ConfigureAwait(false);
                await Task.Delay(1000).ConfigureAwait(false);

                // Based of the color level, set the embed color based off of ...
                // Green - Positive, LightOrange - Neutral, Red - Negative
                embedBuilder.Color =
                    level switch
                    {
                        1 => Color.Green,
                        2 => Color.LightOrange,
                        _ => Color.Red
                    };
                // Output the received answer to the user.
                stringBuilder.AppendLine($"Your answer is, \"{phrase}\"");
                embedBuilder.Description = stringBuilder.ToString();
                await message.ModifyAsync(msg => msg.Embed = embedBuilder.Build()).ConfigureAwait(false);
            });
        }

    }
}