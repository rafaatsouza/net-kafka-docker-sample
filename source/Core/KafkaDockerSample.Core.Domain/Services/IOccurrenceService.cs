using System;
using System.Threading.Tasks;

namespace KafkaDockerSample.Core.Domain.Services
{
    public interface IOccurrenceService
    {
        /// <summary>
        /// Registers an Event
        /// </summary>
        /// <param name="topic">Nome of the topic</param>
        /// <param name="description">Description of the occurrence</param>
        /// <param name="maxRetry">Max attempts</param>
        Task RegisterOccurrenceAsync(string description, 
            DateTime date, int maxRetry = 0);
    }
}