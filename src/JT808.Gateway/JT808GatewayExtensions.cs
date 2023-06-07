using JT808.Gateway.Abstractions;
using JT808.Gateway.Authorization;
using JT808.Gateway.Abstractions.Configurations;
using JT808.Gateway.Internal;
using JT808.Gateway.Services;
using JT808.Gateway.Session;
using JT808.Protocol;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Linq;

[assembly: InternalsVisibleTo("JT808.Gateway.TestHosting")]
[assembly: InternalsVisibleTo("JT808.Gateway.Test")]
namespace JT808.Gateway
{
    /// <summary>
    /// JT808网关注册扩展
    /// </summary>
    public static partial class JT808GatewayExtensions
    {
        /// <summary>
        /// 添加808网关
        /// </summary>
        /// <param name="jT808Builder"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IJT808GatewayBuilder AddGateway(this IJT808Builder jT808Builder, Action<JT808Configuration> config)
        {
            JT808GatewayBuilderDefault jT808GatewayBuilderDefault = new JT808GatewayBuilderDefault(jT808Builder);
            jT808GatewayBuilderDefault.JT808Builder.Services.Configure(config);
            jT808GatewayBuilderDefault.AddJT808Core();
            return jT808GatewayBuilderDefault;
        }
        /// <summary>
        /// 添加808网关
        /// </summary>
        /// <param name="jT808Builder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IJT808GatewayBuilder AddGateway(this IJT808Builder jT808Builder, IConfiguration configuration)
        {
            JT808GatewayBuilderDefault jT808GatewayBuilderDefault = new JT808GatewayBuilderDefault(jT808Builder);
            jT808GatewayBuilderDefault.JT808Builder.Services.Configure<JT808Configuration>(configuration.GetSection("JT808Configuration"));
            jT808GatewayBuilderDefault.AddJT808Core();
            return jT808GatewayBuilderDefault;
        }
        /// <summary>
        /// 添加tcp服务器
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IJT808GatewayBuilder AddTcp(this IJT808GatewayBuilder config)
        {
            config.JT808Builder.Services.AddHostedService<JT808TcpServer>();
            config.JT808Builder.Services.AddHostedService<JT808TcpReceiveTimeoutHostedService>();
            return config;
        }
        /// <summary>
        /// 添加udp服务器
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IJT808GatewayBuilder AddUdp(this IJT808GatewayBuilder config)
        {
            config.JT808Builder.Services.AddHostedService<JT808UdpServer>();
            config.JT808Builder.Services.AddHostedService<JT808UdpReceiveTimeoutHostedService>();
            return config;
        }
        /// <summary>
        /// 添加消息业务处理程序
        /// </summary>
        /// <typeparam name="TJT808MessageHandler"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IJT808GatewayBuilder AddMessageHandler<TJT808MessageHandler>(this IJT808GatewayBuilder config)
             where TJT808MessageHandler : JT808MessageHandler
        {
            config.JT808Builder.Services.Replace(new ServiceDescriptor(typeof(JT808MessageHandler), typeof(TJT808MessageHandler), ServiceLifetime.Singleton));
            return config;
        }  
        /// <summary>
        /// 添加消息生产者
        /// </summary>
        /// <typeparam name="TJT808MsgProducer"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IJT808GatewayBuilder AddMsgProducer<TJT808MsgProducer>(this IJT808GatewayBuilder config)
                where TJT808MsgProducer : IJT808MsgProducer
        {
            config.JT808Builder.Services.Replace(new ServiceDescriptor(typeof(IJT808MsgProducer), typeof(TJT808MsgProducer), ServiceLifetime.Singleton));
            return config;
        }
        /// <summary>
        /// 添加消息应答后的应答生产者
        /// </summary>
        /// <typeparam name="TJT808MsgReplyLoggingProducer"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IJT808GatewayBuilder AddMsgReplyLoggingProducer<TJT808MsgReplyLoggingProducer>(this IJT808GatewayBuilder config)
        where TJT808MsgReplyLoggingProducer : IJT808MsgReplyLoggingProducer
        {
            config.JT808Builder.Services.Replace(new ServiceDescriptor(typeof(IJT808MsgReplyLoggingProducer), typeof(TJT808MsgReplyLoggingProducer), ServiceLifetime.Singleton));
            return config;
        }
        /// <summary>
        /// 添加消息应答消费者
        /// </summary>
        /// <typeparam name="TJT808MsgReplyConsumer"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IJT808GatewayBuilder AddMsgReplyConsumer<TJT808MsgReplyConsumer>(this IJT808GatewayBuilder config)
             where TJT808MsgReplyConsumer : IJT808MsgReplyConsumer
        {
            config.JT808Builder.Services.Replace(new ServiceDescriptor(typeof(IJT808MsgReplyConsumer), typeof(TJT808MsgReplyConsumer), ServiceLifetime.Singleton));
            return config;
        }
        /// <summary>
        /// 添加公共模块
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static IJT808GatewayBuilder AddJT808Core(this IJT808GatewayBuilder config)
        {
            config.JT808Builder.Services.AddSingleton<JT808MessageHandler>();
            config.JT808Builder.Services.AddSingleton<JT808BlacklistManager>();
            config.JT808Builder.Services.AddSingleton<JT808SessionManager>();
            config.JT808Builder.Services.AddSingleton<IJT808MsgProducer, JT808MsgProducer_Empty>();
            config.JT808Builder.Services.AddSingleton<IJT808MsgReplyLoggingProducer, JT808MsgReplyLoggingProducer_Empty>();
            config.JT808Builder.Services.AddSingleton<IJT808MsgReplyConsumer, JT808MsgReplyConsumer_Empry>();
            config.JT808Builder.Services.AddHostedService<JT808MsgReplyHostedService>();
            return config;
        }
    }
}