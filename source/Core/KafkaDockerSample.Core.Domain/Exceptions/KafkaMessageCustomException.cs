using KafkaDockerSample.Core.Domain.Exceptions.Custom;
using System.Net;

namespace KafkaDockerSample.Core.Domain.Exceptions
{
    public class KafkaMessageCustomException : CustomException<KafkaMessageCustomError>
    {
        public KafkaMessageCustomException(KafkaMessageCustomError error) : base(error)
        { }
    }

    public class KafkaMessageCustomError : CustomError
    {
        public static KafkaMessageCustomError MessageContentNullOrEmpty =>
            new KafkaMessageCustomError(HttpStatusCode.BadRequest, 
                "The message content is null or empty");

        public static KafkaMessageCustomError MessageNotRetrieved(string errorMessage = null)
        {
            var message = $"The message could not be retrieved; error: {errorMessage}";

            if (!string.IsNullOrEmpty(errorMessage))
                message += $"; {errorMessage}";

            return new KafkaMessageCustomError(
                HttpStatusCode.InternalServerError, message);
        }            

        public static KafkaMessageCustomError MessageNotSent(string errorMessage = null)
        {
            var message = $"The message could not be sent; error: {errorMessage}";

            if (!string.IsNullOrEmpty(errorMessage))
                message += $"; {errorMessage}";

            return new KafkaMessageCustomError(
                HttpStatusCode.InternalServerError, message);
        }            
                        
        protected KafkaMessageCustomError(HttpStatusCode statusCode, string error) : base(statusCode, error)
        { }
    }
}