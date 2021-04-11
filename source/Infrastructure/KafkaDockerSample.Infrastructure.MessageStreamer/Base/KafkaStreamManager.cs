using Confluent.Kafka;
using Confluent.Kafka.Admin;
using KafkaDockerSample.Core.Domain.Models;
using KafkaDockerSample.Infrastructure.MessageStreamer.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace KafkaDockerSample.Infrastructure.MessageStreamer.Senders.Base
{
    public abstract class KafkaStreamManager<TKey, TValue>
    {
        private readonly ILogger logger;
        private readonly ProducerConfig producerConfig;
        private readonly AdminClientConfig adminClientConfig;

        public KafkaStreamManager(ILogger logger, string server)
        {
            this.logger = logger 
                ?? throw new ArgumentNullException(nameof(logger));
                
            if (string.IsNullOrEmpty(server))
                throw new ArgumentNullException(server);

            producerConfig = new ProducerConfig() 
            { 
                BootstrapServers = server
            };

            adminClientConfig = new AdminClientConfig() 
            { 
                BootstrapServers = server
            };
        }

        protected async Task<SendMessageResult> SendMessageWithRetryAsync(
            string topic, TKey key, TValue value, int maxRetry = 0)
        {
            await CreatesTopicAsync(topic);
            
            var errorMessage = (string)null;
            var timer = new Stopwatch();

            timer.Start();

            try
            {
                _ = await ProduceMessageAsync(
                    topic, key, value, maxRetry);
            }
            catch (MessageSenderCustomException ex)
            {
                errorMessage = ex.Message;
            }
            finally
            {
                timer.Stop();
            }

            return new SendMessageResult(
                timer.ElapsedMilliseconds, errorMessage);
        }

        private async Task<DeliveryResult<TKey, TValue>> ProduceMessageAsync(
            string topic, TKey key, TValue value, int maxRetry)
        {
            var kafkaMessage = new Message<TKey, TValue> 
            { 
                Key = key,
                Value = value 
            };

            var attemptCount = 0;

            while (attemptCount == 0 || attemptCount < maxRetry)
            {
                var producerBuild = new ProducerBuilder<TKey, TValue>(producerConfig);

                using (var producer = producerBuild.Build())
                {
                    try
                    {
                        var deliveryResult = await producer
                            .ProduceAsync(topic, kafkaMessage);

                        if (deliveryResult.Status == PersistenceStatus.NotPersisted)
                        {
                            logger.LogWarning("Message not set: {@deliveryResult}", 
                                deliveryResult);

                            attemptCount++;
                        }
                        else
                        {
                            return deliveryResult;
                        }
                    }
                    catch (ProduceException<TKey, TValue> ex)
                    {               
                        throw new MessageSenderCustomException(
                            MessageSenderCustomError.KafkaError(ex.Error));
                    }
                }
            }

            throw new MessageSenderCustomException(
                MessageSenderCustomError.MaxAttemptsExceeded);
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