using JT808.DotNetty.Core.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.DotNetty.Core.Jobs
{
    public class JT808UdpMaintainSessionJob : JT808BackgroundService
    {
        private readonly ILogger<JT808UdpMaintainSessionJob> _logger;

        private readonly JT808UdpSessionManager jT808UdpSessionManager;
        private readonly IOptionsMonitor<JT808Configuration> jT808ConfigurationAccessor;

        public JT808UdpMaintainSessionJob(
            JT808UdpSessionManager jT808UdpSessionManager,
          IOptionsMonitor<JT808Configuration> jT808ConfigurationAccessor,
            ILoggerFactory loggerFactory)
        {
            this.jT808UdpSessionManager = jT808UdpSessionManager;
            this.jT808ConfigurationAccessor = jT808ConfigurationAccessor;
            _logger = loggerFactory.CreateLogger<JT808UdpMaintainSessionJob>();
        }

        public override string ServiceName => nameof(JT808UdpMaintainSessionJob);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{ServiceName} is starting.");
            stoppingToken.Register(() => _logger.LogInformation($"{ServiceName} background task is stopping."));
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{ServiceName} task doing background work.");
                jT808UdpSessionManager.TimerToRemoveExpiredData();
                await Task.Delay(TimeSpan.FromSeconds(jT808ConfigurationAccessor.CurrentValue.UdpSlidingExpirationTimeSeconds), stoppingToken);
            }
            _logger.LogInformation($"{ServiceName} background task is stopping.");
        }
    }
}