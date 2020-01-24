
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
        public static IJT808ClientBuilder AddJT808InPlugReplyMessage(this IJT808ClientBuilder jT808ClientBuilder)
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton<JT808ReplyMessageHandler>();
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808ReplyMessageHostedService>();
            return jT808ClientBuilder;
        }
        /// <summary>
        /// 消息应答服务（不同的消费者实例）
        /// </summary>
        /// <typeparam name="TReplyMessageService">自定义消息回复服务</typeparam>
        /// <param name="jT808ClientBuilder"></param>
        /// <returns></returns>
        public static IJT808ClientBuilder AddJT808InPlugReplyMessage<TReplyMessageHandler>(this IJT808ClientBuilder jT808ClientBuilder)
            where TReplyMessageHandler : JT808ReplyMessageHandler
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton<JT808ReplyMessageHandler, TReplyMessageHandler>();
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808ReplyMessageHostedService>();
            return jT808ClientBuilder;
        }
        /// <summary>
        /// 消息应答服务（消费者单实例）
        /// </summary>
        /// <typeparam name="TReplyMessageService">自定义消息回复服务</typeparam>
        /// <param name="jT808GatewayBuilder"></param>
        /// <returns></returns>
        public static IJT808GatewayBuilder AddJT808InMemoryReplyMessage<TReplyMessageHandler>(this IJT808GatewayBuilder jT808GatewayBuilder)
            where TReplyMessageHandler : JT808ReplyMessageHandler
        {
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<JT808ReplyMessageHandler, TReplyMessageHandler>();
            jT808GatewayBuilder.JT808Builder.Services.AddHostedService<JT808ReplyMessageHostedService>();
            return jT808GatewayBuilder;
        }
        /// <summary>
        /// 消息应答服务（消费者单实例）
        /// </summary>
        /// <param name="jT808GatewayBuilder"></param>
        /// <returns></returns>
        public static IJT808GatewayBuilder AddJT808InMemoryReplyMessage(this IJT808GatewayBuilder jT808GatewayBuilder)
        {
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<JT808ReplyMessageHandler>();
            jT808GatewayBuilder.JT808Builder.Services.AddHostedService<JT808ReplyMessageHostedService>();
            return jT808GatewayBuilder;
        }
    }
}
