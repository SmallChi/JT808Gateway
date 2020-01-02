using JT808.Gateway.Abstractions;
using JT808.Gateway.Configurations;
using JT808.Gateway.Internal;
using JT808.Gateway.Services;
using JT808.Gateway.Session;
using JT808.Protocol;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("JT808.Gateway.TestHosting")]
[assembly: InternalsVisibleTo("JT808.Gateway.Test")]
namespace JT808.Gateway
{
    public static partial class JT808GatewayExtensions
    {
        public static IJT808GatewayBuilder AddJT808DevelopmentGateway(this IJT808Builder jt808Builder)
        {
            IJT808GatewayBuilder server = new JT808GatewayBuilderDefault(jt808Builder);
            server.JT808Builder.Services.TryAddSingleton<JT808Configuration>();
            server.AddJT808Core();
            return server;
        }

        public static IJT808GatewayBuilder AddJT808Gateway(this IJT808Builder jt808Builder,Action<JT808Configuration> config)
        {
            IJT808GatewayBuilder server = new JT808GatewayBuilderDefault(jt808Builder);
            server.JT808Builder.Services.Configure(config);
            server.AddJT808Core();
            return server;
        }

        public static IJT808GatewayBuilder AddJT808Gateway(this IJT808Builder jt808Builder, IConfiguration  configuration)
        {
            IJT808GatewayBuilder server = new JT808GatewayBuilderDefault(jt808Builder);
            server.JT808Builder.Services.Configure<JT808Configuration>(configuration.GetSection("JT808Configuration"));
            server.AddJT808Core();
            return server;
        }

        public static IJT808GatewayBuilder AddTcp(this IJT808GatewayBuilder config)
        {
            config.JT808Builder.Services.AddHostedService<JT808TcpServer>();
            config.JT808Builder.Services.AddHostedService<JT808TcpReceiveTimeoutHostedService>();
            return config;
        }

        public static IJT808GatewayBuilder AddUdp(this IJT808GatewayBuilder config)
        {
            config.JT808Builder.Services.AddHostedService<JT808UdpServer>();
            config.JT808Builder.Services.AddHostedService<JT808UdpReceiveTimeoutHostedService>();
            return config;
        }

        public static IJT808GatewayBuilder AddGrpc(this IJT808GatewayBuilder config)
        {
            config.JT808Builder.Services.AddHostedService<JT808GrpcServer>();
            return config;
        }

        private static IJT808GatewayBuilder AddJT808Core(this IJT808GatewayBuilder config)
        {
            config.JT808Builder.Services.TryAddSingleton<JT808AtomicCounterServiceFactory>();
            config.JT808Builder.Services.TryAddSingleton<IJT808MsgProducer, JT808MsgProducerDefault>();
            config.JT808Builder.Services.TryAddSingleton<IJT808MsgReplyConsumer, JT808MsgReplyConsumerDefault>();
            config.JT808Builder.Services.TryAddSingleton<JT808SessionManager>();
            config.JT808Builder.Services.TryAddSingleton<JT808MsgService>();
            config.JT808Builder.Services.AddHostedService<JT808MsgReplyHostedService>();
            return config;
        }
    }
}