using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Core.Configurations;
using JT808.DotNetty.Core.Impls;
using JT808.DotNetty.Core.Interfaces;
using JT808.DotNetty.Core.Services;
using JT808.DotNetty.Core.Session;
using JT808.DotNetty.Internal;
using JT808.Protocol;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("JT808.DotNetty.Core.Test")]
[assembly: InternalsVisibleTo("JT808.DotNetty.Tcp.Test")]
[assembly: InternalsVisibleTo("JT808.DotNetty.Udp.Test")]
[assembly: InternalsVisibleTo("JT808.DotNetty.Tcp")]
[assembly: InternalsVisibleTo("JT808.DotNetty.Udp")]
[assembly: InternalsVisibleTo("JT808.DotNetty.WebApi")]
namespace JT808.DotNetty.Core
{
    public static class JT808CoreDotnettyExtensions
    {
        public static IJT808NettyBuilder AddJT808NettyCore(this IJT808Builder jt808Builder, IConfiguration configuration)
        {
            IJT808NettyBuilder nettyBuilder = new JT808NettyBuilderDefault(jt808Builder);
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

        public static IJT808NettyBuilder AddJT808NettyCore(this IJT808Builder jt808Builder, Action<JT808Configuration> jt808Options)
        {
            IJT808NettyBuilder nettyBuilder = new JT808NettyBuilderDefault(jt808Builder);
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