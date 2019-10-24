using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.BusinessServices.ReplyMessage
{
    public static class JT808ReplyMessageExtensions
    {
        /// <summary>
        /// 独享消息应答服务（不同的消费者实例）
        /// </summary>
        /// <param name="jT808ClientBuilder"></param>
        /// <returns></returns>
        public static IJT808ClientBuilder AddInprocJT808ReplyMessage(this IJT808ClientBuilder jT808ClientBuilder)
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton<JT808ReplyMessageService>();
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808ReplyMessageHostedService>();
            return jT808ClientBuilder;
        }
        /// <summary>
        /// 独享消息应答服务（不同的消费者实例）
        /// </summary>
        /// <typeparam name="TReplyMessageService">自定义消息回复服务</typeparam>
        /// <param name="jT808ClientBuilder"></param>
        /// <returns></returns>
        public static IJT808ClientBuilder AddInprocJT808ReplyMessage<TReplyMessageService>(this IJT808ClientBuilder jT808ClientBuilder)
            where TReplyMessageService : JT808ReplyMessageService
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton<JT808ReplyMessageService,TReplyMessageService>();
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808ReplyMessageHostedService>();
            return jT808ClientBuilder;
        }
        /// <summary>
        /// 共享消息应答服务（消费者单实例）
        /// </summary>
        /// <typeparam name="TReplyMessageService">自定义消息回复服务</typeparam>
        /// <param name="jT808ClientBuilder"></param>
        /// <returns></returns>
        public static IJT808ClientBuilder AddShareJT808ReplyMessage<TReplyMessageService>(this IJT808ClientBuilder jT808ClientBuilder)
            where TReplyMessageService : JT808ReplyMessageService
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton<JT808ReplyMessageService, TReplyMessageService>();
            return jT808ClientBuilder;
        }
        /// <summary>
        /// 共享消息应答服务（消费者单实例）
        /// </summary>
        /// <param name="jT808ClientBuilder"></param>
        /// <returns></returns>
        public static IJT808ClientBuilder AddShareJT808ReplyMessage(this IJT808ClientBuilder jT808ClientBuilder)
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton<JT808ReplyMessageService>();
            return jT808ClientBuilder;
        }
    }
}
