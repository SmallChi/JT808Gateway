using JT808.DotNetty.Core.Services;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.DotNetty.Core.Jobs
{
    internal class JT808TcpTrafficResetDailyJob : JT808BackgroundService
    {
        private readonly ILogger<JT808TcpTrafficResetDailyJob> _logger;

        private readonly JT808TrafficService _jT808TrafficService;

        public JT808TcpTrafficResetDailyJob(
            JT808TrafficServiceFactory  jT808TrafficServiceFactory,
            ILoggerFactory loggerFactory)
        {
            _jT808TrafficService = jT808TrafficServiceFactory.Create( Enums.JT808ModeType.Tcp);
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
                _jT808TrafficService.ResetSize();
                await Task.Delay(DelayTimeSpan, stoppingToken);
            }
            _logger.LogInformation($"{ServiceName} background task is stopping.");
        }
    }
}
