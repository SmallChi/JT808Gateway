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
using JT808.Gateway.Client.Internal;
using JT808.Protocol.Enums;

namespace JT808.Gateway.Client
{

    public class JT808TcpClient:IDisposable
    {
        private bool disposed = false;
        private Socket clientSocket;
        private readonly ILogger Logger;
        private readonly JT808Serializer JT808Serializer;
        private readonly JT808SendAtomicCounterService SendAtomicCounterService;
        private readonly JT808ReceiveAtomicCounterService ReceiveAtomicCounterService;
        private readonly JT808RetryBlockingCollection RetryBlockingCollection;
        private bool socketState = true;
        public JT808DeviceConfig DeviceConfig { get; }
        private IJT808MessageProducer producer;
        private Task heartbeatTask;
        private CancellationTokenSource heartbeatCTS;
        public JT808TcpClient(
            JT808DeviceConfig deviceConfig,
            IServiceProvider serviceProvider)
        {
            DeviceConfig = deviceConfig;
            WriteableTimeout = DateTime.UtcNow.AddSeconds(DeviceConfig.Heartbeat);
            heartbeatCTS = new CancellationTokenSource();
            SendAtomicCounterService = serviceProvider.GetRequiredService<JT808SendAtomicCounterService>();
            ReceiveAtomicCounterService = serviceProvider.GetRequiredService<JT808ReceiveAtomicCounterService>();
            JT808Serializer = serviceProvider.GetRequiredService<IJT808Config>().GetSerializer();
            Logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<JT808TcpClient>();
            producer = serviceProvider.GetRequiredService<IJT808MessageProducer>();
            RetryBlockingCollection = serviceProvider.GetRequiredService<JT808RetryBlockingCollection>();
        }
        public async ValueTask<bool> ConnectAsync()
        {
            var remoteEndPoint = new IPEndPoint(IPAddress.Parse(DeviceConfig.TcpHost), DeviceConfig.TcpPort);
            clientSocket = new Socket(remoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                if (!string.IsNullOrEmpty(DeviceConfig.LocalIPAddress))
                {
                    IPAddress localIPAddress = IPAddress.Parse(DeviceConfig.LocalIPAddress);
                    clientSocket.Bind(new IPEndPoint(localIPAddress, DeviceConfig.LocalPort));
                }
                await clientSocket.ConnectAsync(remoteEndPoint);
                try
                {
                    await Task.Factory.StartNew(async () =>
                    {
                        while (!heartbeatCTS.IsCancellationRequested)
                        {
                            if (WriteableTimeout <= DateTime.UtcNow)
                            {
                                try
                                {
                                    if (Logger.IsEnabled(LogLevel.Information))
                                    {
                                        Logger.LogInformation($"{DeviceConfig.Heartbeat}s send heartbeat:{DeviceConfig.TerminalPhoneNo}-{DeviceConfig.Version.ToString()}");
                                    }
                                    if (DeviceConfig.Version == Protocol.Enums.JT808Version.JTT2013 || DeviceConfig.Version == Protocol.Enums.JT808Version.JTT2011)
                                    {
                                        var package = JT808.Protocol.Enums.JT808MsgId._0x0002.Create(DeviceConfig.TerminalPhoneNo);
                                        await SendAsync(new JT808ClientRequest(package));
                                    }
                                    else
                                    {
                                        var package = JT808.Protocol.Enums.JT808MsgId._0x0002.Create2019(DeviceConfig.TerminalPhoneNo);
                                        await SendAsync(new JT808ClientRequest(package));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Logger.LogError(ex, "");
                                }
                            }
                            await Task.Delay(TimeSpan.FromSeconds(DeviceConfig.Heartbeat), heartbeatCTS.Token);
                        }
                    }, heartbeatCTS.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                }
                catch (Exception)
                {

                }
                return true;
            }
            catch (Exception e)
            {
                Logger.LogError(e, "ConnectAsync Error");
                RetryBlockingCollection.RetryBlockingCollection.Add(DeviceConfig);
                return false;
            }
        }
        public async void StartAsync(CancellationToken cancellationToken)
        {
            try
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
                    RetryBlockingCollection.RetryBlockingCollection.Add(DeviceConfig);
                }, clientSocket, cancellationToken, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
            }
            catch (Exception)
            {

            }
        }
        private async Task FillPipeAsync(Socket session, PipeWriter writer, CancellationToken cancellationToken)
        {
            while (true)
            {
                try
                {
                    Memory<byte> memory = writer.GetMemory(8096);
                    int bytesRead = await session.ReceiveAsync(memory, SocketFlags.None, cancellationToken);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    writer.Advance(bytesRead);
                }
                catch (OperationCanceledException ex)
                {
                    Logger.LogError($"[Receive Timeout]:{session.RemoteEndPoint}");
                    break;
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
                            var data = seqReader.Sequence.Slice(totalConsumed, seqReader.Consumed - totalConsumed).ToArray();
                            var package = JT808Serializer.Deserialize(data, minBufferSize: 8096);
                            if (producer != null)
                            {
                                producer.ProduceAsync(package);
                            }
                            ReceiveAtomicCounterService.MsgSuccessIncrement();
                            if (Logger.IsEnabled(LogLevel.Debug)) Logger.LogDebug($"[Atomic Success Counter]:{ReceiveAtomicCounterService.MsgSuccessCount}");
                            if (Logger.IsEnabled(LogLevel.Trace)) Logger.LogTrace($"[Accept Hex {session.RemoteEndPoint}]:{data.ToHexString()}");
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
        public async ValueTask<bool> SendAsync(JT808ClientRequest message)
        {
            if (disposed) return false;
            if (IsOpen && socketState)
            {
                if (message.Package != null)
                {
                    try
                    {
                        var sendData = JT808Serializer.Serialize(message.Package, DeviceConfig.Version, minBufferSize: message.MinBufferSize);
                        await clientSocket.SendAsync(sendData, SocketFlags.None);
                        SendAtomicCounterService.MsgSuccessIncrement();
                        WriteableTimeout = DateTime.UtcNow.AddSeconds(DeviceConfig.Heartbeat);
                        return true;
                    }
                    catch (System.Net.Sockets.SocketException ex)
                    {
                        socketState = false;
                        Logger.LogError($"[{ex.SocketErrorCode.ToString()},{ex.Message},{DeviceConfig.TerminalPhoneNo}]");
                        return false;
                    }
                    catch (System.Exception ex)
                    {
                        Logger.LogError(ex.Message);
                        return false;
                    }
                }
                else if (message.HexData != null)
                {
                    try
                    {
                        await clientSocket.SendAsync(message.HexData, SocketFlags.None);
                        SendAtomicCounterService.MsgSuccessIncrement();
                        WriteableTimeout = DateTime.UtcNow.AddSeconds(DeviceConfig.Heartbeat);
                        return true;
                    }
                    catch (System.Net.Sockets.SocketException ex)
                    {
                        socketState = false;
                        Logger.LogError($"[{ex.SocketErrorCode.ToString()},{ex.Message},{DeviceConfig.TerminalPhoneNo}]");
                        return false;
                    }
                    catch (System.Exception ex)
                    {
                        Logger.LogError(ex.Message);
                        return false;
                    }
                }
            }
            return false;
        }
        public void Close()
        {
            if (disposed) return;
            if (clientSocket == null) return;
            try
            {
                clientSocket?.Shutdown(SocketShutdown.Both);
                heartbeatShutdown();
            }
            finally
            {
                clientSocket?.Close();
            }          
        }

        private void heartbeatShutdown()
        {
            try
            {
                heartbeatCTS.Cancel();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "");
            }
        }
        private void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                // 清理托管资源
                clientSocket.Dispose();
                heartbeatCTS.Dispose();
            }
            disposed = true;
        }

        public DateTime WriteableTimeout { get; private set; }

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
