
using JT808.Gateway.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.ReplyMessage
{
    public static class JT808ReplyMessageExtensions
    {
        ///// <summary>
        ///// 消息应答服务（不同的消费者实例）
        ///// </summary>
        ///// <param name="jT808ClientBuilder"></param>
        ///// <returns></returns>
        //public static IJT808ClientBuilder AddReplyMessage(this IJT808ClientBuilder jT808ClientBuilder)
        //{
        //    jT808ClientBuilder.JT808Builder.Services.AddSingleton<JT808QueueReplyMessageHandler>();
        //    jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808ReplyMessageHostedService>();
        //    return jT808ClientBuilder;
        //}
        ///// <summary>
        ///// 消息应答服务（不同的消费者实例）
        ///// </summary>
        ///// <typeparam name="TReplyMessageService">自定义消息回复服务</typeparam>
        ///// <param name="jT808ClientBuilder"></param>
        ///// <returns></returns>
        //public static IJT808ClientBuilder AddReplyMessage<TReplyMessageHandler>(this IJT808ClientBuilder jT808ClientBuilder)
        //    where TReplyMessageHandler : JT808QueueReplyMessageHandler
        //{
        //    jT808ClientBuilder.JT808Builder.Services.AddSingleton<JT808QueueReplyMessageHandler, TReplyMessageHandler>();
        //    jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808ReplyMessageHostedService>();
        //    return jT808ClientBuilder;
        //}
    }
}
