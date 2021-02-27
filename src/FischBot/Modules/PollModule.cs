using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using FischBot.Services.DiscordModuleService;

namespace FischBot.Modules
{
    public class PollModule : FischBotModuleBase<SocketCommandContext>
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

        [Command("poll")]
        [Summary("Reacts to the message with emoji to start a poll.")]
        public async Task CreateLetterPoll(
            [Summary("The number of options to create")] int numberOfOptions,
            [Summary("Your poll message")][Remainder] string message = null)
        {
            if (numberOfOptions > 20)
            {
                await ReplyAsync("The maximum number of options is 20.");
            }
            else
            {
                for (int i = 0; i < numberOfOptions; i++)
                {
                    var emoji = new Emoji(char.ConvertFromUtf32(_letterPollEmoji[i]));

                    await Context.Message.AddReactionAsync(emoji);
                }
            }
        }
    }
}