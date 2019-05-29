using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using DotNetty.Transport.Libuv;
using JT808.DotNetty.Client.Handlers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Threading.Tasks;
using JT808.DotNetty.Client.Metadata;
using JT808.DotNetty.Client.Codecs;

namespace JT808.DotNetty.Client
{
    public sealed class JT808TcpClient : IDisposable
    {
        private MultithreadEventLoopGroup group;

        private IChannel clientChannel;

        private bool disposed = false;

        public DeviceConfig DeviceConfig { get; private set; }

        public ILoggerFactory LoggerFactory { get; private set; }

        public JT808TcpClient(DeviceConfig deviceConfig, IServiceProvider serviceProvider)
        {
            DeviceConfig = deviceConfig;
            LoggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            group = new MultithreadEventLoopGroup(1);
            Bootstrap bootstrap = new Bootstrap();
            bootstrap.Group(group);
            bootstrap.Channel<TcpSocketChannel>();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                bootstrap.Option(ChannelOption.SoReuseport, true);
            }
            bootstrap
               .Option(ChannelOption.SoBacklog, 8192)
               .Handler(new ActionChannelInitializer<IChannel>(channel =>
               {
                   channel.Pipeline.AddLast("jt808TcpBuffer", new DelimiterBasedFrameDecoder(int.MaxValue,
                        Unpooled.CopiedBuffer(new byte[] { JT808.Protocol.JT808Package.BeginFlag }),
                        Unpooled.CopiedBuffer(new byte[] { JT808.Protocol.JT808Package.EndFlag })));
                   channel.Pipeline.AddLast("systemIdleState", new IdleStateHandler(60, deviceConfig.Heartbeat, 3600));
                   channel.Pipeline.AddLast("jt808TcpDecode", new JT808ClientTcpDecoder());
                   channel.Pipeline.AddLast("jt808TcpEncode", new JT808ClientTcpEncoder(LoggerFactory));
                   channel.Pipeline.AddLast("jt808TcpClientConnection", new JT808TcpClientConnectionHandler(this));
                   channel.Pipeline.AddLast("jt808TcpService", new JT808TcpClientHandler(this));
               }));
            clientChannel = bootstrap.ConnectAsync(IPAddress.Parse(DeviceConfig.TcpHost), DeviceConfig.TcpPort).Result;
        }

        public async void Send(JT808ClientRequest request)
        {
            if (disposed) return;
            if (clientChannel == null) throw new NullReferenceException("Channel is empty.");
            if (request == null) throw new ArgumentNullException("JT808ClientRequest Parameter is empty.");
            if (clientChannel.Active && clientChannel.Open)
            {
                await clientChannel.WriteAndFlushAsync(request);
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                // 清理托管资源
                group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
            }
            disposed = true;
        }

        ~JT808TcpClient()
        {
            //必须为false
            //这表明，隐式清理时，只要处理非托管资源就可以了。
            Dispose(false);
        }

        public void Dispose()
        {
            //必须为true
            Dispose(true);
            //通知垃圾回收机制不再调用终结器（析构器）
            GC.SuppressFinalize(this);
        }
    }
}
