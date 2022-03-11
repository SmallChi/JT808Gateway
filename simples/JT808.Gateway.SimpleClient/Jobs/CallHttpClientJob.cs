using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using JT808.Protocol.Extensions;
using Microsoft.Extensions.Options;
using JT808.Gateway.Abstractions.Configurations;
using JT808.Gateway.WebApiClientTool;

namespace JT808.Gateway.SimpleClient.Jobs
{
    public class CallHttpClientJob :IHostedService
    {

        private readonly ILogger Logger;
        private JT808HttpClient jT808HttpClient;
        public CallHttpClientJob(
            ILoggerFactory loggerFactory,
            JT808HttpClient jT808HttpClient)
        {
            Logger = loggerFactory.CreateLogger<CallHttpClientJob>();
            this.jT808HttpClient = jT808HttpClient;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var result2 = await jT808HttpClient.GetTcpSessionAll();
                    var result3 = await jT808HttpClient.UnificationSend(new Abstractions.Dtos.JT808UnificationSendRequestDto 
                    { 
                        TerminalPhoneNo= "123456789012",
                        HexData= "7E02000026123456789012007D02000000010000000200BA7F0E07E4F11C0028003C00001810151010100104000000640202007D01137E"
                    });
                    var result4 = await jT808HttpClient.QueryTcpSessionByTerminalPhoneNo(new Abstractions.Dtos.JT808TerminalPhoneNoDto { TerminalPhoneNo= "33333333333" });
                    Logger.LogInformation($"[GetTcpAtomicCounter]:{JsonSerializer.Serialize(result2)}");
                    Logger.LogInformation($"[UnificationSend]:{JsonSerializer.Serialize(result3)}");
                    Logger.LogInformation($"[QueryTcpSessionByTerminalPhoneNo]:{JsonSerializer.Serialize(result4)}");
                    Thread.Sleep(3000);
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
