using System;
using FischBot.Models;

namespace FischBot.Api.StarsAndStripesDailyClient
{
    public interface IStarsAndStripesDailyClient
    {
        UsFlagHalfMastInfo GetUsFlagHalfMastInfo(DateTime date);
    }
}