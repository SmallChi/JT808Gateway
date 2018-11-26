using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xunit;
using JT808.Protocol.Extensions;
using System.Threading;
using DotNetty.Transport.Channels;
using System.Runtime.InteropServices;
using DotNetty.Transport.Libuv;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Buffers;
using DotNetty.Codecs;
using JT808.DotNetty.Codecs;

namespace JT808.DotNetty.Test.Internal
{
    public class JT808SourcePackageDispatcherDefaultImplTest: TestBase
    {
        private IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6565);

        public  JT808SimpleTcpClient SimpleTcpClient;

        public JT808SourcePackageDispatcherDefaultImplTest()
        {
            SimpleTcpClient = new JT808SimpleTcpClient(endPoint);
        }

        [Fact]
        public void Test1()
        {
            // 作为源包转发服务端
            DispatcherEventLoopGroup bossGroup = new DispatcherEventLoopGroup();
            WorkerEventLoopGroup workerGroup = new WorkerEventLoopGroup(bossGroup, 1);
            ServerBootstrap bootstrap = new ServerBootstrap();
            bootstrap.Group(bossGroup, workerGroup);
            bootstrap.Channel<TcpServerChannel>();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                bootstrap
                    .Option(ChannelOption.SoReuseport, true)
                    .ChildOption(ChannelOption.SoReuseaddr, true);
            }
            bootstrap
               .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
               {
                   IChannelPipeline pipeline = channel.Pipeline;
                       channel.Pipeline.AddLast("jt808Buffer", new DelimiterBasedFrameDecoder(int.MaxValue,
                           Unpooled.CopiedBuffer(new byte[] { JT808.Protocol.JT808Package.BeginFlag }),
                           Unpooled.CopiedBuffer(new byte[] { JT808.Protocol.JT808Package.EndFlag })));
                       channel.Pipeline.AddLast("jt808Decode", new JT808ClientDecoder());
               }));
            bootstrap.BindAsync(6655);
            //作为设备上传
            byte[] bytes = "7E 02 00 00 26 12 34 56 78 90 12 00 7D 02 00 00 00 01 00 00 00 02 00 BA 7F 0E 07 E4 F1 1C 00 28 00 3C 00 00 18 10 15 10 10 10 01 04 00 00 00 64 02 02 00 7D 01 13 7E".ToHexBytes();
            SimpleTcpClient.WriteAsync(bytes);
            Thread.Sleep(10000);
            SimpleTcpClient.Down();
        }
    }
}
