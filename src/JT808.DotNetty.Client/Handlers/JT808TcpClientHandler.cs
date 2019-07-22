using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using JT808.DotNetty.Client.Services;

namespace JT808.DotNetty.Client.Handlers
{
    /// <summary>
    /// JT808客户端处理程序
    /// </summary>
    internal class JT808TcpClientHandler : SimpleChannelInboundHandler<byte[]>
    {
        private readonly ILogger<JT808TcpClientHandler> logger;
        private readonly JT808ReceiveAtomicCounterService jT808ReceiveAtomicCounterService;
        public JT808TcpClientHandler(JT808ReceiveAtomicCounterService jT808ReceiveAtomicCounterService,JT808TcpClient jT808TcpClient)
        {
            logger = jT808TcpClient.LoggerFactory.CreateLogger<JT808TcpClientHandler>();
            this.jT808ReceiveAtomicCounterService= jT808ReceiveAtomicCounterService;
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, byte[] msg)
        {
            if(logger.IsEnabled(LogLevel.Trace))
                logger.LogTrace("accept msg<<<" + ByteBufferUtil.HexDump(msg));
            jT808ReceiveAtomicCounterService.MsgSuccessIncrement();
        }
    }
}
