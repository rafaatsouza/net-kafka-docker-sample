using Confluent.Kafka;
using Confluent.Kafka.Admin;
using KafkaDockerSample.Core.Domain.Models;
using KafkaDockerSample.Core.Domain.Producers;
using KafkaDockerSample.Infrastructure.DistributedStreamer.Exceptions;
using KafkaDockerSample.Infrastructure.DistributedStreamer.Serializers;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace KafkaDockerSample.Infrastructure.DistributedStreamer.Producers
{
    public class OccurrenceProducer : IOccurrenceProducer
    {
        private readonly ILogger<OccurrenceProducer> logger;
        private readonly ProducerConfig producerConfig;
        private readonly AdminClientConfig adminClientConfig;

        public OccurrenceProducer(ILogger<OccurrenceProducer> logger, 
            DistributedStreamerConfiguration config)
        {
            this.logger = logger 
                ?? throw new ArgumentNullException(nameof(logger));
                
            if (string.IsNullOrEmpty(config?.Server))
                throw new ArgumentNullException(nameof(config.Server));

            producerConfig = new ProducerConfig() 
            { 
                BootstrapServers = config.Server
            };

            adminClientConfig = new AdminClientConfig() 
            { 
                BootstrapServers = config.Server
            };
        }

        public async Task<RegisterOccurrenceResult> RegisterOccurrenceAsync(
            string topic, string description, DateTime date, int maxRetry = 0)
        {
            await CreatesTopicAsync(topic);

            var occurrence = new Occurrence(description, date);
            var errorMessage = (string)null;
            var timer = new Stopwatch();

            timer.Start();

            try
            {
                var result = await RegisterOccurrenceWithRetryAsync(
                    topic, occurrence, maxRetry);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            finally
            {
                timer.Stop();
            }

            return new RegisterOccurrenceResult(
                timer.ElapsedMilliseconds, occurrence.Id, errorMessage);
        }

        private async Task<DeliveryResult<String, Occurrence>> RegisterOccurrenceWithRetryAsync(
            string topic, Occurrence occurrence, int maxRetry = 0)
        {
            var kafkaMessage = new Message<String, Occurrence> 
            { 
                Key = occurrence.Id.ToString(),
                Value = occurrence
            };

            var attemptCount = 0;

            while (attemptCount == 0 || attemptCount < maxRetry)
            {
                var producerBuild = new ProducerBuilder<String, Occurrence>(producerConfig)
                    .SetValueSerializer(new OccurrenceSerializer());

                using (var producer = producerBuild.Build())
                {
                    try
                    {
                        var deliveryResult = await producer
                            .ProduceAsync(topic, kafkaMessage);

                        if (deliveryResult.Status == PersistenceStatus.NotPersisted)
                        {
                            logger.LogWarning("Occurrence not registered: {@deliveryResult}", 
                                deliveryResult);

                            attemptCount++;
                        }
                        else
                        {
                            return deliveryResult;
                        }
                    }
                    catch (ProduceException<String, Occurrence> ex)
                    {               
                        throw new OccurrenceProducerCustomException(
                            OccurrenceProducerCustomError.KafkaError(ex.Error));
                    }
                }
            }

            throw new OccurrenceProducerCustomException(
                OccurrenceProducerCustomError.MaxAttemptsExceeded);
        }

        private async Task CreatesTopicAsync(string topic)
        {
            var topicSpecifications = new TopicSpecification[] 
            { 
                new TopicSpecification()
                { 
                    Name = topic, 
                    ReplicationFactor = 1, 
                    NumPartitions = 1 
                } 
            };

            using (var adminClient = new AdminClientBuilder(adminClientConfig).Build())
            {
                try
                {
                    await adminClient.CreateTopicsAsync(topicSpecifications);
                }
                catch (CreateTopicsException ex)
                {
                    var topicAlreadyExists = ex?.Message.Contains(
                        $"Topic '{topic}' already exists.", StringComparison.OrdinalIgnoreCase);

                    if (!(topicAlreadyExists ?? false))
                    {
                        logger.LogError(ex, "Could not create topic - Error: {@exception}", ex);
                        throw;
                    }
                }
            }
        }
    }
}