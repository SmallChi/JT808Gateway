using JT808.DotNetty.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Udp.Test
{
    public class TestBase
    {
        public static IServiceProvider ServiceProvider;

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
                    services.AddJT808Core(hostContext.Configuration)
                            .AddJT808UdpHost();
                });
            var build = serverHostBuilder.Build();
            build.Start();
            ServiceProvider = build.Services;

        }
    }
}
