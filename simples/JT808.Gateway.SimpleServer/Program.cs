using JT808.Gateway.Abstractions.Enums;
using JT808.Gateway.ReplyMessage;
using JT808.Gateway.MsgLogging;
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
using JT808.Gateway.SimpleServer.Services;
using JT808.Gateway.Abstractions;
using JT808.Gateway.Transmit;
using Microsoft.AspNetCore.Hosting;
using JT808.Gateway.Abstractions.Configurations;
using Microsoft.AspNetCore.Builder;

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
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<ILoggerFactory, LoggerFactory>();
                    services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                    //使用内存队列实现会话通知
                    services.AddSingleton<JT808SessionService>();
                    services.AddSingleton<IJT808SessionProducer, JT808SessionProducer>();
                    services.AddSingleton<IJT808SessionConsumer, JT808SessionConsumer>();
                    services.AddJT808Configure()
                            .AddGateway(hostContext.Configuration)
                            .AddMessageHandler<JT808MessageHandlerImpl>()
                            .AddMsgLogging<JT808MsgLogging>()
                            .AddSessionNotice()
                            .AddTransmit(hostContext.Configuration)
                            .AddTcp()
                            .AddUdp()
                            .Builder();
                }).ConfigureWebHostDefaults(webBuilder => {
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
