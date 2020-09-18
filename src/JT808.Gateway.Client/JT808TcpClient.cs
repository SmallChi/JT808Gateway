using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO.Pipelines;
using System.Buffers;
using JT808.Protocol;
using Microsoft.Extensions.Logging;
using JT808.Protocol.Exceptions;
using JT808.Protocol.Extensions;
using JT808.Gateway.Client.Services;
using JT808.Gateway.Client.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace JT808.Gateway.Client
{

    public class JT808TcpClient:IDisposable
    {
        //todo: 客户端的断线重连
        //todo: 客户端的消息处理handler
        private bool disposed = false;
        private Socket clientSocket;
        private readonly ILogger Logger;
        private readonly JT808Serializer JT808Serializer;
        private readonly JT808SendAtomicCounterService SendAtomicCounterService;
        private readonly JT808ReceiveAtomicCounterService ReceiveAtomicCounterService;
        private bool socketState = true;
        public JT808DeviceConfig DeviceConfig { get; }
        public JT808TcpClient(
            JT808DeviceConfig deviceConfig,
            IServiceProvider serviceProvider)
        {
            DeviceConfig = deviceConfig;
            SendAtomicCounterService = serviceProvider.GetRequiredService<JT808SendAtomicCounterService>();
            ReceiveAtomicCounterService = serviceProvider.GetRequiredService<JT808ReceiveAtomicCounterService>();
            JT808Serializer = serviceProvider.GetRequiredService<IJT808Config>().GetSerializer();
            Logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<JT808TcpClient>();
        }
        public async ValueTask<bool> ConnectAsync(EndPoint remoteEndPoint)
        {
            clientSocket = new Socket(remoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                await clientSocket.ConnectAsync(remoteEndPoint);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public async void StartAsync(CancellationToken cancellationToken)
        {
            await Task.Factory.StartNew(async (state) =>
            {
                var session = (Socket)state;
                if (Logger.IsEnabled(LogLevel.Information))
                {
                    Logger.LogInformation($"[Connected]:{session.LocalEndPoint} to {session.RemoteEndPoint}");
                }
                var pipe = new Pipe();
                Task writing = FillPipeAsync(session, pipe.Writer, cancellationToken);
                Task reading = ReadPipeAsync(session, pipe.Reader);
                await Task.WhenAll(reading, writing);
            }, clientSocket);
        }
        private async Task FillPipeAsync(Socket session, PipeWriter writer, CancellationToken cancellationToken)
        {
            while (true)
            {
                try
                {
                    Memory<byte> memory = writer.GetMemory(80960);
                    int bytesRead = await session.ReceiveAsync(memory, SocketFlags.None, cancellationToken);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    writer.Advance(bytesRead);
                }
                catch (System.Net.Sockets.SocketException ex)
                {
                    Logger.LogError($"[{ex.SocketErrorCode.ToString()},{ex.Message}]:{session.RemoteEndPoint}");
                    break;
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"[Receive Error]:{session.RemoteEndPoint}");
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
        private async Task ReadPipeAsync(Socket session, PipeReader reader)
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
                        ReaderBuffer(ref buffer, session, out consumed, out examined);
                    }
                }
                catch (Exception ex)
                {
                    Close();
                    break;
                }
                finally
                {
                    reader.AdvanceTo(consumed, examined);
                }
            }
            reader.Complete();
        }
        private void ReaderBuffer(ref ReadOnlySequence<byte> buffer, Socket session, out SequencePosition consumed, out SequencePosition examined)
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
                            var package = JT808Serializer.HeaderDeserialize(seqReader.Sequence.Slice(totalConsumed, seqReader.Consumed - totalConsumed).FirstSpan,minBufferSize:10240);
                            ReceiveAtomicCounterService.MsgSuccessIncrement();
                            if (Logger.IsEnabled(LogLevel.Debug)) Logger.LogDebug($"[Atomic Success Counter]:{ReceiveAtomicCounterService.MsgSuccessCount}");
                            if (Logger.IsEnabled(LogLevel.Trace)) Logger.LogTrace($"[Accept Hex {session.RemoteEndPoint}]:{package.OriginalData.ToArray().ToHexString()}");
                        }
                        catch (JT808Exception ex)
                        {
                            Logger.LogError(ex, $"[HeaderDeserialize ErrorCode]:{ ex.ErrorCode}");
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
            if (seqReader.Length == totalConsumed)
            {
                examined = consumed = buffer.End;
            }
            else
            {
                consumed = buffer.GetPosition(totalConsumed);
            }
        }
        public async ValueTask SendAsync(JT808ClientRequest message)
        {
            if (disposed) return;
            if (IsOpen && socketState)
            {
                if (message.Package != null)
                {
                    try
                    {
                        var sendData = JT808Serializer.Serialize(message.Package, minBufferSize: message.MinBufferSize);
                        //clientSocket.Send(sendData);
                        await clientSocket.SendAsync(sendData, SocketFlags.None);
                        SendAtomicCounterService.MsgSuccessIncrement();
                    }
                    catch (System.Net.Sockets.SocketException ex)
                    {
                        socketState = false;
                        Logger.LogError($"[{ex.SocketErrorCode.ToString()},{ex.Message},{DeviceConfig.TerminalPhoneNo}]");
                    }
                    catch (System.Exception ex)
                    {
                        Logger.LogError(ex.Message);
                    }
                }
                else if (message.HexData != null)
                {
                    try
                    {
                        clientSocket.Send(message.HexData);
                        SendAtomicCounterService.MsgSuccessIncrement();
                    }
                    catch (System.Net.Sockets.SocketException ex)
                    {
                        socketState = false;
                        Logger.LogError($"[{ex.SocketErrorCode.ToString()},{ex.Message},{DeviceConfig.TerminalPhoneNo}]");
                    }
                    catch (System.Exception ex)
                    {
                        Logger.LogError(ex.Message);
                    }
                }
            }
        }

        public void Close()
        {
            if (disposed) return;
            var socket = clientSocket;
            if (socket == null)
                return;
            if (Interlocked.CompareExchange(ref clientSocket, null, socket) == socket)
            {
                try
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                }
                finally
                {
                    clientSocket.Close();
                }
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                // 清理托管资源
                clientSocket.Dispose();
            }
            disposed = true;
        }

        ~JT808TcpClient()
        {
            //必须为false
            //这表明，隐式清理时，只要处理非托管资源就可以了。
            Dispose(false);
        }

        public void Dispose()
        {
            //必须为true
            Dispose(true);
            //通知垃圾回收机制不再调用终结器（析构器）
            GC.SuppressFinalize(this);
        }

        public bool IsOpen 
        { 
            get
            {
                if (disposed) return false;
                if (clientSocket != null)
                {
                    return clientSocket.Connected && socketState;
                }
                return false;
            } 
        }
    }
}
