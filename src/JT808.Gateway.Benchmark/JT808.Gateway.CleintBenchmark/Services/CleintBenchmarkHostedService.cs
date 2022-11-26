using JT808.Gateway.CleintBenchmark.Configs;
using JT808.Gateway.Client;
using JT808.Protocol.Enums;
using JT808.Protocol.Extensions;
using JT808.Protocol.MessageBody;
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


namespace JT808.Gateway.CleintBenchmark.Services
{
    public class CleintBenchmarkHostedService : IHostedService
    {
        private readonly ClientBenchmarkOptions clientBenchmarkOptions;

        private readonly ILogger logger;

        private readonly IJT808TcpClientFactory jT808TcpClientFactory;

        private TaskFactory taskFactory;

        public CleintBenchmarkHostedService(
            ILoggerFactory loggerFactory,
            IJT808TcpClientFactory jT808TcpClientFactory,
            IOptions<ClientBenchmarkOptions> clientBenchmarkOptionsAccessor)
        {
            this.jT808TcpClientFactory = jT808TcpClientFactory;
            clientBenchmarkOptions = clientBenchmarkOptionsAccessor.Value;
            logger = loggerFactory.CreateLogger<CleintBenchmarkHostedService>();

        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("StartAsync...");
            ThreadPool.GetMinThreads(out var minWorkerThreads, out var minCompletionPortThreads);
            ThreadPool.GetMaxThreads(out var maxWorkerThreads, out var maxCompletionPortThreads);
            logger.LogInformation($"GetMinThreads:{minWorkerThreads}-{minCompletionPortThreads}");
            logger.LogInformation($"GetMaxThreads:{maxWorkerThreads}-{maxCompletionPortThreads}");
            taskFactory = new TaskFactory(cancellationToken, TaskCreationOptions.PreferFairness, TaskContinuationOptions.PreferFairness, TaskScheduler.Default);
            Task.Run(() => {
                for (int i = 0; i < clientBenchmarkOptions.DeviceCount; i++)
                {
                    taskFactory.StartNew(async (state) => {
                        string deviceNo = ((int)state + 1 + clientBenchmarkOptions.DeviceTemplate).ToString();
                        var client = await jT808TcpClientFactory.Create(new JT808DeviceConfig(deviceNo,
                            clientBenchmarkOptions.IP,
                            clientBenchmarkOptions.Port,
                            clientBenchmarkOptions.LocalIPAddress,
                            clientBenchmarkOptions.LocalPort + (int)state + 1), cancellationToken);
                        while (!cancellationToken.IsCancellationRequested)
                        {
                            try
                            {
                                int lat = new Random(1000).Next(100000, 180000);
                                int Lng = new Random(1000).Next(100000, 180000);
                                if (client != null)
                                {
                                    await client.SendAsync(JT808MsgId._0x0200.Create(client.DeviceConfig.TerminalPhoneNo, new JT808_0x0200()
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
                            }
                            catch (Exception ex)
                            {
                                logger.LogError(ex.Message);
                            }
                            await Task.Delay(TimeSpan.FromMilliseconds(clientBenchmarkOptions.Interval));
                        }
                    }, i,cancellationToken, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
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
