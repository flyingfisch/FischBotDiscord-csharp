using System;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using FischBot.Services.DiscordModuleService;
using FischBot.Services.HolidayService;

namespace FischBot.Modules
{
    [Group("holiday", "Commands for counting down or displaying holidays")]
    public class HolidayModule : FischBotInteractionModuleBase<SocketInteractionContext>
    {
        private readonly IHolidayService _holidayService;

        public HolidayModule(IDiscordModuleService moduleService, IHolidayService holidayService) : base(moduleService)
        {
            _holidayService = holidayService;
        }

        [SlashCommand("countdown", "Displays days until a given holiday")]
        public async Task GetDaysUntilHoliday([Summary(description: "Name of the holiday to get a countdown for.")] string holidayName)
        {
            var now = DateTime.Now;

            try
            {
                var holiday = await _holidayService.GetHolidayByName(holidayName, "US", now.Year);

                var daysUntilHoliday = holiday.Date.Subtract(now).Days;

                if (daysUntilHoliday > 0)
                {
                    await RespondAsync($"There are {daysUntilHoliday} days until {holiday.Name}.");
                }
                else
                {
                    await RespondAsync($"{holiday.Name} was {daysUntilHoliday * -1} days ago.");
                }
            }
            catch
            {
                await RespondAsync("Could not find that holiday.", ephemeral: true);
            }
        }

        [SlashCommand("today", "Displays today's holiday(s)")]
        public async Task GetTodaysHolidays()
        {
            var now = DateTime.Now;
            var holidays = await _holidayService.GetHolidaysByDate(now, "US");

            if (holidays.Length > 0)
            {
                var embed = new EmbedBuilder()
                    .WithTitle("Today's Holidays");

                foreach (var holiday in holidays)
                {

                    embed.AddField(holiday.Name, holiday.Type);
                }

                await RespondAsync(embed: embed.Build());
            }
            else
            {
                await RespondAsync("There are no US holidays today.");
            }
        }
    }
}