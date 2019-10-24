using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.BusinessServices.SessionNotice
{
    public static class JT808SessionNoticeExtensions
    {
        /// <summary>
        /// 独享消息会话通知服务（不同的消费者实例）
        /// </summary>
        /// <param name="jT808ClientBuilder"></param>
        /// <returns></returns>
        public static IJT808ClientBuilder AddInprocJT808SessionNotice(this IJT808ClientBuilder jT808ClientBuilder)
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton<JT808SessionNoticeService>();
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808SessionNoticeHostedService>();
            return jT808ClientBuilder;
        }

        /// <summary>
        /// 独享消息会话通知服务（不同的消费者实例）
        /// </summary>
        /// <typeparam name="TSessionNoticeService">自定义会话通知服务</typeparam>
        /// <param name="jT808ClientBuilder"></param>
        /// <returns></returns>
        public static IJT808ClientBuilder AddInprocJT808SessionNotice<TSessionNoticeService>(this IJT808ClientBuilder jT808ClientBuilder)
           where TSessionNoticeService : JT808SessionNoticeService
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton<JT808SessionNoticeService,TSessionNoticeService>();
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808SessionNoticeHostedService>();
            return jT808ClientBuilder;
        }

        /// <summary>
        /// 共享消息会话通知服务（消费者单实例）
        /// </summary>
        /// <typeparam name="TSessionNoticeService">自定义会话通知服务</typeparam>
        /// <param name="jT808ClientBuilder"></param>
        /// <returns></returns>
        public static IJT808ClientBuilder AddShareJT808SessionNotice<TSessionNoticeService>(this IJT808ClientBuilder jT808ClientBuilder)
          where TSessionNoticeService : JT808SessionNoticeService
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton<JT808SessionNoticeService, TSessionNoticeService>();
            return jT808ClientBuilder;
        }

        /// <summary>
        /// 共享消息会话通知服务（消费者单实例）
        /// </summary>
        /// <param name="jT808ClientBuilder"></param>
        /// <returns></returns>
        public static IJT808ClientBuilder AddShareJT808SessionNotice(this IJT808ClientBuilder jT808ClientBuilder)
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton<JT808SessionNoticeService>();
            return jT808ClientBuilder;
        }
    }
}
