using System.Threading.Tasks;
using KafkaDockerSample.Core.Domain.Models;
using KafkaDockerSample.Core.Domain.Receivers;

namespace KafkaDockerSample.Infrastructure.MessageStreamer.Receivers
{
    public class MessageReceiver : IMessageReceiver
    {
        public Task<GetMessageResult> GetLastMessageAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}