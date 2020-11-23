using JT808.Gateway.CleintBenchmark.Configs;
using JT808.Gateway.CleintBenchmark.Hubs;
using JT808.Gateway.Client;
using JT808.Gateway.Client.Metadata;
using JT808.Gateway.Client.Services;
using JT808.Protocol.Enums;
using JT808.Protocol.Extensions;
using JT808.Protocol.MessageBody;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;


namespace JT808.Gateway.CleintBenchmark.Services
{
    public class QueryReportHostedService : BackgroundService
    {
        private readonly IJT808TcpClientFactory clientFactory;
        private readonly JT808ReceiveAtomicCounterService ReceiveAtomicCounterService;
        private readonly JT808SendAtomicCounterService SendAtomicCounterService;
        private readonly ILogger logger;
        private readonly IHubContext<ReportHub> _hubContext;

        public QueryReportHostedService(
            ILoggerFactory loggerFactory,
            IHubContext<ReportHub> hubContext,
             JT808ReceiveAtomicCounterService jT808ReceiveAtomicCounterService,
            JT808SendAtomicCounterService jT808SendAtomicCounterService,
            IJT808TcpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
            this._hubContext = hubContext;
            logger = loggerFactory.CreateLogger<QueryReportHostedService>();
            ReceiveAtomicCounterService = jT808ReceiveAtomicCounterService;
            SendAtomicCounterService = jT808SendAtomicCounterService;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var clients = clientFactory.GetAll();
                    JT808Report report = new JT808Report()
                    {
                        SendTotalCount = SendAtomicCounterService.MsgSuccessCount,
                        ReceiveTotalCount = ReceiveAtomicCounterService.MsgSuccessCount,
                        CurrentDate = DateTime.Now,
                        Connections = clients.Count,
                        OnlineConnections = clients.Where(w => w.IsOpen).Count(),
                        OfflineConnections = clients.Where(w => !w.IsOpen).Count(),
                    };
                    await _hubContext.Clients.All.SendAsync("ReceiveMessage", "query", JsonSerializer.Serialize(report));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "");
                }
                finally
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                }
            }
        }
    }
}
