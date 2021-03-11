
using JT808.Gateway.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.ReplyMessage
{
    /// <summary>
    /// 消息应答服务扩展
    /// </summary>
    public static class JT808ReplyMessageExtensions
    {
        /// <summary>
        /// 消息下行服务
        /// </summary>
        /// <param name="jT808ClientBuilder"></param>
        /// <returns></returns>
        public static IJT808ClientBuilder AddReplyMessage<TJT808ReplyMessageHandler>(this IJT808ClientBuilder jT808ClientBuilder)
            where TJT808ReplyMessageHandler : IJT808DownMessageHandler
        {
            jT808ClientBuilder.JT808Builder.Services.Add(new ServiceDescriptor(typeof(IJT808DownMessageHandler),typeof(TJT808ReplyMessageHandler), ServiceLifetime.Singleton));
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808ReplyMessageHostedService>();
            return jT808ClientBuilder;
        }
    }
}
