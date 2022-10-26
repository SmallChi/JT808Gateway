using JT808.Gateway.Client.Internal;
using JT808.Gateway.Client.Metadata;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.Gateway.Client.Services
{
    internal class JT808RetryClientHostedService : IHostedService
    {
        private readonly IJT808TcpClientFactory jT808TcpClientFactory;

        private readonly ILogger logger;

        private readonly JT808RetryBlockingCollection RetryBlockingCollection;

        public JT808RetryClientHostedService(
            JT808RetryBlockingCollection retryBlockingCollection,
            ILoggerFactory loggerFactory,
            IJT808TcpClientFactory jT808TcpClientFactory)
        {
            logger = loggerFactory.CreateLogger<JT808RetryClientHostedService>();
            this.jT808TcpClientFactory = jT808TcpClientFactory;
            RetryBlockingCollection = retryBlockingCollection;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            new Thread(async () =>
            {
                try
                {
                    foreach (var item in RetryBlockingCollection.RetryBlockingCollection.GetConsumingEnumerable(cancellationToken))
                    {
                        try
                        {
                            jT808TcpClientFactory.Remove(item);
                            if (item.AutoReconnection)
                            {
                                var result = await jT808TcpClientFactory.Create(item, cancellationToken);
                                if (result != null)
                                {
                                    if (logger.IsEnabled(LogLevel.Information))
                                    {
                                        logger.LogInformation($"Retry Success-{JsonSerializer.Serialize(item)}");
                                    }
                                }
                                else
                                {
                                    if (logger.IsEnabled(LogLevel.Warning))
                                    {
                                        logger.LogWarning($"Retry Fail-{JsonSerializer.Serialize(item)}");
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, $"Retry Error-{JsonSerializer.Serialize(item)}");
                            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                        }
                    }
                }
                catch (Exception)
                {

                }
            }).Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
