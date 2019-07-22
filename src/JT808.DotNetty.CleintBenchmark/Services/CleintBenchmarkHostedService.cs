using JT808.DotNetty.CleintBenchmark.Configs;
using JT808.DotNetty.Client;
using JT808.Protocol.MessageBody;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace JT808.DotNetty.CleintBenchmark.Services
{
    public class CleintBenchmarkHostedService : IHostedService
    {
        private readonly ClientBenchmarkOptions clientBenchmarkOptions;

        private readonly ILogger logger;

        private readonly IJT808TcpClientFactory jT808TcpClientFactory;

        private CancellationTokenSource cts=new CancellationTokenSource();

        private TaskFactory taskFactory;

        public CleintBenchmarkHostedService(
            ILoggerFactory loggerFactory,
            IJT808TcpClientFactory jT808TcpClientFactory,
            IOptions<ClientBenchmarkOptions> clientBenchmarkOptionsAccessor)
        {
            this.jT808TcpClientFactory = jT808TcpClientFactory;
            clientBenchmarkOptions = clientBenchmarkOptionsAccessor.Value;
            logger = loggerFactory.CreateLogger("CleintBenchmarkHostedService");
            taskFactory = new TaskFactory();
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("StartAsync...");
            ThreadPool.GetMinThreads(out var minWorkerThreads, out var minCompletionPortThreads);
            ThreadPool.GetMaxThreads(out var maxWorkerThreads, out var maxCompletionPortThreads);
            logger.LogInformation($"GetMinThreads:{minWorkerThreads}-{minCompletionPortThreads}");
            logger.LogInformation($"GetMaxThreads:{maxWorkerThreads}-{maxCompletionPortThreads}");
            //ThreadPool.SetMaxThreads(20, 20);
            //ThreadPool.GetMaxThreads(out var setMaxWorkerThreads, out var setMaxCompletionPortThreads);
            //logger.LogInformation($"SetMaxThreads:{setMaxWorkerThreads}-{setMaxCompletionPortThreads}");
            for (int i=0;i< clientBenchmarkOptions.DeviceCount; i++)
            {
                taskFactory.StartNew((item) => 
                {

                    var client = jT808TcpClientFactory.Create(new DeviceConfig(((int)item).ToString(), clientBenchmarkOptions.IP, clientBenchmarkOptions.Port));
                    int lat = new Random(1000).Next(100000, 180000);
                    int Lng = new Random(1000).Next(100000, 180000);
                    while (!cts.IsCancellationRequested)
                    {
                        client.Send(new JT808_0x0200()
                        {
                            Lat = lat,
                            Lng = Lng,
                            GPSTime = DateTime.Now,
                            Speed = 50,
                            Direction = 30,
                            AlarmFlag = 5,
                            Altitude = 50,
                            StatusFlag = 10
                        });
                        Thread.Sleep(clientBenchmarkOptions.Interval);
                    }
                }, i,cts.Token);
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            cts.Cancel();
            logger.LogInformation("StopAsync...");
            return Task.CompletedTask;
        }
    }
}
