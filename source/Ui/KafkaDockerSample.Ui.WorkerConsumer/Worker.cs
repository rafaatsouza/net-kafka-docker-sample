using Confluent.Kafka;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Confluent.Kafka.Admin;

namespace KafkaDockerSample.Ui.WorkerConsumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly string server;
        private readonly string topic;

        private const string ConsumerGroupId = "WorkerConsumer";

        public Worker(ILogger<Worker> logger, 
            ConsumerConfiguration configuration)
        {
            this.logger = logger
                ?? throw new ArgumentNullException(nameof(logger));

            this.server = string.IsNullOrEmpty(configuration?.KafkaServer)
                ? throw new ArgumentNullException(nameof(configuration.KafkaServer))
                : configuration.KafkaServer;

            this.topic = string.IsNullOrEmpty(configuration?.KafkaTopic)
                ? throw new ArgumentNullException(nameof(configuration.KafkaTopic))
                : configuration.KafkaTopic;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            await CreatesTopic();

            var conf = new ConsumerConfig
            {
                GroupId = ConsumerGroupId,
                BootstrapServers = server,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                consumer.Subscribe(topic);
                
                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var message = consumer.Consume(stoppingToken);
                        
                        logger.LogInformation("Message received: {@message}", message);
                        Console.WriteLine($"Message received: '{message.Message.Value}'");
                    }
                }
                catch (OperationCanceledException)
                {
                    consumer.Close();
                }
            }
        }

        private async Task CreatesTopic()
        {
            var clientConfig = new AdminClientConfig() 
            { 
                BootstrapServers = server
            };

            var topicSpecifications = new TopicSpecification[] 
            { 
                new TopicSpecification()
                { 
                    Name = topic, 
                    ReplicationFactor = 1, 
                    NumPartitions = 1 
                } 
            };

            using (var adminClient = new AdminClientBuilder(clientConfig).Build())
            {
                try
                {
                    await adminClient.CreateTopicsAsync(topicSpecifications);
                }
                catch (CreateTopicsException e)
                {
                    logger.LogError(e, "Could not create topic - Error: {@exception}", e);
                }
            }
        }
    }
}
