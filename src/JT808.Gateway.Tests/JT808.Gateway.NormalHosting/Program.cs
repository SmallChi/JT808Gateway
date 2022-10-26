using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using JT808.Protocol;
using Microsoft.Extensions.Configuration;
using NLog.Extensions.Logging;
using JT808.Gateway.NormalHosting.Impl;
using JT808.Gateway.MsgLogging;
using JT808.Gateway.Transmit;
using JT808.Gateway.NormalHosting.Services;
using JT808.Gateway.Abstractions;
using JT808.Gateway.SessionNotice;
using JT808.Gateway.Client;
using JT808.Gateway.NormalHosting.Jobs;
using JT808.Gateway.WebApiClientTool;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration.Json;
using JT808.Gateway.Abstractions.Configurations;
using System.Net;
using JT808.Gateway.Extensions;
using JT808.Gateway.NormalHosting.Customs;

namespace JT808.Gateway.NormalHosting
{
    class Program
    {
        static void Main(string[] args)
        {
            //ref:https://andrewlock.net/exploring-dotnet-6-part-2-comparing-webapplicationbuilder-to-the-generic-host/
            //the new hotness in .NET 6.
            var builder = WebApplication.CreateBuilder();
            builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
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
                //使用内存队列实现会话通知
                services.AddSingleton<JT808SessionService>();
                services.AddSingleton<IJT808SessionProducer, JT808SessionProducer>();
                services.AddSingleton<IJT808SessionConsumer, JT808SessionConsumer>();
                //使用内存队列实现应答生产消费
                services.AddSingleton<JT808MsgReplyDataService>();
                services.AddSingleton<IJT808MsgReplyProducer, JT808MsgReplyProducer>();
                services.AddJT808Configure()
                        //添加客户端工具
                        .AddClient()
                        .Builder()
                        //方式1:客户端webapi调用
                        .AddWebApiClientTool<JT808HttpClientExt>(hostContext.Configuration)
                        .AddGateway(hostContext.Configuration)
                        .AddMessageHandler<JT808CustomMessageHandlerImpl>()
                        .AddMsgReplyConsumer<JT808MsgReplyConsumer>()
                        .AddMsgLogging<JT808MsgLogging>()
                        .AddSessionNotice()
                        .AddTransmit(hostContext.Configuration)
                        .AddTcp()
                        .AddUdp();
                //方式2:客户端webapi调用
                //services.AddJT808WebApiClientTool(hostContext.Configuration);
                //httpclient客户端调用
                services.AddHostedService<CallHttpClientJob>();
                //客户端测试  依赖AddClient()服务
                services.AddHostedService<UpJob>();
                
                //需要跨域的
                services.AddCors(options =>
                   options.AddPolicy("jt808", builder =>
                   builder.AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials()
                          .SetIsOriginAllowed(o => true)));
            });
            builder.WebHost.UseKestrel((app, serverOptions) =>
            {
                //1.配置webapi端口监听
                var jT808Configuration = app.Configuration.GetSection(nameof(JT808Configuration)).Get<JT808Configuration>();
                serverOptions.ListenAnyIP(jT808Configuration.WebApiPort);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddControllers();
            });
            var app = builder.Build();

            app.UseRouting();
            app.UseCors();

            app.MapControllers().RequireCors("jt808");

            app.Run();
        }
    }
}
