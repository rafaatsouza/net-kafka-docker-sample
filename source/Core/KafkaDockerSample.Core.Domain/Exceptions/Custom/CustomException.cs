using System;
using System.Net;

namespace KafkaDockerSample.Core.Domain.Exceptions.Custom
{
    public abstract class CustomException : Exception
    {
        protected CustomException(string message) : base(message)
        { }

        public abstract HttpStatusCode StatusCode { get; protected set; }

        public abstract string Key { get; protected set; }
    }

    public abstract class CustomException<T> : CustomException where T : CustomError
    {
        public override HttpStatusCode StatusCode { get; protected set; }

        public override string Key { get; protected set; }

        protected CustomException(CustomError error) : base(error.Message)
        {
            StatusCode = error.StatusCode;
            Key = error.Key;
        }
    }
}