using JT808.DotNetty.Core;
using JT808.DotNetty.Udp;
using JT808.DotNetty.Tcp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using JT808.DotNetty.WebApiClientTool;
using System.Net;
using JT808.Protocol;
using JT808.Protocol.Extensions;
using System.Threading;
using JT808.Protocol.Interfaces;

namespace JT808.DotNetty.WebApi.Test
{
    public class TestBase
    {
        public static IServiceProvider ServiceProvider;
        public static JT808Serializer JT808Serializer;
        static TestBase()
        {
            var serverHostBuilder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<JT808DotNettyWebApiOptions>(hostContext.Configuration.GetSection("JT808DotNettyWebApiOptions"));
                    services.AddSingleton<ILoggerFactory, LoggerFactory>();
                    services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                    services.AddJT808Configure()
                            .AddJT808NettyCore(hostContext.Configuration)
                            .AddJT808TcpNettyHost()
                            .Builder()
                            .AddJT808UdpNettyHost()
                            .Builder()
                            .AddJT808WebApiNettyHost();
                });
            var build = serverHostBuilder.Build();
            build.Start();
            ServiceProvider = build.Services;
            JT808Serializer = ServiceProvider.GetRequiredService<IJT808Config>().GetSerializer();
        }

        static IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12808);

        static IPEndPoint endPoint1 = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12818);

        JT808SimpleTcpClient SimpleTcpClient1;
        JT808SimpleTcpClient SimpleTcpClient2;
        JT808SimpleTcpClient SimpleTcpClient3;
        JT808SimpleTcpClient SimpleTcpClient4;
        JT808SimpleTcpClient SimpleTcpClient5;


        JT808SimpleUdpClient SimpleUdpClient1;
        JT808SimpleUdpClient SimpleUdpClient2;
        JT808SimpleUdpClient SimpleUdpClient3;
        JT808SimpleUdpClient SimpleUdpClient4;
        JT808SimpleUdpClient SimpleUdpClient5;




        public TestBase()
        {
            SimpleTcpClient1 = new JT808SimpleTcpClient(endPoint);
            SimpleTcpClient2 = new JT808SimpleTcpClient(endPoint);
            SimpleTcpClient3 = new JT808SimpleTcpClient(endPoint);
            SimpleTcpClient4 = new JT808SimpleTcpClient(endPoint);
            SimpleTcpClient5 = new JT808SimpleTcpClient(endPoint);
            // 心跳会话包
            JT808Package jT808Package1 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("123456789001");
            SimpleTcpClient1.WriteAsync(JT808Serializer.Serialize(jT808Package1));

            // 心跳会话包
            JT808Package jT808Package2 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("123456789002");
            SimpleTcpClient2.WriteAsync(JT808Serializer.Serialize(jT808Package2));

            // 心跳会话包
            JT808Package jT808Package3 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("123456789003");
            SimpleTcpClient3.WriteAsync(JT808Serializer.Serialize(jT808Package3));

            // 心跳会话包
            JT808Package jT808Package4 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("123456789004");
            SimpleTcpClient4.WriteAsync(JT808Serializer.Serialize(jT808Package4));

            // 心跳会话包
            JT808Package jT808Package5 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("123456789005");
            SimpleTcpClient5.WriteAsync(JT808Serializer.Serialize(jT808Package5));

            SimpleUdpClient1 = new JT808SimpleUdpClient(endPoint1);
            SimpleUdpClient2 = new JT808SimpleUdpClient(endPoint1);
            SimpleUdpClient3 = new JT808SimpleUdpClient(endPoint1);
            SimpleUdpClient4 = new JT808SimpleUdpClient(endPoint1);
            SimpleUdpClient5 = new JT808SimpleUdpClient(endPoint1);
            // 心跳会话包
            JT808Package jT808Package12 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("123456789011");
            SimpleUdpClient1.WriteAsync(JT808Serializer.Serialize(jT808Package12));
            Thread.Sleep(300);
            // 心跳会话包
            JT808Package jT808Package23 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("123456789012");
            SimpleUdpClient2.WriteAsync(JT808Serializer.Serialize(jT808Package23));
            Thread.Sleep(300);
            // 心跳会话包
            JT808Package jT808Package34 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("123456789013");
            SimpleUdpClient3.WriteAsync(JT808Serializer.Serialize(jT808Package34));
            Thread.Sleep(300);
            // 心跳会话包
            JT808Package jT808Package45 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("123456789014");
            SimpleUdpClient4.WriteAsync(JT808Serializer.Serialize(jT808Package45));
            Thread.Sleep(300);
            // 心跳会话包
            JT808Package jT808Package56 = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("123456789015");
            SimpleUdpClient5.WriteAsync(JT808Serializer.Serialize(jT808Package56));

            Thread.Sleep(300);
        }

        public void Dispose()
        {
            SimpleTcpClient1.Down();
            SimpleTcpClient2.Down();
            SimpleTcpClient3.Down();
            SimpleTcpClient4.Down();
            SimpleTcpClient5.Down();

            SimpleUdpClient1.Down();
            SimpleUdpClient2.Down();
            SimpleUdpClient3.Down();
            SimpleUdpClient4.Down();
            SimpleUdpClient5.Down();
        }
    }
}
