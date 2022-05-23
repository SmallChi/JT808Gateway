using JT808.Gateway.Client;
using JT808.Protocol.Enums;
using JT808.Protocol.Extensions;
using JT808.Protocol.MessageBody;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.Gateway.NormalHosting.Jobs
{
    public class UpJob : IHostedService
    {
        private readonly IJT808TcpClientFactory jT808TcpClientFactory;
        private readonly ILogger Logger;
        public UpJob(
            ILoggerFactory loggerFactory,
            IJT808TcpClientFactory jT808TcpClientFactory)
        {
            Logger = loggerFactory.CreateLogger<UpJob>();
            this.jT808TcpClientFactory = jT808TcpClientFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                await Task.Delay(2 * 1000);
                var client = await jT808TcpClientFactory.Create(new JT808DeviceConfig("1234567890", "127.0.0.1", 808), cancellationToken);
                var client1 = await jT808TcpClientFactory.Create(new JT808DeviceConfig("1234567891", "127.0.0.1", 808), cancellationToken);
                if (client != null && client1!=null)
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        try
                        {
                            int lat = new Random(1000).Next(100000, 180000);
                            int Lng = new Random(1000).Next(100000, 180000);
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

                            int lat1 = new Random(1000).Next(100000, 180000);
                            int Lng1 = new Random(1000).Next(100000, 180000);
                            await client1.SendAsync(JT808MsgId._0x0200.Create(client1.DeviceConfig.TerminalPhoneNo, new JT808_0x0200()
                            {
                                Lat = lat1,
                                Lng = Lng1,
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
                            Logger.LogError(ex.Message);
                        }
                        await Task.Delay(3 * 1000);
                    }
                }
            }, cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            jT808TcpClientFactory.Dispose();
            return Task.CompletedTask;
        }
    }
}
