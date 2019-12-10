using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using JT808.Protocol;
using System;
using Microsoft.Extensions.Logging;
using JT808.DotNetty.Core.Metadata;
using JT808.DotNetty.Core.Services;
using JT808.DotNetty.Abstractions.Enums;
using JT808.DotNetty.Core.Session;
using JT808.DotNetty.Abstractions;

namespace JT808.DotNetty.Udp.Handlers
{
    /// <summary>
    /// JT808 Udp服务端处理程序
    /// </summary>
    internal class JT808UdpServerHandler : SimpleChannelInboundHandler<JT808UdpPackage>
    {
        private readonly JT808AtomicCounterService jT808AtomicCounterService;

        private readonly ILogger<JT808UdpServerHandler> logger;

        private readonly JT808SessionManager jT808UdpSessionManager;

        private readonly JT808Serializer JT808Serializer;

        private readonly IJT808MsgProducer JT808MsgProducer;

        public JT808UdpServerHandler(
            IJT808MsgProducer jT808MsgProducer,
            IJT808Config jT808Config,
            ILoggerFactory loggerFactory,
            JT808AtomicCounterServiceFactory  jT808AtomicCounterServiceFactory,
            JT808SessionManager jT808UdpSessionManager)
        {
            this.JT808MsgProducer = jT808MsgProducer;
            this.jT808AtomicCounterService = jT808AtomicCounterServiceFactory.Create(JT808TransportProtocolType.udp);
            this.jT808UdpSessionManager = jT808UdpSessionManager;
            logger = loggerFactory.CreateLogger<JT808UdpServerHandler>();
            JT808Serializer = jT808Config.GetSerializer();
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, JT808UdpPackage msg)
        {
            try
            {
                //解析到头部,然后根据具体的消息Id通过队列去进行消费
                //要是一定要解析到数据体可以在JT808MsgIdHandlerBase类中根据具体的消息，
                //解析具体的消息体，具体调用JT808Serializer.Deserialize<T>
                JT808HeaderPackage jT808HeaderPackage = JT808Serializer.HeaderDeserialize(msg.Buffer);
                if (logger.IsEnabled(LogLevel.Trace))
                {
                    logger.LogTrace($"accept package success count=>{jT808AtomicCounterService.MsgFailCount.ToString()},accept msg=>{ByteBufferUtil.HexDump(msg.Buffer)}");
                }
                jT808AtomicCounterService.MsgSuccessIncrement();
                jT808UdpSessionManager.TryAdd(ctx.Channel, msg.Sender, jT808HeaderPackage.Header.TerminalPhoneNo);
                JT808MsgProducer.ProduceAsync(jT808HeaderPackage.Header.TerminalPhoneNo, msg.Buffer);
            }
            catch (JT808.Protocol.Exceptions.JT808Exception ex)
            {
                jT808AtomicCounterService.MsgFailIncrement();
                if (logger.IsEnabled(LogLevel.Error))
                {
                    logger.LogError(ex, $"accept package fail count=>{jT808AtomicCounterService.MsgFailCount.ToString()},accept msg=>{ByteBufferUtil.HexDump(msg.Buffer)}");
                }
            }
            catch (Exception ex)
            {
                jT808AtomicCounterService.MsgFailIncrement();
                if (logger.IsEnabled(LogLevel.Error))
                {
                    logger.LogError(ex, $"accept package fail count=>{jT808AtomicCounterService.MsgFailCount.ToString()},accept msg=>{ByteBufferUtil.HexDump(msg.Buffer)}");
                }
            }
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();
    }
}
