using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using JT808.Protocol;
using Microsoft.Extensions.Configuration;
using NLog.Extensions.Logging;
using JT808.Gateway.TestHosting.Jobs;
using JT808.Gateway.Enums;
using JT808.Gateway.Kafka;

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
                            //用于测试网关
                            .AddJT808DevelopmentGateway()
                            //用于生产环境
                            //.AddJT808Gateway(options =>
                            //{
                            //    options.TcpPort=8086;
                            //    options.UdpPort=8086;
                            //    options.MessageQueueType = JT808MessageQueueType.InPlug;                           
                            //})
                            .AddTcp()
                            .AddUdp()
                            .AddGrpc()
                            //kafka插件
                            //.AddJT808ServerKafkaMsgProducer(hostContext.Configuration)
                            //.AddJT808ServerKafkaMsgReplyConsumer(hostContext.Configuration)
                            //.AddJT808ServerKafkaSessionProducer(hostContext.Configuration)
                            ;
                    //services.AddHostedService<CallGrpcClientJob>();
                });

            await serverHostBuilder.RunConsoleAsync();
        }
    }
}
