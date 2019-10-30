using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels.Embedded;
using JT808.DotNetty.Core;
using JT808.DotNetty.Core.Codecs;
using JT808.DotNetty.Core.Impls;
using JT808.DotNetty.Core.Metadata;
using JT808.DotNetty.Tcp.Handlers;
using JT808.Protocol;
using JT808.Protocol.Extensions;
using JT808.Protocol.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Tcp.Test
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
            serviceDescriptors.AddJT808TcpNettyHostTest();
            ServiceProvider = serviceDescriptors.BuildServiceProvider();
            JT808Serializer = ServiceProvider.GetRequiredService<IJT808Config>().GetSerializer();
        }

        public EmbeddedChannel CreateEmbeddedChannel()
        {
            using (var soppe = ServiceProvider.CreateScope())
            {
                var handler1 = soppe.ServiceProvider.GetRequiredService<JT808TcpEncoder>();
                var handler2 = soppe.ServiceProvider.GetRequiredService<JT808TcpDecoder>();
                var handler3 = soppe.ServiceProvider.GetRequiredService<JT808TcpServerHandler>();
                var ch = new EmbeddedChannel(new JT808DefaultChannelId(), 
                                    new DelimiterBasedFrameDecoder(int.MaxValue,Unpooled.CopiedBuffer(new byte[] { JT808.Protocol.JT808Package.BeginFlag }),Unpooled.CopiedBuffer(new byte[] { JT808.Protocol.JT808Package.EndFlag })), 
                                    handler1, 
                                    handler2, 
                                    handler3);
                return ch;
            }
        }

        public Dictionary<string, EmbeddedChannel> Channels = new Dictionary<string, EmbeddedChannel>();

        public void SeedSession(params string[] terminalPhoneNos)
        {
            foreach (var item in terminalPhoneNos)
            {
                JT808Package jT808Package = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create(item);
                var tmp1=JT808Serializer.Serialize(jT808Package);
                var ch = CreateEmbeddedChannel();
                ch.WriteInbound(Unpooled.CopiedBuffer(tmp1));
                Channels.Add(item, ch);
            }
        }
    }
}
