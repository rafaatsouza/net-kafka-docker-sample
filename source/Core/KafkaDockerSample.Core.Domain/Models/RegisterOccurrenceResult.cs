using System;

namespace KafkaDockerSample.Core.Domain.Models
{
    public class RegisterOccurrenceResult
    {
        public long ElapsedTime { get; }

        public bool Success { get => string.IsNullOrEmpty(ErrorMessage); }

        public string ErrorMessage { get; } = null;

        public Guid OccurrenceId { get; }
        
        public RegisterOccurrenceResult(long elapsedTime, 
            Guid occurrenceId, string errorMessage = null)
        {
            OccurrenceId = occurrenceId;
            ErrorMessage = errorMessage;
            ElapsedTime = elapsedTime;
        }
    }
}