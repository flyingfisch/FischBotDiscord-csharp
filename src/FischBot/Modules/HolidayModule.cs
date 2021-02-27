using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using FischBot.Services.DiscordModuleService;
using FischBot.Services.HolidayService;

namespace FischBot.Modules
{
    [Group("holiday")]
    public class HolidayModule : FischBotModuleBase<SocketCommandContext>
    {
        private readonly IHolidayService _holidayService;

        public HolidayModule(IDiscordModuleService moduleService, IHolidayService holidayService) : base(moduleService)
        {
            _holidayService = holidayService;
        }

        [Summary("Displays days until a given holiday")]
        [Command("countdown")]
        public async Task GetDaysUntilHoliday([Remainder][Summary("Name of the holiday to get a countdown for.")] string holidayName)
        {
            var now = DateTime.Now;

            var holiday = await _holidayService.GetHolidayByName(holidayName, "US", now.Year);

            var daysUntilHoliday = holiday.Date.Subtract(now).Days;

            if (daysUntilHoliday > 0)
            {
                await ReplyAsync($"There are {daysUntilHoliday} days until {holiday.Name}.");
            }
            else
            {
                await ReplyAsync($"{holiday.Name} was {daysUntilHoliday * -1} days ago.");
            }
        }

        [Summary("Displays today's holiday(s)")]
        [Command("today")]
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

                await ReplyAsync(embed: embed.Build());
            }
            else
            {
                await ReplyAsync("There are no US holidays today.");
            }
        }
    }
}