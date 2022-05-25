using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using JT808.Protocol;
using Microsoft.Extensions.Configuration;
using NLog.Extensions.Logging;
using JT808.Gateway.MsgLogging;
using JT808.Gateway.ReplyMessage;
using JT808.Gateway.Transmit;
using JT808.Gateway.Abstractions;
using JT808.Gateway.SessionNotice;
using JT808.Gateway.Client;
using JT808.Gateway.QueueHosting.Jobs;
using JT808.Gateway.Kafka;
using JT808.Gateway.WebApiClientTool;
using JT808.Gateway.QueueHosting.Impl;
using JT808.Gateway.MsgIdHandler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using JT808.Gateway.Abstractions.Configurations;

namespace JT808.Gateway.QueueHosting
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var serverHostBuilder = Host.CreateDefaultBuilder()
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
                            .AddClient()
                            .AddClientReport()
                            .Builder()
                            //方式1:客户端webapi调用
                            .AddWebApiClientTool(hostContext.Configuration)
                            //添加客户端服务
                            .AddClientKafka()
                            .AddMsgConsumer(hostContext.Configuration)
                            //添加消息上行处理器
                            .AddMsgIdHandler<JT808UpMessageHandlerImpl>()
                            //添加消息应答生产者
                            .AddMsgReplyProducer(hostContext.Configuration)
                            //添加消息应答服务并实现消息应答处理
                            .AddReplyMessage<JT808DownMessageHandlerImpl>()
                            .Builder()
                            //添加消息应答处理
                            .AddGateway(hostContext.Configuration)
                            .AddMessageHandler<JT808CustomMessageHandlerImpl>()
                            .AddServerKafkaMsgProducer(hostContext.Configuration)
                            .AddServerKafkaSessionProducer(hostContext.Configuration)
                            .AddServerKafkaMsgReplyConsumer(hostContext.Configuration)
                            .AddTcp()
                            .AddUdp();
                    //方式2:客户端webapi调用
                    //services.AddJT808WebApiClientTool(hostContext.Configuration);
                    //httpclient客户端调用
                    services.AddHostedService<CallHttpClientJob>();
                    //客户端测试
                    services.AddHostedService<UpJob>();
                })
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseKestrel((app, ksOptions) =>
                    {
                        //1.配置webapi端口监听
                        var jT808Configuration = app.Configuration.GetSection(nameof(JT808Configuration)).Get<JT808Configuration>();
                        ksOptions.ListenAnyIP(jT808Configuration.WebApiPort);
                    })
                    .UseStartup<Startup>();
                });
            await serverHostBuilder.RunConsoleAsync();
        }
    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
