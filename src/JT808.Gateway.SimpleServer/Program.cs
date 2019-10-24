using JT808.Gateway.Services;
using JT808.Gateway.Tcp;
using JT808.Gateway.Udp;
using JT808.Protocol;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.IO;
using System.Net;

namespace JT808.Gateway.SimpleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureLogging((hostContext, configLogging) => {
                    Console.WriteLine($"Environment.OSVersion.Platform:{Environment.OSVersion.Platform.ToString()}");
                    NLog.LogManager.LoadConfiguration($"Configs/nlog.{Environment.OSVersion.Platform.ToString()}.config");
                    configLogging.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
                    configLogging.SetMinimumLevel(LogLevel.Trace);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .ConfigureKestrel(options =>
                    {
                        options.Listen(IPAddress.Any, 5001, listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http2;
                            listenOptions.UseHttps($"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configs", "test.cer")}", "");
                        });
                    })
                    .Configure(app =>
                    {
                        app.UseRouting();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapGrpcService<JT808GatewayService>();
                        });
                    });
                })
                .ConfigureServices((hostContext,services) =>
                {
                    //services.Configure<KestrelServerOptions>(hostContext.Configuration.GetSection("Kestrel"));
                    services.AddGrpc();
                    services.AddSingleton<ILoggerFactory, LoggerFactory>();
                    services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                    services.AddJT808Configure()
                            .AddJT808Gateway(hostContext.Configuration)
                            .AddJT808GatewayTcpHost()
                            .AddJT808GatewayUdpHost()
                            .Builder();
                })
                .Build()
                .Run();
        }
    }
}
