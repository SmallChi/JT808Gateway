
using JT808.Gateway.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.SessionNotice
{
    public static class JT808SessionNoticeExtensions
    {
        /// <summary>
        /// 会话通知服务（不同的消费者实例）
        /// </summary>
        /// <param name="jT808ClientBuilder"></param>
        /// <returns></returns>
        public static IJT808ClientBuilder AddInPlugSessionNotice(this IJT808ClientBuilder jT808ClientBuilder)
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton<JT808SessionNoticeService>();
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808SessionNoticeHostedService>();
            return jT808ClientBuilder;
        }

        /// <summary>
        /// 消息会话通知服务（不同的消费者实例）
        /// </summary>
        /// <typeparam name="TSessionNoticeService">自定义会话通知服务</typeparam>
        /// <param name="jT808ClientBuilder"></param>
        /// <returns></returns>
        public static IJT808ClientBuilder AddInPlugSessionNotice<TSessionNoticeService>(this IJT808ClientBuilder jT808ClientBuilder)
           where TSessionNoticeService : JT808SessionNoticeService
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton<JT808SessionNoticeService,TSessionNoticeService>();
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808SessionNoticeHostedService>();
            return jT808ClientBuilder;
        }

        /// <summary>
        /// 消息会话通知服务（消费者单实例）
        /// </summary>
        /// <typeparam name="TSessionNoticeService">自定义会话通知服务</typeparam>
        /// <param name="jT808GatewayBuilder"></param>
        /// <returns></returns>
        public static IJT808GatewayBuilder AddInMemorySessionNotice<TSessionNoticeService>(this IJT808GatewayBuilder jT808GatewayBuilder)
          where TSessionNoticeService : JT808SessionNoticeService
        {
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<JT808SessionNoticeService, TSessionNoticeService>();
            jT808GatewayBuilder.JT808Builder.Services.AddHostedService<JT808SessionNoticeHostedService>();
            return jT808GatewayBuilder;
        }

        /// <summary>
        /// 消息会话通知服务（消费者单实例）
        /// </summary>
        /// <param name="jT808GatewayBuilder"></param>
        /// <returns></returns>
        public static IJT808GatewayBuilder AddInMemorySessionNotice(this IJT808GatewayBuilder jT808GatewayBuilder)
        {
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<JT808SessionNoticeService>();
            jT808GatewayBuilder.JT808Builder.Services.AddHostedService<JT808SessionNoticeHostedService>();
            return jT808GatewayBuilder;
        }
    }
}
