using JT808.DotNetty.Core.Codecs;
using JT808.DotNetty.Core;
using JT808.DotNetty.Core.Handlers;
using JT808.DotNetty.Core.Services;
using JT808.DotNetty.Tcp.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using JT808.DotNetty.Core.Jobs;

[assembly: InternalsVisibleTo("JT808.DotNetty.Tcp.Test")]

namespace JT808.DotNetty.Tcp
{
    public static class JT808TcpDotnettyExtensions
    {
        public static IServiceCollection AddJT808TcpHost(this IServiceCollection  serviceDescriptors)
        {
            serviceDescriptors.TryAddSingleton<JT808TcpSessionManager>();
            serviceDescriptors.TryAddSingleton<JT808TransmitAddressFilterService>();
            serviceDescriptors.TryAddSingleton<JT808MsgIdTcpHandlerBase, JT808MsgIdDefaultTcpHandler>();
            serviceDescriptors.TryAddScoped<JT808TcpConnectionHandler>();
            serviceDescriptors.TryAddScoped<JT808TcpDecoder>();
            serviceDescriptors.TryAddScoped<JT808TcpServerHandler>();
            serviceDescriptors.AddHostedService<JT808TcpAtomicCouterResetDailyJob>();
            serviceDescriptors.AddHostedService<JT808TcpTrafficResetDailyJob>();
            serviceDescriptors.AddHostedService<JT808TcpServerHost>();
            return serviceDescriptors;
        }
    }
}