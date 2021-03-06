using FluentValidation;
using FluentValidation.AspNetCore;
using KafkaDockerSample.Ui.Api.Validators;
using KafkaDockerSample.Ui.Api.Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Hosting;
using Serilog;
using KafkaDockerSample.Core.Application;

namespace KafkaDockerSample.Ui.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IValidator<RegisterOccurrence>, RegisterOccurrenceValidator>();

            var applicationConfiguration = Configuration
                .GetSection("ApplicationConfiguration").Get<ApplicationConfiguration>();

            services.AddServices(applicationConfiguration);

            services
                .AddMvc(m =>
                {
                    m.EnableEndpointRouting = false;
                })
                .AddFluentValidation()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.WriteIndented = true;
                });

            services.AddSwaggerGen(s =>
            {
                var apiInfo = new OpenApiInfo()
                {
                    Title = "KafkaDocker Web API",
                    Version = "v1",
                    Description = ".NET Web API created to simulate simple Kafka interaction"
                };

                s.SwaggerDoc("v1", apiInfo);
            });
        }

        public void Configure(IApplicationBuilder app, 
            IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseRouting();
            app.UseSerilogRequestLogging();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });

            app.UseMvc();
        }
    }
}
