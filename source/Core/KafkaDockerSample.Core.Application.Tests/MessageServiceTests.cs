using KafkaDockerSample.Core.Application.Services;
using KafkaDockerSample.Core.Application.Tests.Util;
using KafkaDockerSample.Core.Domain.Exceptions;
using KafkaDockerSample.Core.Domain.Models;
using KafkaDockerSample.Core.Domain.Producers;
using KafkaDockerSample.Core.Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KafkaDockerSample.Core.Application.Tests
{
    public class OccurrenceServiceTests
    {
        private readonly IOccurrenceService occurrenceService;

        private readonly LoggerStub<OccurrenceService> stubLogger;
        private readonly Mock<IOccurrenceProducer> mockOccurrenceProducer;

        private const string Topic = "mytopic";

        public OccurrenceServiceTests()
        {
            stubLogger = new LoggerStub<OccurrenceService>();
            mockOccurrenceProducer = new Mock<IOccurrenceProducer>();

            occurrenceService = new OccurrenceService(
                stubLogger,
                mockOccurrenceProducer.Object,
                new ApplicationConfiguration()
                {
                    DistributedStreamerTopic = Topic
                }
            );
        }

        [Fact]
        [Trait(nameof(IOccurrenceService.RegisterOccurrenceAsync), "Error_DescriptionNull")]
        public async Task RegisterOccurrenceAsync_Error_DescriptionNull()
        {
            var expectedError = OccurrenceCustomError
                .OccurrenceDescriptionNullOrEmpty;

            var result = await Assert.ThrowsAsync<OccurrenceCustomException>(async () => 
            {
                await occurrenceService.RegisterOccurrenceAsync(null, DateTime.Now);
            });

            Assert.Equal(expectedError.Key, result.Key);
            Assert.Empty(stubLogger.logRegisters);
        }

        [Fact]
        [Trait(nameof(IOccurrenceService.RegisterOccurrenceAsync), "Error_DescriptionEmpty")]
        public async Task RegisterOccurrenceAsync_Error_DescriptionEmpty()
        {
            var expectedError = OccurrenceCustomError
                .OccurrenceDescriptionNullOrEmpty;

            var result = await Assert.ThrowsAsync<OccurrenceCustomException>(async () => 
            {
                await occurrenceService.RegisterOccurrenceAsync("", DateTime.Now);
            });

            Assert.Equal(expectedError.Key, result.Key);
            Assert.Empty(stubLogger.logRegisters);
        }

        [Fact]
        [Trait(nameof(IOccurrenceService.RegisterOccurrenceAsync), "Error_NullResult")]
        public async Task RegisterOccurrenceAsync_Error_NullResult()
        {
            var content = "test";
            var maxRetry = 3;
            var date = DateTime.Now;

            var expectedError = OccurrenceCustomError
                .OccurrenceNotRegistered();

            MockRegisterOccurrence(Topic, content, date, 
                maxRetry, (RegisterOccurrenceResult)null);

            var result = await Assert.ThrowsAsync<OccurrenceCustomException>(async () => 
            {
                await occurrenceService.RegisterOccurrenceAsync(
                    content, date, maxRetry);
            });

            Assert.Equal(expectedError.Key, result.Key);
            Assert.Empty(stubLogger.logRegisters);
        }

        [Fact]
        [Trait(nameof(IOccurrenceService.RegisterOccurrenceAsync), "Error_ResultNotSuccess")]
        public async Task RegisterOccurrenceAsync_Error_ResultNotSuccess()
        {
            var content = "test";
            var maxRetry = 3;
            var date = DateTime.Now;

            var expectedResult = new RegisterOccurrenceResult(
                100, Guid.NewGuid(), "Test");

            var expectedError = OccurrenceCustomError
                .OccurrenceNotRegistered(expectedResult.ErrorMessage);

            MockRegisterOccurrence(Topic, content, date, maxRetry, expectedResult);

            var result = await Assert.ThrowsAsync<OccurrenceCustomException>(async () => 
            {
                await occurrenceService.RegisterOccurrenceAsync(content, date, maxRetry);
            });

            Assert.Equal(expectedError.Key, result.Key);
            Assert.Empty(stubLogger.logRegisters);
        }

        [Fact]
        [Trait(nameof(IOccurrenceService.RegisterOccurrenceAsync), "Success")]
        public async Task RegisterOccurrenceAsync_Success()
        {
            var content = "test";
            var maxRetry = 3;
            var date = DateTime.Now;
            
            var expectedResult = new RegisterOccurrenceResult(100, Guid.NewGuid());

            MockRegisterOccurrence(Topic, content, 
                date, maxRetry, expectedResult);

            await occurrenceService.RegisterOccurrenceAsync(
                content, date, maxRetry);

            Assert.Single(stubLogger.logRegisters);
            Assert.Equal(LogLevel.Information, stubLogger.logRegisters.Single());
        }

        private void MockRegisterOccurrence(string topic, string content, 
            DateTime date, int maxRetry, RegisterOccurrenceResult result)
        {
            mockOccurrenceProducer
                .Setup(m => m.RegisterOccurrenceAsync(It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .Callback<string, string, DateTime, int>((topicCallback, 
                    contentCallback, dateCallback, maxRetryCallback) => 
                {
                    Assert.Equal(topic, topicCallback);
                    Assert.Equal(content, contentCallback);
                    Assert.Equal(date, dateCallback);
                    Assert.Equal(maxRetry, maxRetryCallback);
                })
                .ReturnsAsync(result);
        }
    }
}
