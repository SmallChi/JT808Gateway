using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Core.Configurations;
using JT808.DotNetty.Core.Converters;
using JT808.DotNetty.Core.Impls;
using JT808.DotNetty.Core.Interfaces;
using JT808.DotNetty.Core.Services;
using JT808.DotNetty.Internal;
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
                return settings;
            });
        }

        public static IServiceCollection AddJT808Core(this IServiceCollection  serviceDescriptors, IConfiguration configuration, Newtonsoft.Json.JsonSerializerSettings settings=null)
        {
            if (settings != null)
            {
                JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
                {
                    settings.Converters.Add(new JsonIPAddressConverter());
                    settings.Converters.Add(new JsonIPEndPointConverter());
                    settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    return settings;
                });
            }
            serviceDescriptors.Configure<JT808Configuration>(configuration.GetSection("JT808Configuration"));
            serviceDescriptors.TryAddSingleton<IJT808SessionPublishing, JT808SessionPublishingEmptyImpl>();
            serviceDescriptors.TryAddSingleton<IJT808SourcePackageDispatcher, JT808SourcePackageDispatcherEmptyImpl>();
            serviceDescriptors.TryAddSingleton<JT808AtomicCounterServiceFactory>();
            serviceDescriptors.TryAddSingleton<JT808TrafficServiceFactory>();
            serviceDescriptors.TryAddSingleton<IJT808UnificationTcpSendService, JT808UnificationTcpSendService>();
            serviceDescriptors.TryAddSingleton<IJT808UnificationUdpSendService, JT808UnificationUdpSendService>();
            serviceDescriptors.TryAddSingleton<IJT808TcpSessionService, JT808TcpSessionService>();
            serviceDescriptors.TryAddSingleton<IJT808UdpSessionService, JT808UdpSessionService>();
            serviceDescriptors.TryAddSingleton<JT808SimpleSystemCollectService>();
            return serviceDescriptors;
        }

        public static IServiceCollection AddJT808Core(this IServiceCollection serviceDescriptors, Action<JT808Configuration> jt808Options, Newtonsoft.Json.JsonSerializerSettings settings = null)
        {
            if (settings != null)
            {
                JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
                {
                    settings.Converters.Add(new JsonIPAddressConverter());
                    settings.Converters.Add(new JsonIPEndPointConverter());
                    settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    return settings;
                });
            }
            serviceDescriptors.Configure(jt808Options);
            serviceDescriptors.TryAddSingleton<IJT808SessionPublishing, JT808SessionPublishingEmptyImpl>();
            serviceDescriptors.TryAddSingleton<IJT808SourcePackageDispatcher, JT808SourcePackageDispatcherEmptyImpl>();
            serviceDescriptors.TryAddSingleton<JT808AtomicCounterServiceFactory>();
            serviceDescriptors.TryAddSingleton<JT808TrafficServiceFactory>();
            serviceDescriptors.TryAddSingleton<IJT808UnificationTcpSendService, JT808UnificationTcpSendService>();
            serviceDescriptors.TryAddSingleton<IJT808UnificationUdpSendService, JT808UnificationUdpSendService>();
            serviceDescriptors.TryAddSingleton<IJT808TcpSessionService, JT808TcpSessionService>();
            serviceDescriptors.TryAddSingleton<IJT808UdpSessionService, JT808UdpSessionService>();
            serviceDescriptors.TryAddSingleton<JT808SimpleSystemCollectService>();
            return serviceDescriptors;
        }
    }
}