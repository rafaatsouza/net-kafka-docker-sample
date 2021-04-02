namespace KafkaDockerSample.Core.Domain.Models
{
    public class GetMessageResult
    {
        public Message Message { get; }

        public long ElapsedTime { get; }

        public bool Success => Message != null;

        public string ErrorMessage { get; } = null;

        public GetMessageResult(Message message, long elapsedTime)
        {
            Message = message;
            ElapsedTime = elapsedTime;
        }

        public GetMessageResult(string errorMessage, long elapsedTime)
        {
            Message = null;
            ErrorMessage = errorMessage;
            ElapsedTime = elapsedTime;
        }
    }
}