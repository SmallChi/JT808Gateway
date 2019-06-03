using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Core;
using JT808.DotNetty.Core.Handlers;
using JT808.DotNetty.Hosting.Handlers;
using JT808.DotNetty.Hosting.Impls;
using JT808.DotNetty.Tcp;
using JT808.DotNetty.Udp;
using JT808.DotNetty.WebApi;
using JT808.DotNetty.WebApiClientTool;
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
using WebApiClient.Extensions.DependencyInjection;

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
                    services.AddJT808Core(hostContext.Configuration)
                            .AddJT808TcpHost()
                            .AddJT808UdpHost()
                            .AddJT808WebApiHost();
                    // 自定义Tcp消息处理业务
                    services.Replace(new ServiceDescriptor(typeof(JT808MsgIdTcpHandlerBase), typeof(JT808MsgIdTcpCustomHandler), ServiceLifetime.Singleton));
                    // 自定义Udp消息处理业务
                    services.Replace(new ServiceDescriptor(typeof(JT808MsgIdUdpHandlerBase), typeof(JT808MsgIdUdpCustomHandler), ServiceLifetime.Singleton));
                    services.Replace(new ServiceDescriptor(typeof(IJT808DownlinkPacket), typeof(JT808DownlinkPacketLogging), ServiceLifetime.Singleton));
                    // 自定义会话通知（在线/离线）使用异步方式
                    //services.Replace(new ServiceDescriptor(typeof(IJT808SessionPublishing), typeof(CustomJT808SessionPublishing), ServiceLifetime.Singleton));
                    //  自定义原包转发 使用异步方式
                    //services.Replace(new ServiceDescriptor(typeof(IJT808SourcePackageDispatcher), typeof(CustomJT808SourcePackageDispatcher), ServiceLifetime.Singleton));
                    // webapi客户端调用
                    //services.AddHttpApi<IJT808DotNettyWebApi>().ConfigureHttpApiConfig((c, p) =>
                    //{
                    //    c.HttpHost = new Uri("http://localhost:12828/jt808api/");
                    //    c.FormatOptions.DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
                    //    c.LoggerFactory = p.GetRequiredService<ILoggerFactory>();
                    //});
                    //var client = services.BuildServiceProvider().GetRequiredService<IJT808DotNettyWebApi>();
                    //var result = client.GetTcpAtomicCounter().InvokeAsync().Result;
                });

            await serverHostBuilder.RunConsoleAsync();
        }
    }
}
