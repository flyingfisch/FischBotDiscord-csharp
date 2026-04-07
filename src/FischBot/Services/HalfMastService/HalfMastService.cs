using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FischBot.Api.HalfStaffJsScraperClient.Dtos;
using FischBot.Api.HalfStaffJsScraperClient;
using FischBot.Models;

namespace FischBot.Services.HalfMastService
{
    public class HalfMastService : IHalfMastService
    {
        private readonly IHalfStaffJsScraperClient _halfStaffJsScraperClient;

        public HalfMastService(IHalfStaffJsScraperClient halfStaffJsScraperClient)
        {
            _halfStaffJsScraperClient = halfStaffJsScraperClient;
        }

        public async Task<HalfMastStatus> GetHalfMastStatus()
        {
            var response = await _halfStaffJsScraperClient.GetHalfStaffStatus();

            var description = response.IsHalfStaff
                ? BuildActiveDescription(response.ActivePosts)
                : null;

            return new HalfMastStatus()
            {
                IsStatusKnown = response.IsStatusKnown,
                IsHalfStaff = response.IsHalfStaff,
                Description = description,
                SourceUrl = response.SourceUrl,
            };
        }

        private static string BuildActiveDescription(HalfStaffPost[] activePosts)
        {
            if (activePosts == null || activePosts.Length == 0)
            {
                return null;
            }

            var jurisdictionLabels = activePosts
                .Select(GetJurisdictionLabel)
                .Distinct()
                .ToArray();

            var descriptionLines = new List<string>
            {
                $"Active notices found for: {string.Join(", ", jurisdictionLabels)}"
            };

            foreach (var activePost in activePosts.Take(3))
            {
                var jurisdiction = GetJurisdictionLabel(activePost);
                descriptionLines.Add($"{jurisdiction}: {activePost.Title}");
            }

            if (activePosts.Length > 3)
            {
                descriptionLines.Add($"Plus {activePosts.Length - 3} more active notice(s).");
            }

            return string.Join("\n", descriptionLines);
        }

        private static string GetJurisdictionLabel(HalfStaffPost post)
        {
            if (post == null)
            {
                return "Unknown";
            }

            if (string.Equals(post.Type, "Federal", StringComparison.OrdinalIgnoreCase))
            {
                return "Nationwide (Federal)";
            }

            return post.State switch
            {
                "AL" => "Alabama",
                "AK" => "Alaska",
                "AS" => "American Samoa",
                "AZ" => "Arizona",
                "AR" => "Arkansas",
                "CA" => "California",
                "CO" => "Colorado",
                "CT" => "Connecticut",
                "DC" => "District of Columbia",
                "DE" => "Delaware",
                "FL" => "Florida",
                "GA" => "Georgia",
                "GU" => "Guam",
                "HI" => "Hawaii",
                "IA" => "Iowa",
                "ID" => "Idaho",
                "IL" => "Illinois",
                "IN" => "Indiana",
                "KS" => "Kansas",
                "KY" => "Kentucky",
                "LA" => "Louisiana",
                "MA" => "Massachusetts",
                "MD" => "Maryland",
                "ME" => "Maine",
                "MI" => "Michigan",
                "MN" => "Minnesota",
                "MO" => "Missouri",
                "MP" => "Northern Mariana Islands",
                "MS" => "Mississippi",
                "MT" => "Montana",
                "NC" => "North Carolina",
                "ND" => "North Dakota",
                "NE" => "Nebraska",
                "NH" => "New Hampshire",
                "NJ" => "New Jersey",
                "NM" => "New Mexico",
                "NV" => "Nevada",
                "NY" => "New York",
                "OH" => "Ohio",
                "OK" => "Oklahoma",
                "OR" => "Oregon",
                "PA" => "Pennsylvania",
                "PR" => "Puerto Rico",
                "RI" => "Rhode Island",
                "SC" => "South Carolina",
                "SD" => "South Dakota",
                "TN" => "Tennessee",
                "TX" => "Texas",
                "UT" => "Utah",
                "VA" => "Virginia",
                "VI" => "U.S. Virgin Islands",
                "VT" => "Vermont",
                "WA" => "Washington",
                "WI" => "Wisconsin",
                "WV" => "West Virginia",
                "WY" => "Wyoming",
                _ => post.State,
            };
        }
    }
}
