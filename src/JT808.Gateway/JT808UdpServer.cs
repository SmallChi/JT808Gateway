using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JT808.Gateway.Abstractions;
using JT808.Gateway.Abstractions.Configurations;
using JT808.Gateway.Session;
using JT808.Protocol;
using JT808.Protocol.Exceptions;
using JT808.Protocol.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JT808.Gateway
{
    public class JT808UdpServer : IHostedService
    {
        private readonly Socket server;

        private readonly ILogger Logger;

        private readonly JT808SessionManager SessionManager;

        private readonly JT808Serializer Serializer;

        private readonly IPEndPoint LocalIPEndPoint;

        private readonly JT808MessageHandler MessageHandler;

        private readonly IJT808MsgProducer MsgProducer;

        private readonly IJT808MsgReplyLoggingProducer MsgReplyLoggingProducer;

        private readonly IOptionsMonitor<JT808Configuration> ConfigurationMonitor;
        public JT808UdpServer(
                IOptionsMonitor<JT808Configuration> configurationMonitor,
                IJT808MsgProducer msgProducer,
                IJT808MsgReplyLoggingProducer msgReplyLoggingProducer,
                IJT808Config jT808Config,
                ILoggerFactory loggerFactory,
                JT808SessionManager jT808SessionManager,
                JT808MessageHandler messageHandler)
        {
            SessionManager = jT808SessionManager;
            MsgProducer = msgProducer;
            ConfigurationMonitor = configurationMonitor;
            MsgReplyLoggingProducer = msgReplyLoggingProducer;
            Logger = loggerFactory.CreateLogger<JT808UdpServer>();
            Serializer = jT808Config.GetSerializer();
            MessageHandler = messageHandler;
            LocalIPEndPoint = new System.Net.IPEndPoint(IPAddress.Any, ConfigurationMonitor.CurrentValue.UdpPort);
            server = new Socket(LocalIPEndPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            server.Bind(LocalIPEndPoint);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation($"JT808 Udp Server start at {IPAddress.Any}:{ConfigurationMonitor.CurrentValue.UdpPort}.");
            Task.Run(async () => { 
                while (!cancellationToken.IsCancellationRequested)
                {
                    var buffer = ArrayPool<byte>.Shared.Rent(ConfigurationMonitor.CurrentValue.MiniNumBufferSize);
                    try
                    {
                        var segment = new ArraySegment<byte>(buffer);
                        SocketReceiveMessageFromResult result = await server.ReceiveMessageFromAsync(segment, SocketFlags.None, LocalIPEndPoint);
                        ReaderBuffer(buffer.AsSpan(0, result.ReceivedBytes), server, result);
                    }
                    catch(System.ObjectDisposedException ex)
                    {
                        //Logger.LogInformation("Socket Received Bytes Close");
                    }
                    catch(AggregateException ex)
                    {
                        Logger.LogError(ex, "Receive MessageFrom Async");
                    }
                    catch (SocketException ex)
                    {
                        //Logger.LogWarning(ex, $"Socket Error");
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, $"Service Error");
                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(buffer);
                    }
                }
            });
            return Task.CompletedTask;
        }
        private void ReaderBuffer(ReadOnlySpan<byte> buffer, Socket socket,SocketReceiveMessageFromResult receiveMessageFromResult)
        {
            try
            {
                var package = Serializer.HeaderDeserialize(buffer, minBufferSize: 10240);
                if (Logger.IsEnabled(LogLevel.Trace)) Logger.LogTrace($"[Accept Hex {receiveMessageFromResult.RemoteEndPoint}]:{package.OriginalData.ToHexString()}");
                var session = SessionManager.TryLink(package.Header.TerminalPhoneNo, socket, receiveMessageFromResult.RemoteEndPoint);
                if (Logger.IsEnabled(LogLevel.Information))
                {
                    Logger.LogInformation($"[Connected]:{receiveMessageFromResult.RemoteEndPoint}");
                }
                Processor(session, package);
            }
            catch (NotImplementedException ex)
            {
                Logger.LogError(ex.Message);
            }
            catch (JT808Exception ex)
            {
                Logger.LogError($"[HeaderDeserialize ErrorCode]:{ ex.ErrorCode},[ReaderBuffer]:{buffer.ToArray().ToHexString()}");
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
            {
                Logger.LogError(ex, $"[ReaderBuffer]:{ buffer.ToArray().ToHexString()}");
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        private void Processor(in IJT808Session session, in JT808HeaderPackage package)
        {
            try
            {
                MsgProducer?.ProduceAsync(package.Header.TerminalPhoneNo, package.OriginalData);
                var downData = MessageHandler.Processor(package);
                if (ConfigurationMonitor.CurrentValue.IgnoreMsgIdReply != null && ConfigurationMonitor.CurrentValue.IgnoreMsgIdReply.Count > 0)
                {
                    if (!ConfigurationMonitor.CurrentValue.IgnoreMsgIdReply.Contains(package.Header.MsgId))
                    {
                        session.SendAsync(downData);
                    }
                }
                else
                {
                    session.SendAsync(downData);
                }
                if (MsgReplyLoggingProducer != null)
                {
                    if (downData != null)
                        MsgReplyLoggingProducer.ProduceAsync(package.Header.TerminalPhoneNo, downData);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"[Processor]:{package.OriginalData.ToHexString()},{session.Client.RemoteEndPoint},{session.TerminalPhoneNo}");
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("JT808 Udp Server Stop");
            if (server?.Connected ?? false)
                server.Shutdown(SocketShutdown.Both);
            server?.Close();
            return Task.CompletedTask;
        }
    }
}
