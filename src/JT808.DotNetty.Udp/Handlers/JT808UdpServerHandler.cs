using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using JT808.Protocol;
using System;
using Microsoft.Extensions.Logging;
using DotNetty.Transport.Channels.Sockets;
using JT808.DotNetty.Core.Metadata;
using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Core.Services;
using JT808.DotNetty.Core;
using JT808.DotNetty.Core.Handlers;

namespace JT808.DotNetty.Udp.Handlers
{
    /// <summary>
    /// JT808 Udp服务端处理程序
    /// </summary>
    internal class JT808UdpServerHandler : SimpleChannelInboundHandler<JT808UdpPackage>
    {
        private readonly IJT808SourcePackageDispatcher jT808SourcePackageDispatcher;

        private readonly JT808UdpAtomicCounterService jT808UdpAtomicCounterService;

        private readonly ILogger<JT808UdpServerHandler> logger;

        private readonly JT808UdpSessionManager jT808UdpSessionManager;

        private readonly JT808MsgIdUdpHandlerBase handler;

        public JT808UdpServerHandler(
            ILoggerFactory loggerFactory,
            IJT808SourcePackageDispatcher jT808SourcePackageDispatcher,
            JT808MsgIdUdpHandlerBase handler,
            JT808UdpAtomicCounterService jT808UdpAtomicCounterService,
            JT808UdpSessionManager jT808UdpSessionManager)
        {
            this.handler = handler;
            this.jT808SourcePackageDispatcher = jT808SourcePackageDispatcher;
            this.jT808UdpAtomicCounterService = jT808UdpAtomicCounterService;
            this.jT808UdpSessionManager = jT808UdpSessionManager;
            logger = loggerFactory.CreateLogger<JT808UdpServerHandler>();
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, JT808UdpPackage msg)
        {
            try
            {
                jT808SourcePackageDispatcher?.SendAsync(msg.Buffer);
                //解析到头部,然后根据具体的消息Id通过队列去进行消费
                //要是一定要解析到数据体可以在JT808MsgIdHandlerBase类中根据具体的消息，
                //解析具体的消息体，具体调用JT808Serializer.Deserialize<T>
                JT808HeaderPackage jT808HeaderPackage = JT808Serializer.Deserialize<JT808HeaderPackage>(msg.Buffer);
                jT808UdpAtomicCounterService.MsgSuccessIncrement();
                jT808UdpSessionManager.TryAdd(new JT808UdpSession {
                     Channel= ctx.Channel,
                     Sender= msg.Sender,
                     TerminalPhoneNo= jT808HeaderPackage.Header.TerminalPhoneNo
                });
                if (logger.IsEnabled(LogLevel.Debug))
                {
                    logger.LogDebug("accept package success count<<<" + jT808UdpAtomicCounterService.MsgSuccessCount.ToString());
                }
                Func<JT808Request, JT808Response> handlerFunc;
                if (handler.HandlerDict.TryGetValue(jT808HeaderPackage.Header.MsgId, out handlerFunc))
                {
                    JT808Response jT808Response = handlerFunc(new JT808Request(jT808HeaderPackage, msg.Buffer));
                    if (jT808Response != null)
                    {
                        ctx.WriteAndFlushAsync(new DatagramPacket(Unpooled.WrappedBuffer(JT808Serializer.Serialize(jT808Response.Package, jT808Response.MinBufferSize)), msg.Sender));
                    }
                }
            }
            catch (JT808.Protocol.Exceptions.JT808Exception ex)
            {
                jT808UdpAtomicCounterService.MsgFailIncrement();
                if (logger.IsEnabled(LogLevel.Error))
                {
                    logger.LogError("accept package fail count<<<" + jT808UdpAtomicCounterService.MsgFailCount.ToString());
                    logger.LogError(ex, "accept msg<<<" + ByteBufferUtil.HexDump(msg.Buffer));
                }
            }
            catch (Exception ex)
            {
                jT808UdpAtomicCounterService.MsgFailIncrement();
                if (logger.IsEnabled(LogLevel.Error))
                {
                    logger.LogError("accept package fail count<<<" + jT808UdpAtomicCounterService.MsgFailCount.ToString());
                    logger.LogError(ex, "accept msg<<<" + ByteBufferUtil.HexDump(msg.Buffer));
                }
            }
        }
    }
}
