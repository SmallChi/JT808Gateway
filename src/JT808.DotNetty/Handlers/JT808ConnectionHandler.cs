using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using JT808.DotNetty.Metadata;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JT808.DotNetty.Handlers
{
   internal class JT808ConnectionHandler : ChannelHandlerAdapter
    {
        private readonly ILogger<JT808ConnectionHandler> logger;

        private readonly JT808SessionManager jT808SessionManager;

        public JT808ConnectionHandler(
            JT808SessionManager jT808SessionManager,
            ILoggerFactory loggerFactory)
        {
            this.jT808SessionManager = jT808SessionManager;
            logger = loggerFactory.CreateLogger<JT808ConnectionHandler>();
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
            jT808SessionManager.RemoveSessionByID(channelId);
            base.ChannelInactive(context);
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
            jT808SessionManager.RemoveSessionByID(channelId);
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
                string channelId = context.Channel.Id.AsShortText();
                logger.LogInformation($"{idleStateEvent.State.ToString()}>>>{channelId}");
                // 由于808是设备发心跳，如果很久没有上报数据，那么就由服务器主动关闭连接。
                jT808SessionManager.RemoveSessionByID(channelId);
                context.CloseAsync();
            }
            base.UserEventTriggered(context, evt);
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            string channelId = context.Channel.Id.AsShortText();
            logger.LogError(exception,$"{channelId} {exception.Message}" );
            jT808SessionManager.RemoveSessionByID(channelId);
            context.CloseAsync();
        }
    }
}

