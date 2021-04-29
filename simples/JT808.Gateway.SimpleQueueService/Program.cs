using JT808.Protocol;
using JT808.Gateway.Kafka;
using JT808.Gateway.ReplyMessage;
using JT808.Gateway.SessionNotice;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using JT808.Gateway.SimpleQueueService.Impl;

namespace JT808.Gateway.SimpleQueueService
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
                            .AddClientKafka()
                            .AddMsgConsumer(hostContext.Configuration)
                            .AddMsgReplyProducer(hostContext.Configuration)
                            .AddSessionConsumer(hostContext.Configuration)
                            .AddReplyMessage<JT808QueueReplyMessageHandlerImpl>()
                            .AddSessionNotice<JT808SessionNoticeServiceImpl>();
                });

            await hostBuilder.RunConsoleAsync();
        }
    }
}
