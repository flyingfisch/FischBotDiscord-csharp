using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FischBot.Models;
using Microsoft.Extensions.Configuration;
using Tweetinvi;

namespace FischBot.Services.HalfMastService
{
    public class HalfMastService : IHalfMastService
    {
        private readonly IConfiguration _configuration;

        private readonly string _twitterApiConsumerKey;
        private readonly string _twitterApiConsumerSecret;
        private readonly string _twitterApiAccessToken;
        private readonly string _twitterApiAccessTokenSecret;

        private const string _halfMastAlertsTwitterAccountName = "HalfStaffAlerts";

        private readonly TwitterClient _twitterClient;

        public HalfMastService(IConfiguration configuration)
        {
            _configuration = configuration;

            _twitterApiConsumerKey = configuration.GetSection("FischBot:twitterApiConsumerKey").Value;
            _twitterApiConsumerSecret = configuration.GetSection("FischBot:twitterApiConsumerSecret").Value;

            _twitterApiAccessToken = configuration.GetSection("FischBot:twitterApiAccessToken").Value;
            _twitterApiAccessTokenSecret = configuration.GetSection("FischBot:twitterApiAccessTokenSecret").Value;

            _twitterClient = new TwitterClient(_twitterApiConsumerKey,
                                               _twitterApiConsumerSecret,
                                               _twitterApiAccessToken,
                                               _twitterApiAccessTokenSecret);
        }

        public async Task<HalfMastNotice> GetLatestHalfMastNotice(string state)
        {
            var searchState = string.IsNullOrEmpty(state) ? "Entire United States" : state;
            var response = await _twitterClient.SearchV2.SearchTweetsAsync($"(from: {_halfMastAlertsTwitterAccountName}) {searchState}");
            var latestHalfMastTweet = response.Tweets.FirstOrDefault();

            if (latestHalfMastTweet != null)
            {
                var halfMastNotice = new HalfMastNotice()
                {
                    State = WebUtility.HtmlDecode(Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(searchState)),
                    Description = latestHalfMastTweet.Text,
                    SourceUrl = $"https://twitter.com/{HalfMastAlertsTwitterAccountName}/status/{latestHalfMastTweet.Id}"
                };

                return halfMastNotice;
            }
            else
            {
                return null;
            }
        }
    }
}
