using JT808.DotNetty.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Traffic
{
    public static class JT808DotNettyTrafficServiceExtensions
    {
        /// <summary>
        /// 独享消息流量统计服务（不同的消费者实例）
        /// </summary>
        /// <param name="jT808ClientBuilder"></param>
        /// <returns></returns>
        public static IJT808ClientBuilder AddInprocJT808Traffic(this IJT808ClientBuilder jT808ClientBuilder)
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton<JT808DotNettyTrafficService>();
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808DotNettyTrafficServiceHostedService>();
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
            jT808ClientBuilder.JT808Builder.Services.AddSingleton<JT808DotNettyTrafficService>();
            return jT808ClientBuilder;
        }
    }
}
