using JT808.DotNetty.Core.Services;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.DotNetty.Core.Jobs
{
    public class JT808UdpTrafficResetDailyJob : JT808BackgroundService
    {
        private readonly ILogger<JT808UdpTrafficResetDailyJob> _logger;

        private readonly JT808UdpTrafficService _jT808UdpTrafficService;

        public JT808UdpTrafficResetDailyJob(
            JT808UdpTrafficService jT808UdpTrafficService,
            ILoggerFactory loggerFactory)
        {
            _jT808UdpTrafficService = jT808UdpTrafficService;
            _logger =loggerFactory.CreateLogger<JT808UdpTrafficResetDailyJob>();
        }

        public override string ServiceName => nameof(JT808UdpTrafficResetDailyJob);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{ServiceName} is starting.");
            stoppingToken.Register(() => _logger.LogInformation($"{ServiceName} background task is stopping."));
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{ServiceName} task doing background work.");
                _jT808UdpTrafficService.ResetSize();
                await Task.Delay(DelayTimeSpan, stoppingToken);
            }
            _logger.LogInformation($"{ServiceName} background task is stopping.");
        }
    }
}
