using System.Net;

namespace KafkaDockerSample.Core.Domain.Exceptions.Custom
{
    public abstract class CustomError
    {
        public HttpStatusCode StatusCode { get; set; }

        public string Key { get; }

        public string Message { get; }

        public CustomError(HttpStatusCode statusCode, 
            string key, string error)
        {
            StatusCode = statusCode;
            Key = key;
            Message = error;
        }
    }
}