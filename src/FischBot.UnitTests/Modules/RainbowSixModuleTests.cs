using System.Threading.Tasks;
using FischBot.Models;
using FischBot.Modules;
using FischBot.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FischBot.UnitTests.Modules
{
    [TestClass]
    public class RainbowSixModuleTests
    {
        private Mock<IDiscordModuleService> _moduleService;
        private Mock<IRainbowSixService> _rainbowSixService;

        [TestInitialize]
        public void InitializeDependencies()
        {
            _moduleService = new Mock<IDiscordModuleService>();
            _rainbowSixService = new Mock<IRainbowSixService>();
        }

        [TestMethod]
        public async Task GetSiegeStatsAsync_CallsGetRainbowSixUserStatsWithCorrectUserName()
        {
            // Arrange
            var userName = "test";
            var statsType = RainbowSixStatsType.Casual;
            var rainbowSixModule = BuildRainbowSixModule();

            _rainbowSixService
                .Setup(client => client.GetRainbowSixUserStats(It.IsAny<string>()))
                .ReturnsAsync(new RainbowSixUserStats());

            // Act
            await rainbowSixModule.GetSiegeStatsAsync(statsType, userName);

            // Assert
            _rainbowSixService.Verify(
                client => client.GetRainbowSixUserStats(It.Is<string>(u => u == userName)),
                Times.Once());
        }

        private RainbowSixModule BuildRainbowSixModule()
        {
            return new RainbowSixModule(_moduleService.Object, _rainbowSixService.Object);
        }
    }
}