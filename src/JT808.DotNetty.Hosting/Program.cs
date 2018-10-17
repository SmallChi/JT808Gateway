using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace JT808.DotNetty.Hosting
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //7E 01 02 00 06 01 38 12 34 56 78 00 85 32 31 31 33 31 33 B2 7E

            var serverHostBuilder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureLogging((context, logging) =>
                {
                    logging.AddConsole();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<ILoggerFactory, LoggerFactory>();
                    services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                    // 自定义消息处理业务
                    services.Replace(new ServiceDescriptor(typeof(JT808MsgIdHandlerBase), typeof(JT808MsgIdCustomHandler), ServiceLifetime.Singleton));
                })
                .UseJT808Host();
            await serverHostBuilder.RunConsoleAsync();
        }
    }
}
