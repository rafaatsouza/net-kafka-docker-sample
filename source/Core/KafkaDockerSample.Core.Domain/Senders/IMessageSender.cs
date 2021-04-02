using KafkaDockerSample.Core.Domain.Models;
using System.Threading.Tasks;

namespace KafkaDockerSample.Core.Domain.Senders
{
    public interface IMessageSender
    {
        /// <summary>
        /// Sends new message
        /// </summary>
        /// <param name="message">Text of the message</param>
        /// <returns>Result object</returns>
        Task<SendMessageResult> SendMessageAsync(string message);
    }
}