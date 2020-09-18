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
        /// 转发服务（不同的消费者实例）
        /// </summary>
        /// <param name="jT808ClientBuilder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IJT808ClientBuilder AddTransmit(this IJT808ClientBuilder jT808ClientBuilder,IConfiguration configuration)
        {
            jT808ClientBuilder.JT808Builder.Services.Configure<RemoteServerOptions>(configuration.GetSection("RemoteServerOptions"));
            jT808ClientBuilder.JT808Builder.Services.AddSingleton<JT808TransmitService>();
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808TransmitHostedService>();
            return jT808ClientBuilder;
        }

        /// <summary>
        /// 转发服务（不同的消费者实例）
        /// </summary>
        /// <param name="jT808GatewayBuilder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IJT808GatewayBuilder AddTransmit(this IJT808GatewayBuilder jT808GatewayBuilder, IConfiguration configuration)
        {
            jT808GatewayBuilder.JT808Builder.Services.Configure<RemoteServerOptions>(configuration.GetSection("RemoteServerOptions"));
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton<JT808TransmitService>();
            return jT808GatewayBuilder;
        }
    }
}
