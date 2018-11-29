using DotNetty.Transport.Channels;
using JT808.DotNetty.Internal;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace JT808.DotNetty.Handlers
{
    internal class JT808SourcePackageDispatcherHandler: ChannelHandlerAdapter
    {
        private readonly ILogger<JT808SourcePackageDispatcherHandler> logger;

        private readonly JT808SourcePackageChannelService jT808SourcePackageChannelService;

        public JT808SourcePackageDispatcherHandler(JT808SourcePackageChannelService jT808SourcePackageChannelService)
        {
            logger= jT808SourcePackageChannelService.LoggerFactory.CreateLogger<JT808SourcePackageDispatcherHandler>();
            this.jT808SourcePackageChannelService = jT808SourcePackageChannelService;
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            Policy.HandleResult<bool>(context.Channel.Open)
                     .WaitAndRetryForeverAsync(retryAttempt =>
                     {
                         return retryAttempt > 20 ? TimeSpan.FromSeconds(Math.Pow(2, 50)) : TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));//超过重试20次，接近12个小时链接一次
                     },(exception, timespan, ctx) =>
                    {
                        logger.LogError($"Server Disconnection {context.Channel.RemoteAddress.ToString()}，Retry Results{exception.Result}，Retry Number{timespan}，Next Retry Interval(s){ctx.TotalSeconds}");
                    })
                    .ExecuteAsync(async () =>
                    {
                        try
                        {
                            var newChannel = jT808SourcePackageChannelService.channels.FirstOrDefault(m => m.Value == context.Channel);
                            if (default(KeyValuePair<EndPoint, IChannel>).Equals(newChannel))
                            {
                                if(logger.IsEnabled(LogLevel.Debug))
                                    logger.LogDebug($"Server already deleted {context.Channel.RemoteAddress.ToString()} remote server configuration");
                                return true;
                            }
                            var channel = await jT808SourcePackageChannelService.bootstrap.ConnectAsync(context.Channel.RemoteAddress);
                            jT808SourcePackageChannelService.channels.AddOrUpdate(newChannel.Key, channel, (x, y) => channel);
                            return channel.Open;
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex,$"Reconnection abnormal:After the server is disconnected {context.Channel.RemoteAddress.ToString()}");
                            return false;
                        }
                    });
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if(logger.IsEnabled(LogLevel.Debug))
                logger.LogError($"The server returns a message {message.ToString()}");
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            logger.LogError(exception, context.Channel.RemoteAddress.ToString());
            context.CloseAsync();
        }
    }
}
