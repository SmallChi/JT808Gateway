using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using JT808.Gateway.Configurations;
using JT808.Gateway.GrpcService;
using JT808.Gateway.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JT808.Gateway
{
    public class JT808GrpcServer : IHostedService
    {
        private readonly ILogger Logger;
        private readonly JT808Configuration Configuration;
        private readonly IServiceProvider ServiceProvider;
        private Server server;
        public JT808GrpcServer(
                IServiceProvider serviceProvider,
                JT808Configuration jT808Configuration,
                ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger("JT808GrpcServer");
            Configuration = jT808Configuration;
            ServiceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            server = new Server
            {
                Services = { JT808Gateway.BindService(new JT808GatewayService(ServiceProvider)) },
                Ports = { new ServerPort(Configuration.WebApiHost, Configuration.WebApiPort, ServerCredentials.Insecure) }
            };
            Logger.LogInformation($"JT808 Grpc Server start at {Configuration.WebApiHost}:{Configuration.WebApiPort}.");
            try
            {
                server.Start();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "JT808 Grpc Server start error");
            }
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("JT808 Grpc Server Stop");
            server.ShutdownAsync();
            return Task.CompletedTask;
        }
    }
}
