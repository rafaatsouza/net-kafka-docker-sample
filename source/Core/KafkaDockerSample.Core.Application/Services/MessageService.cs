using KafkaDockerSample.Core.Domain.Exceptions;
using KafkaDockerSample.Core.Domain.Senders;
using KafkaDockerSample.Core.Domain.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace KafkaDockerSample.Core.Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly ILogger<MessageService> logger;
        private readonly IMessageSender messageSender;
        private readonly ApplicationConfiguration config;
        
        public MessageService(ILogger<MessageService> logger,
            IMessageSender messageSender, ApplicationConfiguration config)
        {
            this.logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
            this.messageSender = messageSender
                ?? throw new ArgumentNullException(nameof(messageSender));
            this.config = config
                ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task SendMessageAsync(
            string message, int maxRetry = 0)
        {
            var sendMessageResult = await messageSender
                .SendMessageAsync(config.MessageStreamerTopic, 
                    message, maxRetry);

            if (sendMessageResult == null)
                throw new MessageCustomException(
                    MessageCustomError.MessageNotSent());
            
            if (!sendMessageResult.Success)
                throw new MessageCustomException(
                    MessageCustomError.MessageNotSent(
                        sendMessageResult.ErrorMessage));

            logger.LogInformation("{Class} - {Method} - " + 
                "Message successfully sent: {@Result}",
                nameof(MessageService), nameof(SendMessageAsync), 
                sendMessageResult);
        }
    }
}
