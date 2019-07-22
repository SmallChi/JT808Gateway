using JT808.DotNetty.Core;
using JT808.DotNetty.Core.Codecs;
using JT808.DotNetty.Core.Handlers;
using JT808.DotNetty.Core.Impls;
using JT808.DotNetty.Core.Interfaces;
using JT808.DotNetty.Core.Jobs;
using JT808.DotNetty.Core.Services;
using JT808.DotNetty.Internal;
using JT808.DotNetty.Udp.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Internal;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("JT808.DotNetty.Udp.Test")]

namespace JT808.DotNetty.Udp
{
    public static class JT808UdpDotnettyExtensions
    {
        public static IJT808UdpNettyBuilder AddJT808UdpNettyHost(this IJT808NettyBuilder jT808NettyBuilder)
        {
            jT808NettyBuilder.JT808Builder.Services.TryAddSingleton<IJT808UdpCustomMsgIdHandler, JT808UdpCustomMsgIdHandlerEmpty>();
            jT808NettyBuilder.JT808Builder.Services.TryAddSingleton<IJT808UdpSessionService, JT808UdpSessionService>();
            jT808NettyBuilder.JT808Builder.Services.TryAddSingleton<IJT808DatagramPacket, JT808DatagramPacketImpl>();
            jT808NettyBuilder.JT808Builder.Services.TryAddSingleton<IJT808UnificationUdpSendService, JT808UnificationUdpSendService>();
            jT808NettyBuilder.JT808Builder.Services.TryAddSingleton<JT808UdpSessionManager>();
            jT808NettyBuilder.JT808Builder.Services.TryAddSingleton<JT808MsgIdUdpHandlerBase, JT808MsgIdDefaultUdpHandler>();
            jT808NettyBuilder.JT808Builder.Services.TryAddScoped<JT808UdpDecoder>();
            jT808NettyBuilder.JT808Builder.Services.TryAddScoped<JT808UdpServerHandler>();
            jT808NettyBuilder.JT808Builder.Services.AddHostedService<JT808UdpAtomicCouterResetDailyJob>();
            jT808NettyBuilder.JT808Builder.Services.AddHostedService<JT808UdpTrafficResetDailyJob>();
            jT808NettyBuilder.JT808Builder.Services.AddHostedService<JT808UdpServerHost>();
            return new JT1078UdpBuilderDefault(jT808NettyBuilder);
        }
    }
}