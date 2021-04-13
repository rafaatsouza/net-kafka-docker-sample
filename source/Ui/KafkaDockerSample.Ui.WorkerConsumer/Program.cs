using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace KafkaDockerSample.Ui.WorkerConsumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog((hostingContext, loggerConfiguration) =>
            {
                loggerConfiguration.ReadFrom.Configuration(
                    hostingContext.Configuration);
            })
            .ConfigureServices((hostContext, services) =>
            {
                IConfiguration configuration = hostContext.Configuration;

                var config = configuration.GetSection("ConsumerConfiguration")
                    .Get<ConsumerConfiguration>();
                
                services.AddSingleton(config);
                services.AddHostedService<Worker>();
            });
    }
}
