using JT808.DotNetty.Core.Handlers;
using JT808.DotNetty.WebApi.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("JT808.DotNetty.WebApi.Test")]

namespace JT808.DotNetty.WebApi
{
    public static class JT808WebApiDotnettyExtensions
    {
        public static IServiceCollection AddJT808WebApiHost(this IServiceCollection  serviceDescriptors)
        {
            serviceDescriptors.TryAddSingleton<JT808MsgIdHttpHandlerBase, JT808MsgIdDefaultWebApiHandler>();
            serviceDescriptors.TryAddScoped<JT808WebAPIServerHandler>();
            serviceDescriptors.AddHostedService<JT808WebAPIServerHost>();
            return serviceDescriptors;
        }
    }
}