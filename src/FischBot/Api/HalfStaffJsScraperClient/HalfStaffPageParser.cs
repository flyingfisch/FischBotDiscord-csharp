using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using FischBot.Api.HalfStaffJsScraperClient.Dtos;

namespace FischBot.Api.HalfStaffJsScraperClient
{
    public class HalfStaffPageParser
    {
        private static readonly Regex CurrentPostsRegex = new Regex(@"var\s+posts\s*=\s*(\[[\s\S]*?\]);", RegexOptions.Compiled);

        public HalfStaffStatus ParseCurrentStatus(string html, DateTimeOffset now, string defaultSourceUrl)
        {
            if (string.IsNullOrWhiteSpace(html) || !CurrentPostsRegex.IsMatch(html))
            {
                return new HalfStaffStatus
                {
                    IsStatusKnown = false,
                    SourceUrl = defaultSourceUrl,
                };
            }

            var posts = ParseCurrentPosts(html);
            var currentEasternDate = ConvertToEasternDate(now);
            var activePosts = posts
                .Where(post => IsActiveNotice(post, currentEasternDate))
                .ToArray();

            if (activePosts.Length == 0)
            {
                return new HalfStaffStatus
                {
                    IsStatusKnown = true,
                    IsHalfStaff = false,
                    SourceUrl = defaultSourceUrl,
                };
            }

            return new HalfStaffStatus
            {
                IsStatusKnown = true,
                IsHalfStaff = true,
                ActivePosts = activePosts,
                ReasonTitle = HtmlDecode(activePosts[0].Title),
                Reason = HtmlDecode(activePosts[0].Excerpt),
                SourceUrl = defaultSourceUrl,
            };
        }

        public IReadOnlyList<HalfStaffPost> ParseCurrentPosts(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                return Array.Empty<HalfStaffPost>();
            }

            var regexMatch = CurrentPostsRegex.Match(html);
            if (!regexMatch.Success)
            {
                return Array.Empty<HalfStaffPost>();
            }

            try
            {
                var json = regexMatch.Groups[1].Value;
                var posts = JsonSerializer.Deserialize<List<HalfStaffPost>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

                return posts ?? new List<HalfStaffPost>();
            }
            catch (JsonException)
            {
                return Array.Empty<HalfStaffPost>();
            }
        }

        private static DateOnly ConvertToEasternDate(DateTimeOffset now)
        {
            try
            {
                var easternTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                var easternTime = TimeZoneInfo.ConvertTime(now, easternTimeZone);
                return DateOnly.FromDateTime(easternTime.DateTime);
            }
            catch (TimeZoneNotFoundException)
            {
                return DateOnly.FromDateTime(now.LocalDateTime);
            }
        }

        private static bool IsActiveNotice(HalfStaffPost post, DateOnly currentDate)
        {
            if (post == null || string.IsNullOrWhiteSpace(post.DateStart))
            {
                return false;
            }

            var startDate = ParseDate(post.DateStart);
            if (!startDate.HasValue)
            {
                return false;
            }

            var endDate = ParseDate(post.DateEnd);
            if (!endDate.HasValue)
            {
                var thirtyDaysAfterStart = startDate.Value.AddDays(30);
                return currentDate >= startDate.Value && currentDate <= thirtyDaysAfterStart;
            }

            return currentDate >= startDate.Value && currentDate <= endDate.Value;
        }

        private static DateOnly? ParseDate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return DateOnly.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate)
                ? parsedDate
                : null;
        }

        private static string HtmlDecode(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            var decoded = WebUtility.HtmlDecode(value)?.Trim();
            return string.IsNullOrWhiteSpace(decoded) ? null : decoded;
        }
    }
}
