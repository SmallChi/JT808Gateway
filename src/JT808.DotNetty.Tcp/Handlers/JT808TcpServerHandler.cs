using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using JT808.Protocol;
using System;
using JT808.DotNetty.Core;
using JT808.DotNetty.Abstractions;
using Microsoft.Extensions.Logging;
using JT808.DotNetty.Core.Handlers;
using JT808.DotNetty.Core.Services;
using JT808.DotNetty.Core.Metadata;

namespace JT808.DotNetty.Tcp.Handlers
{
    /// <summary>
    /// JT808服务端处理程序
    /// </summary>
    internal class JT808TcpServerHandler : SimpleChannelInboundHandler<byte[]>
    {
        private readonly JT808MsgIdTcpHandlerBase handler;
        
        private readonly JT808TcpSessionManager jT808SessionManager;

        private readonly JT808TransmitAddressFilterService jT808TransmitAddressFilterService;

        private readonly IJT808SourcePackageDispatcher jT808SourcePackageDispatcher;

        private readonly JT808TcpAtomicCounterService jT808AtomicCounterService;

        private readonly ILogger<JT808TcpServerHandler> logger;

        public JT808TcpServerHandler(
            ILoggerFactory loggerFactory,
            JT808TransmitAddressFilterService jT808TransmitAddressFilterService,
            IJT808SourcePackageDispatcher jT808SourcePackageDispatcher,
            JT808MsgIdTcpHandlerBase handler,
            JT808TcpAtomicCounterService jT808AtomicCounterService,
            JT808TcpSessionManager jT808SessionManager)
        {
            this.jT808TransmitAddressFilterService = jT808TransmitAddressFilterService;
            this.handler = handler;
            this.jT808SessionManager = jT808SessionManager;
            this.jT808SourcePackageDispatcher = jT808SourcePackageDispatcher;
            this.jT808AtomicCounterService = jT808AtomicCounterService;
            logger = loggerFactory.CreateLogger<JT808TcpServerHandler>();
        }


        protected override void ChannelRead0(IChannelHandlerContext ctx, byte[] msg)
        {
            try
            {
                jT808SourcePackageDispatcher?.SendAsync(msg);
                //解析到头部,然后根据具体的消息Id通过队列去进行消费
                //要是一定要解析到数据体可以在JT808MsgIdHandlerBase类中根据具体的消息，
                //解析具体的消息体，具体调用JT808Serializer.Deserialize<T>
                JT808HeaderPackage jT808HeaderPackage = JT808Serializer.Deserialize<JT808HeaderPackage>(msg);
                jT808AtomicCounterService.MsgSuccessIncrement();
                if (logger.IsEnabled(LogLevel.Debug))
                {
                    logger.LogDebug("accept package success count<<<" + jT808AtomicCounterService.MsgSuccessCount.ToString());
                }
                jT808SessionManager.TryAdd(new JT808TcpSession(ctx.Channel, jT808HeaderPackage.Header.TerminalPhoneNo));
                Func<JT808Request, JT808Response> handlerFunc;
                if (handler.HandlerDict.TryGetValue(jT808HeaderPackage.Header.MsgId, out handlerFunc))
                {
                    JT808Response jT808Response = handlerFunc(new JT808Request(jT808HeaderPackage, msg));
                    if (jT808Response != null)
                    {
                        if (!jT808TransmitAddressFilterService.ContainsKey(ctx.Channel.RemoteAddress))
                        {
                            ctx.WriteAndFlushAsync(Unpooled.WrappedBuffer(JT808Serializer.Serialize(jT808Response.Package, jT808Response.MinBufferSize)));
                        }
                    }
                }
            }
            catch (JT808.Protocol.Exceptions.JT808Exception ex)
            {
                jT808AtomicCounterService.MsgFailIncrement();
                if (logger.IsEnabled(LogLevel.Error))
                {
                    logger.LogError("accept package fail count<<<" + jT808AtomicCounterService.MsgFailCount.ToString());
                    logger.LogError(ex, "accept msg<<<" + ByteBufferUtil.HexDump(msg));
                }
            }
            catch (Exception ex)
            {
                jT808AtomicCounterService.MsgFailIncrement();
                if (logger.IsEnabled(LogLevel.Error))
                {
                    logger.LogError("accept package fail count<<<" + jT808AtomicCounterService.MsgFailCount.ToString());
                    logger.LogError(ex, "accept msg<<<" + ByteBufferUtil.HexDump(msg));
                }
            }
        }
    }
}
