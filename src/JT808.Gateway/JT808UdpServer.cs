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
using JT808.Gateway.Abstractions.Enums;
using JT808.Gateway.Configurations;
using JT808.Gateway.Enums;
using JT808.Gateway.Services;
using JT808.Gateway.Session;
using JT808.Protocol;
using JT808.Protocol.Exceptions;
using JT808.Protocol.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JT808.Gateway
{
    public class JT808UdpServer : IHostedService
    {
        private Socket server;

        private readonly ILogger Logger;

        private readonly JT808SessionManager SessionManager;

        private readonly IJT808MsgProducer MsgProducer;

        private readonly JT808Serializer Serializer;

        private readonly JT808AtomicCounterService AtomicCounterService;

        private readonly JT808Configuration Configuration;

        private IPEndPoint LocalIPEndPoint;

        public JT808UdpServer(
                JT808Configuration jT808Configuration,
                IJT808Config jT808Config,
                ILoggerFactory loggerFactory,
                JT808SessionManager jT808SessionManager,
                IJT808MsgProducer jT808MsgProducer,
                JT808AtomicCounterServiceFactory jT808AtomicCounterServiceFactory)
            {
                SessionManager = jT808SessionManager;
                Logger = loggerFactory.CreateLogger("JT808UdpServer");
                Serializer = jT808Config.GetSerializer();
                MsgProducer = jT808MsgProducer;
                AtomicCounterService = jT808AtomicCounterServiceFactory.Create(JT808TransportProtocolType.udp);
                Configuration = jT808Configuration;
                LocalIPEndPoint = new System.Net.IPEndPoint(IPAddress.Any, jT808Configuration.UdpPort);
                server = new Socket(LocalIPEndPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
                server.Bind(LocalIPEndPoint);
            }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation($"JT808 Udp Server start at {IPAddress.Any}:{Configuration.UdpPort}.");
            Task.Run(async() => {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var buffer = ArrayPool<byte>.Shared.Rent(Configuration.MiniNumBufferSize);
                    try
                    {
                        var segment = new ArraySegment<byte>(buffer);
                        SocketReceiveMessageFromResult result = await server.ReceiveMessageFromAsync(segment, SocketFlags.None, LocalIPEndPoint);
                        ReaderBuffer(buffer.AsSpan(0, result.ReceivedBytes), server, result);
                    }
                    catch(AggregateException ex)
                    {
                        Logger.LogError(ex, "Receive MessageFrom Async");
                    }
                    catch(Exception ex)
                    {
                        Logger.LogError(ex, $"Received Bytes");
                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(buffer);
                    }
                }
            }, cancellationToken);
            return Task.CompletedTask;
        }
        private void ReaderBuffer(ReadOnlySpan<byte> buffer, Socket socket,SocketReceiveMessageFromResult receiveMessageFromResult)
        {
            try
            {
                var package = Serializer.HeaderDeserialize(buffer);
                AtomicCounterService.MsgSuccessIncrement();
                if (Logger.IsEnabled(LogLevel.Debug)) Logger.LogDebug($"[Atomic Success Counter]:{AtomicCounterService.MsgSuccessCount}");
                if (Logger.IsEnabled(LogLevel.Trace)) Logger.LogTrace($"[Accept Hex {receiveMessageFromResult.RemoteEndPoint}]:{package.OriginalData.ToArray().ToHexString()}");
                //设直连模式和转发模式的会话如何处理
                string sessionId= SessionManager.TryLink(package.Header.TerminalPhoneNo, socket, receiveMessageFromResult.RemoteEndPoint);
                if (Logger.IsEnabled(LogLevel.Information))
                {
                    Logger.LogInformation($"[Connected]:{receiveMessageFromResult.RemoteEndPoint}");
                }
                if (Configuration.MessageQueueType == JT808MessageQueueType.InMemory)
                {
                    MsgProducer.ProduceAsync(sessionId, package.OriginalData.ToArray());
                }
                else
                {
                    MsgProducer.ProduceAsync(package.Header.TerminalPhoneNo, package.OriginalData.ToArray());
                }
            }
            catch (JT808Exception ex)
            {
                AtomicCounterService.MsgFailIncrement();
                if (Logger.IsEnabled(LogLevel.Debug)) Logger.LogDebug($"[Atomic Fail Counter]:{AtomicCounterService.MsgFailCount}");
                Logger.LogError(ex, $"[HeaderDeserialize ErrorCode]:{ ex.ErrorCode}-{buffer.ToArray().ToHexString()}");
            }
            catch (Exception ex)
            {
                if (Logger.IsEnabled(LogLevel.Debug)) Logger.LogDebug($"[Atomic Fail Counter]:{AtomicCounterService.MsgFailCount}");
                Logger.LogError(ex, $"[ReaderBuffer]:{ buffer.ToArray().ToHexString()}");
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("808 Udp Server Stop");
            if (server?.Connected ?? false)
                server.Shutdown(SocketShutdown.Both);
            server?.Close();
            return Task.CompletedTask;
        }
    }
}
