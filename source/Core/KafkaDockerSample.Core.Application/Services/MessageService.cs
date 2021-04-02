using KafkaDockerSample.Core.Domain.Exceptions;
using KafkaDockerSample.Core.Domain.Receivers;
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
        private readonly IMessageReceiver messageReceiver;

        public MessageService(ILogger<MessageService> logger,
            IMessageSender messageSender,
            IMessageReceiver messageReceiver)
        {
            this.logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
            this.messageSender = messageSender
                ?? throw new ArgumentNullException(nameof(messageSender));
            this.messageReceiver = messageReceiver
                ?? throw new ArgumentNullException(nameof(messageReceiver));
        }

        public async Task<string> GetLastMessageAsync()
        {
            var getMessageResult = await messageReceiver
                .GetLastMessageAsync();

            if (getMessageResult == null)
                throw new KafkaMessageCustomException(
                    KafkaMessageCustomError.MessageNotRetrieved());
            
            if (!getMessageResult.Success)
                throw new KafkaMessageCustomException(
                    KafkaMessageCustomError.MessageNotRetrieved(
                        getMessageResult.ErrorMessage));

            logger.LogInformation("{Class} - {Method} - " +
                "Message successfully retrieved: {@Result}",
                nameof(MessageService), nameof(GetLastMessageAsync), 
                getMessageResult);

            if (string.IsNullOrEmpty(getMessageResult.Message?.Content))
                throw new KafkaMessageCustomException(
                    KafkaMessageCustomError.MessageContentNullOrEmpty);

            return getMessageResult.Message.Content;
        }

        public async Task SendMessageAsync(string message)
        {
            var sendMessageResult = await messageSender
                .SendMessageAsync(message);

            if (sendMessageResult == null)
                throw new KafkaMessageCustomException(
                    KafkaMessageCustomError.MessageNotSent());
            
            if (!sendMessageResult.Success)
                throw new KafkaMessageCustomException(
                    KafkaMessageCustomError.MessageNotSent(
                        sendMessageResult.ErrorMessage));

            logger.LogInformation("{Class} - {Method} - " + 
                "Message successfully sent: {@Result}",
                nameof(MessageService), nameof(GetLastMessageAsync), 
                sendMessageResult);
        }
    }
}
