using JT808.Gateway.Abstractions.Configurations;
using JT808.Gateway.Session;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.Gateway.Services
{
    internal class JT808UdpReceiveTimeoutHostedService : BackgroundService
    {
        private readonly ILogger Logger;

        private readonly JT808SessionManager SessionManager;

        private readonly JT808Configuration Configuration;
        public JT808UdpReceiveTimeoutHostedService(
                IOptions<JT808Configuration> jT808ConfigurationAccessor,
                ILoggerFactory loggerFactory,
                JT808SessionManager jT808SessionManager
            )
        {
            SessionManager = jT808SessionManager;
            Logger = loggerFactory.CreateLogger<JT808UdpReceiveTimeoutHostedService>();
            Configuration = jT808ConfigurationAccessor.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    List<string> sessionIds = new List<string>();
                    foreach (var item in SessionManager.GetUdpAll())
                    {
                        if (item.ActiveTime.AddSeconds(Configuration.UdpReaderIdleTimeSeconds) < DateTime.Now)
                        {
                            sessionIds.Add(item.SessionID);
                        }
                    }
                    foreach(var item in sessionIds)
                    {
                        SessionManager.RemoveBySessionId(item);
                    }
                    Logger.LogInformation($"[Check Receive Timeout]");
                    Logger.LogInformation($"[Session Online Count]:{SessionManager.UdpSessionCount}");
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"[Receive Timeout]");
                }
                finally
                {
                    await Task.Delay(TimeSpan.FromSeconds(Configuration.UdpReceiveTimeoutCheckTimeSeconds), stoppingToken);
                }
            }
        }
    }
}
