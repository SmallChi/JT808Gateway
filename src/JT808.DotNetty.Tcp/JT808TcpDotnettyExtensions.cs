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
using JT808.DotNetty.Core.Interfaces;
using JT808.DotNetty.Internal;

[assembly: InternalsVisibleTo("JT808.DotNetty.Tcp.Test")]

namespace JT808.DotNetty.Tcp
{
    public static class JT808TcpDotnettyExtensions
    {
        public static IJT808TcpNettyBuilder AddJT808TcpNettyHost(this IJT808NettyBuilder jT808NettyBuilder)
        {
            jT808NettyBuilder.JT808Builder.Services.TryAddSingleton<IJT808TcpCustomMsgIdHandler, JT808TcpCustomMsgIdHandlerEmpty>();
            jT808NettyBuilder.JT808Builder.Services.TryAddSingleton<IJT808TcpSessionService, JT808TcpSessionService>();
            jT808NettyBuilder.JT808Builder.Services.TryAddSingleton<IJT808UnificationTcpSendService, JT808UnificationTcpSendService>();
            jT808NettyBuilder.JT808Builder.Services.TryAddSingleton<JT808TcpSessionManager>();
            jT808NettyBuilder.JT808Builder.Services.TryAddSingleton<JT808TransmitAddressFilterService>();
            jT808NettyBuilder.JT808Builder.Services.TryAddSingleton<JT808MsgIdTcpHandlerBase, JT808MsgIdDefaultTcpHandler>();
            jT808NettyBuilder.JT808Builder.Services.TryAddScoped<JT808TcpConnectionHandler>();
            jT808NettyBuilder.JT808Builder.Services.TryAddScoped<JT808TcpEncoder>();
            jT808NettyBuilder.JT808Builder.Services.TryAddScoped<JT808TcpDecoder>();
            jT808NettyBuilder.JT808Builder.Services.TryAddScoped<JT808TcpServerHandler>();
            jT808NettyBuilder.JT808Builder.Services.AddHostedService<JT808TcpAtomicCouterResetDailyJob>();
            jT808NettyBuilder.JT808Builder.Services.AddHostedService<JT808TcpTrafficResetDailyJob>();
            jT808NettyBuilder.JT808Builder.Services.AddHostedService<JT808TcpServerHost>();
            return new JT808TcpBuilderDefault(jT808NettyBuilder);
        }
    }
}