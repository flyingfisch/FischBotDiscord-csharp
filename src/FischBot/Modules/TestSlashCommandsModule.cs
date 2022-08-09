using System.Threading.Tasks;
using Discord.Interactions;
using FischBot.Services.DiscordModuleService;

namespace FischBot.Modules
{
    public class TestSlashCommandsModule : FischBotInteractionModuleBase<SocketInteractionContext>
    {
        public TestSlashCommandsModule(IDiscordModuleService moduleService) : base(moduleService)
        {
        }

        [SlashCommand("testecho", "Echo an input")]
        public async Task Echo(string input)
        {
            await RespondAsync(input);
        }
    }
}
