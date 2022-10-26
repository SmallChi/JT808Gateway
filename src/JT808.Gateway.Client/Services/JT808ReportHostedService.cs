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
using System.Threading;
using System.Threading.Tasks;

namespace JT808.Gateway.Client.Services
{
    public class JT808ReportHostedService : BackgroundService
    {
        private readonly IOptionsMonitor<JT808ReportOptions> jT808ReportOptions;
        private readonly JT808ReceiveAtomicCounterService jT808ReceiveAtomicCounterService;
        private readonly JT808SendAtomicCounterService jT808SendAtomicCounterService;
        private readonly IJT808TcpClientFactory jT808TcpClientFactory;
        private readonly ILogger logger;

        public JT808ReportHostedService(
            ILoggerFactory loggerFactory,
            IOptionsMonitor<JT808ReportOptions> jT808ReportOptionsAccessor,
            JT808ReceiveAtomicCounterService jT808ReceiveAtomicCounterService,
            JT808SendAtomicCounterService jT808SendAtomicCounterService,
            IJT808TcpClientFactory jT808TcpClientFactory)
        {
            logger = loggerFactory.CreateLogger<JT808ReportHostedService>();
            jT808ReportOptions = jT808ReportOptionsAccessor;
            jT808ReportOptions.CurrentValue.FileExistsAndCreate();
            this.jT808ReceiveAtomicCounterService = jT808ReceiveAtomicCounterService;
            this.jT808SendAtomicCounterService = jT808SendAtomicCounterService;
            this.jT808TcpClientFactory = jT808TcpClientFactory;
            jT808ReportOptions.OnChange((options) => { options.FileExistsAndCreate(); });
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(jT808ReportOptions.CurrentValue.Interval), stoppingToken);
                var clients = jT808TcpClientFactory.GetAll();
                JT808Report report = new JT808Report()
                {
                    SendTotalCount = jT808SendAtomicCounterService.MsgSuccessCount,
                    ReceiveTotalCount = jT808ReceiveAtomicCounterService.MsgSuccessCount,
                    CurrentDate = DateTime.Now,
                    Connections = clients.Count,
                    OnlineConnections = clients.Where(w => w.IsOpen).Count(),
                    OfflineConnections = clients.Where(w => !w.IsOpen).Count(),
                };
                string json = JsonSerializer.Serialize(report);
                if (logger.IsEnabled(LogLevel.Debug))
                {
                    logger.LogDebug(json);
                }
                using (var sw=new StreamWriter(jT808ReportOptions.CurrentValue.FileFullPath,true))
                {
                    sw.WriteLine(json);
                }
            }
        }
    }
}
