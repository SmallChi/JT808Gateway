using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using JT808.Gateway.Configurations;
using JT808.Gateway.GrpcService;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using JT808.Protocol.Extensions;
using Microsoft.Extensions.Options;

namespace JT808.Gateway.NormalHosting.Jobs
{
    public class CallGrpcClientJob :IHostedService
    {
        private Channel channel;
        private readonly ILogger Logger;
        private Grpc.Core.Metadata AuthMetadata;
        public CallGrpcClientJob(
            ILoggerFactory loggerFactory,
            IOptions<JT808Configuration> configurationAccessor)
        {
            Logger = loggerFactory.CreateLogger("CallGrpcClientJob");
            channel = new Channel($"{configurationAccessor.Value.WebApiHost}:{configurationAccessor.Value.WebApiPort}",
                ChannelCredentials.Insecure);
            AuthMetadata = new Grpc.Core.Metadata();
            AuthMetadata.Add("token", configurationAccessor.Value.WebApiToken);
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
                        var result3 = jT808GatewayClient.UnificationSend(new UnificationSendRequest 
                        { 
                           TerminalPhoneNo= "123456789012",
                           Data=Google.Protobuf.ByteString.CopyFrom("7E02000026123456789012007D02000000010000000200BA7F0E07E4F11C0028003C00001810151010100104000000640202007D01137E".ToHexBytes())
                        }, AuthMetadata);
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
