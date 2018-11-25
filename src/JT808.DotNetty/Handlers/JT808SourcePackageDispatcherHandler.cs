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

        private readonly JT808SourcePackageDispatcherDefaultImpl jT808SourcePackageDispatcherDefaultImpl;

        public JT808SourcePackageDispatcherHandler(JT808SourcePackageDispatcherDefaultImpl jT808SourcePackageDispatcherDefaultImpl)
        {
            logger=jT808SourcePackageDispatcherDefaultImpl.loggerFactory.CreateLogger<JT808SourcePackageDispatcherHandler>();
            this.jT808SourcePackageDispatcherDefaultImpl = jT808SourcePackageDispatcherDefaultImpl;
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            Policy.HandleResult<bool>(context.Channel.Open)
                     .WaitAndRetryForeverAsync(retryAttempt =>
                     {
                         return retryAttempt > 20 ? TimeSpan.FromSeconds(Math.Pow(2, 50)) : TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));//超过重试20次，接近12个小时链接一次
                     },(exception, timespan, ctx) =>
                    {
                        logger.LogError($"服务端断开{context.Channel.RemoteAddress.ToString()}，重试结果{exception.Result}，重试次数{timespan}，下次重试间隔(s){ctx.TotalSeconds}");
                    })
                    .ExecuteAsync(async () =>
                    {
                        try
                        {
                            var newChannel = jT808SourcePackageDispatcherDefaultImpl.channels.FirstOrDefault(m => m.Value == context.Channel);
                            if (default(KeyValuePair<EndPoint, IChannel>).Equals(newChannel))
                            {
                                if(logger.IsEnabled(LogLevel.Debug))
                                    logger.LogDebug($"服务器已经删除了{context.Channel.RemoteAddress.ToString()}远程服务器配置");
                                return true;
                            }
                            var channel = await jT808SourcePackageDispatcherDefaultImpl.bootstrap.ConnectAsync(context.Channel.RemoteAddress);
                            jT808SourcePackageDispatcherDefaultImpl.channels.AddOrUpdate(newChannel.Key, channel, (x, y) => channel);
                            return channel.Open;
                        }
                        catch (Exception ex)
                        {
                            logger.LogError($"服务端断开后{context.Channel.RemoteAddress.ToString()}，重连异常：{ex}");
                            return false;
                        }
                    });
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if(logger.IsEnabled(LogLevel.Debug))
                logger.LogError($"服务端返回消息{message.ToString()}");
            //throw new Exception("test");
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            logger.LogError(exception, context.Channel.RemoteAddress.ToString());
            context.CloseAsync();
        }
    }
}
