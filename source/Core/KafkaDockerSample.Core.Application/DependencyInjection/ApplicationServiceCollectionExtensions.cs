using System;
using KafkaDockerSample.Core.Application;
using KafkaDockerSample.Core.Application.Services;
using KafkaDockerSample.Core.Domain.Services;
using KafkaDockerSample.Infrastructure.DistributedStreamer;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(
            this IServiceCollection services, ApplicationConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            var streamerConfig = new DistributedStreamerConfiguration(
                config.DistributedStreamerServer);

            services.AddDistributedStreamer(streamerConfig);

            services.AddSingleton(config);
            services.AddScoped<IOccurrenceService, OccurrenceService>();

            return services;
        }
    }
}