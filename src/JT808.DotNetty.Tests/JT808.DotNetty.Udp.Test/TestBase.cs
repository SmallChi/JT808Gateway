using JT808.DotNetty.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using JT808.Protocol;
using JT808.Protocol.Interfaces;

namespace JT808.DotNetty.Udp.Test
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
                    config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<ILoggerFactory, LoggerFactory>();
                    services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                    services.AddJT808Configure()
                            .AddJT808NettyCore(hostContext.Configuration)
                            .AddJT808UdpNettyHost();
                });
            var build = serverHostBuilder.Build();
            build.Start();
            ServiceProvider = build.Services;
            JT808Serializer = ServiceProvider.GetRequiredService<IJT808Config>().GetSerializer();
        }
    }
}
