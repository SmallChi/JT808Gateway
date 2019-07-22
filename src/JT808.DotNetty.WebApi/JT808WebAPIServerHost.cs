using DotNetty.Codecs.Http;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Libuv;
using JT808.DotNetty.Core.Configurations;
using JT808.DotNetty.WebApi.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.DotNetty.WebApi
{
    /// <summary>
    /// JT808 集成一个webapi服务
    /// </summary>
    internal class JT808WebAPIServerHost : IHostedService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly JT808Configuration configuration;
        private readonly ILogger<JT808WebAPIServerHost> logger;
        private DispatcherEventLoopGroup bossGroup;
        private WorkerEventLoopGroup workerGroup;
        private IChannel bootstrapChannel;

        public JT808WebAPIServerHost(
            IServiceProvider provider,
            ILoggerFactory loggerFactory,
            IOptions<JT808Configuration> jT808ConfigurationAccessor)
        {
            serviceProvider = provider;
            configuration = jT808ConfigurationAccessor.Value;
            logger = loggerFactory.CreateLogger<JT808WebAPIServerHost>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            bossGroup = new DispatcherEventLoopGroup();
            workerGroup = new WorkerEventLoopGroup(bossGroup, 1);
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
                    .Option(ChannelOption.SoBacklog, 8192)
                    .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;
                        using (var scope = serviceProvider.CreateScope())
                        {
                            pipeline.AddLast("http_encoder", new HttpResponseEncoder());
                            pipeline.AddLast("http_decoder", new HttpRequestDecoder(4096, 8192, 8192, false));
                            //将多个消息转换为单一的request或者response对象 =>IFullHttpRequest
                            pipeline.AddLast("http_aggregator", new HttpObjectAggregator(int.MaxValue));
                            pipeline.AddLast("http_jt808webapihandler", scope.ServiceProvider.GetRequiredService<JT808WebAPIServerHandler>());
                        }
                    }));
            logger.LogInformation($"JT808 WebAPI Server start at {IPAddress.Any}:{configuration.WebApiPort}.");
            return bootstrap.BindAsync(configuration.WebApiPort).ContinueWith(i => bootstrapChannel = i.Result);
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
