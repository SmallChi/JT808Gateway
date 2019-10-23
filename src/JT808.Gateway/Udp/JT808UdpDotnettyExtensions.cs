using JT808.Gateway.Codecs;
using JT808.Gateway.Handlers;
using JT808.Gateway.Impls;
using JT808.Gateway.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Runtime.CompilerServices;

namespace JT808.Gateway.Udp
{
    public static class JT808UdpDotnettyExtensions
    {
        public static IJT808GatewayBuilder AddJT808GatewayUdpHost(this IJT808GatewayBuilder jT808NettyBuilder)
        {
            jT808NettyBuilder.JT808Builder.Services.TryAddSingleton<IJT808DatagramPacket, JT808DatagramPacketImpl>();
            jT808NettyBuilder.JT808Builder.Services.TryAddScoped<JT808UdpDecoder>();
            jT808NettyBuilder.JT808Builder.Services.TryAddScoped<JT808UdpServerHandler>();
            jT808NettyBuilder.JT808Builder.Services.AddHostedService<JT808UdpServerHost>();
            return jT808NettyBuilder;
        }
    }
}