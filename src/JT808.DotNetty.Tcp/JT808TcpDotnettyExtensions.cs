using JT808.DotNetty.Core.Codecs;
using JT808.DotNetty.Tcp.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Runtime.CompilerServices;
using JT808.DotNetty.Core.Interfaces;

[assembly: InternalsVisibleTo("JT808.DotNetty.Tcp.Test")]

namespace JT808.DotNetty.Tcp
{
    public static class JT808TcpDotnettyExtensions
    {
        public static IJT808NettyBuilder AddJT808TcpNettyHost(this IJT808NettyBuilder jT808NettyBuilder)
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