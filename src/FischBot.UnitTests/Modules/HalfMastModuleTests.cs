using System;
using System.Threading.Tasks;
using FischBot.Api;
using FischBot.Models;
using FischBot.Modules;
using FischBot.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FischBot.UnitTests.Modules
{
    [TestClass]
    public class HalfMastModuleTests
    {
        private Mock<IDiscordModuleService> _moduleService;
        private Mock<IStarsAndStripesDailyClient> _starsAndStripesDailyClient;

        [TestInitialize]
        public void InitializeDependencies()
        {
            _moduleService = new Mock<IDiscordModuleService>();
            _starsAndStripesDailyClient = new Mock<IStarsAndStripesDailyClient>();
        }

        [TestMethod]
        public async Task GetUsFlagHalfMastInfoAsync_CallsStarsAndStripesDailyClientWithCorrectDate()
        {
            // Arrange
            var date = DateTime.Today;
            var halfMastModule = BuildHalfMastModule();

            _starsAndStripesDailyClient
                .Setup(client => client.GetUsFlagHalfMastInfo(It.IsAny<DateTime>()))
                .Returns(new UsFlagHalfMastInfo()
                {
                    Title = "Test Title",
                    Description = "Test Description",
                    FlagIsHalfMast = false
                });

            // Act
            await halfMastModule.DisplayHalfMastInformationAsync(date);

            // Assert
            _starsAndStripesDailyClient.Verify(
                client => client.GetUsFlagHalfMastInfo(It.Is<DateTime>(dateTime => dateTime == date)),
                Times.Once());
        }

        private HalfMastModule BuildHalfMastModule()
        {
            return new HalfMastModule(_moduleService.Object, _starsAndStripesDailyClient.Object);
        }
    }
}