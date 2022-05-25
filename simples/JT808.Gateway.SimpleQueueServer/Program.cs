using JT808.Protocol;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using JT808.Gateway.Kafka;

namespace JT808.Gateway.SimpleQueueServer
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
                            .AddGateway(hostContext.Configuration)
                            .AddServerKafkaMsgProducer(hostContext.Configuration)
                            .AddServerKafkaMsgReplyConsumer(hostContext.Configuration)
                            .AddServerKafkaSessionProducer(hostContext.Configuration)
                            .AddTcp()
                            .AddUdp();
                });

            await serverHostBuilder.RunConsoleAsync();
        }
    }
}
