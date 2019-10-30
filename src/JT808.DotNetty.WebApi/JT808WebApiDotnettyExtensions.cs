using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Core.Handlers;
using JT808.DotNetty.Core.Interfaces;
using JT808.DotNetty.WebApi.Authorization;
using JT808.DotNetty.WebApi.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("JT808.DotNetty.WebApi.Test")]

namespace JT808.DotNetty.WebApi
{
    public static class JT808WebApiDotnettyExtensions
    {
        public static IJT808WebApiNettyBuilder AddJT808WebApiNettyHost(this IJT808NettyBuilder jT808NettyBuilder)
        {
            jT808NettyBuilder.JT808Builder.Services.TryAddSingleton<JT808MsgIdHttpHandlerBase, JT808MsgIdDefaultWebApiHandler>();
            jT808NettyBuilder.JT808Builder.Services.TryAddSingleton<IJT808WebApiAuthorization, JT808AuthorizationDefault>();
            jT808NettyBuilder.JT808Builder.Services.TryAddScoped<JT808WebAPIServerHandler>();
            jT808NettyBuilder.JT808Builder.Services.AddHostedService<JT808WebAPIServerHost>();
            return new JT808WebApiBuilderDefault(jT808NettyBuilder);
        }

        internal static IServiceCollection  AddJT808WebApiNettyHostTest(this IServiceCollection  serviceDescriptors)
        {
            serviceDescriptors.TryAddSingleton<JT808MsgIdHttpHandlerBase, JT808MsgIdDefaultWebApiHandler>();
            serviceDescriptors.TryAddSingleton<IJT808WebApiAuthorization, JT808AuthorizationDefault>();
            serviceDescriptors.TryAddScoped<JT808WebAPIServerHandler>();
            return serviceDescriptors;
        }
    }
}