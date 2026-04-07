using System;
using FischBot.Api.HalfStaffJsScraperClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FischBot.UnitTests.Api.HalfStaffJsScraperClient
{
    [TestClass]
    public class HalfStaffPageParserTests
    {
        private const string DefaultSourceUrl = "https://halfstaff.org/";
        private HalfStaffPageParser _parser;

        [TestInitialize]
        public void InitializeDependencies()
        {
            _parser = new HalfStaffPageParser();
        }

        [TestMethod]
        public void ParseCurrentStatus_ReturnsActiveHalfStaffStatusForCurrentPosts()
        {
            var html = @"<script>
                var posts = [
                    {""title"":""Honoring A Fallen Public Servant"",""permalink"":""https://halfstaff.org/halfstaff/honoring-a-fallen-public-servant/"",""date_start"":""2026-04-05"",""date_end"":""2026-04-08"",""type"":""Federal"",""excerpt"":""As a mark of respect, the flag should be flown at half-staff nationwide.""},
                    {""title"":""Honoring A Local Hero"",""permalink"":""https://halfstaff.org/halfstaff/ia-local-hero/"",""date_start"":""2026-04-06"",""date_end"":""undetermined"",""type"":""Local"",""state"":""IA"",""excerpt"":""Flags lowered in Iowa.""}
                ];
                </script>";

            var status = _parser.ParseCurrentStatus(html, new DateTimeOffset(2026, 4, 7, 12, 0, 0, TimeSpan.Zero), DefaultSourceUrl);

            Assert.IsTrue(status.IsStatusKnown);
            Assert.IsTrue(status.IsHalfStaff);
            Assert.AreEqual(2, status.ActivePosts.Length);
            Assert.AreEqual("Honoring A Fallen Public Servant", status.ReasonTitle);
            Assert.AreEqual("As a mark of respect, the flag should be flown at half-staff nationwide.", status.Reason);
            Assert.AreEqual(DefaultSourceUrl, status.SourceUrl);
        }

        [TestMethod]
        public void ParseCurrentStatus_ReturnsInactiveStatusWhenThereAreNoCurrentPosts()
        {
            var html = @"<script>
                var posts = [
                ];
                </script>";

            var status = _parser.ParseCurrentStatus(html, new DateTimeOffset(2026, 4, 7, 12, 0, 0, TimeSpan.Zero), DefaultSourceUrl);

            Assert.IsTrue(status.IsStatusKnown);
            Assert.IsFalse(status.IsHalfStaff);
            Assert.AreEqual(DefaultSourceUrl, status.SourceUrl);
        }

        [TestMethod]
        public void ParseCurrentStatus_ReturnsUnknownStatusWhenEmbeddedPostsPayloadIsMissing()
        {
            var html = "<html><body><p>No inline post data was found.</p></body></html>";

            var status = _parser.ParseCurrentStatus(html, new DateTimeOffset(2026, 4, 7, 12, 0, 0, TimeSpan.Zero), DefaultSourceUrl);

            Assert.IsFalse(status.IsStatusKnown);
            Assert.AreEqual(DefaultSourceUrl, status.SourceUrl);
        }

        [TestMethod]
        public void ParseCurrentStatus_ExpiresUndeterminedNoticeAfterThirtyDays()
        {
            var html = @"<script>
                var posts = [
                    {""title"":""Expired Undetermined Notice"",""permalink"":""https://halfstaff.org/halfstaff/expired-undetermined/"",""date_start"":""2026-02-20"",""date_end"":""undetermined"",""type"":""Local"",""state"":""KS"",""excerpt"":""Should no longer be active.""}
                ];
                </script>";

            var status = _parser.ParseCurrentStatus(html, new DateTimeOffset(2026, 4, 7, 12, 0, 0, TimeSpan.Zero), DefaultSourceUrl);

            Assert.IsTrue(status.IsStatusKnown);
            Assert.IsFalse(status.IsHalfStaff);
            Assert.AreEqual(DefaultSourceUrl, status.SourceUrl);
        }

        [TestMethod]
        public void ParseCurrentStatus_KeepsUndeterminedNoticeActiveWithinThirtyDays()
        {
            var html = @"<script>
                var posts = [
                    {""title"":""Recent Undetermined Notice"",""permalink"":""https://halfstaff.org/halfstaff/recent-undetermined/"",""date_start"":""2026-03-20"",""date_end"":""undetermined"",""type"":""Local"",""state"":""KS"",""excerpt"":""Should still be active.""}
                ];
                </script>";

            var status = _parser.ParseCurrentStatus(html, new DateTimeOffset(2026, 4, 7, 12, 0, 0, TimeSpan.Zero), DefaultSourceUrl);

            Assert.IsTrue(status.IsStatusKnown);
            Assert.IsTrue(status.IsHalfStaff);
            Assert.AreEqual(1, status.ActivePosts.Length);
        }
    }
}
