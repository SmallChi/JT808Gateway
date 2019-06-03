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
using JT808.DotNetty.Core.Interfaces;
using JT808.DotNetty.Abstractions.Enums;

namespace JT808.DotNetty.Udp.Handlers
{
    /// <summary>
    /// JT808 Udp服务端处理程序
    /// </summary>
    internal class JT808UdpServerHandler : SimpleChannelInboundHandler<JT808UdpPackage>
    {
        private readonly IJT808SourcePackageDispatcher jT808SourcePackageDispatcher;

        private readonly JT808AtomicCounterService jT808AtomicCounterService;

        private readonly ILogger<JT808UdpServerHandler> logger;

        private readonly JT808UdpSessionManager jT808UdpSessionManager;

        private readonly JT808MsgIdUdpHandlerBase handler;

        private readonly JT808TrafficService jT808TrafficService;

        private readonly IJT808UplinkPacket jT808UplinkPacket;

        private readonly IJT808DatagramPacket jT808DatagramPacket;

        private readonly ILogger unknownLogger;
        public JT808UdpServerHandler(
            IJT808DatagramPacket jT808DatagramPacket,
            JT808TrafficServiceFactory  jT808TrafficServiceFactory,
            ILoggerFactory loggerFactory,
            IJT808SourcePackageDispatcher jT808SourcePackageDispatcher,
            IJT808UplinkPacket jT808UplinkPacket,
            JT808MsgIdUdpHandlerBase handler,
            JT808AtomicCounterServiceFactory  jT808AtomicCounterServiceFactory,
            JT808UdpSessionManager jT808UdpSessionManager)
        {
            this.jT808DatagramPacket = jT808DatagramPacket;
            this.jT808TrafficService = jT808TrafficServiceFactory.Create(JT808TransportProtocolType.udp);
            this.handler = handler;
            this.jT808SourcePackageDispatcher = jT808SourcePackageDispatcher;
            this.jT808AtomicCounterService = jT808AtomicCounterServiceFactory.Create(JT808TransportProtocolType.udp);
            this.jT808UplinkPacket = jT808UplinkPacket;
            this.jT808UdpSessionManager = jT808UdpSessionManager;
            logger = loggerFactory.CreateLogger<JT808UdpServerHandler>();
            unknownLogger = loggerFactory.CreateLogger("udp_unknown_msgid");
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, JT808UdpPackage msg)
        {
            try
            {
                jT808SourcePackageDispatcher.SendAsync(msg.Buffer);
                jT808UplinkPacket.ProcessorAsync(msg.Buffer, JT808TransportProtocolType.udp);
                //解析到头部,然后根据具体的消息Id通过队列去进行消费
                //要是一定要解析到数据体可以在JT808MsgIdHandlerBase类中根据具体的消息，
                //解析具体的消息体，具体调用JT808Serializer.Deserialize<T>
                JT808HeaderPackage jT808HeaderPackage = JT808Serializer.Deserialize<JT808HeaderPackage>(msg.Buffer);
                jT808AtomicCounterService.MsgSuccessIncrement();
                jT808TrafficService.ReceiveSize(msg.Buffer.Length);
                jT808UdpSessionManager.TryAdd(ctx.Channel, msg.Sender, jT808HeaderPackage.Header.TerminalPhoneNo);
                if (logger.IsEnabled(LogLevel.Trace))
                {
                    logger.LogTrace("accept package success count<<<" + jT808AtomicCounterService.MsgSuccessCount.ToString());
                    logger.LogTrace("accept msg <<< " + ByteBufferUtil.HexDump(msg.Buffer));
                }
                if (handler.HandlerDict.TryGetValue(jT808HeaderPackage.Header.MsgId, out var handlerFunc))
                {
                    IJT808Reply jT808Response = handlerFunc(new JT808Request(jT808HeaderPackage, msg.Buffer));
                    if (jT808Response != null)
                    {
                        var sendData = JT808Serializer.Serialize(jT808Response.Package, jT808Response.MinBufferSize);
                        ctx.WriteAndFlushAsync(jT808DatagramPacket.Create(sendData,msg.Sender));
                    }
                }
                else
                {
                    //未知的消息类型已日志形式输出
                    if (unknownLogger.IsEnabled(LogLevel.Debug))
                    {
                        unknownLogger.LogDebug(ByteBufferUtil.HexDump(msg.Buffer));
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

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

    }
}
