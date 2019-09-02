using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Polly;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Microsoft.Extensions.Logging;

namespace JT808.DotNetty.Transmit.Handlers
{
    public class ClientConnectionHandler : ChannelHandlerAdapter
    {
        private readonly Bootstrap bootstrap;
        public Dictionary<string, IChannel> channeldic;
        private readonly ILogger<ClientConnectionHandler> logger;
        public ClientConnectionHandler(Bootstrap bootstrap,
                                                            Dictionary<string, IChannel> channeldic,
                                                            ILoggerFactory loggerFactory)
        {
            this.bootstrap = bootstrap;
            this.channeldic = channeldic;
            logger = loggerFactory.CreateLogger<ClientConnectionHandler>();
        }
        public override void ChannelInactive(IChannelHandlerContext context)
        {
            Policy.HandleResult<bool>(context.Channel.Open)
                     .WaitAndRetryForeverAsync(retryAttempt =>
                     {
                         return retryAttempt > 20 ? TimeSpan.FromSeconds(Math.Pow(2, 50)) : TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));//超过重试20次，之后重试都是接近12个小时重试一次
                     },
                        (exception, timespan, ctx) =>
                        {
                            logger.LogError($"服务端断开{context.Channel.RemoteAddress}，重试结果{exception.Result}，重试次数{timespan}，下次重试间隔(s){ctx.TotalSeconds}");
                        })
                    .ExecuteAsync(async () =>
                    {
                        try
                        {
                            var oldChannel = channeldic.FirstOrDefault(m => m.Value == context.Channel);
                            if (default(KeyValuePair<string, IChannel>).Equals(oldChannel))
                            {
                                if(logger.IsEnabled( LogLevel.Debug))
                                    logger.LogDebug($"服务器已经删除了{oldChannel.Key}远程服务器配置");
                                return true;
                            }
                            var channel = await bootstrap.ConnectAsync(context.Channel.RemoteAddress);
                            channeldic.Remove(oldChannel.Key);
                            channeldic.Add(oldChannel.Key, channel);
                            return channel.Open;
                        }
                        catch (Exception ex)
                        {
                            logger.LogError($"服务端断开后{context.Channel.RemoteAddress}，重连异常：{ex}");
                            return false;
                        }
                    });
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogError($"服务端返回消息{message}");
            }
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            logger.LogError($"服务端Exception: {exception}");
            context.CloseAsync();
        }
    }
}
