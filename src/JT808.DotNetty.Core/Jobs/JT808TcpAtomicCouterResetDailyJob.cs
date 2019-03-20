using JT808.DotNetty.Core.Services;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.DotNetty.Core.Jobs
{
    internal class JT808TcpAtomicCouterResetDailyJob : JT808BackgroundService
    {
        private readonly ILogger<JT808TcpAtomicCouterResetDailyJob> _logger;

        private readonly JT808AtomicCounterService _jT808AtomicCounterService;

        public JT808TcpAtomicCouterResetDailyJob(
            JT808AtomicCounterServiceFactory  jT808AtomicCounterServiceFactory,
            ILoggerFactory loggerFactory)
        {
            _jT808AtomicCounterService = jT808AtomicCounterServiceFactory.Create(Enums.JT808ModeType.Tcp);
            _logger =loggerFactory.CreateLogger<JT808TcpAtomicCouterResetDailyJob>();
        }

        public override string ServiceName => nameof(JT808TcpAtomicCouterResetDailyJob);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{ServiceName} is starting.");
            stoppingToken.Register(() => _logger.LogInformation($"{ServiceName} background task is stopping."));
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{ServiceName} task doing background work.");
                _jT808AtomicCounterService.Reset();
                await Task.Delay(DelayTimeSpan, stoppingToken);
            }
            _logger.LogInformation($"{ServiceName} background task is stopping.");
        }
    }
}
