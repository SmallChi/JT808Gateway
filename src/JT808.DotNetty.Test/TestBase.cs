using JT808.DotNetty.Codecs;
using JT808.DotNetty.Configurations;
using JT808.DotNetty.Handlers;
using JT808.DotNetty.Interfaces;
using JT808.DotNetty.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using Xunit;

namespace JT808.DotNetty.Test
{
    public class TestBase:IDisposable
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
                })
                .UseJT808Host();
            var build=serverHostBuilder.Build();
            build.Start();
            ServiceProvider = build.Services;

        }

        public virtual void Dispose()
        {
    
        }
    }
}
