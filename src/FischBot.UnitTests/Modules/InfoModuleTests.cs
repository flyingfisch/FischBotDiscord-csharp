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
        private Mock<CommandService> _commands;
        private Mock<IConfiguration> _configuration;

        [TestInitialize]
        public void InitializeDependencies()
        {
            _moduleService = new Mock<IDiscordModuleService>();
            _commands = new Mock<CommandService>();
            _configuration = new Mock<IConfiguration>();
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
            _moduleService.Verify(moduleService => moduleService.ReplyAsync(It.IsAny<ICommandContext>(),
                                                                            It.Is<string>(m => m == echoMessage),
                                                                            It.IsAny<bool>(),
                                                                            It.IsAny<Embed>(),
                                                                            It.IsAny<RequestOptions>(),
                                                                            It.IsAny<AllowedMentions>(),
                                                                            It.IsAny<MessageReference>(),
                                                                            It.IsAny<MessageComponent>(),
                                                                            It.IsAny<ISticker[]>(),
                                                                            It.IsAny<Embed[]>(),
                                                                            It.IsAny<MessageFlags>()), Times.Once);
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
            _moduleService.Verify(moduleService => moduleService.ReplyAsync(It.IsAny<ICommandContext>(),
                                                                            It.Is<string>(m => m == blushEmoji),
                                                                            It.IsAny<bool>(),
                                                                            It.IsAny<Embed>(),
                                                                            It.IsAny<RequestOptions>(),
                                                                            It.IsAny<AllowedMentions>(),
                                                                            It.IsAny<MessageReference>(),
                                                                            It.IsAny<MessageComponent>(),
                                                                            It.IsAny<ISticker[]>(),
                                                                            It.IsAny<Embed[]>(),
                                                                            It.IsAny<MessageFlags>()), Times.Once);
        }

        private InfoModule BuildInfoModule()
        {
            return new InfoModule(_moduleService.Object, _commands.Object, _configuration.Object);
        }
    }
}