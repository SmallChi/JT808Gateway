using JT808.DotNetty.CleintBenchmark.Configs;
using JT808.DotNetty.Client;
using JT808.Protocol.Enums;
using JT808.Protocol.Extensions;
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

        public CleintBenchmarkHostedService(
            ILoggerFactory loggerFactory,
            IJT808TcpClientFactory jT808TcpClientFactory,
            IOptions<ClientBenchmarkOptions> clientBenchmarkOptionsAccessor)
        {
            this.jT808TcpClientFactory = jT808TcpClientFactory;
            clientBenchmarkOptions = clientBenchmarkOptionsAccessor.Value;
            logger = loggerFactory.CreateLogger("CleintBenchmarkHostedService");
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("StartAsync...");
            ThreadPool.GetMinThreads(out var minWorkerThreads, out var minCompletionPortThreads);
            ThreadPool.GetMaxThreads(out var maxWorkerThreads, out var maxCompletionPortThreads);
            logger.LogInformation($"GetMinThreads:{minWorkerThreads}-{minCompletionPortThreads}");
            logger.LogInformation($"GetMaxThreads:{maxWorkerThreads}-{maxCompletionPortThreads}");
            //先建立连接
            for (int i=0;i< clientBenchmarkOptions.DeviceCount; i++)
            {
                var client = jT808TcpClientFactory.Create(new JT808DeviceConfig((i+1+ clientBenchmarkOptions.DeviceTemplate).ToString(), 
                    clientBenchmarkOptions.IP, 
                    clientBenchmarkOptions.Port));
            }

            ThreadPool.QueueUserWorkItem((state) =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    Parallel.ForEach(jT808TcpClientFactory.GetAll(), new ParallelOptions { MaxDegreeOfParallelism = 100 }, (item) =>
                    {
                        try
                        {
                            int lat = new Random(1000).Next(100000, 180000);
                            int Lng = new Random(1000).Next(100000, 180000);
                            item.Send(JT808MsgId.位置信息汇报.Create(item.DeviceConfig.TerminalPhoneNo, new JT808_0x0200()
                            {
                                Lat = lat,
                                Lng = Lng,
                                GPSTime = DateTime.Now,
                                Speed = 50,
                                Direction = 30,
                                AlarmFlag = 5,
                                Altitude = 50,
                                StatusFlag = 10
                            }));
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex.Message);
                        }
                    });
                    Thread.Sleep(clientBenchmarkOptions.Interval);
                }
            });
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            jT808TcpClientFactory.Dispose();
            logger.LogInformation("StopAsync...");
            return Task.CompletedTask;
        }
    }
}
