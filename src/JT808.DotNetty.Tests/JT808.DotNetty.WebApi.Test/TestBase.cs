using JT808.DotNetty.Core;
using JT808.DotNetty.Udp;
using JT808.DotNetty.Tcp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using JT808.DotNetty.WebApiClientTool;

namespace JT808.DotNetty.WebApi.Test
{
    public class TestBase
    {
        public static IServiceProvider ServiceProvider;

        static TestBase()
        {
            var serverHostBuilder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<JT808DotNettyWebApiOptions>(hostContext.Configuration.GetSection("JT808DotNettyWebApiOptions"));
                    services.AddSingleton<ILoggerFactory, LoggerFactory>();
                    services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                    services.AddJT808Core(hostContext.Configuration)
                            .AddJT808TcpHost()
                            .AddJT808UdpHost()
                            .AddJT808WebApiHost();
                });
            var build = serverHostBuilder.Build();
            build.Start();
            ServiceProvider = build.Services;
        }


    }
}
