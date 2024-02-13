using KafkaDockerSample.Core.Domain.Exceptions;
using KafkaDockerSample.Core.Domain.Producers;
using KafkaDockerSample.Core.Domain.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace KafkaDockerSample.Core.Application.Services
{
    public class OccurrenceService : IOccurrenceService
    {
        private readonly ILogger<OccurrenceService> logger;
        private readonly IOccurrenceProducer occurrenceProducer;
        private readonly ApplicationConfiguration config;
        
        public OccurrenceService(ILogger<OccurrenceService> logger,
            IOccurrenceProducer occurrenceProducer, ApplicationConfiguration config)
        {
            this.logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
            this.occurrenceProducer = occurrenceProducer
                ?? throw new ArgumentNullException(nameof(occurrenceProducer));
            this.config = config
                ?? throw new ArgumentNullException(nameof(config));
        }

        public Task RegisterOccurrenceAsync(string description, 
            DateTime date, int maxRetry = 0)
        {
            if (string.IsNullOrEmpty(description))
                throw new OccurrenceCustomException(
                    OccurrenceCustomError.OccurrenceDescriptionNullOrEmpty);

            return ExecuteRegisterOccurrenceAsync(
                description, date, maxRetry);
        }
        
        private async Task ExecuteRegisterOccurrenceAsync(
            string description, DateTime date, int maxRetry = 0)
        {
            var registerOccurrenceResult = await occurrenceProducer
                .RegisterOccurrenceAsync(config.DistributedStreamerTopic, 
                    description, date, maxRetry);

            if (registerOccurrenceResult == null)
                throw new OccurrenceCustomException(
                    OccurrenceCustomError.OccurrenceNotRegistered());
            
            if (!registerOccurrenceResult.Success)
                throw new OccurrenceCustomException(
                    OccurrenceCustomError.OccurrenceNotRegistered(
                        registerOccurrenceResult.ErrorMessage));

            logger.LogInformation("{Class} - {Method} - " + 
                "Occurrence successfully registered: {@Result}",
                nameof(OccurrenceService), nameof(RegisterOccurrenceAsync), 
                registerOccurrenceResult);
        }
    }
}
