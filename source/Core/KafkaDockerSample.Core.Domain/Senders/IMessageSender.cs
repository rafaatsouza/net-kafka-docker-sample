using System.Threading.Tasks;
using KafkaDockerSample.Core.Domain.Models;

namespace KafkaDockerSample.Core.Domain.Senders
{
    public interface IMessageSender
    {
        /// <summary>
        /// Sends new message
        /// </summary>
        /// <param name="topic">Nome of the topic</param>
        /// <param name="message">Text of the message</param>
        /// <param name="maxRetry">Max attempts</param>
        /// <returns>Result object</returns>
        Task<SendMessageResult> SendMessageAsync(string topic, 
            string message, int maxRetry = 0);   
    }
}