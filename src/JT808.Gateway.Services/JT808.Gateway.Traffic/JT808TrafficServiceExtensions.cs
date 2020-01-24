using JT808.Gateway.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.Traffic
{
    public static class JT808TrafficServiceExtensions
    {
        /// <summary>
        /// 消息流量统计服务（不同的消费者实例）
        /// </summary>
        /// <param name="jT808ClientBuilder"></param>
        /// <returns></returns>
        public static IJT808ClientBuilder AddJT808InPlugTraffic(this IJT808ClientBuilder jT808ClientBuilder)
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton<JT808TrafficService>();
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808TrafficServiceHostedService>();
            return jT808ClientBuilder;
        }
        /// <summary>
        /// 消息流量统计服务（消费者单实例）
        /// </summary>
        /// <typeparam name="TReplyMessageService"></typeparam>
        /// <param name="jT808GatewayBuilder"></param>
        /// <returns></returns>
        //public static IJT808GatewayBuilder AddJT808InMemoryTraffic(this IJT808GatewayBuilder jT808GatewayBuilder)
        //{
        //    jT808GatewayBuilder.JT808Builder.Services.AddSingleton<JT808TrafficService>();
        //    jT808GatewayBuilder.JT808Builder.Services.AddHostedService<JT808TrafficServiceHostedService>();
        //    return jT808GatewayBuilder;
        //}
    }
}
