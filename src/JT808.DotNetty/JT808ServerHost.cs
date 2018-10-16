using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Libuv;
using JT808.DotNetty.Codecs;
using JT808.DotNetty.Configurations;
using JT808.DotNetty.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.DotNetty
{
    public class JT808ServerHost : IHostedService
    {
        public IServiceProvider Provider { get; }
        private readonly JT808Configuration configuration;
        private readonly ILogger<JT808ServerHost> logger;
        private DispatcherEventLoopGroup bossGroup;
        private WorkerEventLoopGroup workerGroup;
        private IChannel bootstrapChannel;

        public JT808ServerHost(
            IServiceProvider provider,
            ILoggerFactory loggerFactory,
            IOptions<JT808Configuration> jT808ConfigurationAccessor)
        {
            Provider = provider;
            configuration = jT808ConfigurationAccessor.Value;
            logger=loggerFactory.CreateLogger<JT808ServerHost>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            bossGroup = new DispatcherEventLoopGroup();
            workerGroup = new WorkerEventLoopGroup(bossGroup, configuration.EventLoopCount);
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
               .Option(ChannelOption.SoBacklog, configuration.SoBacklog)
               .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
               {
                   IChannelPipeline pipeline = channel.Pipeline;
                   using(var scope= Provider.CreateScope())
                   {
                       channel.Pipeline.AddLast("systemIdleState", new IdleStateHandler(
                                                configuration.ReaderIdleTimeSeconds,
                                                configuration.WriterIdleTimeSeconds,
                                                configuration.AllIdleTimeSeconds));
                       channel.Pipeline.AddLast("jt808Connection", scope.ServiceProvider.GetRequiredService<JT808ConnectionHandler>());
                       channel.Pipeline.AddLast("jt808Buffer", new DelimiterBasedFrameDecoder(int.MaxValue,
                           Unpooled.CopiedBuffer(new byte[] { JT808.Protocol.JT808Package.BeginFlag }),
                           Unpooled.CopiedBuffer(new byte[] { JT808.Protocol.JT808Package.EndFlag })));
                       channel.Pipeline.AddLast("jt808Decode", scope.ServiceProvider.GetRequiredService<JT808Decoder>());
                       channel.Pipeline.AddLast("jt808Service", scope.ServiceProvider.GetRequiredService<JT808ServerHandler>());
                   }
               }));
            logger.LogInformation($"Server start at {IPAddress.Any}:{configuration.Port}.");
            return bootstrap.BindAsync(configuration.Port)
                .ContinueWith(i => bootstrapChannel = i.Result);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await bootstrapChannel.CloseAsync();
            var quietPeriod = configuration.QuietPeriodTimeSpan;
            var shutdownTimeout = configuration.ShutdownTimeoutTimeSpan;
            await workerGroup.ShutdownGracefullyAsync(quietPeriod, shutdownTimeout);
            await bossGroup.ShutdownGracefullyAsync(quietPeriod, shutdownTimeout);
        }
    }
}
