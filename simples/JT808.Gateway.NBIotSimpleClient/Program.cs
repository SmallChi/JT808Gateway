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
using JT808.Gateway.WebApiClientTool;
using JT808.Gateway.NBIotSimpleClient.Jobs;
using JT808.Gateway.NBIotSimpleClient.Services;

namespace JT808.Gateway.NBIotSimpleClient
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
                services.AddSingleton<ReceviePackageService>();
                services.AddSingleton<DeviceInfoService>();
                services.AddJT808Configure()
                        .AddClient()
                        .AddMessageProducer<JT808MessageProducerImpl>();
                services.AddHostedService<Up2013Service>()
                        .AddHostedService<ProccessPackageService>()
                        .AddHostedService<AEPMsgConsumerService>();
            });
            await serverHostBuilder.RunConsoleAsync();
        }
    }
}
