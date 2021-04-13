using Confluent.Kafka;
using KafkaDockerSample.Core.Domain.Exceptions.Custom;
using System.Net;

namespace KafkaDockerSample.Infrastructure.DistributedStreamer.Exceptions
{
    internal class OccurrenceProducerCustomException : CustomException<OccurrenceProducerCustomError>
    {
        public OccurrenceProducerCustomException(OccurrenceProducerCustomError error) : base(error)
        { }
    }

    internal class OccurrenceProducerCustomError : CustomError
    {
        public static OccurrenceProducerCustomError MaxAttemptsExceeded
            => new OccurrenceProducerCustomError(HttpStatusCode.BadGateway,
                nameof(MaxAttemptsExceeded), "None of the attempts were successful.");

        public static OccurrenceProducerCustomError KafkaError(Error kafkaError)
            => new OccurrenceProducerCustomError(HttpStatusCode.InternalServerError, nameof(MaxAttemptsExceeded), 
                $"Occurrence could not be registered. Error {kafkaError.Code} - {kafkaError.Reason}");

        protected OccurrenceProducerCustomError(HttpStatusCode statusCode, 
            string key, string error) : base(statusCode, key, error)
        { }
    }
}