﻿using Acme.ChatABP.Messages;
using Acme.ChatABP.RabbitMQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Acme.ChatABP
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<ChatABPHttpApiHostModule>();
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder
                            .WithOrigins("http://localhost:4300")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });
           
            services.AddScoped<IProducer, Producer>();
            services.AddScoped<IChatAppServices, ChatAppservice>();
            services.AddScoped<IKafkaProducer, KafkaProducer>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.InitializeApplication();
            app.UseCors(MyAllowSpecificOrigins);
            
        }
    }
}
