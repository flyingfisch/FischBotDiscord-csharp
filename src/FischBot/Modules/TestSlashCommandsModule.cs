using System.Threading.Tasks;
using Discord.Interactions;

namespace FischBot.Modules
{
    public class TestSlashCommandsModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("echo", "Echo an input")]
        public async Task Echo(string input)
        {
            await RespondAsync(input);
        }
    }
}
