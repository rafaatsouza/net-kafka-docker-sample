using Confluent.Kafka;
using KafkaDockerSample.Core.Domain.Models;
using KafkaDockerSample.Core.Domain.Senders;
using KafkaDockerSample.Infrastructure.MessageStreamer.Senders.Base;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace KafkaDockerSample.Infrastructure.MessageStreamer.Senders
{
    public class MessageSender 
        : KafkaStreamManager<Null, string>, IMessageSender
    {
        public MessageSender(ILogger<MessageSender> logger, 
            MessageStreamerConfiguration config) : base(logger, config.Server)
        { }

        public Task<SendMessageResult> SendMessageAsync(
            string topic, string message, int maxRetry = 0)
            => this.SendMessageWithRetryAsync(topic, null, message, maxRetry);
    }
}