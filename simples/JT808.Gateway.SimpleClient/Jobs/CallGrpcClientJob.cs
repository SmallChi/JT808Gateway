using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using JT808.Gateway.GrpcService;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace JT808.Gateway.SimpleClient.Jobs
{
    public class CallGrpcClientJob :IHostedService
    {
        private Channel channel;
        private readonly ILogger Logger;
        private Grpc.Core.Metadata AuthMetadata;
        public CallGrpcClientJob(
            ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger("CallGrpcClientJob");
            channel = new Channel("localhost:828",
                ChannelCredentials.Insecure);
            AuthMetadata = new Grpc.Core.Metadata();
            AuthMetadata.Add("token", "smallchi518");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    JT808Gateway.JT808GatewayClient jT808GatewayClient = new JT808Gateway.JT808GatewayClient(channel);
                    try
                    {
                        var result1 = jT808GatewayClient.GetTcpAtomicCounter(new Empty(), AuthMetadata);
                        var result2 = jT808GatewayClient.GetTcpSessionAll(new Empty(), AuthMetadata);
                        Logger.LogInformation($"[GetTcpAtomicCounter]:{JsonSerializer.Serialize(result1)}");
                        Logger.LogInformation($"[GetTcpSessionAll]:{JsonSerializer.Serialize(result2)}");
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "Call Grpc Error");
                    }
                    try
                    {
                        var result1 = jT808GatewayClient.GetTcpAtomicCounter(new Empty());
                    }
                    catch (RpcException ex)
                    {
                        Logger.LogError($"{ex.StatusCode.ToString()}-{ex.Message}");
                    }
                    Thread.Sleep(3000);
                }
            }, cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            channel.ShutdownAsync();
            return Task.CompletedTask;
        }
    }
}
