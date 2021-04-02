using KafkaDockerSample.Core.Domain.Receivers;
using KafkaDockerSample.Core.Domain.Senders;
using KafkaDockerSample.Infrastructure.MessageStreamer.Receivers;
using KafkaDockerSample.Infrastructure.MessageStreamer.Senders;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddMessageStreamer(
            this IServiceCollection services)
        {
            services.AddScoped<IMessageReceiver , MessageReceiver>();
            services.AddScoped<IMessageSender , MessageSender>();

            return services;
        }
    }
}