using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Core;
using JT808.DotNetty.Core.Handlers;
using JT808.DotNetty.Tcp;
using JT808.DotNetty.Udp;
using JT808.DotNetty.WebApi;
using JT808.DotNetty.WebApiClientTool;
using JT808.Protocol;
using JT808.Protocol.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using JT808.DotNetty.Kafka;

namespace JT808.DotNetty.Hosting
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //7E020000220138123456780085000000010000000101EA2A3F08717931000C015400201901032000020104000000E6F87E
            var serverHostBuilder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureLogging((context, logging) =>
                {
                    if (Environment.OSVersion.Platform == PlatformID.Unix)
                    {
                        NLog.LogManager.LoadConfiguration("Configs/nlog.unix.config");
                    }
                    else
                    {
                        NLog.LogManager.LoadConfiguration("Configs/nlog.win.config");
                    }
                    logging.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<ILoggerFactory, LoggerFactory>();
                    services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                    services.AddJT808Configure()
                            .AddJT808NettyCore(hostContext.Configuration)
                            .AddJT808TcpNettyHost()
                            .AddJT808UdpNettyHost()
                            .AddJT808WebApiNettyHost()
                            //扩展webapi JT808MsgIdHttpHandlerBase
                            //.ReplaceMsgIdHandler<JT808MsgIdHttpCustomHandler>()
                            .Builder()
                            //添加kafka插件
                            .AddJT808ServerKafkaMsgProducer(hostContext.Configuration)
                            .AddJT808ServerKafkaMsgReplyConsumer(hostContext.Configuration)
                            .AddJT808ServerKafkaSessionProducer(hostContext.Configuration)
                            .Builder();
                        //使用微软自带的webapi客户端
                        //services.AddHttpClient("jt808webapi", c =>
                        //{
                        //    c.BaseAddress = new Uri("http://localhost:828/");
                        //    c.DefaultRequestHeaders.Add("token", "123456);
                        //})
                        //.AddTypedClient<JT808HttpClient>();
                        //var client = services.BuildServiceProvider().GetRequiredService<JT808HttpClient>();
                        //var result = client.GetTcpAtomicCounter();
                });

            await serverHostBuilder.RunConsoleAsync();
        }
    }
}
