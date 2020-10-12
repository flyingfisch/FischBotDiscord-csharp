using System;
using System.Threading.Tasks;
using FischBot.Api.StatsDbClient;
using FischBot.Api.StatsDbClient.Dtos;
using FischBot.Api.StatsDbClient.Dtos.RainbowSix;
using FischBot.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FischBot.UnitTests.Services
{
    [TestClass]
    public class RainbowSixServiceTests
    {
        private Mock<IStatsDbClient> _statsDbClient;

        [TestInitialize]
        public void InitializeDependencies()
        {
            _statsDbClient = new Mock<IStatsDbClient>();
        }

        [TestMethod]
        public async Task GetRainbowSixUserStats_SearchUsersPayloadIsNull_ThrowsException()
        {
            // Arrange
            var userName = "someone";
            var rainbowSixService = BuildRainbowSixService();

            _statsDbClient
                .Setup(client => client.SearchRainbowSixUsers(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new StatsDbApiResponse<SearchUsersPayload>() { Payload = null });

            // Act and Assert
            await Assert.ThrowsExceptionAsync<Exception>(() => rainbowSixService.GetRainbowSixUserStats(userName));
        }

        [TestMethod]
        public async Task GetRainbowSixUserStats_SearchUsersCountIsZero_ThrowsException()
        {
            // Arrange
            var userName = "someone";
            var rainbowSixService = BuildRainbowSixService();

            _statsDbClient
                .Setup(client => client.SearchRainbowSixUsers(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new StatsDbApiResponse<SearchUsersPayload>() { Count = 0 });

            // Act and Assert
            await Assert.ThrowsExceptionAsync<Exception>(() => rainbowSixService.GetRainbowSixUserStats(userName));
        }

        [TestMethod]
        public async Task GetRainbowSixUserStats_UserExists_CallsGetUserStatsWithCorrectUserName()
        {
            // Arrange
            var userId = "anid";
            var userName = "someone";
            var rainbowSixService = BuildRainbowSixService();

            _statsDbClient
                .Setup(client => client.SearchRainbowSixUsers(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new StatsDbApiResponse<SearchUsersPayload>()
                {
                    Payload = new SearchUsersPayload()
                    {
                        Exists = true,
                        User = new User()
                        {
                            Id = userId
                        }
                    },
                    Count = 1,
                });

            _statsDbClient
                .Setup(client => client.GetRainbowSixUserStats(It.IsAny<string>()))
                .ReturnsAsync(new StatsDbApiResponse<GetUserStatsPayload>()
                {
                    Payload = new GetUserStatsPayload()
                    {
                        User = new User(),
                        Stats = new UserStatistics()
                        {
                            Casual = new CasualStatistics(),
                            Rank = new RankedStatistics(),
                            Unranked = new UnrankedStatistics(),
                            General = new GeneralStatistics()
                        }
                    },
                });

            // Act
            await rainbowSixService.GetRainbowSixUserStats(userName);

            // Assert
            _statsDbClient.Verify(client => client.GetRainbowSixUserStats(It.Is<string>(u => u == userId)), Times.Once);
        }

        private RainbowSixService BuildRainbowSixService()
        {
            return new RainbowSixService(_statsDbClient.Object);
        }
    }
}