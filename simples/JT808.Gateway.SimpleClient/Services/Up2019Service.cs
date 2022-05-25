using JT808.Gateway.Client;
using JT808.Protocol.MessageBody;
using JT808.Protocol.Enums;
using JT808.Protocol.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.Gateway.SimpleClient.Services
{
    public class Up2019Service : IHostedService
    {
        private readonly IJT808TcpClientFactory jT808TcpClientFactory;

        public Up2019Service(IJT808TcpClientFactory jT808TcpClientFactory)
        {
            this.jT808TcpClientFactory = jT808TcpClientFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            string sim = "22222222222";
            JT808TcpClient client1 = await jT808TcpClientFactory.Create(new JT808DeviceConfig(sim, "127.0.0.1", 808, version:JT808Version.JTT2019), cancellationToken);
            await Task.Delay(2000);
            //1.终端注册
            await client1.SendAsync(JT808MsgId._0x0100.Create2019(sim, new JT808_0x0100()
            {
                PlateNo = "粤A12346",
                PlateColor = 2,
                AreaID = 0,
                CityOrCountyId = 0,
                MakerId = "Koike002",
                TerminalId = "Koike002",
                TerminalModel = "Koike002"
            }));
            //2.终端鉴权
            await client1.SendAsync(JT808MsgId._0x0102.Create2019(sim, new JT808_0x0102()
            {
                Code = "6666",
                IMEI="123456",
                SoftwareVersion="v1.0.0"
            }));
            _ = Task.Run(async() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var i = 0;
                    //3.每5秒发一次
                    await client1.SendAsync(JT808MsgId._0x0200.Create2019(sim, new JT808_0x0200()
                    {
                        Lat = 110000 + i,
                        Lng = 100000 + i,
                        GPSTime = DateTime.Now,
                        Speed = 50,
                        Direction = 30,
                        AlarmFlag = 5,
                        Altitude = 50,
                        StatusFlag = 10
                    }));
                    i++;
                    await Task.Delay(5000, cancellationToken);
                }
            });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
