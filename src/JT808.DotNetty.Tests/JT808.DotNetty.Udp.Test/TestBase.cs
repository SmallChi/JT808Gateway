using JT808.DotNetty.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using JT808.Protocol;
using JT808.Protocol.Interfaces;
using DotNetty.Transport.Channels.Embedded;
using JT808.DotNetty.Core.Codecs;
using JT808.DotNetty.Udp.Handlers;
using JT808.Protocol.Extensions;
using DotNetty.Transport.Channels.Sockets;
using DotNetty.Buffers;
using System.Net;
using JT808.DotNetty.Core.Impls;

namespace JT808.DotNetty.Udp.Test
{
    public class TestBase
    {
        public IServiceProvider ServiceProvider;
        public JT808Serializer JT808Serializer;
        public TestBase()
        {
            IServiceCollection serviceDescriptors = new ServiceCollection();
            serviceDescriptors.AddSingleton<ILoggerFactory, LoggerFactory>();
            serviceDescriptors.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            serviceDescriptors.AddJT808Configure()
                                .AddJT808NettyCore(options => { })
                                .Builder();
            serviceDescriptors.AddJT808UdpNettyHostTest();
            ServiceProvider = serviceDescriptors.BuildServiceProvider();
            JT808Serializer = ServiceProvider.GetRequiredService<IJT808Config>().GetSerializer();
        }

        public EmbeddedChannel CreateEmbeddedChannel()
        {
            using (var soppe = ServiceProvider.CreateScope())
            {
                var handler1 = soppe.ServiceProvider.GetRequiredService<JT808UdpDecoder>();
                var handler2 = soppe.ServiceProvider.GetRequiredService<JT808UdpServerHandler>();
                var ch = new EmbeddedChannel(new JT808DefaultChannelId(),handler1, handler2);
                return ch;
            }
        }

        public Dictionary<string, EmbeddedChannel> Channels = new Dictionary<string, EmbeddedChannel>();

        public void SeedSession(params string[] terminalPhoneNos)
        {
            foreach(var item in terminalPhoneNos)
            {
                JT808Package jT808Package = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create(item);
                var ch = CreateEmbeddedChannel();
                DatagramPacket datagramPacket = new DatagramPacket(
                    Unpooled.CopiedBuffer(JT808Serializer.Serialize(jT808Package))
                    , new IPEndPoint(IPAddress.Any, 0)
                    , new IPEndPoint(IPAddress.Any, 0));
                ch.WriteInbound(datagramPacket);
                Channels.Add(item, ch);
            }
        }
    }
}
