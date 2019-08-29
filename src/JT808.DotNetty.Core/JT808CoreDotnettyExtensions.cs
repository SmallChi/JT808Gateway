using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Core.Configurations;
using JT808.DotNetty.Core.Converters;
using JT808.DotNetty.Core.Impls;
using JT808.DotNetty.Core.Interfaces;
using JT808.DotNetty.Core.Services;
using JT808.DotNetty.Internal;
using JT808.Protocol;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("JT808.DotNetty.Core.Test")]
[assembly: InternalsVisibleTo("JT808.DotNetty.Tcp.Test")]
[assembly: InternalsVisibleTo("JT808.DotNetty.Udp.Test")]
[assembly: InternalsVisibleTo("JT808.DotNetty.WebApi.Test")]
[assembly: InternalsVisibleTo("JT808.DotNetty.Tcp")]
[assembly: InternalsVisibleTo("JT808.DotNetty.Udp")]
[assembly: InternalsVisibleTo("JT808.DotNetty.WebApi")]
namespace JT808.DotNetty.Core
{
    public static class JT808CoreDotnettyExtensions
    {
        static JT808CoreDotnettyExtensions()
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

        public static IJT808NettyBuilder AddJT808NettyCore(this IJT808Builder jt808Builder, IConfiguration configuration, Newtonsoft.Json.JsonSerializerSettings settings=null)
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
            IJT808NettyBuilder nettyBuilder = new JT808NettyBuilderDefault(jt808Builder);
            nettyBuilder.JT808Builder.Services.Configure<JT808Configuration>(configuration.GetSection("JT808Configuration"));
            nettyBuilder.JT808Builder.Services.TryAddSingleton<JT808AtomicCounterServiceFactory>();
            return nettyBuilder;
        }

        public static IJT808NettyBuilder AddJT808NettyCore(this IJT808Builder jt808Builder, Action<JT808Configuration> jt808Options, Newtonsoft.Json.JsonSerializerSettings settings = null)
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
            IJT808NettyBuilder nettyBuilder = new JT808NettyBuilderDefault(jt808Builder);
            nettyBuilder.JT808Builder.Services.Configure(jt808Options);
            nettyBuilder.JT808Builder.Services.TryAddSingleton<JT808AtomicCounterServiceFactory>();
            return nettyBuilder;
        }
    }
}