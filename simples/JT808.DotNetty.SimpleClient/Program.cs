using JT808.DotNetty.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using JT808.Protocol;
using JT808.Protocol.MessageBody;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;
using JT808.DotNetty.SimpleClient.Services;

namespace JT808.DotNetty.SimpleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serverHostBuilder = new HostBuilder()
            .ConfigureLogging((context, logging) =>
            {
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Trace);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ILoggerFactory, LoggerFactory>();
                services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                services.AddLogging(options => {
                    options.AddConsole();
                    options.SetMinimumLevel(LogLevel.Trace);
                });
                services.AddJT808Configure()
                        .AddJT808Client();
                services.AddHostedService<Up2011Service>();
                services.AddHostedService<Up2013Service>();
                services.AddHostedService<Up2019Service>();
            });
            await serverHostBuilder.RunConsoleAsync();
        }
    }
}
