using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using FischBot.Models;

namespace FischBot.Services.RouletteService
{
    public class RouletteService : IRouletteService
    {
        private readonly RouletteResult[] _rouletteResults =
        {
            new RouletteResult("A high five", HighFive, OutcomeType.Good, probability: 10),
            new RouletteResult("A kick in the rear", KickToTheRear, OutcomeType.Good, probability: 20),
        };


        private static async Task HighFive(ICommandContext context)
        {
            var emoji = new Emoji("✋");

            await context.Message.AddReactionAsync(emoji);
        }

        private static async Task KickToTheRear(ICommandContext context)
        {
            var emoji = new Emoji("🥾");

            await context.Message.AddReactionAsync(emoji);
        }
    }
}