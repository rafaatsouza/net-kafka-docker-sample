using KafkaDockerSample.Core.Domain.Exceptions.Custom;
using System.Net;

namespace KafkaDockerSample.Core.Domain.Exceptions
{
    public class MessageCustomException : CustomException<MessageCustomError>
    {
        public MessageCustomException(MessageCustomError error) : base(error)
        { }
    }

    public class MessageCustomError : CustomError
    {
        public static MessageCustomError MessageContentNullOrEmpty =>
            new MessageCustomError(HttpStatusCode.BadRequest, 
                nameof(MessageContentNullOrEmpty), "The message content is null or empty.");

        public static MessageCustomError MessageNotRetrieved(string errorMessage = null)
        {
            var message = $"The message could not be retrieved.";

            if (!string.IsNullOrEmpty(errorMessage))
                message += $" Error: {errorMessage}";

            return new MessageCustomError(HttpStatusCode.InternalServerError, 
                nameof(MessageNotRetrieved), message);
        }            

        public static MessageCustomError MessageNotSent(string errorMessage = null)
        {
            var message = $"The message could not be sent.";

            if (!string.IsNullOrEmpty(errorMessage))
                message += $" Error: {errorMessage}";

            return new MessageCustomError(HttpStatusCode.InternalServerError,
                nameof(MessageNotSent), message);
        }            
                        
        protected MessageCustomError(HttpStatusCode statusCode, 
            string key, string error) : base(statusCode, key, error)
        { }
    }
}