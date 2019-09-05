using DotNetty.Buffers;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using JT808.DotNetty.Transmit.Configs;
using System.Linq;
using JT808.DotNetty.Transmit.Handlers;

namespace JT808.DotNetty.Transmit
{
    public class JT808DotNettyTransmitService 
    {
        private readonly ILogger logger;
        private readonly ILoggerFactory loggerFactory;
        private IOptionsMonitor<RemoteServerOptions> optionsMonitor;
        public Dictionary<string, IChannel> channeldic = new Dictionary<string, IChannel>();
        public JT808DotNettyTransmitService(ILoggerFactory loggerFactory,
                                            IOptionsMonitor<RemoteServerOptions> optionsMonitor)
        {
            this.loggerFactory = loggerFactory;
            logger = loggerFactory.CreateLogger("JT808DotNettyTransmitService");
            this.optionsMonitor = optionsMonitor;
            InitialDispatcherClient();
        }
        public void Send((string TerminalNo, byte[] Data) parameter)
        {
            if (optionsMonitor.CurrentValue.DataTransfer != null)
            {
                foreach (var item in optionsMonitor.CurrentValue.DataTransfer)
                {
                    if (channeldic.TryGetValue($"all_{item.Host}", out var allClientChannel))
                    {
                        try
                        {
                            if (allClientChannel.Open)
                            {
                                if (logger.IsEnabled(LogLevel.Debug))
                                {
                                    logger.LogDebug($"转发所有数据到该网关{item.Host}");
                                }
                                allClientChannel.WriteAndFlushAsync(Unpooled.WrappedBuffer(parameter.Data));
                            }
                            else
                            {
                                logger.LogError($"{item.Host}链接已关闭");
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.LogError($"{item.Host}发送数据出现异常：{ex}");
                        }
                    }
                    else
                    {
                        if (item.TerminalNos.Contains(parameter.TerminalNo) && channeldic.TryGetValue($"{parameter.TerminalNo}_{item.Host}", out var clientChannel))
                        {
                            try
                            {
                                if (clientChannel.Open)
                                {
                                    if (logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Debug))
                                        logger.LogDebug($"转发{parameter.TerminalNo}到该网关{item.Host}");
                                    clientChannel.WriteAndFlushAsync(Unpooled.WrappedBuffer(parameter.Data));
                                }
                                else
                                {
                                    logger.LogError($"{item.Host},{parameter.TerminalNo}链接已关闭");
                                }
                            }
                            catch (Exception ex)
                            {
                                logger.LogError($"{item.Host},{parameter.TerminalNo}发送数据出现异常：{ex}");
                            }
                        }
                    }
                }
            }
        }

        public void InitialDispatcherClient()
        {
            Task.Run(async () =>
            {
                var group = new MultithreadEventLoopGroup();
                var bootstrap = new Bootstrap();
                bootstrap.Group(group)
                 .Channel<TcpSocketChannel>()
                 .Option(ChannelOption.TcpNodelay, true)
                 .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                 {
                     IChannelPipeline pipeline = channel.Pipeline;
                     pipeline.AddLast(new ClientConnectionHandler(bootstrap, channeldic, loggerFactory));
                 }));
                optionsMonitor.OnChange(options =>
                {
                    List<string> lastRemoteServers = new List<string>();
                    if (options.DataTransfer != null)
                    {
                        if (options.DataTransfer.Any())
                        {
                            foreach (var item in options.DataTransfer)
                            {
                                if (item.TerminalNos != null)
                                {
                                    if (item.TerminalNos.Any())
                                    {
                                        foreach (var terminal in item.TerminalNos)
                                        {
                                            lastRemoteServers.Add($"{terminal}_{item.Host}");
                                        }
                                    }
                                    else
                                    {
                                        lastRemoteServers.Add($"all_{item.Host}");
                                    }
                                }
                                else
                                {
                                    lastRemoteServers.Add($"all_{item.Host}");
                                }
                            }
                        }
                    }
                    DelRemoteServsers(lastRemoteServers);
                    AddRemoteServsers(bootstrap, lastRemoteServers);
                });
                await InitRemoteServsers(bootstrap);
            });
        }
        /// <summary>
        /// 初始化远程服务器
        /// </summary>
        /// <param name="bootstrap"></param>
        /// <param name="remoteServers"></param>
        /// <returns></returns>
        private async Task InitRemoteServsers(Bootstrap bootstrap)
        {
            List<string> remoteServers = new List<string>();
            if (optionsMonitor.CurrentValue.DataTransfer != null)
            {
                if (optionsMonitor.CurrentValue.DataTransfer.Any())
                {
                    foreach (var item in optionsMonitor.CurrentValue.DataTransfer)
                    {
                        if (item.TerminalNos != null)
                        {
                            if (item.TerminalNos.Any())
                            {
                                foreach (var terminal in item.TerminalNos)
                                {
                                    remoteServers.Add($"{terminal}_{item.Host}");
                                }
                            }
                            else
                            {
                                remoteServers.Add($"all_{item.Host}");
                            }
                        }
                        else
                        {
                            remoteServers.Add($"all_{item.Host}");
                        }
                    }
                }
            }
            foreach (var item in remoteServers)
            {
                try
                {
                    string ip_port = item.Split('_')[1];
                    IChannel clientChannel = await bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse(ip_port.Split(':')[0]), int.Parse(ip_port.Split(':')[1])));
                    channeldic.Add(item, clientChannel);
                    if (clientChannel.Open)
                    {
                        if (logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Debug))
                        {
                            logger.LogDebug($"该终端{item.Replace("_", "已连接上该服务器")}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError($"初始化配置链接远程服务端{item},链接异常：{ex}");
                }
            }
            await Task.CompletedTask;
        }
        /// <summary>
        /// 动态删除远程服务器
        /// </summary>
        /// <param name="lastRemoteServers"></param>
        private void DelRemoteServsers(List<string> lastRemoteServers)
        {
            var delChannels = channeldic.Keys.Except(lastRemoteServers).ToList();
            foreach (var item in delChannels)
            {
                channeldic[item].CloseAsync();
                channeldic.Remove(item);
            }
        }
        /// <summary>
        /// 动态添加远程服务器
        /// </summary>
        /// <param name="bootstrap"></param>
        /// <param name="lastRemoteServers"></param>
        private void AddRemoteServsers(Bootstrap bootstrap, List<string> lastRemoteServers)
        {
            var addChannels = lastRemoteServers.Except(channeldic.Keys).ToList();
            foreach (var item in addChannels)
            {
                try
                {
                    var ip_port = item.Split('_')[1];
                    IChannel clientChannel = bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse(ip_port.Split(':')[0]), int.Parse(ip_port.Split(':')[1]))).Result;
                    channeldic.Add(item, clientChannel);
                    if (clientChannel.Open) {
                        if (logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Debug))
                        {
                            logger.LogDebug($"该终端{item.Replace("_", "已连接上该服务器")}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError($"变更配置后链接远程服务端{item},重连异常：{ex}");
                }
            }
        }
    }
}
