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
using System.Net;

namespace JT808.Gateway.SimpleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //ref https://docs.microsoft.com/zh-cn/aspnet/core/grpc/troubleshoot?view=aspnetcore-3.0#call-insecure-grpc-services-with-net-core-client
            //ref https://docs.microsoft.com/zh-cn/aspnet/core/grpc/troubleshoot?view=aspnetcore-3.0

            //先执行 dotnet dev-certs https --trust  命令生成开发证书
            //使用 certmgr.msc 导出证书在服务端配置对应证书文件
            //Uri "https://localhost:5001"

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
                        o.Address = new Uri("https://localhost:5001");
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
