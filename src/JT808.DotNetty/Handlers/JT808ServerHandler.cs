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

namespace JT808.DotNetty.Handlers
{
    /// <summary>
    /// JT808服务端处理程序
    /// </summary>
    internal class JT808ServerHandler : SimpleChannelInboundHandler<byte[]>
    {
        private readonly JT808MsgIdHandlerBase handler;
        
        private readonly JT808SessionManager jT808SessionManager;

        private readonly JT808TransmitAddressFilterService jT808TransmitAddressFilterService;

        private readonly IJT808SourcePackageDispatcher jT808SourcePackageDispatcher;

        private readonly JT808AtomicCounterService jT808AtomicCounterService;

        private readonly ILogger<JT808ServerHandler> logger;

        public JT808ServerHandler(
            ILoggerFactory loggerFactory,
            JT808TransmitAddressFilterService jT808TransmitAddressFilterService,
            IJT808SourcePackageDispatcher jT808SourcePackageDispatcher,
            JT808MsgIdHandlerBase handler,
            JT808AtomicCounterService jT808AtomicCounterService,
            JT808SessionManager jT808SessionManager)
        {
            this.jT808TransmitAddressFilterService = jT808TransmitAddressFilterService;
            this.handler = handler;
            this.jT808SessionManager = jT808SessionManager;
            this.jT808SourcePackageDispatcher = jT808SourcePackageDispatcher;
            this.jT808AtomicCounterService = jT808AtomicCounterService;
            logger = loggerFactory.CreateLogger<JT808ServerHandler>();
        }


        protected override void ChannelRead0(IChannelHandlerContext ctx, byte[] msg)
        {
            try
            {
                jT808SourcePackageDispatcher?.SendAsync(msg);
                //在压力大的情况下可以只解析到头部
                //然后根据具体的消息Id通过队列去进行消费
                //JT808HeaderPackage jT808HeaderPackage = JT808Serializer.Deserialize<JT808HeaderPackage>(msg);
                JT808Package jT808Package = JT808Serializer.Deserialize(msg);
                jT808AtomicCounterService.MsgSuccessIncrement();
                if (logger.IsEnabled(LogLevel.Debug))
                {
                    logger.LogDebug("accept package success count<<<" + jT808AtomicCounterService.MsgSuccessCount.ToString());
                }
                jT808SessionManager.TryAdd(new JT808Session(ctx.Channel, jT808Package.Header.TerminalPhoneNo));
                Func<JT808Request, JT808Response> handlerFunc;
                if (handler.HandlerDict.TryGetValue(jT808Package.Header.MsgId, out handlerFunc))
                {
                    //JT808Response jT808Response = handlerFunc(new JT808Request(jT808HeaderPackage, msg));
                    JT808Response jT808Response = handlerFunc(new JT808Request(jT808Package, msg));
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
