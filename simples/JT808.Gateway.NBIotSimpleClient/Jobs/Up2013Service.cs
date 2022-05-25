using JT808.Gateway.Client;
using JT808.Protocol.MessageBody;
using JT808.Protocol;
using JT808.Protocol.Enums;
using JT808.Protocol.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using JT808.Gateway.NBIotSimpleClient.Services;

namespace JT808.Gateway.NBIotSimpleClient.Jobs
{
    public class Up2013Service : IHostedService
    {
        IJT808TcpClientFactory jT808TcpClientFactory;
        JT808Serializer  Serializer;
        DeviceInfoService DeviceInfoService;
        public Up2013Service(
             DeviceInfoService deviceInfoService,
            IJT808Config jT808Config,
            IJT808TcpClientFactory jT808TcpClientFactory)
        {
            this.jT808TcpClientFactory = jT808TcpClientFactory;
            Serializer = jT808Config.GetSerializer();
            DeviceInfoService = deviceInfoService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            string sim = "22222222222";
            var address = Dns.GetHostAddresses("jtt808.ctwing.cn");
            JT808TcpClient client1 = await jT808TcpClientFactory.Create(new JT808DeviceConfig(sim, address[0].ToString(), 6001, version:JT808Version.JTT2019), cancellationToken);
            await Task.Delay(1000);
            var p1 = JT808MsgId._0x0100.Create(sim, new JT808_0x0100()
            {
                PlateNo = "粤A12346",
                PlateColor = 0,
                AreaID = 0,
                CityOrCountyId = 0, 
                MakerId = "12345",         //制造商编号
                TerminalModel = "123456",  //设备型号
                TerminalId = "1234567",    //设备编号
            });
            var p1_1 = Serializer.Serialize(p1).ToHexString();
            //1.终端注册
            await client1.SendAsync(p1);
            _ = Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(3000);
                    if (DeviceInfoService.DeviceInfos.TryGetValue(sim, out var deviceInfo))
                    {
                        if (deviceInfo.Successed)
                        {
                            break;
                        }
                        if (!string.IsNullOrEmpty(deviceInfo.Code))
                        {
                            //2.终端鉴权
                            await client1.SendAsync(JT808MsgId._0x0102.Create(sim, new JT808_0x0102()
                            {
                                 Code= deviceInfo.Code,
                            }));
                        }
                    }
                }
            }, cancellationToken);
            _ = Task.Run(async() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var i = 0;
                    //3.每5秒发一次
                    if(DeviceInfoService.DeviceInfos.TryGetValue(sim,out var deviceInfo))
                    {
                        if (deviceInfo.Successed)
                        {
                            await client1.SendAsync(JT808MsgId._0x0200.Create(sim, new JT808_0x0200()
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
                        }
                    }
                    i++;
                    await Task.Delay(5000, cancellationToken);
                }
            }, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
