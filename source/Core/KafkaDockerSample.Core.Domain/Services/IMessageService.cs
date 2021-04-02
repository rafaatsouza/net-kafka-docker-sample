using System.Threading.Tasks;

namespace KafkaDockerSample.Core.Domain.Services
{
    public interface IMessageService
    {
        /// <summary>
        /// Get the content from the last message from the stream
        /// </summary>
        /// <returns>Message's content</returns>
        Task<string> GetLastMessageAsync();

        /// <summary>
        /// Sends new message
        /// </summary>
        /// <param name="message">Text of the message</param>
        Task SendMessageAsync(string message);
    }
}