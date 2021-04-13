using KafkaDockerSample.Core.Domain.Exceptions.Custom;
using System.Net;

namespace KafkaDockerSample.Core.Domain.Exceptions
{
    public class OccurrenceCustomException : CustomException<OccurrenceCustomError>
    {
        public OccurrenceCustomException(OccurrenceCustomError error) : base(error)
        { }
    }

    public class OccurrenceCustomError : CustomError
    {
        public static OccurrenceCustomError OccurrenceDescriptionNullOrEmpty =>
            new OccurrenceCustomError(HttpStatusCode.BadRequest, 
                nameof(OccurrenceDescriptionNullOrEmpty), "The occurrence description is null or empty.");

        public static OccurrenceCustomError OccurrenceNotRegistered(string errorMessage = null)
        {
            var message = $"The occurrence could not be registered.";

            if (!string.IsNullOrEmpty(errorMessage))
                message += $" Error: {errorMessage}";

            return new OccurrenceCustomError(HttpStatusCode.InternalServerError,
                nameof(OccurrenceNotRegistered), message);
        }            

        protected OccurrenceCustomError(HttpStatusCode statusCode, 
            string key, string error) : base(statusCode, key, error)
        { }
    }
}