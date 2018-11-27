using DotNetty.Buffers;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using JT808.DotNetty.Configurations;
using JT808.DotNetty.Handlers;
using JT808.DotNetty.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.DotNetty.Internal
{
    /// <summary>
    /// 源包分发器默认实现
    /// </summary>
    internal class JT808SourcePackageDispatcherDefaultImpl : IJT808SourcePackageDispatcher, IDisposable
    {
        private readonly MultithreadEventLoopGroup group = new MultithreadEventLoopGroup();
        internal readonly Bootstrap bootstrap = new Bootstrap();
        internal readonly ConcurrentDictionary<EndPoint, IChannel> channels = new ConcurrentDictionary<EndPoint, IChannel>();
        private readonly ILogger<JT808SourcePackageDispatcherDefaultImpl> logger;
        private IOptionsMonitor<JT808Configuration> jT808ConfigurationOptionsMonitor;
        internal readonly ILoggerFactory loggerFactory;

        public JT808SourcePackageDispatcherDefaultImpl(ILoggerFactory loggerFactory,
                                        IOptionsMonitor<JT808Configuration> jT808ConfigurationOptionsMonitor)
        {
            this.loggerFactory = loggerFactory;
            this.logger = loggerFactory.CreateLogger<JT808SourcePackageDispatcherDefaultImpl>();
            this.jT808ConfigurationOptionsMonitor = jT808ConfigurationOptionsMonitor;
            StartAsync();
        }

        public async Task SendAsync(byte[] data)
        {
            foreach (var item in channels)
            {
                try
                {
                    if (item.Value.Open && item.Value.Active)
                    {
                        await item.Value.WriteAndFlushAsync(Unpooled.WrappedBuffer(data));
                    }
                    else
                    {
                        logger.LogInformation($"{item} link closed.");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex,$"{item} Send Data Error.");
                }
            }
            await Task.CompletedTask;
        }

        public void StartAsync()
        {
            bootstrap
                .Group(group)
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.TcpNodelay, true)
                .Handler(new ActionChannelInitializer<IChannel>(channel =>
                {
                    channel.Pipeline.AddLast(new JT808SourcePackageDispatcherHandler(this));
                }));
                jT808ConfigurationOptionsMonitor.OnChange(options =>
                {
                    List<JT808ClientConfiguration> chgRemoteServers = new List<JT808ClientConfiguration>();
                    if (jT808ConfigurationOptionsMonitor.CurrentValue.SourcePackageDispatcherClientConfigurations != null && jT808ConfigurationOptionsMonitor.CurrentValue.SourcePackageDispatcherClientConfigurations.Count > 0)
                    {
                        chgRemoteServers = options.SourcePackageDispatcherClientConfigurations;
                    }
                    DelRemoteServsers(chgRemoteServers);
                    AddRemoteServsers(chgRemoteServers);
                });
                if (jT808ConfigurationOptionsMonitor.CurrentValue.SourcePackageDispatcherClientConfigurations != null && 
                    jT808ConfigurationOptionsMonitor.CurrentValue.SourcePackageDispatcherClientConfigurations.Count > 0)
                {
                    foreach (var item in jT808ConfigurationOptionsMonitor.CurrentValue.SourcePackageDispatcherClientConfigurations)
                    {
                        try
                        {
                            Task.Run(async () =>
                            {
                                IChannel clientChannel = await bootstrap.ConnectAsync(item.EndPoint);
                                channels.TryAdd(item.EndPoint, clientChannel);
                                logger.LogInformation($"init remote link {item.EndPoint.ToString()}.");
                            });
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex,$"there is an exception in sending data {item}.");
                        }
                    }
                }
        }

        public Task StopAsync()
        {
            foreach (var channel in channels)
            {
                try
                {
                    channel.Value.CloseAsync();
                }
                catch
                {

                }
            }
            group.ShutdownGracefullyAsync(jT808ConfigurationOptionsMonitor.CurrentValue.QuietPeriodTimeSpan, jT808ConfigurationOptionsMonitor.CurrentValue.ShutdownTimeoutTimeSpan);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 动态删除远程服务器
        /// </summary>
        /// <param name="chgRemoteServers"></param>
        private void DelRemoteServsers(List<JT808ClientConfiguration> chgRemoteServers)
        {
            var delChannels = channels.Keys.Except(chgRemoteServers.Select(s=>s.EndPoint)).ToList();
            foreach (var item in delChannels)
            {
                try
                {
                    channels.TryRemove(item, out var channel);
                    channel.CloseAsync();
                }
                catch
                {
                    
                }
            }
        }

        /// <summary>
        /// 动态添加远程服务器
        /// </summary>
        /// <param name="bootstrap"></param>
        /// <param name="chgRemoteServers"></param>
        private async void AddRemoteServsers(List<JT808ClientConfiguration> chgRemoteServers)
        {
            var addChannels = chgRemoteServers.Select(s=>s.EndPoint).Except(channels.Keys).ToList();
            foreach (var item in addChannels)
            {
                try
                {
                    IChannel clientChannel =await bootstrap.ConnectAsync(item);
                    channels.TryAdd(item, clientChannel);
                    logger.LogInformation($"link to the remote server after the change {item}.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex,$"reconnect the remote server after the exception changes {item}.");
                }
            }
        }

        public void Dispose()
        {
            StopAsync();
        }
    }
}
