using Confluent.Kafka;
using KafkaDockerSample.Core.Domain.Exceptions.Custom;
using System.Net;

namespace KafkaDockerSample.Infrastructure.MessageStreamer.Exceptions
{
    internal class MessageSenderCustomException : CustomException<MessageSenderCustomError>
    {
        public MessageSenderCustomException(MessageSenderCustomError error) : base(error)
        { }
    }

    internal class MessageSenderCustomError : CustomError
    {
        public static MessageSenderCustomError MaxAttemptsExceeded
            => new MessageSenderCustomError(HttpStatusCode.BadGateway,
                nameof(MaxAttemptsExceeded), "None of the attempts were successful.");

        public static MessageSenderCustomError KafkaError(Error kafkaError)
            => new MessageSenderCustomError(HttpStatusCode.InternalServerError, nameof(MaxAttemptsExceeded), 
                $"Message could not be sent. Error {kafkaError.Code} - {kafkaError.Reason}");

        protected MessageSenderCustomError(HttpStatusCode statusCode, 
            string key, string error) : base(statusCode, key, error)
        { }
    }
}