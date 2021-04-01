using KafkaDockerSample.Core.Domain.Services;
using System;
using System.Threading.Tasks;

namespace KafkaDockerSample.Core.Application.Services
{
    public class MessageSenderService : IMessageSenderService
    {
        public async Task SendMessageAsync(string message)
        {
            throw new NotImplementedException();
        }
    }
}
