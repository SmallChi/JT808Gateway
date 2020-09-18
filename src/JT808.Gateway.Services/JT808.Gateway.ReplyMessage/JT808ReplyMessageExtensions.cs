
using JT808.Gateway.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.ReplyMessage
{
    public static class JT808ReplyMessageExtensions
    {
        /// <summary>
        /// 消息应答服务（不同的消费者实例）
        /// </summary>
        /// <param name="jT808ClientBuilder"></param>
        /// <returns></returns>
        public static IJT808ClientBuilder AddReplyMessage<TJT808ReplyMessageHandler>(this IJT808ClientBuilder jT808ClientBuilder)
            where TJT808ReplyMessageHandler : IJT808ReplyMessageHandler
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton(new ServiceDescriptor(typeof(IJT808ReplyMessageHandler),typeof(TJT808ReplyMessageHandler), ServiceLifetime.Singleton));
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808ReplyMessageHostedService>();
            return jT808ClientBuilder;
        }
    }
}
