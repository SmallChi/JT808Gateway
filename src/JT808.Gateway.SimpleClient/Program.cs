using JT808.Gateway.Client;
using JT808.Gateway.SimpleClient.Services;
using JT808.Protocol;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using JT808.Gateway.GrpcService;

namespace JT808.Gateway.SimpleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //ref https://docs.microsoft.com/zh-cn/aspnet/core/grpc/troubleshoot?view=aspnetcore-3.0#call-insecure-grpc-services-with-net-core-client
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var serverHostBuilder = new HostBuilder()
              .ConfigureLogging((context, logging) =>
              {
                  logging.AddConsole();
                  logging.SetMinimumLevel(LogLevel.Trace);
              })
              .ConfigureServices((hostContext, services) =>
              {
                  services.AddGrpcClient<JT808Gateway.JT808GatewayClient>(o =>
                  {
                        o.Address = new Uri("https://127.0.0.1:5001");
                  });
                  services.AddSingleton<ILoggerFactory, LoggerFactory>();
                  services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                  services.AddLogging(options => {
                      options.AddConsole();
                      options.SetMinimumLevel(LogLevel.Trace);
                  });
                  services.AddJT808Configure()
                          .AddJT808Client();
                  services.AddHostedService<UpService>();
                  services.AddHostedService<GrpcClientService>();
              });
            await serverHostBuilder.RunConsoleAsync();
        }
    }
}
