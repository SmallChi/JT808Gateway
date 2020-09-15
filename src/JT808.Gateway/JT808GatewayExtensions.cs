using JT808.Gateway.Abstractions;
using JT808.Gateway.Authorization;
using JT808.Gateway.Abstractions.Configurations;
using JT808.Gateway.Handlers;
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
        /// 添加http服务器
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IJT808GatewayBuilder AddHttp(this IJT808GatewayBuilder config)
        {
            config.JT808Builder.Services.AddSingleton<IJT808Authorization, JT808AuthorizationDefault>();
            config.JT808Builder.Services.AddSingleton<JT808MsgIdDefaultWebApiHandler>();
            config.JT808Builder.Services.AddHostedService<JT808HttpServer>();
            return config;
        }
        /// <summary>
        /// 添加http服务器
        /// </summary>
        /// <typeparam name="TJT808MsgIdDefaultWebApiHandler"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IJT808GatewayBuilder AddHttp<TJT808MsgIdDefaultWebApiHandler>(this IJT808GatewayBuilder config)
            where TJT808MsgIdDefaultWebApiHandler: JT808MsgIdDefaultWebApiHandler
        {
            config.JT808Builder.Services.AddSingleton<IJT808Authorization, JT808AuthorizationDefault>();
            config.JT808Builder.Services.AddSingleton(typeof(JT808MsgIdDefaultWebApiHandler),typeof(TJT808MsgIdDefaultWebApiHandler));
            config.JT808Builder.Services.AddHostedService<JT808HttpServer>();
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
        /// 添加Http服务认证机制
        /// </summary>
        /// <typeparam name="TJT808Authorization"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IJT808GatewayBuilder AddHttpAuthorization<TJT808Authorization>(this IJT808GatewayBuilder config)
             where TJT808Authorization : IJT808Authorization
        {
            config.JT808Builder.Services.Replace(new ServiceDescriptor(typeof(IJT808Authorization), typeof(TJT808Authorization), ServiceLifetime.Singleton));
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
            config.JT808Builder.Services.Add(new ServiceDescriptor(typeof(IJT808MsgReplyConsumer), typeof(TJT808MsgReplyConsumer), ServiceLifetime.Singleton));
            return config;
        }
        /// <summary>
        /// 必须注册的
        /// </summary>
        /// <param name="config"></param>
        public static void Register(this IJT808GatewayBuilder config)
        {
            if(config.JT808Builder.Services.Where(s => s.ServiceType == typeof(IJT808MsgReplyConsumer)).Count() > 0)
            {
                config.JT808Builder.Services.AddHostedService<JT808MsgReplyHostedService>();
            }
        }
        /// <summary>
        /// 添加公共模块
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static IJT808GatewayBuilder AddJT808Core(this IJT808GatewayBuilder config)
        {
            config.JT808Builder.Services.AddSingleton<JT808MessageHandler>();
            config.JT808Builder.Services.AddSingleton<JT808SessionManager>();
            config.JT808Builder.Services.AddSingleton<IJT808MsgProducer, JT808MsgProducer_Empty>();
            config.JT808Builder.Services.AddSingleton<IJT808MsgReplyLoggingProducer, JT808MsgReplyLoggingProducer_Empty>();
            return config;
        }
    }
}