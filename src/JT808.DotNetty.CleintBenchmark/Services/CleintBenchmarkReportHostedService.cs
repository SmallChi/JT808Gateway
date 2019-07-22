using JT808.DotNetty.CleintBenchmark.Configs;
using JT808.DotNetty.Client;
using JT808.DotNetty.Client.Services;
using JT808.Protocol.MessageBody;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace JT808.DotNetty.CleintBenchmark.Services
{
    public class CleintBenchmarkReportHostedService : IHostedService
    {
        private readonly JT808ReportService jT808ReportService;

        private CancellationTokenSource cts=new CancellationTokenSource();

        private readonly ILogger logger;
        public CleintBenchmarkReportHostedService(
            ILoggerFactory loggerFactory,
            JT808ReportService jT808ReportService)
        {
            this.jT808ReportService = jT808ReportService;
            logger = loggerFactory.CreateLogger("CleintBenchmarkReportHostedService");
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("StartAsync...");
            Task.Run(() => {
                while (!cts.IsCancellationRequested)
                {
                    logger.LogInformation(JsonConvert.SerializeObject(jT808ReportService.JT808Reports.LastOrDefault()));
                    Thread.Sleep(3000);
                }
            }, cts.Token);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("StopAsync...");
            cts.Cancel();
            logger.LogInformation("正在生成报表...");
            logger.LogInformation(JsonConvert.SerializeObject(jT808ReportService.JT808Reports,Formatting.Indented));
            return Task.CompletedTask;
        }
    }
}
