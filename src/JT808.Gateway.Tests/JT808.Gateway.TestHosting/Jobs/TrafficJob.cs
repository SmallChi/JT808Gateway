using JT808.Gateway.Client;
using JT808.Gateway.Traffic;
using JT808.Protocol.Enums;
using JT808.Protocol.Extensions;
using JT808.Protocol.MessageBody;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.Gateway.TestHosting.Jobs
{
    public class TrafficJob : IHostedService
    {
        private readonly IJT808Traffic jT808Traffic;
        private readonly ILogger Logger;
        public TrafficJob(
            ILoggerFactory loggerFactory,
            IJT808Traffic jT808Traffic)
        {
            Logger = loggerFactory.CreateLogger("TrafficJob");
            this.jT808Traffic = jT808Traffic;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(2 * 1000);
                    foreach (var item in jT808Traffic.GetAll())
                    {
                        Logger.LogDebug($"{item.Item1}-{item.Item2}");
                    }
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
