using System.Threading.Tasks;
using KafkaDockerSample.Core.Domain.Models;

namespace KafkaDockerSample.Core.Domain.Receivers
{
    public interface IMessageReceiver
    {
        /// <summary>
        /// Receives the last message from the stream
        /// </summary>
        /// <returns>Result object</returns>
        Task<GetMessageResult> GetLastMessageAsync();
    }
}