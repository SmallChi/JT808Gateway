using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using JT808.Gateway.Abstractions.Configurations;
using JT808.Gateway.WebApiClientTool;

namespace JT808.Gateway.QueueHosting.Jobs
{
    public class CallHttpClientJob : IHostedService
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

                    try
                    {
                        var result2 = await jT808HttpClient.GetTcpSessionAll();
                        Logger.LogInformation($"[GetTcpSessionAll]:{JsonSerializer.Serialize(result2)}");
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "Call HttpClient Error");
                    }
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
