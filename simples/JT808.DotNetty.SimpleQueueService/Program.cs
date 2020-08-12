using JT808.DotNetty.Kafka;
using JT808.DotNetty.ReplyMessage;
using JT808.DotNetty.SimpleQueueService.Impl;
using JT808.Protocol;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace JT808.DotNetty.SimpleQueueService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, config) => {
                    config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureLogging((hostContext, configLogging) => {
                    configLogging.AddConsole();
                    configLogging.SetMinimumLevel(LogLevel.Trace);
                })
                .ConfigureServices((hostContext, services) => {
                    services.AddSingleton<ILoggerFactory, LoggerFactory>();
                    services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                    services.AddJT808Configure()
                            .AddJT808ClientKafka()
                            .AddMsgReplyProducer(hostContext.Configuration)
                            .AddMsgConsumer(hostContext.Configuration)
                            .AddInprocJT808ReplyMessage<JT808DotNettyReplyMessageServiceInherited>();
                            ;
                });

            await hostBuilder.RunConsoleAsync();
        }
    }
}
