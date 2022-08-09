using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using FischBot.Helpers;
using FischBot.Services.DiscordModuleService;

namespace FischBot.Modules
{
    [Group("games", "A collection of text games")]
    public class GameModule : FischBotInteractionModuleBase<SocketInteractionContext>
    {
        private readonly List<(string Phrase, int Level)> _eightBallResults;
        private static readonly Random Random = new Random();

        public GameModule(IDiscordModuleService moduleService) : base(moduleService)
        {
            _eightBallResults = new List<(string phrase, int level)>
            {
                ("It is certain.", 1),
                ("It is decidedly so.", 1),
                ("Without a doubt.", 1),
                ("Yes - definitely.", 1),
                ("You may rely on it.", 1),
                ("As I see it, yes.", 1),
                ("Most likely.", 1),
                ("Outlook good.", 1),
                ("Yes.", 1),
                ("Signs point to yes.", 1),
                ("Reply hazy, try again.", 2),
                ("Ask again later.", 2),
                ("Better not tell you now.", 2),
                ("Cannot predict now.", 2),
                ("Concentrate and ask again.", 2),
                ("Don't count on it.", 3),
                ("My reply is no.", 3),
                ("My sources say no.", 3),
                ("Outlook not so good.", 3),
                ("Very doubtful.", 3)
            };
        }

        /// <summary>
        /// Return a response to the users question, randomly choosing from a scrambled list of possible responses.
        /// </summary>
        /// <param name="question">Users question for the Magic 8-Ball.</param>
        /// <returns></returns>
        [SlashCommand("8ball", "Asks the Magic 8-Ball for the answer that you seek.")]
        public async Task QuestionThe8Ball([Summary(description: "The question for which you seek an answer.")] string question)
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

            // Scramble our list of possible results
            var shuffledList = ShuffleTheList();

            // Get an answer
            var (phrase, level) = shuffledList[Random.Next(0, shuffledList.Count)];

            await Task.Run(async () =>
            {
                // Output the initial message for the user, and wait 1 second.
                await RespondAsync(embed: embedBuilder.Build()).ConfigureAwait(false);
                var message = await GetOriginalResponseAsync();

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

        // Loop through a random amount of times to scramble our list of possible response for the Magic 8ball game.
        private List<(string phrase, int level)> ShuffleTheList()
        {
            var timesToJumble = Random.Next(2, 10);
            var randomizedList = _eightBallResults;

            for (var i = 0; i < timesToJumble; i++)
            {
                randomizedList = randomizedList.Shuffle().ToList();
            }

            return randomizedList;
        }
    }
}