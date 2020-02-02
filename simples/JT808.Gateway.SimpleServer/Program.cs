using JT808.Gateway.Abstractions.Enums;
using JT808.Gateway.InMemoryMQ;
using JT808.Gateway.ReplyMessage;
using JT808.Gateway.MsgLogging;
using JT808.Gateway.Traffic;
using JT808.Gateway.MsgIdHandler;
using JT808.Gateway.SessionNotice;
using JT808.Protocol;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using JT808.Gateway.SimpleServer.Impl;

namespace JT808.Gateway.SimpleServer
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
                            .AddTcp()
                            .AddServerInMemoryMQ(JT808ConsumerType.MsgIdHandlerConsumer|
                                                JT808ConsumerType.ReplyMessageConsumer |
                                                JT808ConsumerType.MsgLoggingConsumer |
                                                JT808ConsumerType.ReplyMessageLoggingConsumer)
                            .AddInMemoryMsgIdHandler<JT808MsgIdHandler>()
                            .AddInMemoryReplyMessage()
                            .AddInMemoryMsgLogging<JT808MsgLogging>()
                            .AddInMemorySessionNotice()
                            .Builder();
                });

            await serverHostBuilder.RunConsoleAsync();
        }
    }
}
