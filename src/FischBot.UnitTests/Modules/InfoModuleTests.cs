using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using FischBot.Modules;
using FischBot.Services.DiscordModuleService;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FischBot.UnitTests.Modules
{
    [TestClass]
    public class InfoModuleTests
    {
        private Mock<IDiscordModuleService> _moduleService;

        [TestInitialize]
        public void InitializeDependencies()
        {
            _moduleService = new Mock<IDiscordModuleService>();
        }

        [TestMethod]
        public async Task SayAsync_CallsReplyAsyncWithProvidedMessage()
        {
            // Arrange
            var echoMessage = "this is a message to echo";
            var infoModule = BuildInfoModule();

            // Act
            await infoModule.SayAsync(echoMessage);

            // Assert
            _moduleService.Verify(moduleService => moduleService.RespondAsync(It.IsAny<IInteractionContext>(),
                                                                              It.Is<string>(m => m == echoMessage),
                                                                              It.IsAny<Embed[]>(),
                                                                              It.IsAny<bool>(),
                                                                              It.IsAny<bool>(),
                                                                              It.IsAny<AllowedMentions>(),
                                                                              It.IsAny<RequestOptions>(),
                                                                              It.IsAny<MessageComponent>(),
                                                                              It.IsAny<Embed>()), Times.Once);
        }

        [TestMethod]
        public async Task ReceivePraiseAsync_CallsReplyAsyncWithBlushEmoji()
        {
            // Arrange
            var blushEmoji = "😊";
            var infoModule = BuildInfoModule();

            // Act
            await infoModule.ReceivePraiseAsync();

            // Assert
            _moduleService.Verify(moduleService => moduleService.RespondAsync(It.IsAny<IInteractionContext>(),
                                                                              It.Is<string>(m => m == blushEmoji),
                                                                              It.IsAny<Embed[]>(),
                                                                              It.IsAny<bool>(),
                                                                              It.IsAny<bool>(),
                                                                              It.IsAny<AllowedMentions>(),
                                                                              It.IsAny<RequestOptions>(),
                                                                              It.IsAny<MessageComponent>(),
                                                                              It.IsAny<Embed>()), Times.Once);
        }

        private InfoModule BuildInfoModule()
        {
            return new InfoModule(_moduleService.Object);
        }
    }
}