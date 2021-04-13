using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;

namespace KafkaDockerSample.Core.Application.Tests.Util
{
    public class LoggerStub<T> : ILogger<T>
    {
         private readonly ILogger<T> logger;

         public IList<LogLevel> logRegisters { get; private set; } 
            = new List<LogLevel>();

        public LoggerStub()
        {
            this.logger = new Mock<ILogger<T>>().Object;
        }

        public IDisposable BeginScope<TState>(TState state)
            => logger.BeginScope(state);

        public bool IsEnabled(LogLevel logLevel)
            => logger.IsEnabled(logLevel);

        public void Log<TState>(LogLevel logLevel, EventId eventId,
            TState state, Exception exception, Func<TState, Exception, string> formatter)
            => logRegisters.Add(logLevel);
    } 
}
