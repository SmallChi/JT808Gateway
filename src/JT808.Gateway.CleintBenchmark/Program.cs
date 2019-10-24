using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Threading.Tasks;
using JT808.Protocol;
using Microsoft.Extensions.Configuration;
using JT808.Gateway.CleintBenchmark.Configs;
using JT808.Gateway.Client;
using JT808.Gateway.CleintBenchmark.Services;

namespace JT808.Gateway.CleintBenchmark
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
                if (Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    NLog.LogManager.LoadConfiguration("Configs/nlog.unix.config");
                }
                else
                {
                    NLog.LogManager.LoadConfiguration("Configs/nlog.win.config");
                }
                logging.AddNLog();
                logging.SetMinimumLevel(LogLevel.Trace);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<ClientBenchmarkOptions>(hostContext.Configuration.GetSection("ClientBenchmarkOptions"));
                services.AddSingleton<ILoggerFactory, LoggerFactory>();
                services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                services.AddJT808Configure()
                        .AddJT808Client();
                services.AddHostedService<CleintBenchmarkHostedService>();
                services.AddHostedService<CleintBenchmarkReportHostedService>();
            });
            await serverHostBuilder.RunConsoleAsync();
        }
    }
}
