using System;
using FischBot.Models;

namespace FischBot.Api
{
    public interface IStarsAndStripesDailyClient
    {
        UsFlagHalfMastInfo GetUsFlagHalfMastInfo(DateTime date);
    }
}