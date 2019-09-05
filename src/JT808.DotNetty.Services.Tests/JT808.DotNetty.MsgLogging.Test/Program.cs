using JT808.DotNetty.Kafka;
using JT808.Protocol;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Threading.Tasks;

namespace JT808.DotNetty.MsgLogging.Test
{
    class Program
    {
       async static Task Main(string[] args)
        {
            var hostBuilder = new HostBuilder()
                .UseEnvironment(args[0].Split('=')[1])
                .ConfigureAppConfiguration((hostContext,config)=> {
                    config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                             .AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
                })
                .ConfigureLogging((hostContext, configLogging) => {
                    configLogging.AddConsole();
                    configLogging.SetMinimumLevel(LogLevel.Trace);
                })
                .ConfigureServices((hostContext, services) => {
                    services.AddSingleton<ILoggerFactory, LoggerFactory>();
                    services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                    services.AddJT808Configure()
                                .AddJT808ClientKafka()
                                .AddMsgConsumer(hostContext.Configuration)
                                .AddMsgReplyConsumer(hostContext.Configuration)
                                .AddJT808MsgLogging<JT808MsgLoggingImpl>();
                })
                ;

            await hostBuilder.RunConsoleAsync();
        }
    }
}
