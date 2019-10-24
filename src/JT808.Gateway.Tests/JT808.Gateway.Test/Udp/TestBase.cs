using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using JT808.Protocol;
using JT808.Protocol.Interfaces;
using JT808.Gateway.Udp;
using System.IO;

namespace JT808.Gateway.Test.Udp
{
    public class TestBase
    {
        public static IServiceProvider ServiceProvider;
        public static JT808Serializer JT808Serializer;
        static TestBase()
        {
            var serverHostBuilder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Udp"));
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<ILoggerFactory, LoggerFactory>();
                    services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                    services.AddJT808Configure()
                            .AddJT808Gateway(hostContext.Configuration)
                            .AddJT808GatewayUdpHost();
                });
            var build = serverHostBuilder.Build();
            build.Start();
            ServiceProvider = build.Services;
            JT808Serializer = ServiceProvider.GetRequiredService<IJT808Config>().GetSerializer();
        }
    }
}
