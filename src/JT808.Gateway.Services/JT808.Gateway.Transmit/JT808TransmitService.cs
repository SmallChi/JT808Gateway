using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using JT808.Gateway.Transmit.Configs;
using System.Net.Sockets;
using System.Collections.Concurrent;
using System.Threading;

namespace JT808.Gateway.Transmit
{
    public class JT808TransmitService
    {
        private readonly ILogger logger;
        private IOptionsMonitor<RemoteServerOptions> optionsMonitor;
        private ConcurrentDictionary<string, Socket> channeldic = new ConcurrentDictionary<string, Socket>();
        private const int time=20*1000;
        private CancellationTokenSource cts = new CancellationTokenSource();
        public JT808TransmitService(ILoggerFactory loggerFactory,
                                    IOptionsMonitor<RemoteServerOptions> optionsMonitor)
        {
            logger = loggerFactory.CreateLogger<JT808TransmitService>();
            this.optionsMonitor = optionsMonitor;
            InitialDispatcherClient();
        }
        public async void SendAsync((string TerminalNo, byte[] Data) parameter)
        {
            if (optionsMonitor.CurrentValue.DataTransfer != null)
            {
                foreach (var item in optionsMonitor.CurrentValue.DataTransfer)
                {
                    string key = $"all_{item.Host}";
                    if (channeldic.TryGetValue(key, out var clientAll))
                    {
                        try
                        {
                            if (clientAll.Connected)
                            {
                                if (logger.IsEnabled(LogLevel.Debug))
                                {
                                    logger.LogDebug($"转发所有数据到该网关{item.Host}");
                                }
                                await clientAll.SendAsync(parameter.Data,SocketFlags.None);
                            }
                            else
                            {
                                channeldic.TryRemove(key, out _);
                                logger.LogError($"{item.Host}链接已关闭");
                            }
                        }
                        catch (SocketException ex)
                        {
                            channeldic.TryRemove(key, out _);
                            logger.LogError($"{item.Host}发送数据出现异常：{ex}");
                        }
                        catch (Exception ex)
                        {
                            channeldic.TryRemove(key, out _);
                            logger.LogError($"{item.Host}发送数据出现异常：{ex}");
                        }
                    }
                    else
                    {
                        key = $"{parameter.TerminalNo}_{item.Host}";
                        if (item.TerminalNos!=null && item.TerminalNos.Contains(parameter.TerminalNo) && channeldic.TryGetValue(key, out var client))
                        {
                            try
                            {
                                if (client.Connected)
                                {
                                    if (logger.IsEnabled(LogLevel.Debug))
                                        logger.LogDebug($"转发{parameter.TerminalNo}到该网关{item.Host}");
                                    await client.SendAsync(parameter.Data, SocketFlags.None);
                                }
                                else
                                {
                                    channeldic.TryRemove(key, out _);
                                    logger.LogError($"{item.Host},{parameter.TerminalNo}链接已关闭");
                                }
                            }
                            catch (SocketException ex)
                            {
                                channeldic.TryRemove(key, out _);
                                logger.LogError($"{item.Host}发送数据出现异常：{ex}");
                            }
                            catch (Exception ex)
                            {
                                channeldic.TryRemove(key, out _);
                                logger.LogError($"{item.Host},{parameter.TerminalNo}发送数据出现异常：{ex}");
                            }
                        }
                    }
                }
            }
        }

        public void Stop()
        {
            cts.Cancel();
        }
    
        public void InitialDispatcherClient()
        {
            Task.Run(async () =>
            {
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
                    AddRemoteServsers(lastRemoteServers);
                });
                await InitRemoteServsers();
            });
            Task.Factory.StartNew(async() =>
            {
                while (!cts.IsCancellationRequested)
                {
                    await Task.Delay(time);
                    List<string> lastRemoteServers = new List<string>();
                    if (optionsMonitor.CurrentValue != null && optionsMonitor.CurrentValue.DataTransfer!=null)
                    {
                        foreach (var item in optionsMonitor.CurrentValue.DataTransfer)
                        {
                            if (item.TerminalNos != null && item.TerminalNos.Any())
                            {
                                foreach (var terminal in item.TerminalNos)
                                {
                                    string tmpKey = $"{terminal}_{item.Host}";
                                    if (!channeldic.ContainsKey(tmpKey))
                                    {
                                        lastRemoteServers.Add(tmpKey);
                                    }
                                }
                            }
                            else
                            {
                                string key = $"all_{item.Host}";
                                if (!channeldic.ContainsKey(key))
                                {
                                    lastRemoteServers.Add(key);
                                }
                            }
                        }
                    }
                    foreach (var item in lastRemoteServers)
                    {
                        try
                        {
                            var ip_port = item.Split('_')[1];
                            EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip_port.Split(':')[0]), int.Parse(ip_port.Split(':')[1]));
                            Socket client = new Socket(remoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                            await client.ConnectAsync(remoteEndPoint);
                            if (client.Connected)
                            {
                                channeldic.TryAdd(item, client);
                                if (logger.IsEnabled(LogLevel.Information))
                                {
                                    logger.LogInformation($"该终端{item.Replace("_", "已尝试连接上该服务器")}");
                                }
                            }
                            else
                            {
                                if (logger.IsEnabled(LogLevel.Information))
                                {
                                    logger.LogInformation($"该终端{item.Replace("_", "已尝试未连接上该服务器")}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, $"该终端{item.Replace("_", "已尝试未连接上该服务器")}");
                        }
                    }
                }
            }, cts.Token,TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }
        /// <summary>
        /// 初始化远程服务器
        /// </summary>
        /// <param name="bootstrap"></param>
        /// <param name="remoteServers"></param>
        /// <returns></returns>
        private async Task InitRemoteServsers()
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
                    EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip_port.Split(':')[0]), int.Parse(ip_port.Split(':')[1]));
                    Socket client = new Socket(remoteEndPoint.AddressFamily, SocketType.Stream,ProtocolType.Tcp);
                    await client.ConnectAsync(remoteEndPoint);
                    if (client.Connected)
                    {
                        channeldic.TryAdd(item, client);
                        if (logger.IsEnabled(LogLevel.Debug))
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
                var client = channeldic[item];
                try
                {
                    client.Shutdown(SocketShutdown.Both);
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    client.Close();
                    channeldic.TryRemove(item,out _);
                }
            }
        }
        /// <summary>
        /// 动态添加远程服务器
        /// </summary>
        /// <param name="bootstrap"></param>
        /// <param name="lastRemoteServers"></param>
        private async void AddRemoteServsers(List<string> lastRemoteServers)
        {
            var addChannels = lastRemoteServers.Except(channeldic.Keys).ToList();
            foreach (var item in addChannels)
            {
                try
                {
                    var ip_port = item.Split('_')[1];
                    EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip_port.Split(':')[0]), int.Parse(ip_port.Split(':')[1]));
                    Socket client = new Socket(remoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    await client.ConnectAsync(remoteEndPoint);
                    if (client.Connected) 
                    {
                        channeldic.TryAdd(item, client);
                        if (logger.IsEnabled(LogLevel.Debug))
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
