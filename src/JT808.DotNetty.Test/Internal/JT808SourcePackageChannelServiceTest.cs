using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Libuv;
using JT808.DotNetty.Codecs;
using JT808.DotNetty.Configurations;
using JT808.DotNetty.Internal;
using JT808.Protocol;
using JT808.Protocol.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace JT808.DotNetty.Test.Internal
{
    public class JT808SourcePackageChannelServiceTest:TestBase
    {
        private JT808SourcePackageChannelService jT808SourcePackageChannelService;

        private IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6565);

        public JT808SimpleTcpClient SimpleTcpClient;

        /// <summary>
        /// 需要使用 SocketTool 创建tcp服务器
        /// </summary>
        public JT808SourcePackageChannelServiceTest()
        {
            SimpleTcpClient = new JT808SimpleTcpClient(endPoint, new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6555));
            //作为设备5秒上传
            Task.Run(() =>
            {
                Random random = new Random();
                while (true)
                {

                    JT808Package jT808Package = JT808.Protocol.Enums.JT808MsgId.终端心跳.Create("12345678900"+ random.Next(0,2).ToString());
                    SimpleTcpClient.WriteAsync(JT808Serializer.Serialize(jT808Package));
                    Thread.Sleep(1000);
                }
            });

            // 作为源包转发服务端
            DispatcherEventLoopGroup bossGroup = new DispatcherEventLoopGroup();
            WorkerEventLoopGroup workerGroup = new WorkerEventLoopGroup(bossGroup, 1);
            ServerBootstrap bootstrap = new ServerBootstrap();
            bootstrap.Group(bossGroup, workerGroup);
            bootstrap.Channel<TcpServerChannel>();
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

            DispatcherEventLoopGroup bossGroup1 = new DispatcherEventLoopGroup();
            WorkerEventLoopGroup workerGroup1 = new WorkerEventLoopGroup(bossGroup1, 1);
            ServerBootstrap bootstrap1 = new ServerBootstrap();
            bootstrap1.Group(bossGroup1, workerGroup1);
            bootstrap1.Channel<TcpServerChannel>();
            bootstrap1
               .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
               {
                   IChannelPipeline pipeline = channel.Pipeline;
                   channel.Pipeline.AddLast("jt808Buffer", new DelimiterBasedFrameDecoder(int.MaxValue,
                       Unpooled.CopiedBuffer(new byte[] { JT808.Protocol.JT808Package.BeginFlag }),
                       Unpooled.CopiedBuffer(new byte[] { JT808.Protocol.JT808Package.EndFlag })));
                   channel.Pipeline.AddLast("jt808Decode", new JT808ClientDecoder());
               }));
            bootstrap1.BindAsync(6656);
        }

        [Fact]
        public void Test1()
        {
            //预热
            Thread.Sleep(3000);

            jT808SourcePackageChannelService = ServiceProvider.GetService<JT808SourcePackageChannelService>();
            var result = jT808SourcePackageChannelService.GetAll();

            //创建服务
            DispatcherEventLoopGroup bossGroup2 = new DispatcherEventLoopGroup();
            WorkerEventLoopGroup workerGroup2 = new WorkerEventLoopGroup(bossGroup2, 1);
            ServerBootstrap bootstrap2 = new ServerBootstrap();
            bootstrap2.Group(bossGroup2, workerGroup2);
            bootstrap2.Channel<TcpServerChannel>();
            bootstrap2
               .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
               {
                   IChannelPipeline pipeline = channel.Pipeline;
                   channel.Pipeline.AddLast("jt808Buffer", new DelimiterBasedFrameDecoder(int.MaxValue,
                       Unpooled.CopiedBuffer(new byte[] { JT808.Protocol.JT808Package.BeginFlag }),
                       Unpooled.CopiedBuffer(new byte[] { JT808.Protocol.JT808Package.EndFlag })));
                   channel.Pipeline.AddLast("jt808Decode", new JT808ClientDecoder());
               }));
            bootstrap2.BindAsync(6522);

            //添加服务
            var addResult = jT808SourcePackageChannelService.Add(new Dtos.JT808IPAddressDto
            {
                Host = "127.0.0.1",
                Port = 6522
            }).Result;

            Thread.Sleep(3000);

            var result1 = jT808SourcePackageChannelService.GetAll();

            //删除
            var result2 = jT808SourcePackageChannelService.Remove(new Dtos.JT808IPAddressDto
            {
                Host = "127.0.0.1",
                Port = 6522
            }).Result;
            //[::ffff:127.0.0.1]:13196
            var result3 = jT808SourcePackageChannelService.GetAll();
        }


    }
}
