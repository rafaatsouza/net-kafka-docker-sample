using System.Threading.Tasks;

namespace KafkaDockerSample.Core.Domain.Services
{
    public interface IMessageSenderService
    {
        /// <summary>
        /// Sends new message
        /// </summary>
        /// <param name="message">Text of the message</param>
        Task SendMessageAsync(string message);
    }
}