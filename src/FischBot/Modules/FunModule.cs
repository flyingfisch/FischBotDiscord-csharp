using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using FischBot.Helpers;
using FischBot.Services.DiscordModuleService;

namespace FischBot.Modules
{
    public class FunModule : FischBotInteractionModuleBase<SocketInteractionContext>
    {
        private static readonly string[] _emojiPartyEmojis =
        {
            "😇", "😍", "🤑", "🤓", "🤖", "👽", "👾", "👻", "💯", "🤟",
            "👀", "🍿", "🍔", "📞", "📀", "📸", "💹", "💰", "✏", "📊",
            "🔓", "🔫", "📡", "🔭", "💉", "⚰", "☢", "⚜", "🎉", "🥳",
            "🎊", "🤩", "☄", "🎂", "🥮", "🍪", "⚡", "⚔", "☘", "☠",
            "🤭", "🤠", "🥺", "😈", "☝", "✊", "🦊", "🐠"
        };

        private static readonly string[] _partyTimeMessages =
        {
            "**PARTAY TIME**", "**TIME TO _PARTY_**", "Everybody loves EMOJIS"
        };

        public FunModule(IDiscordModuleService moduleService) : base(moduleService)
        {
        }

        [SlashCommand("emojiparty", "Throws an emoji party!")]
        public async Task ThrowEmojiParty()
        {
            var emojisToPartyWith = _emojiPartyEmojis.Distinct().Shuffle().Take(20);

            await RespondAsync(_partyTimeMessages.Shuffle().First());

            var partyTimeMessage = await GetOriginalResponseAsync();
            await partyTimeMessage.AddReactionsAsync(emojisToPartyWith.Select(emoji => new Emoji(emoji)));
        }

        [SlashCommand("lamp", "Announces that the lamp has spoken, thus ending arguments.")]
        public async Task TheLampHasSpoken()
        {
            await RespondAsync("https://raw.githubusercontent.com/flyingfisch/FischBotDiscord-csharp/master/assets/the-lamp-has-spoken.png");
        }
    }
}