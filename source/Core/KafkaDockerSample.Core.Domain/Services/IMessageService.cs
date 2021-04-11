using System.Threading.Tasks;

namespace KafkaDockerSample.Core.Domain.Services
{
    public interface IMessageService
    {
        /// <summary>
        /// Sends new message
        /// </summary>
        /// <param name="topic">Nome of the topic</param>
        /// <param name="message">Text of the message</param>
        /// <param name="maxRetry">Max attempts</param>
        Task SendMessageAsync(string message, int maxRetry = 0);
    }
}