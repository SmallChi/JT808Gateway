using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using JT808.DotNetty.Client.Handlers;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using JT808.DotNetty.Client.Metadata;
using JT808.DotNetty.Client.Codecs;
using JT808.DotNetty.Client.Services;
using JT808.Protocol;
using System.Threading.Tasks;

namespace JT808.DotNetty.Client
{
    public sealed class JT808TcpClient : IDisposable
    {
        private MultithreadEventLoopGroup group;

        private IChannel clientChannel;

        private bool disposed = false;

        public JT808DeviceConfig DeviceConfig { get; private set; }

        public ILoggerFactory LoggerFactory { get; private set; }

        public JT808TcpClient(JT808DeviceConfig deviceConfig, IServiceProvider serviceProvider)
        {
            DeviceConfig = deviceConfig;
            LoggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            JT808SendAtomicCounterService jT808SendAtomicCounterService = serviceProvider.GetRequiredService<JT808SendAtomicCounterService>();
            JT808ReceiveAtomicCounterService jT808ReceiveAtomicCounterService = serviceProvider.GetRequiredService<JT808ReceiveAtomicCounterService>();
            IJT808Config jT808Config = serviceProvider.GetRequiredService<IJT808Config>();
            group = new MultithreadEventLoopGroup(1);
            Bootstrap bootstrap = new Bootstrap();
            bootstrap.Group(group);
            bootstrap.Channel<TcpSocketChannel>();
            bootstrap
               .Option(ChannelOption.Allocator, new UnpooledByteBufferAllocator())
               .Handler(new ActionChannelInitializer<IChannel>(channel =>
               {
                   channel.Pipeline.AddLast("jt808TcpBuffer", new DelimiterBasedFrameDecoder(65535,
                        Unpooled.CopiedBuffer(new byte[] { JT808.Protocol.JT808Package.BeginFlag }),
                        Unpooled.CopiedBuffer(new byte[] { JT808.Protocol.JT808Package.EndFlag })));
                   channel.Pipeline.AddLast("systemIdleState", new IdleStateHandler(60, deviceConfig.Heartbeat, 3600));
                   channel.Pipeline.AddLast("jt808TcpDecode", new JT808ClientTcpDecoder());
                   channel.Pipeline.AddLast("jt808TcpEncode", new JT808ClientTcpEncoder(jT808Config,jT808SendAtomicCounterService, LoggerFactory));
                   channel.Pipeline.AddLast("jt808TcpClientConnection", new JT808TcpClientConnectionHandler(this));
                   channel.Pipeline.AddLast("jt808TcpService", new JT808TcpClientHandler(jT808ReceiveAtomicCounterService,this));
               }));
            Task.Run(async () =>
            {
                clientChannel = await bootstrap.ConnectAsync(IPAddress.Parse(DeviceConfig.TcpHost), DeviceConfig.TcpPort);
            });
        }

        public  void Send(JT808ClientRequest request)
        {
            if (disposed) return;
            if (clientChannel == null) throw new NullReferenceException("Channel is empty.");
            if (request == null) throw new ArgumentNullException("JT808ClientRequest Parameter is empty.");
            if (clientChannel.Active && clientChannel.Open)
            {
                 clientChannel.WriteAndFlushAsync(request);
            }
        }

        public bool IsOpen
        {
            get
            {
                if (clientChannel == null) return false;
                return clientChannel.Active && clientChannel.Open;
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
