using System.Threading.Tasks;
using KafkaDockerSample.Core.Domain.Models;
using KafkaDockerSample.Core.Domain.Senders;

namespace KafkaDockerSample.Infrastructure.MessageStreamer.Senders
{
    public class MessageSender : IMessageSender
    {
        public Task<SendMessageResult> SendMessageAsync(string message)
        {
            throw new System.NotImplementedException();
        }
    }
}