using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using JT808.Protocol;
using Microsoft.Extensions.Configuration;
using JT808.Gateway.Abstractions;
using JT808.Gateway.Transmit.Configs;

namespace JT808.Gateway.Transmit
{
    public static  class JT808TransmitExtensions
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
            jT808ClientBuilder.JT808Builder.Services.AddSingleton<JT808TransmitService>();
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808TransmitHostedService>();
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
            jT808ClientBuilder.JT808Builder.Services.AddSingleton<JT808TransmitService>();
            return jT808ClientBuilder;
        }
    }
}
