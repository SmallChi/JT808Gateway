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
using System.Threading.Tasks;

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

        private readonly JT808UdpTrafficService jT808UdpTrafficService;

        public JT808UdpServerHandler(
            JT808UdpTrafficService jT808UdpTrafficService,
            ILoggerFactory loggerFactory,
            IJT808SourcePackageDispatcher jT808SourcePackageDispatcher,
            JT808MsgIdUdpHandlerBase handler,
            JT808UdpAtomicCounterService jT808UdpAtomicCounterService,
            JT808UdpSessionManager jT808UdpSessionManager)
        {
            this.jT808UdpTrafficService = jT808UdpTrafficService;
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
                jT808UdpTrafficService.ReceiveSize(msg.Buffer.Length);
                //解析到头部,然后根据具体的消息Id通过队列去进行消费
                //要是一定要解析到数据体可以在JT808MsgIdHandlerBase类中根据具体的消息，
                //解析具体的消息体，具体调用JT808Serializer.Deserialize<T>
                JT808HeaderPackage jT808HeaderPackage = JT808Serializer.Deserialize<JT808HeaderPackage>(msg.Buffer);
                jT808UdpAtomicCounterService.MsgSuccessIncrement();
                jT808UdpSessionManager.TryAdd(new JT808UdpSession(ctx.Channel, msg.Sender, jT808HeaderPackage.Header.TerminalPhoneNo));
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
                        var sendData = JT808Serializer.Serialize(jT808Response.Package, jT808Response.MinBufferSize);
                        jT808UdpTrafficService.SendSize(sendData.Length);
                        ctx.WriteAndFlushAsync(new DatagramPacket(Unpooled.WrappedBuffer(sendData), msg.Sender));
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

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

    }
}
