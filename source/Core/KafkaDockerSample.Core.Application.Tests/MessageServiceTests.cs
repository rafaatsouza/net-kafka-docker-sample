using KafkaDockerSample.Core.Application.Services;
using KafkaDockerSample.Core.Domain.Exceptions;
using KafkaDockerSample.Core.Domain.Models;
using KafkaDockerSample.Core.Domain.Senders;
using KafkaDockerSample.Core.Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KafkaDockerSample.Core.Application.Tests
{
    public class MessageServiceTests
    {
        private readonly IMessageService messageService;

        private readonly LoggerStub<MessageService> stubLogger;
        private readonly Mock<IMessageSender> mockMessageSender;

        private const string Topic = "mytopic";

        public MessageServiceTests()
        {
            stubLogger = new LoggerStub<MessageService>();
            mockMessageSender = new Mock<IMessageSender>();

            messageService = new MessageService(
                stubLogger,
                mockMessageSender.Object,
                new ApplicationConfiguration()
                {
                    MessageStreamerTopic = Topic
                }
            );
        }

        [Fact]
        [Trait(nameof(IMessageService.SendMessageAsync), "Error_NullResult")]
        public async Task SendMessageAsync_Error_NullResult()
        {
            var content = "test";
            var maxRetry = 3;

            var expectedError = MessageCustomError
                .MessageNotSent();

            MockSendMessage(Topic, content, maxRetry, (SendMessageResult)null);

            var result = await Assert.ThrowsAsync<MessageCustomException>(async () => 
            {
                await messageService.SendMessageAsync(content, maxRetry);
            });

            Assert.Equal(expectedError.Key, result.Key);
            Assert.Empty(stubLogger.logRegisters);
        }

        [Fact]
        [Trait(nameof(IMessageService.SendMessageAsync), "Error_ResultNotSuccess")]
        public async Task SendMessageAsync_Error_ResultNotSuccess()
        {
            var content = "test";
            var maxRetry = 3;

            var expectedResult = new SendMessageResult(100, "Test");

            var expectedError = MessageCustomError
                .MessageNotSent(expectedResult.ErrorMessage);

            MockSendMessage(Topic, content, maxRetry, expectedResult);

            var result = await Assert.ThrowsAsync<MessageCustomException>(async () => 
            {
                await messageService.SendMessageAsync(content, maxRetry);
            });

            Assert.Equal(expectedError.Key, result.Key);
            Assert.Empty(stubLogger.logRegisters);
        }

        [Fact]
        [Trait(nameof(IMessageService.SendMessageAsync), "Success")]
        public async Task SendMessageAsync_Success()
        {
            var content = "test";
            var maxRetry = 3;
            
            var expectedResult = new SendMessageResult(100);

            MockSendMessage(Topic, content, maxRetry, expectedResult);

            await messageService.SendMessageAsync(content, maxRetry);

            Assert.Single(stubLogger.logRegisters);
            Assert.Equal(LogLevel.Information, stubLogger.logRegisters.Single());
        }

        private void MockSendMessage(string topic, string content, 
            int maxRetry, SendMessageResult result)
        {
            mockMessageSender
                .Setup(m => m.SendMessageAsync(It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<int>()))
                .Callback<string, string, int>((topicCallback, 
                    contentCallback, maxRetryCallback) => 
                {
                    Assert.Equal(topic, topicCallback);
                    Assert.Equal(content, contentCallback);
                    Assert.Equal(maxRetry, maxRetryCallback);
                })
                .ReturnsAsync(result);
        }
    }
}
