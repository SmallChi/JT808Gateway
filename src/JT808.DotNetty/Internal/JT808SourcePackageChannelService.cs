using DotNetty.Buffers;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using JT808.DotNetty.Configurations;
using JT808.DotNetty.Dtos;
using JT808.DotNetty.Handlers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JT808.DotNetty.Internal
{
    /// <summary>
    /// 原包分发器通道服务
    /// </summary>
    internal class JT808SourcePackageChannelService:IDisposable
    {
        private readonly IOptionsMonitor<JT808Configuration> jT808ConfigurationOptionsMonitor;

        internal readonly ConcurrentDictionary<EndPoint, IChannel> channels;

        private readonly ILogger<JT808SourcePackageChannelService> logger;

        private readonly MultithreadEventLoopGroup group;

        internal readonly ILoggerFactory LoggerFactory;

        internal readonly Bootstrap bootstrap;

        private IDisposable jT808ConfigurationOptionsMonitorDisposable;

        public JT808SourcePackageChannelService(
            ILoggerFactory  loggerFactory,
            IOptionsMonitor<JT808Configuration> jT808ConfigurationOptionsMonitor)
        {
            this.LoggerFactory = loggerFactory;
            this.logger = loggerFactory.CreateLogger<JT808SourcePackageChannelService>();
            this.channels = new ConcurrentDictionary<EndPoint, IChannel>();
            this.jT808ConfigurationOptionsMonitor = jT808ConfigurationOptionsMonitor;
            this.group = new MultithreadEventLoopGroup();
            this.bootstrap = new Bootstrap();
            jT808ConfigurationOptionsMonitorDisposable = jT808ConfigurationOptionsMonitor.OnChange(options =>
            {
                List<JT808ClientConfiguration> chgRemoteServers = new List<JT808ClientConfiguration>();
                if (jT808ConfigurationOptionsMonitor.CurrentValue.SourcePackageDispatcherClientConfigurations != null && jT808ConfigurationOptionsMonitor.CurrentValue.SourcePackageDispatcherClientConfigurations.Count > 0)
                {
                    chgRemoteServers = options.SourcePackageDispatcherClientConfigurations;
                }
                DelRemoteServsers(chgRemoteServers);
                AddRemoteServsers(chgRemoteServers);
            });
            StartAsync();
        }

        /// <summary>
        /// 下发数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Task SendAsync(byte[] data)
        {
            foreach (var item in channels)
            {
                try
                {
                    if (item.Value.Open && item.Value.Active)
                    {
                         item.Value.WriteAndFlushAsync(Unpooled.WrappedBuffer(data));
                    }
                    else
                    {
                        logger.LogInformation($"{item} link closed.");
                    }
                }
                catch (AggregateException ex)
                {
                    logger.LogError(ex, $"{item} Send Data Error.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"{item} Send Data Error.");
                }
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// 获取通道信息集合
        /// </summary>
        /// <returns></returns>
        public JT808ResultDto<List<JT808SourcePackageChannelInfoDto>> GetAll()
        {
            JT808ResultDto<List<JT808SourcePackageChannelInfoDto>> jT808ResultDto = new JT808ResultDto<List<JT808SourcePackageChannelInfoDto>>();
            jT808ResultDto.Data = new List<JT808SourcePackageChannelInfoDto>();
            jT808ResultDto.Code = JT808ResultCode.Ok;
            foreach (var item in channels)
            {
                JT808SourcePackageChannelInfoDto jT808SourcePackageChannelInfoDto = new JT808SourcePackageChannelInfoDto();
                jT808SourcePackageChannelInfoDto.Active = item.Value.Active;
                jT808SourcePackageChannelInfoDto.Open = item.Value.Open;
                jT808SourcePackageChannelInfoDto.Registered = item.Value.Registered;
                jT808SourcePackageChannelInfoDto.LocalAddress = item.Value.LocalAddress.ToString().Replace("[::ffff:", "").Replace("]", "");
                jT808SourcePackageChannelInfoDto.RemoteAddress = item.Value.RemoteAddress.ToString().Replace("[::ffff:", "").Replace("]", "");
                jT808ResultDto.Data.Add(jT808SourcePackageChannelInfoDto);
            }
            return jT808ResultDto;
        }

        /// <summary>
        /// 添加地址
        /// </summary>
        /// <returns></returns>
        public async Task<JT808ResultDto<bool>> Add(JT808IPAddressDto jT808IPAddressDto)
        {
            JT808ResultDto<bool> jT808ResultDto = new JT808ResultDto<bool>();
            jT808ResultDto.Code= JT808ResultCode.Ok;
            jT808ResultDto.Data = true;
            if (!channels.ContainsKey(jT808IPAddressDto.EndPoint))
            {
                try
                {
                    IChannel clientChannel = await bootstrap.ConnectAsync(jT808IPAddressDto.EndPoint);
                    channels.TryAdd(jT808IPAddressDto.EndPoint, clientChannel);
                }
                catch (AggregateException ex)
                {
                    jT808ResultDto.Data = false;
                    jT808ResultDto.Code = JT808ResultCode.Error;
                    jT808ResultDto.Message = JsonConvert.SerializeObject(ex);
                }
                catch (Exception ex)
                {
                    jT808ResultDto.Data = false;
                    jT808ResultDto.Code= JT808ResultCode.Error;
                    jT808ResultDto.Message = JsonConvert.SerializeObject(ex);
                }
            }
            return jT808ResultDto;
        }

        /// <summary>
        /// 删除地址
        /// </summary>
        /// <returns></returns>
        public async Task<JT808ResultDto<bool>> Remove(JT808IPAddressDto jT808IPAddressDto)
        {
            JT808ResultDto<bool> jT808ResultDto = new JT808ResultDto<bool>();
            jT808ResultDto.Code = JT808ResultCode.Ok;
            jT808ResultDto.Data = true;

            if(jT808ConfigurationOptionsMonitor.CurrentValue.SourcePackageDispatcherClientConfigurations!=null &&
                jT808ConfigurationOptionsMonitor.CurrentValue.SourcePackageDispatcherClientConfigurations.Any(a=>a.EndPoint.ToString()== jT808IPAddressDto.EndPoint.ToString())
                )
            {
                jT808ResultDto.Data = false;
                jT808ResultDto.Message = "不能删除服务器配置的地址";
            }
            else
            {
                if (channels.TryRemove(jT808IPAddressDto.EndPoint, out var channel))
                {
                    try
                    {
                        await channel.CloseAsync();
                    }
                    catch (AggregateException ex)
                    {
                        jT808ResultDto.Data = false;
                        jT808ResultDto.Code = JT808ResultCode.Error;
                        jT808ResultDto.Message = JsonConvert.SerializeObject(ex);
                    }
                    catch (Exception ex)
                    {
                        jT808ResultDto.Data = false;
                        jT808ResultDto.Code = JT808ResultCode.Error;
                        jT808ResultDto.Message = JsonConvert.SerializeObject(ex);
                    }
                }
            }
            return jT808ResultDto;
        }

        private void StartAsync()
        {
            bootstrap
                .Group(group)
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.TcpNodelay, true)
                .Handler(new ActionChannelInitializer<IChannel>(channel =>
                {
                    channel.Pipeline.AddLast(new JT808SourcePackageDispatcherHandler(this));
                }));
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
                        logger.LogError(ex, $"there is an exception in sending data {item}.");
                    }
                }
            }
        }

        private Task StopAsync()
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
            var delChannels = channels.Keys.Except(chgRemoteServers.Select(s => s.EndPoint)).ToList();
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
            var addChannels = chgRemoteServers.Select(s => s.EndPoint).Except(channels.Keys).ToList();
            foreach (var item in addChannels)
            {
                try
                {
                    IChannel clientChannel = await bootstrap.ConnectAsync(item);
                    channels.TryAdd(item, clientChannel);
                    logger.LogInformation($"link to the remote server after the change {item}.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"reconnect the remote server after the exception changes {item}.");
                }
            }
        }

        public void Dispose()
        {
            jT808ConfigurationOptionsMonitorDisposable.Dispose();
            StopAsync();
        }
    }
}
