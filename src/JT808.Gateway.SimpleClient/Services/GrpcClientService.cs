using JT808.Gateway.Client;
using JT808.Protocol.MessageBody;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JT808.Gateway.GrpcService;
using static JT808.Gateway.GrpcService.JT808Gateway;
using Google.Protobuf;
using System.Text.Json;
using JT808.Protocol.Extensions;

namespace JT808.Gateway.SimpleClient.Services
{
    public class GrpcClientService : IHostedService
    {
        private readonly ILogger logger;
        private readonly JT808GatewayClient client;

        public GrpcClientService(
            ILoggerFactory loggerFactory,
            JT808GatewayClient  jT808GatewayClient)
        {
            this.client = jT808GatewayClient;
            logger = loggerFactory.CreateLogger("GrpcClientService");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() => {
                while (!cancellationToken.IsCancellationRequested)
                {
                    Thread.Sleep(1000 * 10);
                    var result1 = client.GetTcpAtomicCounter(new Empty());
                    var result2 = client.GetUdpAtomicCounter(new Empty());
                    var result3 = client.GetTcpSessionAll(new Empty());
                    var result4 = client.GetUdpSessionAll(new Empty());
                    var result5 = client.UnificationSend(new UnificationSendRequest() 
                    {
                         TerminalPhoneNo= "12345678910",
                         Data= ByteString.CopyFrom("7E 02 00 00 26 12 34 56 78 90 12 00 7D 02 00 00 00 01 00 00 00 02 00 BA 7F 0E 07 E4 F1 1C 00 28 00 3C 00 00 18 10 15 10 10 10 01 04 00 00 00 64 02 02 00 7D 01 13 7E".ToHexBytes())
                    });
                    var result6 = client.RemoveSessionByTerminalPhoneNo(new  SessionRemoveRequest()
                    {
                         TerminalPhoneNo= "12345678910"
                    });

                    logger.LogDebug(JsonSerializer.Serialize(result1));
                    logger.LogDebug(JsonSerializer.Serialize(result2));
                    logger.LogDebug(JsonSerializer.Serialize(result3));
                    logger.LogDebug(JsonSerializer.Serialize(result4));
                    logger.LogDebug(JsonSerializer.Serialize(result5));
                    logger.LogDebug(JsonSerializer.Serialize(result6));
                }  
            }, cancellationToken);  
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
