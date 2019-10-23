using JT808.Gateway.Codecs;
using JT808.Gateway.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Runtime.CompilerServices;

namespace JT808.Gateway.Tcp
{
    public static class JT808TcpDotnettyExtensions
    {
        public static IJT808GatewayBuilder AddJT808GatewayTcpHost(this IJT808GatewayBuilder jT808NettyBuilder)
        {
            jT808NettyBuilder.JT808Builder.Services.TryAddScoped<JT808TcpConnectionHandler>();
            jT808NettyBuilder.JT808Builder.Services.TryAddScoped<JT808TcpEncoder>();
            jT808NettyBuilder.JT808Builder.Services.TryAddScoped<JT808TcpDecoder>();
            jT808NettyBuilder.JT808Builder.Services.TryAddScoped<JT808TcpServerHandler>();
            jT808NettyBuilder.JT808Builder.Services.AddHostedService<JT808TcpServerHost>();
            return jT808NettyBuilder;
        }
    }
}