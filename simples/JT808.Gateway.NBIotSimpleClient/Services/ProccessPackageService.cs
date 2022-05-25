using JT808.Gateway.Client;
using JT808.Protocol;
using JT808.Protocol.Enums;
using JT808.Protocol.Extensions;
using JT808.Protocol.MessageBody;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.Gateway.NBIotSimpleClient.Services
{
    /// <summary>
    /// 处理平台下发的数据包
    /// </summary>
    public class ProccessPackageService : IHostedService
    {
        ReceviePackageService ReceviePackageService;
        IJT808TcpClientFactory TcpClientFactory;
        JT808Serializer Serializer;
        DeviceInfoService DeviceInfoService;
        ILogger Logger;
        public ProccessPackageService(
            ILoggerFactory loggerFactory,
            IJT808Config jT808Config,
            IJT808TcpClientFactory tcpClientFactory,
            DeviceInfoService deviceInfoService,
            ReceviePackageService receviePackageService)
        {
            ReceviePackageService = receviePackageService;
            TcpClientFactory = tcpClientFactory;
            Serializer = jT808Config.GetSerializer();
            DeviceInfoService = deviceInfoService;
            Logger = loggerFactory.CreateLogger<ProccessPackageService>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() =>
            {
                try
                {
                    foreach(var package in ReceviePackageService.BlockingCollection.GetConsumingEnumerable(cancellationToken))
                    {
                        if(package.Header.MsgId == JT808MsgId._0x8100.ToUInt16Value())
                        {
                            if (package.Bodies is JT808_0x8100 body)
                            {
                                DeviceInfoService.DeviceInfos.TryAdd(package.Header.TerminalPhoneNo, new Metadata.DeviceInfo {
                                    TerminalPhoneNo = package.Header.TerminalPhoneNo,
                                    Code = body.Code
                                });
                                Logger.LogInformation($"{package.Header.TerminalPhoneNo}-{body.Code}-success");
                            }
                        }
                        else if (package.Header.MsgId == JT808MsgId._0x8001.ToValue())
                        {
                            if(package.Bodies is  JT808_0x8001 body)
                            {
                                if(body.AckMsgId== JT808MsgId._0x0102.ToUInt16Value()) 
                                { 
                                    if(body.JT808PlatformResult== JT808PlatformResult.succeed)
                                    {
                                        if (DeviceInfoService.DeviceInfos.TryGetValue(package.Header.TerminalPhoneNo, out var deviceInfo))
                                        {
                                            deviceInfo.Successed = true;
                                            DeviceInfoService.DeviceInfos.TryUpdate(package.Header.TerminalPhoneNo, deviceInfo, deviceInfo);
                                        }
                                    }
                                    Logger.LogInformation($"{package.Header.TerminalPhoneNo}-{body.JT808PlatformResult.ToString()}");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            });
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
