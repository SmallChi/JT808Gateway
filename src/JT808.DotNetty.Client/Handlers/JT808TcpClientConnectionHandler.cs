using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using JT808.Protocol.Enums;
using JT808.Protocol.Extensions;
using JT808.Protocol.MessageBody;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace JT808.DotNetty.Client.Handlers
{
    /// <summary>
    /// JT808客户端连接通道处理程序
    /// </summary>
    public class JT808TcpClientConnectionHandler : ChannelHandlerAdapter
    {
        private readonly ILogger logger;
        private readonly JT808TcpClient jT808TcpClient;

        public JT808TcpClientConnectionHandler(
            JT808TcpClient jT808TcpClient)
        {
            logger = jT808TcpClient.LoggerFactory.CreateLogger<JT808TcpClientConnectionHandler>();
            this.jT808TcpClient = jT808TcpClient;
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
                if(idleStateEvent.State== IdleState.WriterIdle)
                {
                    string channelId = context.Channel.Id.AsShortText();
                    logger.LogInformation($"{idleStateEvent.State.ToString()}>>>{channelId}");
                    if(jT808TcpClient.DeviceConfig.Version== JT808Version.JTT2019)
                    {
                        jT808TcpClient.Send(JT808MsgId.终端心跳.Create2019(jT808TcpClient.DeviceConfig.TerminalPhoneNo,new JT808_0x0002()));
                    }
                    else
                    {
                        jT808TcpClient.Send(JT808MsgId.终端心跳.Create(jT808TcpClient.DeviceConfig.TerminalPhoneNo, new JT808_0x0002()));
                    }
                }
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

