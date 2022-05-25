using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using JT808.Protocol;
using JT808.Protocol.MessageBody;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;
using JT808.Gateway.Client;
using JT808.Gateway.SimpleClient.Services;
using JT808.Gateway.SimpleClient.Jobs;
using JT808.Gateway.WebApiClientTool;
using JT808.Gateway.SimpleClient.Customs;

namespace JT808.Gateway.SimpleClient
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
                services.AddJT808Configure()
                        .AddClient()
                        .Builder()
                        //可以注册一个或者可以自定义扩展
                        .AddWebApiClientTool(new Uri("http://127.0.0.1:828/"), "123456")
                        .AddWebApiClientTool<JT808HttpClientExt>(new Uri("http://127.0.0.1:828/"), "123456");
                services.AddHostedService<Up2011Service>();
                services.AddHostedService<Up2013Service>();
                services.AddHostedService<Up2019Service>();
                services.AddHostedService<CallHttpClientJob>();
                services.AddHostedService<CallHttpClientJobExt>();
            });
            await serverHostBuilder.RunConsoleAsync();
        }
    }
}
