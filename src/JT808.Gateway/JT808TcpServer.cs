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
    public class JT808TcpServer:IHostedService
    {
        private Socket server;

        private readonly ILogger Logger;

        private readonly JT808SessionManager SessionManager;

        private readonly IJT808MsgProducer MsgProducer;

        private readonly JT808Serializer Serializer;

        private readonly JT808AtomicCounterService AtomicCounterService;

        private readonly JT808Configuration Configuration;

        public JT808TcpServer(
                JT808Configuration jT808Configuration,
                IJT808Config jT808Config,
                ILoggerFactory loggerFactory,
                JT808SessionManager jT808SessionManager,
                IJT808MsgProducer jT808MsgProducer,
                JT808AtomicCounterServiceFactory jT808AtomicCounterServiceFactory)
            {
                SessionManager = jT808SessionManager;
                Logger = loggerFactory.CreateLogger("JT808TcpServer");
                Serializer = jT808Config.GetSerializer();
                MsgProducer = jT808MsgProducer;
                AtomicCounterService = jT808AtomicCounterServiceFactory.Create(JT808TransportProtocolType.tcp);
                Configuration = jT808Configuration;
                var IPEndPoint = new System.Net.IPEndPoint(IPAddress.Any, jT808Configuration.TcpPort);
                server = new Socket(IPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, true);
                server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                server.LingerState = new LingerOption(false, 0);
                server.Bind(IPEndPoint);
                server.Listen(jT808Configuration.SoBacklog);
            }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation($"JT808 TCP Server start at {IPAddress.Any}:{Configuration.TcpPort}.");
            Task.Run(async() => {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var socket = await server.AcceptAsync();
                    JT808TcpSession jT808TcpSession = new JT808TcpSession(socket);
                    await Task.Factory.StartNew(async (state) =>
                    {
                        var session = (JT808TcpSession)state;
                        SessionManager.TryAdd(session);
                        if (Logger.IsEnabled(LogLevel.Information))
                        {
                            Logger.LogInformation($"[Connected]:{session.Client.RemoteEndPoint}");
                        }
                        var pipe = new Pipe();
                        Task writing = FillPipeAsync(session, pipe.Writer);
                        Task reading = ReadPipeAsync(session, pipe.Reader);
                        await Task.WhenAll(reading, writing);
                        SessionManager.RemoveBySessionId(session.SessionID);
                    }, jT808TcpSession);
                }
            }, cancellationToken);
            return Task.CompletedTask;
        }
        private  async Task FillPipeAsync(JT808TcpSession session, PipeWriter writer)
        {
            while (true)
            {
                try
                {
                    Memory<byte> memory = writer.GetMemory(Configuration.MiniNumBufferSize);
                    //设备多久没发数据就断开连接 Receive Timeout.
                    int bytesRead = await session.Client.ReceiveAsync(memory, SocketFlags.None, session.ReceiveTimeout.Token);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    writer.Advance(bytesRead);
                }
                catch(OperationCanceledException)
                {
                    Logger.LogError($"[Receive Timeout]:{session.Client.RemoteEndPoint}");
                    break;
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"[Receive Error]:{session.Client.RemoteEndPoint}");
                    break;
                }
                FlushResult result = await writer.FlushAsync();
                if (result.IsCompleted)
                {
                    break;
                }
            }
            writer.Complete();
        }
        private  async Task ReadPipeAsync(JT808TcpSession session, PipeReader reader)
        {
            while (true)
            {
                ReadResult result = await reader.ReadAsync();
                if (result.IsCompleted)
                {
                    break;
                }
                ReadOnlySequence<byte> buffer = result.Buffer;
                SequencePosition consumed = buffer.Start;
                SequencePosition examined = buffer.End;
                try
                {
                    if (result.IsCanceled) break;
                    if (buffer.Length > 0)
                    {
                        ReaderBuffer(ref buffer, session,out consumed, out examined);
                    }
                }
                catch (Exception ex)
                {
                    SessionManager.RemoveBySessionId(session.SessionID);
                    break;
                }
                finally
                {
                    reader.AdvanceTo(consumed, examined);
                }
            }
            reader.Complete();
        }
        private  void ReaderBuffer(ref ReadOnlySequence<byte> buffer, JT808TcpSession session, out SequencePosition consumed, out SequencePosition examined)
        {
            consumed = buffer.Start;
            examined = buffer.End;
            SequenceReader<byte> seqReader = new SequenceReader<byte>(buffer);
            if (seqReader.TryPeek(out byte beginMark))
            {
                if (beginMark != JT808Package.BeginFlag) throw new ArgumentException("Not JT808 Packages.");
            }
            byte mark = 0;
            long totalConsumed = 0;
            while (!seqReader.End)
            {
                if (seqReader.IsNext(JT808Package.BeginFlag, advancePast: true))
                {
                    if (mark == 1)
                    {
                        try
                        {
                            var package = Serializer.HeaderDeserialize(seqReader.Sequence.Slice(totalConsumed, seqReader.Consumed - totalConsumed).FirstSpan);
                            AtomicCounterService.MsgSuccessIncrement();
                            if (Logger.IsEnabled(LogLevel.Debug)) Logger.LogDebug($"[Atomic Success Counter]:{AtomicCounterService.MsgSuccessCount}");
                            if (Logger.IsEnabled(LogLevel.Trace)) Logger.LogTrace($"[Accept Hex {session.Client.RemoteEndPoint}]:{package.OriginalData.ToArray().ToHexString()}");
                            //设直连模式和转发模式的会话如何处理
                            SessionManager.TryLink(package.Header.TerminalPhoneNo, session);
                            if(Configuration.MessageQueueType == JT808MessageQueueType.InMemory)
                            {
                                MsgProducer.ProduceAsync(session.SessionID, package.OriginalData.ToArray());
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
                            Logger.LogError(ex,$"[HeaderDeserialize ErrorCode]:{ ex.ErrorCode}"); 
                        }
                        totalConsumed += (seqReader.Consumed - totalConsumed);
                        if (seqReader.End) break;
                        seqReader.Advance(1);
                        mark = 0;
                    }
                    mark++;
                }
                else
                {
                    seqReader.Advance(1);
                }
            }
            if (seqReader.Length== totalConsumed)
            {
                examined = consumed = buffer.End;
            }
            else
            {
                consumed = buffer.GetPosition(totalConsumed);
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("808 Tcp Server Stop");
            if (server?.Connected ?? false)
                server.Shutdown(SocketShutdown.Both);
            server?.Close();
            return Task.CompletedTask;
        }
    }
}
