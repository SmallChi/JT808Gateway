using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using JT808.Protocol;
using Microsoft.Extensions.Configuration;
using NLog.Extensions.Logging;
using JT808.Gateway.TestHosting.Jobs;
using JT808.Gateway.Kafka;
using JT808.Gateway.InMemoryMQ;
using JT808.Gateway.ReplyMessage;
using JT808.Gateway.Client;
using JT808.Gateway.SessionNotice;
using JT808.Gateway.Abstractions.Enums;
using JT808.Gateway.MsgIdHandler;
using JT808.Gateway.MsgLogging;
using JT808.Gateway.Traffic;
using JT808.Gateway.Transmit;
using JT808.Gateway.TestHosting.Impl;

namespace JT808.Gateway.TestHosting
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serverHostBuilder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                          .AddJsonFile($"appsettings.{ hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
                })
                .ConfigureLogging((context, logging) =>
                {
                    Console.WriteLine($"Environment.OSVersion.Platform:{Environment.OSVersion.Platform.ToString()}");
                    NLog.LogManager.LoadConfiguration($"Configs/nlog.{Environment.OSVersion.Platform.ToString()}.config");
                    logging.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<ILoggerFactory, LoggerFactory>();
                    services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                    services.AddJT808Configure()
                            //添加客户端工具
                            //.AddClient()
                            //.AddGateway(options =>
                            ////{
                            ////    options.TcpPort = 808;
                            ////    options.UdpPort = 808;
                            ////})                            
                            .AddGateway(hostContext.Configuration)
                            .AddTcp()
                            .AddUdp()
                            .AddGrpc()
                            //InMemoryMQ 按需要加载对应的服务
                            //注意:不需要的就不用add进来了
                            .AddServerInMemoryMQ(JT808ConsumerType.All)
                            //方式1
                            //.AddServerInMemoryMQ(JT808ConsumerType.MsgIdHandlerConsumer| JT808ConsumerType.ReplyMessageConsumer)
                            //方式2
                            //.AddServerInMemoryMQ(JT808ConsumerType.MsgIdHandlerConsumer,JT808ConsumerType.ReplyMessageConsumer)
                            .AddInMemoryTraffic()
                            .AddInMemoryTransmit(hostContext.Configuration)
                            .AddInMemoryMsgIdHandler<JT808MsgIdHandler>()
                            .AddInMemoryMsgLogging<JT808MsgLogging>()
                            .AddInMemorySessionNotice()
                            .AddInMemoryReplyMessage()
                            //kafka插件
                            //.AddServerKafkaMsgProducer(hostContext.Configuration)
                            //.AddServerKafkaMsgReplyConsumer(hostContext.Configuration)
                            //.AddServerKafkaSessionProducer(hostContext.Configuration)
                            ;
                    //流量统计
                    //services.AddHostedService<TrafficJob>();
                    //grpc客户端调用
                    //services.AddHostedService<CallGrpcClientJob>();
                    //客户端测试
                    //services.AddHostedService<UpJob>();
                });

            await serverHostBuilder.RunConsoleAsync();
        }
    }
}
