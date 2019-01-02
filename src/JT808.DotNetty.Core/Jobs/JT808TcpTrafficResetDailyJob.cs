using JT808.DotNetty.Core.Services;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.DotNetty.Core.Jobs
{
    public class JT808TcpTrafficResetDailyJob : JT808BackgroundService
    {
        private readonly ILogger<JT808TcpTrafficResetDailyJob> _logger;

        private readonly JT808TcpTrafficService _jT808TcpTrafficService;

        public JT808TcpTrafficResetDailyJob(
            JT808TcpTrafficService jT808TcpTrafficService,
            ILoggerFactory loggerFactory)
        {
            _jT808TcpTrafficService = jT808TcpTrafficService;
            _logger =loggerFactory.CreateLogger<JT808TcpTrafficResetDailyJob>();
        }

        public override string ServiceName => nameof(JT808TcpTrafficResetDailyJob);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{ServiceName} is starting.");
            stoppingToken.Register(() => _logger.LogInformation($"{ServiceName} background task is stopping."));
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{ServiceName} task doing background work.");
                _jT808TcpTrafficService.ResetSize();
                await Task.Delay(DelayTimeSpan, stoppingToken);
            }
            _logger.LogInformation($"{ServiceName} background task is stopping.");
        }
    }
}
