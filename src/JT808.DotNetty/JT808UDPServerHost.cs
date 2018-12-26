using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
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
    /// <summary>
    /// JT808 Udp网关服务
    /// </summary>
    internal class JT808UdpServerHost : IHostedService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly JT808Configuration configuration;
        private readonly ILogger<JT808UdpServerHost> logger;
        private MultithreadEventLoopGroup group;
        private IChannel bootstrapChannel;

        public JT808UdpServerHost(
            IServiceProvider provider,
            ILoggerFactory loggerFactory,
            IOptions<JT808Configuration> jT808ConfigurationAccessor)
        {
            serviceProvider = provider;
            configuration = jT808ConfigurationAccessor.Value;
            logger=loggerFactory.CreateLogger<JT808UdpServerHost>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            group = new MultithreadEventLoopGroup();
            Bootstrap bootstrap = new Bootstrap();
            bootstrap.Group(group);
            bootstrap.Channel<SocketDatagramChannel>();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                bootstrap
                    .Option(ChannelOption.SoReuseport, true);
            }
            bootstrap
               .Option(ChannelOption.SoBacklog, configuration.SoBacklog)
               .Handler(new ActionChannelInitializer<IChannel>(channel =>
               {
                   IChannelPipeline pipeline = channel.Pipeline;
                   using (var scope = serviceProvider.CreateScope())
                   {
                       pipeline.AddLast(new JT808UDPDecoder());
                       pipeline.AddLast("jt808UDPService", scope.ServiceProvider.GetRequiredService<JT808UDPServerHandler>());
                   }
               }));
            logger.LogInformation($"Udp Server start at {IPAddress.Any}:{configuration.UDPPort}.");
            return bootstrap.BindAsync(configuration.UDPPort)
                .ContinueWith(i => bootstrapChannel = i.Result);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await bootstrapChannel.CloseAsync();
            var quietPeriod = configuration.QuietPeriodTimeSpan;
            var shutdownTimeout = configuration.ShutdownTimeoutTimeSpan;
            await group.ShutdownGracefullyAsync(quietPeriod, shutdownTimeout);
        }
    }
}
