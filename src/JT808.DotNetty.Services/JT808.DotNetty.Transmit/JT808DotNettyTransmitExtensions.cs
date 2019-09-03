using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using JT808.Protocol;
using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Transmit;
using Microsoft.Extensions.Configuration;
using JT808.DotNetty.Transmit.Configs;

namespace JT808.DotNetty.Transmit
{
    public static  class JT808DotNettyTransmitExtensions
    {
        /// <summary>
        /// 独享转发服务（不同的消费者实例）
        /// </summary>
        /// <param name="jT808ClientBuilder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IJT808ClientBuilder AddInprocJT808Transmit(this IJT808ClientBuilder jT808ClientBuilder,IConfiguration configuration)
        {
            jT808ClientBuilder.JT808Builder.Services.Configure<RemoteServerOptions>(configuration.GetSection("RemoteServerOptions"));
            jT808ClientBuilder.JT808Builder.Services.AddSingleton<JT808DotNettyTransmitService>();
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808DotNettyTransmitHostedService>();
            return jT808ClientBuilder;
        }
        /// <summary>
        /// 共享转发服务（消费者单实例）
        /// </summary>
        /// <param name="jT808ClientBuilder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IJT808ClientBuilder AddShareJT808Transmit(this IJT808ClientBuilder jT808ClientBuilder, IConfiguration configuration)
        {
            jT808ClientBuilder.JT808Builder.Services.Configure<RemoteServerOptions>(configuration.GetSection("RemoteServerOptions"));
            jT808ClientBuilder.JT808Builder.Services.AddSingleton<JT808DotNettyTransmitService>();
            return jT808ClientBuilder;
        }
    }
}
