using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace FischBot.Models
{
    public enum OutcomeType
    {
        Good,
        Neutral,
        Bad
    }

    public class RouletteResult
    {
        public string Description { get; set; }
        public int Probability { get; set; }
        public Func<ICommandContext, Task> Action { get; set; }
        public OutcomeType OutcomeType { get; set; }

        public RouletteResult(string description, Func<ICommandContext, Task> action, OutcomeType outcomeType, int probability)
        {
            Description = description;
            Probability = probability;
            Action = action;
            OutcomeType = outcomeType;
        }
    }
}