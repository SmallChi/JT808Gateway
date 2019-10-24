using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.BusinessServices.Traffic
{
    public static class JT808TrafficServiceExtensions
    {
        /// <summary>
        /// 独享消息流量统计服务（不同的消费者实例）
        /// </summary>
        /// <param name="jT808ClientBuilder"></param>
        /// <returns></returns>
        public static IJT808ClientBuilder AddInprocJT808Traffic(this IJT808ClientBuilder jT808ClientBuilder)
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton<JT808TrafficService>();
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808TrafficServiceHostedService>();
            return jT808ClientBuilder;
        }
        /// <summary>
        /// 共享消息流量统计服务（消费者单实例）
        /// </summary>
        /// <typeparam name="TReplyMessageService"></typeparam>
        /// <param name="jT808ClientBuilder"></param>
        /// <returns></returns>
        public static IJT808ClientBuilder AddShareJT808Traffic(this IJT808ClientBuilder jT808ClientBuilder)
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton<JT808TrafficService>();
            return jT808ClientBuilder;
        }
    }
}
