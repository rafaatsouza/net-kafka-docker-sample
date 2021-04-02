using KafkaDockerSample.Core.Application.Services;
using KafkaDockerSample.Core.Domain.Exceptions;
using KafkaDockerSample.Core.Domain.Models;
using KafkaDockerSample.Core.Domain.Receivers;
using KafkaDockerSample.Core.Domain.Senders;
using KafkaDockerSample.Core.Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
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
        private readonly Mock<IMessageReceiver> mockMessageReceiver;

        public MessageServiceTests()
        {
            stubLogger = new LoggerStub<MessageService>();
            mockMessageSender = new Mock<IMessageSender>();
            mockMessageReceiver = new Mock<IMessageReceiver>();

            messageService = new MessageService(
                stubLogger,
                mockMessageSender.Object,
                mockMessageReceiver.Object
            );
        }

        [Fact]
        [Trait(nameof(IMessageService.GetLastMessageAsync), "Error_NullResult")]
        public async Task GetLastMessageAsync_Error_NullResult()
        {
            var expectedError = KafkaMessageCustomError
                .MessageNotRetrieved();

            mockMessageReceiver
                .Setup(m => m.GetLastMessageAsync())
                .ReturnsAsync((GetMessageResult)null);

            var result = await Assert.ThrowsAsync<KafkaMessageCustomException>(async () => 
            {
                _ = await messageService.GetLastMessageAsync();
            });

            Assert.Equal(expectedError.Key, result.Key);
            Assert.Empty(stubLogger.logRegisters);
        }

        [Fact]
        [Trait(nameof(IMessageService.GetLastMessageAsync), "Error_ResultNotSuccess")]
        public async Task GetLastMessageAsync_Error_ResultNotSuccess()
        {
            var expectedResult = new GetMessageResult("Test", 100);

            var expectedError = KafkaMessageCustomError
                .MessageNotRetrieved(expectedResult.ErrorMessage);

            mockMessageReceiver
                .Setup(m => m.GetLastMessageAsync())
                .ReturnsAsync(expectedResult);

            var result = await Assert.ThrowsAsync<KafkaMessageCustomException>(async () => 
            {
                _ = await messageService.GetLastMessageAsync();
            });

            Assert.Equal(expectedError.Key, result.Key);
            Assert.Empty(stubLogger.logRegisters);
        }

        [Fact]
        [Trait(nameof(IMessageService.GetLastMessageAsync), "Success")]
        public async Task GetLastMessageAsync_Success()
        {
            var expectedMessage = new Message()
            {
                Content = "Test"
            };

            var expectedResult = new GetMessageResult(
                expectedMessage, 100);

            mockMessageReceiver
                .Setup(m => m.GetLastMessageAsync())
                .ReturnsAsync(expectedResult);

            var result = await messageService.GetLastMessageAsync();

            Assert.Equal(expectedMessage.Content, result);
            Assert.Single(stubLogger.logRegisters);
            Assert.Equal(LogLevel.Information, stubLogger.logRegisters.Single());
        }

        [Fact]
        [Trait(nameof(IMessageService.SendMessageAsync), "Error_NullResult")]
        public async Task SendMessageAsync_Error_NullResult()
        {
            var content = "test";

            var expectedError = KafkaMessageCustomError
                .MessageNotSent();

            mockMessageSender
                .Setup(m => m.SendMessageAsync(It.IsAny<string>()))
                .Callback<string>(contentCallback => 
                {
                    Assert.Equal(content, contentCallback);
                })
                .ReturnsAsync((SendMessageResult)null);

            var result = await Assert.ThrowsAsync<KafkaMessageCustomException>(async () => 
            {
                await messageService.SendMessageAsync(content);
            });

            Assert.Equal(expectedError.Key, result.Key);
            Assert.Empty(stubLogger.logRegisters);
        }

        [Fact]
        [Trait(nameof(IMessageService.SendMessageAsync), "Error_ResultNotSuccess")]
        public async Task SendMessageAsync_Error_ResultNotSuccess()
        {
            var content = "test";
            
            var expectedResult = new SendMessageResult("Test", 100);

            var expectedError = KafkaMessageCustomError
                .MessageNotSent(expectedResult.ErrorMessage);

            mockMessageSender
                .Setup(m => m.SendMessageAsync(It.IsAny<string>()))
                .Callback<string>(contentCallback => 
                {
                    Assert.Equal(content, contentCallback);
                })
                .ReturnsAsync(expectedResult);

            var result = await Assert.ThrowsAsync<KafkaMessageCustomException>(async () => 
            {
                await messageService.SendMessageAsync(content);
            });

            Assert.Equal(expectedError.Key, result.Key);
            Assert.Empty(stubLogger.logRegisters);
        }

        [Fact]
        [Trait(nameof(IMessageService.SendMessageAsync), "Success")]
        public async Task SendMessageAsync_Success()
        {
            var content = "test";
            
            var expectedResult = new SendMessageResult(100);

            mockMessageSender
                .Setup(m => m.SendMessageAsync(It.IsAny<string>()))
                .Callback<string>(contentCallback => 
                {
                    Assert.Equal(content, contentCallback);
                })
                .ReturnsAsync(expectedResult);
            
            await messageService.SendMessageAsync(content);

            Assert.Single(stubLogger.logRegisters);
            Assert.Equal(LogLevel.Information, stubLogger.logRegisters.Single());
        }
    }
}
