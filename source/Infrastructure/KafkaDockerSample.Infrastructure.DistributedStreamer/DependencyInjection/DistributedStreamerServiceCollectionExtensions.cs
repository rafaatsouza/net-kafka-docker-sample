using System;
using KafkaDockerSample.Core.Domain.Producers;
using KafkaDockerSample.Infrastructure.DistributedStreamer;
using KafkaDockerSample.Infrastructure.DistributedStreamer.Producers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DistributedStreamerServiceCollectionExtensions
    {
        public static IServiceCollection AddDistributedStreamer(
            this IServiceCollection services, DistributedStreamerConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            services.AddSingleton(configuration);
            services.AddScoped<IOccurrenceProducer, OccurrenceProducer>();

            return services;
        }
    }
}