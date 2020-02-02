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
        public static IJT808ClientBuilder AddInPlugTraffic<TIJT808Traffic>(this IJT808ClientBuilder jT808ClientBuilder)
            where TIJT808Traffic:IJT808Traffic
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton(typeof(IJT808Traffic), typeof(TIJT808Traffic));
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808TrafficServiceHostedService>();
            return jT808ClientBuilder;
        }
        /// <summary>
        /// 消息流量统计服务（消费者单实例）
        /// </summary>
        /// <typeparam name="TReplyMessageService"></typeparam>
        /// <param name="jT808GatewayBuilder"></param>
        /// <returns></returns>
        public static IJT808GatewayBuilder AddInMemoryTraffic(this IJT808GatewayBuilder jT808GatewayBuilder)
        {
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton(typeof(IJT808Traffic), typeof(JT808TrafficDefault));
            jT808GatewayBuilder.JT808Builder.Services.AddHostedService<JT808TrafficServiceInMemoryHostedService>();
            return jT808GatewayBuilder;
        }
        /// <summary>
        /// 消息流量统计服务（消费者单实例）
        /// </summary>
        /// <typeparam name="TReplyMessageService"></typeparam>
        /// <param name="jT808GatewayBuilder"></param>
        /// <returns></returns>
        public static IJT808GatewayBuilder AddInMemoryTraffic<TIJT808Traffic>(this IJT808GatewayBuilder jT808GatewayBuilder)
        {
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton(typeof(IJT808Traffic), typeof(TIJT808Traffic));
            jT808GatewayBuilder.JT808Builder.Services.AddHostedService<JT808TrafficServiceInMemoryHostedService>();
            return jT808GatewayBuilder;
        }
    }
}
