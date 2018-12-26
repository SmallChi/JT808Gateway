using JT808.DotNetty.Codecs;
using JT808.DotNetty.Core;
using JT808.DotNetty.Core.Handlers;
using JT808.DotNetty.Core.Services;
using JT808.DotNetty.Udp;
using JT808.DotNetty.Udp.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("JT808.DotNetty.Test")]

namespace JT808.DotNetty.Udp
{
    public static class JT808UdpDotnettyExtensions
    {
        public static IServiceCollection AddJT808UdpHost(this IServiceCollection  serviceDescriptors)
        {
            serviceDescriptors.TryAddSingleton<JT808UdpSessionManager>();
            serviceDescriptors.TryAddSingleton<JT808UdpAtomicCounterService>();
            serviceDescriptors.TryAddSingleton<JT808MsgIdUdpHandlerBase, JT808MsgIdDefaultUdpHandler>();
            serviceDescriptors.TryAddScoped<JT808UdpDecoder>();
            serviceDescriptors.TryAddScoped<JT808UdpServerHandler>();
            serviceDescriptors.AddHostedService<JT808UdpServerHost>();
            return serviceDescriptors;
        }
    }
}