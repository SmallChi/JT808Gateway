using JT808.Gateway;
using JT808.Gateway.Configurations;
using JT808.Gateway.Impls;
using JT808.Gateway.Interfaces;
using JT808.Gateway.PubSub;
using JT808.Gateway.Services;
using JT808.Gateway.Session;
using JT808.Protocol;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("JT808.Gateway.Test")]

namespace JT808.Gateway
{
    public static class JT808GatewayExtensions
    {
        public static IJT808GatewayBuilder AddJT808Gateway(this IJT808Builder jt808Builder, IConfiguration configuration)
        {
            IJT808GatewayBuilder nettyBuilder = new JT808GatewayBuilderDefault(jt808Builder);
            nettyBuilder.JT808Builder.Services.Configure<JT808Configuration>(configuration.GetSection("JT808Configuration"));
            nettyBuilder.JT808Builder.Services.TryAddSingleton<JT808AtomicCounterServiceFactory>();
            nettyBuilder.JT808Builder.Services.TryAddSingleton<JT808SessionManager>();
            nettyBuilder.JT808Builder.Services.TryAddSingleton<IJT808UnificationSendService, JT808UnificationSendService>();
            nettyBuilder.JT808Builder.Services.TryAddSingleton<IJT808SessionService, JT808SessionService>();
            nettyBuilder.JT808Builder.Services.TryAddSingleton<IJT808MsgProducer, JT808MsgProducerDefaultImpl>();
            nettyBuilder.JT808Builder.Services.TryAddSingleton<IJT808MsgReplyConsumer, JT808MsgReplyConsumerDefaultImpl>();
            nettyBuilder.JT808Builder.Services.TryAddSingleton<IJT808SessionProducer, JT808SessionProducerDefaultImpl>();
            nettyBuilder.JT808Builder.Services.TryAddSingleton<JT808MsgService>();
            nettyBuilder.JT808Builder.Services.AddHostedService<JT808MsgReplyHostedService>();
            return nettyBuilder;
        }

        public static IJT808GatewayBuilder AddJT808Gateway(this IJT808Builder jt808Builder, Action<JT808Configuration> jt808Options)
        {
            IJT808GatewayBuilder nettyBuilder = new JT808GatewayBuilderDefault(jt808Builder);
            nettyBuilder.JT808Builder.Services.Configure(jt808Options);
            nettyBuilder.JT808Builder.Services.TryAddSingleton<JT808AtomicCounterServiceFactory>();
            nettyBuilder.JT808Builder.Services.TryAddSingleton<JT808SessionManager>();
            nettyBuilder.JT808Builder.Services.TryAddSingleton<IJT808UnificationSendService, JT808UnificationSendService>();
            nettyBuilder.JT808Builder.Services.TryAddSingleton<IJT808SessionService, JT808SessionService>();
            nettyBuilder.JT808Builder.Services.TryAddSingleton<IJT808MsgProducer, JT808MsgProducerDefaultImpl>();
            nettyBuilder.JT808Builder.Services.TryAddSingleton<IJT808MsgReplyConsumer, JT808MsgReplyConsumerDefaultImpl>();
            nettyBuilder.JT808Builder.Services.TryAddSingleton<JT808MsgService>();
            nettyBuilder.JT808Builder.Services.TryAddSingleton<IJT808SessionProducer, JT808SessionProducerDefaultImpl>();
            nettyBuilder.JT808Builder.Services.AddHostedService<JT808MsgReplyHostedService>();
            return nettyBuilder;
        }
    }
}