using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace JT808.DotNetty.Client.Handlers
{
    /// <summary>
    /// JT808客户端处理程序
    /// </summary>
    internal class JT808TcpClientHandler : SimpleChannelInboundHandler<byte[]>
    {
        private readonly ILogger<JT808TcpClientHandler> logger;

        public JT808TcpClientHandler(JT808TcpClient jT808TcpClient)
        {
            logger = jT808TcpClient.LoggerFactory.CreateLogger<JT808TcpClientHandler>();
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, byte[] msg)
        {
            if(logger.IsEnabled(LogLevel.Debug))
                logger.LogDebug("accept msg<<<" + ByteBufferUtil.HexDump(msg));
        }
    }
}
