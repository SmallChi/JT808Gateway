using JT808.DotNetty.Core.Services;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.DotNetty.Core.Jobs
{
    public class JT808TcpAtomicCouterResetDailyJob : JT808BackgroundService
    {
        private readonly ILogger<JT808TcpAtomicCouterResetDailyJob> _logger;

        private readonly JT808TcpAtomicCounterService _jT808TcpAtomicCounterService;

        public JT808TcpAtomicCouterResetDailyJob(
            JT808TcpAtomicCounterService jT808TcpAtomicCounterService,
            ILoggerFactory loggerFactory)
        {
            _jT808TcpAtomicCounterService = jT808TcpAtomicCounterService;
            _logger =loggerFactory.CreateLogger<JT808TcpAtomicCouterResetDailyJob>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(JT808TcpAtomicCouterResetDailyJob)} is starting.");
            stoppingToken.Register(() => _logger.LogInformation($"{nameof(JT808TcpAtomicCouterResetDailyJob)} background task is stopping."));
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(JT808TcpAtomicCouterResetDailyJob)} task doing background work.");
                _jT808TcpAtomicCounterService.Reset();
                await Task.Delay(DelayTimeSpan, stoppingToken);
            }
            _logger.LogInformation($"{nameof(JT808TcpAtomicCouterResetDailyJob)} background task is stopping.");
        }
    }
}
