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
using JT808.DotNetty.Core.Interfaces;

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

        private readonly JT808AtomicCounterService jT808AtomicCounterService;

        private readonly JT808TrafficService jT808TrafficService;

        private readonly ILogger<JT808TcpServerHandler> logger;

        private readonly ILogger unknownLogger;

        public JT808TcpServerHandler(
            JT808TrafficServiceFactory  jT808TrafficServiceFactory,
            ILoggerFactory loggerFactory,
            JT808TransmitAddressFilterService jT808TransmitAddressFilterService,
            IJT808SourcePackageDispatcher jT808SourcePackageDispatcher,
            JT808MsgIdTcpHandlerBase handler,
            JT808AtomicCounterServiceFactory  jT808AtomicCounterServiceFactory,
            JT808TcpSessionManager jT808SessionManager)
        {
            this.jT808TrafficService = jT808TrafficServiceFactory.Create(Core.Enums.JT808ModeType.Tcp);
            this.jT808TransmitAddressFilterService = jT808TransmitAddressFilterService;
            this.handler = handler;
            this.jT808SessionManager = jT808SessionManager;
            this.jT808SourcePackageDispatcher = jT808SourcePackageDispatcher;
            this.jT808AtomicCounterService = jT808AtomicCounterServiceFactory.Create(Core.Enums.JT808ModeType.Tcp);
            logger = loggerFactory.CreateLogger<JT808TcpServerHandler>();
            unknownLogger = loggerFactory.CreateLogger("tcp_unknown_msgid");
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
                jT808TrafficService.ReceiveSize(msg.Length);
                if (logger.IsEnabled(LogLevel.Debug))
                {
                    logger.LogDebug("accept package success count<<<" + jT808AtomicCounterService.MsgSuccessCount.ToString());
                }
                jT808SessionManager.TryAdd(jT808HeaderPackage.Header.TerminalPhoneNo,ctx.Channel);
                if (handler.HandlerDict.TryGetValue(jT808HeaderPackage.Header.MsgId, out var handlerFunc))
                {
                    IJT808Reply jT808Response = handlerFunc(new JT808Request(jT808HeaderPackage, msg));
                    if (jT808Response != null)
                    {
                        if (!jT808TransmitAddressFilterService.ContainsKey(ctx.Channel.RemoteAddress))
                        {
                            ctx.WriteAndFlushAsync(jT808Response);
                        }
                    }
                }
                else
                {
                    //未知的消息类型已日志形式输出
                    if (unknownLogger.IsEnabled(LogLevel.Debug))
                    {
                        unknownLogger.LogDebug(ByteBufferUtil.HexDump(msg));
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
