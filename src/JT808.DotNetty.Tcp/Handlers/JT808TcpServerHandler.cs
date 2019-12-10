using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using JT808.Protocol;
using System;
using Microsoft.Extensions.Logging;
using JT808.DotNetty.Core.Services;
using JT808.DotNetty.Abstractions.Enums;
using JT808.Protocol.Exceptions;
using JT808.DotNetty.Core.Session;
using JT808.DotNetty.Abstractions;

namespace JT808.DotNetty.Tcp.Handlers
{
    /// <summary>
    /// JT808服务端处理程序
    /// </summary>
    internal class JT808TcpServerHandler : SimpleChannelInboundHandler<byte[]>
    {       
        private readonly JT808SessionManager jT808SessionManager;

        private readonly JT808AtomicCounterService jT808AtomicCounterService;

        private readonly ILogger<JT808TcpServerHandler> logger;

        private readonly JT808Serializer JT808Serializer;

        private readonly IJT808MsgProducer JT808MsgProducer;

        public JT808TcpServerHandler(
            IJT808MsgProducer jT808MsgProducer,
            IJT808Config jT808Config,
            ILoggerFactory loggerFactory,
            JT808AtomicCounterServiceFactory  jT808AtomicCounterServiceFactory,
            JT808SessionManager jT808SessionManager)
        {
            this.jT808SessionManager = jT808SessionManager;
            this.JT808MsgProducer = jT808MsgProducer;
            this.jT808AtomicCounterService = jT808AtomicCounterServiceFactory.Create(JT808TransportProtocolType.tcp);
            this.JT808Serializer = jT808Config.GetSerializer();
            logger = loggerFactory.CreateLogger<JT808TcpServerHandler>();
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, byte[] msg)
        {
            try
            {
                //解析到头部,然后根据具体的消息Id通过队列去进行消费
                //要是一定要解析到数据体可以在JT808MsgIdHandlerBase类中根据具体的消息，
                //解析具体的消息体，具体调用JT808Serializer.Deserialize<T>
                JT808HeaderPackage jT808HeaderPackage = JT808Serializer.HeaderDeserialize(msg);
                if (logger.IsEnabled(LogLevel.Trace))
                {
                    logger.LogTrace($"accept package success count=>{jT808AtomicCounterService.MsgSuccessCount.ToString()},accept msg=>{ByteBufferUtil.HexDump(msg)}");
                }
                jT808AtomicCounterService.MsgSuccessIncrement();
                jT808SessionManager.TryAdd(jT808HeaderPackage.Header.TerminalPhoneNo,ctx.Channel);
                JT808MsgProducer.ProduceAsync(jT808HeaderPackage.Header.TerminalPhoneNo, msg);
            }
            catch (JT808Exception ex)
            {
                jT808AtomicCounterService.MsgFailIncrement();
                if (logger.IsEnabled(LogLevel.Error))
                {
                    logger.LogError(ex,$"accept package fail count=>{jT808AtomicCounterService.MsgFailCount.ToString()},accept msg=>{ByteBufferUtil.HexDump(msg)}");
                }
            }
            catch (Exception ex)
            {
                jT808AtomicCounterService.MsgFailIncrement();
                if (logger.IsEnabled(LogLevel.Error))
                {
                    logger.LogError(ex, $"accept package fail count=>{jT808AtomicCounterService.MsgFailCount.ToString()},accept msg=>{ByteBufferUtil.HexDump(msg)}");
                }
            }
        }
    }
}
