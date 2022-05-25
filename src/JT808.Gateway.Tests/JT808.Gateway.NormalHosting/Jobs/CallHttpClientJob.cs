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
using JT808.Gateway.NormalHosting.Customs;

namespace JT808.Gateway.NormalHosting.Jobs
{
    public class CallHttpClientJob :IHostedService
    {

        private readonly ILogger Logger;
        private JT808HttpClientExt jT808HttpClient;
        public CallHttpClientJob(
            ILoggerFactory loggerFactory,
            JT808HttpClientExt jT808HttpClient)
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
                    var result1 = await jT808HttpClient.GetIndex1();
                    var result2 = await jT808HttpClient.GetTcpSessionAll();
                    var result3 = await jT808HttpClient.UnificationSend(new Abstractions.Dtos.JT808UnificationSendRequestDto 
                    { 
                        TerminalPhoneNo= "123456789012",
                        HexData= "7E02000026123456789012007D02000000010000000200BA7F0E07E4F11C0028003C00001810151010100104000000640202007D01137E"
                    });
                    Logger.LogInformation($"[GetIndex Ext]:{JsonSerializer.Serialize(result1)}");
                    Logger.LogInformation($"[GetTcpSessionAll]:{JsonSerializer.Serialize(result2)}");
                    Logger.LogInformation($"[UnificationSend]:{JsonSerializer.Serialize(result3)}");
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
