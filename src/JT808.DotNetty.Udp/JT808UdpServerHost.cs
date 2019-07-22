using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using JT808.DotNetty.Core.Codecs;
using JT808.DotNetty.Core.Configurations;
using JT808.DotNetty.Udp.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.DotNetty.Udp
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
               .Option(ChannelOption.SoBroadcast, true)
               .Handler(new ActionChannelInitializer<IChannel>(channel =>
               {
                   IChannelPipeline pipeline = channel.Pipeline;
                   using (var scope = serviceProvider.CreateScope())
                   {   
                       pipeline.AddLast("jt808UdpDecoder", scope.ServiceProvider.GetRequiredService<JT808UdpDecoder>());
                       pipeline.AddLast("jt808UdpService", scope.ServiceProvider.GetRequiredService<JT808UdpServerHandler>());
                   }
               }));
            logger.LogInformation($"JT808 Udp Server start at {IPAddress.Any}:{configuration.UdpPort}.");
            return bootstrap.BindAsync(configuration.UdpPort)
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
