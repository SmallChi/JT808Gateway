using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Core.Codecs;
using JT808.DotNetty.Core.Impls;
using JT808.DotNetty.Core.Interfaces;
using JT808.DotNetty.Udp.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("JT808.DotNetty.Udp.Test")]

namespace JT808.DotNetty.Udp
{
    public static class JT808UdpDotnettyExtensions
    {
        public static IJT808NettyBuilder AddJT808UdpNettyHost(this IJT808NettyBuilder jT808NettyBuilder)
        {
            jT808NettyBuilder.JT808Builder.Services.TryAddSingleton<IJT808DatagramPacket, JT808DatagramPacketImpl>();
            jT808NettyBuilder.JT808Builder.Services.TryAddScoped<JT808UdpDecoder>();
            jT808NettyBuilder.JT808Builder.Services.TryAddScoped<JT808UdpServerHandler>();
            jT808NettyBuilder.JT808Builder.Services.AddHostedService<JT808UdpServerHost>();
            return jT808NettyBuilder;
        }

        internal static IServiceCollection AddJT808UdpNettyHostTest(this IServiceCollection  serviceDescriptors)
        {
            serviceDescriptors.TryAddSingleton<IJT808DatagramPacket, JT808DatagramPacketImpl>();
            serviceDescriptors.TryAddScoped<JT808UdpDecoder>();
            serviceDescriptors.TryAddScoped<JT808UdpServerHandler>();
            return serviceDescriptors;
        }
    }
}