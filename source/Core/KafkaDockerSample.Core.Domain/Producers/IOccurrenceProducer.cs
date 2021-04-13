using System;
using System.Threading.Tasks;
using KafkaDockerSample.Core.Domain.Models;

namespace KafkaDockerSample.Core.Domain.Producers
{
    public interface IOccurrenceProducer
    {
        /// <summary>
        /// Register an Occurrence
        /// </summary>
        /// <param name="topic">Nome of the topic</param>
        /// <param name="description">Description of the occurrence</param>
        /// <param name="maxRetry">Max attempts</param>
        /// <returns>Result object</returns>
        Task<RegisterOccurrenceResult> RegisterOccurrenceAsync(string topic, 
            string description, DateTime date, int maxRetry = 0);   
    }
}