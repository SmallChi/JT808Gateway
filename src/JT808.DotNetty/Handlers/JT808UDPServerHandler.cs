using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using JT808.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using JT808.DotNetty.Metadata;
using JT808.DotNetty.Internal;
using JT808.DotNetty.Interfaces;
using Microsoft.Extensions.Logging;
using DotNetty.Transport.Channels.Sockets;

namespace JT808.DotNetty.Handlers
{
    /// <summary>
    /// JT808 UDP服务端处理程序
    /// </summary>
    internal class JT808UDPServerHandler : SimpleChannelInboundHandler<JT808UDPPackage>
    {
        private readonly JT808MsgIdHandlerBase handler;
        
        private readonly JT808SessionManager jT808SessionManager;

        private readonly IJT808SourcePackageDispatcher jT808SourcePackageDispatcher;

        private readonly JT808AtomicCounterService jT808AtomicCounterService;

        private readonly ILogger<JT808UDPServerHandler> logger;

        public JT808UDPServerHandler(
            ILoggerFactory loggerFactory,
            IJT808SourcePackageDispatcher jT808SourcePackageDispatcher,
            JT808MsgIdHandlerBase handler,
            JT808AtomicCounterService jT808AtomicCounterService,
            JT808SessionManager jT808SessionManager)
        {
            this.handler = handler;
            this.jT808SessionManager = jT808SessionManager;
            this.jT808SourcePackageDispatcher = jT808SourcePackageDispatcher;
            this.jT808AtomicCounterService = jT808AtomicCounterService;
            logger = loggerFactory.CreateLogger<JT808UDPServerHandler>();
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, JT808UDPPackage msg)
        {
            try
            {
                jT808SourcePackageDispatcher?.SendAsync(msg.Buffer);
                //解析到头部,然后根据具体的消息Id通过队列去进行消费
                //要是一定要解析到数据体可以在JT808MsgIdHandlerBase类中根据具体的消息，
                //解析具体的消息体，具体调用JT808Serializer.Deserialize<T>
                JT808HeaderPackage jT808HeaderPackage = JT808Serializer.Deserialize<JT808HeaderPackage>(msg.Buffer);
                jT808AtomicCounterService.MsgSuccessIncrement();
                if (logger.IsEnabled(LogLevel.Debug))
                {
                    logger.LogDebug("accept package success count<<<" + jT808AtomicCounterService.MsgSuccessCount.ToString());
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
                jT808AtomicCounterService.MsgFailIncrement();
                if (logger.IsEnabled(LogLevel.Error))
                {
                    logger.LogError("accept package fail count<<<" + jT808AtomicCounterService.MsgFailCount.ToString());
                    logger.LogError(ex, "accept msg<<<" + ByteBufferUtil.HexDump(msg.Buffer));
                }
            }
            catch (Exception ex)
            {
                jT808AtomicCounterService.MsgFailIncrement();
                if (logger.IsEnabled(LogLevel.Error))
                {
                    logger.LogError("accept package fail count<<<" + jT808AtomicCounterService.MsgFailCount.ToString());
                    logger.LogError(ex, "accept msg<<<" + ByteBufferUtil.HexDump(msg.Buffer));
                }
            }
        }
    }
}
