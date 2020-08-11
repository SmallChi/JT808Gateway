using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using JT808.DotNetty.Core;
using JT808.DotNetty.Core.Session;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace JT808.DotNetty.Tcp.Handlers
{
    /// <summary>
    /// JT808服务通道处理程序
    /// </summary>
    internal class JT808TcpConnectionHandler : ChannelHandlerAdapter
    {
        private readonly ILogger<JT808TcpConnectionHandler> logger;

        private readonly JT808SessionManager jT808SessionManager;

        public JT808TcpConnectionHandler(
            JT808SessionManager jT808SessionManager,
            ILoggerFactory loggerFactory)
        {
            this.jT808SessionManager = jT808SessionManager;
            logger = loggerFactory.CreateLogger<JT808TcpConnectionHandler>();
        }

        /// <summary>
        /// 通道激活
        /// </summary>
        /// <param name="context"></param>
        public override void ChannelActive(IChannelHandlerContext context)
        {
            string channelId = context.Channel.Id.AsShortText();
            if (logger.IsEnabled(LogLevel.Debug))
                logger.LogDebug($"<<<{ channelId } Successful client connection to server.");
            base.ChannelActive(context);
        }

        /// <summary>
        /// 设备主动断开
        /// </summary>
        /// <param name="context"></param>
        public override void ChannelInactive(IChannelHandlerContext context)
        {
            string channelId = context.Channel.Id.AsShortText();
            if (logger.IsEnabled(LogLevel.Debug))
                logger.LogDebug($">>>{ channelId } The client disconnects from the server.");
            base.ChannelInactive(context);
            base.CloseAsync(context);
        }

        /// <summary>
        /// 服务器主动断开
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task CloseAsync(IChannelHandlerContext context)
        {
            string channelId = context.Channel.Id.AsShortText();
            if (logger.IsEnabled(LogLevel.Debug))
                logger.LogDebug($"<<<{ channelId } The server disconnects from the client.");
            jT808SessionManager.RemoveSessionByChannel(context.Channel);
            return base.CloseAsync(context);
        }

        public override void ChannelReadComplete(IChannelHandlerContext context)=> context.Flush();

        /// <summary>
        /// 超时策略
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evt"></param>
        public override void UserEventTriggered(IChannelHandlerContext context, object evt)
        {
            IdleStateEvent idleStateEvent = evt as IdleStateEvent;
            if (idleStateEvent != null)
            {
                if(idleStateEvent.State== IdleState.ReaderIdle)
                {
                    string channelId = context.Channel.Id.AsShortText();
                    logger.LogInformation($"{idleStateEvent.State.ToString()}>>>{channelId}");
                    // 由于808是设备发心跳，如果很久没有上报数据，那么就由服务器主动关闭连接。
                    context.CloseAsync();
                }
                // 按照808的消息，有些请求必须要应答，但是转发可以不需要有应答可以节省部分资源包括：
                // 1.消息的序列化
                // 2.消息的下发
                // 都有一定的性能损耗，那么不需要判断写超时 IdleState.WriterIdle 
                // 就跟神兽貔貅一样。。。
            }
            base.UserEventTriggered(context, evt);
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            string channelId = context.Channel.Id.AsShortText();
            logger.LogError(exception,$"{channelId} {exception.Message}" );
            context.CloseAsync();
        }
    }
}

