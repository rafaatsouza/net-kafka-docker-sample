using System;
using KafkaDockerSample.Core.Domain.Senders;
using KafkaDockerSample.Infrastructure.MessageStreamer;
using KafkaDockerSample.Infrastructure.MessageStreamer.Senders;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddMessageStreamer(
            this IServiceCollection services, MessageStreamerConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            services.AddSingleton(configuration);
            services.AddScoped<IMessageSender, MessageSender>();

            return services;
        }
    }
}