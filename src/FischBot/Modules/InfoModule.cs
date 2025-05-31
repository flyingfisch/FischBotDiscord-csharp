using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using FischBot.Services.DiscordModuleService;

namespace FischBot.Modules
{
    public class InfoModule : FischBotInteractionModuleBase<SocketInteractionContext>
    {
        public InfoModule(IDiscordModuleService moduleService) : base(moduleService)
        {
        }

        [SlashCommand("say", "Echoes a message.")]
        public async Task SayAsync([Summary(description: "The text to echo.")] string echo, [Summary(description: "Whether to show your identity. (optional)")] bool anonymous = false)
        {
            if (anonymous)
            {
                await ReplyAsync(echo);
                await RespondAsync("_Shhh... I've said your super secret message._", ephemeral: true);
            }
            else
            {
                await RespondAsync(echo);
            }
        }

        [SlashCommand("good-bot", "Gives the bot some praise.")]
        public async Task ReceivePraiseAsync()
        {
            await RespondAsync("😊");
        }

        [SlashCommand("version", "Displays version information")]
        public async Task DisplayVersionInformation()
        {
            var informationalVersion = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            var assemblyVersion = Assembly.GetEntryAssembly().GetName().Version;

            var embed = new EmbedBuilder()
                .WithTitle("FischBot Version and Runtime Information")
                .AddField("Version", informationalVersion)
                .AddField("Assembly Version", assemblyVersion.ToString())
                .AddField("Operating System", Environment.OSVersion)
                .AddField("64-Bit OS", Environment.Is64BitOperatingSystem)
                .WithFooter("View my source code on Github! ♥ https://github.com/flyingfisch/FischBotDiscord-csharp");

            await RespondAsync(embed: embed.Build());
        }
    }
}