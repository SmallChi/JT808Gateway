using JT808.DotNetty.Core;
using JT808.DotNetty.Tcp;
using JT808.DotNetty.Kafka;
using JT808.Protocol;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace JT808.DotNetty.SimpleQueueServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
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
                services.AddJT808Configure()
                        .AddJT808NettyCore(hostContext.Configuration)
                        .AddJT808TcpNettyHost()
                        .AddJT808ServerKafkaMsgProducer(hostContext.Configuration)
                        .AddJT808ServerKafkaMsgReplyConsumer(hostContext.Configuration)
                        .Builder();
            });

            await serverHostBuilder.RunConsoleAsync();
        }
    }
}
