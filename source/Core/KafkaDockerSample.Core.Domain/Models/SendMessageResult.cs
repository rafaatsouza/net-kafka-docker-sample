namespace KafkaDockerSample.Core.Domain.Models
{
    public class SendMessageResult
    {
        public long ElapsedTime { get; }

        public bool Success { get => string.IsNullOrEmpty(ErrorMessage); }

        public string ErrorMessage { get; } = null;
        
        public SendMessageResult(long elapsedTime, 
            string errorMessage = null)
        {
            ErrorMessage = errorMessage;
            ElapsedTime = elapsedTime;
        }
    }
}