using KafkaDockerSample.Core.Application.Services;
using KafkaDockerSample.Core.Domain.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(
            this IServiceCollection services)
        {
            services.AddScoped<IMessageSenderService , MessageSenderService>();

            return services;
        }
    }
}