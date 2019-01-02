using JT808.DotNetty.Core.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.DotNetty.Core.Jobs
{
    public class JT808UdpAtomicCouterResetDailyJob : JT808BackgroundService
    {
        private readonly ILogger<JT808UdpAtomicCouterResetDailyJob> _logger;

        private readonly JT808UdpAtomicCounterService _jT808UdpAtomicCounterService;

        public JT808UdpAtomicCouterResetDailyJob(
            JT808UdpAtomicCounterService jT808UdpAtomicCounterService,
            ILoggerFactory loggerFactory)
        {
            _jT808UdpAtomicCounterService = jT808UdpAtomicCounterService;
            _logger =loggerFactory.CreateLogger<JT808UdpAtomicCouterResetDailyJob>();
        }

        public override string ServiceName => nameof(JT808UdpAtomicCouterResetDailyJob);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{ServiceName} is starting.");
            stoppingToken.Register(() => _logger.LogInformation($"{ServiceName} background task is stopping."));
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{ServiceName} task doing background work.");
                _jT808UdpAtomicCounterService.Reset();
                await Task.Delay(DelayTimeSpan, stoppingToken);
            }
            _logger.LogInformation($"{ServiceName} background task is stopping.");
        }
    }
}
