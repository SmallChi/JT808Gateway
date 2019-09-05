using JT808.DotNetty.Kafka;
using JT808.Protocol;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace JT808.DotNetty.MsgIdHandler.Test
{
    class Program
    {
       async static Task Main(string[] args)
        {
            var serverHostBuilder = new HostBuilder()
                .UseEnvironment(args[0].Split('=')[1])
                .ConfigureAppConfiguration((hostingContext,config) => {
                    config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                             .AddJsonFile($"appsettings.{ hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
                })
                .ConfigureLogging(configLogging => {
                    configLogging.AddConsole();
                    configLogging.SetMinimumLevel(LogLevel.Trace);
                })
                .ConfigureServices((hostContext, services) => {
                    services.AddSingleton<ILoggerFactory, LoggerFactory>();
                    services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                    services.AddJT808Configure()
                                .AddJT808ClientKafka()
                                .AddMsgConsumer(hostContext.Configuration)
                                .AddJT808MsgIdHandler<JT808DotNettyMsgIdHandlerDefaultImpl>();
                });
            await serverHostBuilder.RunConsoleAsync();
        }
    }
}
