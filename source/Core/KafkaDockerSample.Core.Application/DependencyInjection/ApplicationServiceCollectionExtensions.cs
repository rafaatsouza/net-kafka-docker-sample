using System;
using KafkaDockerSample.Core.Application;
using KafkaDockerSample.Core.Application.Services;
using KafkaDockerSample.Core.Domain.Services;
using KafkaDockerSample.Infrastructure.MessageStreamer;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(
            this IServiceCollection services, ApplicationConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            var streamerConfig = new MessageStreamerConfiguration(
                config.MessageStreamerServer);

            services.AddMessageStreamer(streamerConfig);

            services.AddSingleton(config);
            services.AddScoped<IMessageService , MessageService>();

            return services;
        }
    }
}