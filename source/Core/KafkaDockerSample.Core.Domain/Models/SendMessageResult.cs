namespace KafkaDockerSample.Core.Domain.Models
{
    public class SendMessageResult
    {
        public long ElapsedTime { get; }

        public bool Success { get; }

        public string ErrorMessage { get; } = null;

        public SendMessageResult(long elapsedTime)
        {
            Success = true;
            ElapsedTime = elapsedTime;
        }

        public SendMessageResult(string errorMessage, long elapsedTime)
        {
            Success = false;
            ErrorMessage = errorMessage;
            ElapsedTime = elapsedTime;
        }
    }
}