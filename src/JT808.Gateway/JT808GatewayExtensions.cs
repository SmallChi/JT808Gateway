using JT808.Gateway;
using JT808.Gateway.Configurations;
using JT808.Gateway.Converters;
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
using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;

//[assembly: InternalsVisibleTo("JT808.DotNetty.Core.Test")]

namespace JT808.Gateway
{
    public static class JT808GatewayExtensions
    {
        static JT808GatewayExtensions()
        {
            JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
            {
                Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
                //日期类型默认格式化处理
                settings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
                settings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                //空值处理
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                settings.Converters.Add(new JsonIPAddressConverter());
                settings.Converters.Add(new JsonIPEndPointConverter());
                settings.Converters.Add(new ByteArrayHexConverter());
                return settings;
            });
        }

        public static IJT808GatewayBuilder AddJT808Gateway(this IJT808Builder jt808Builder, IConfiguration configuration, Newtonsoft.Json.JsonSerializerSettings settings=null)
        {
            if (settings != null)
            {
                JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
                {
                    settings.Converters.Add(new JsonIPAddressConverter());
                    settings.Converters.Add(new JsonIPEndPointConverter());
                    settings.Converters.Add(new ByteArrayHexConverter());
                    settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    return settings;
                });
            }
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

        public static IJT808GatewayBuilder AddJT808Gateway(this IJT808Builder jt808Builder, Action<JT808Configuration> jt808Options, Newtonsoft.Json.JsonSerializerSettings settings = null)
        {
            if (settings != null)
            {
                JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
                {
                    settings.Converters.Add(new JsonIPAddressConverter());
                    settings.Converters.Add(new JsonIPEndPointConverter());
                    settings.Converters.Add(new ByteArrayHexConverter());
                    settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    return settings;
                });
            }
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