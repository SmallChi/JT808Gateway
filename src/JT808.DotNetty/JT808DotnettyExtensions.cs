using JT808.DotNetty.Codecs;
using JT808.DotNetty.Configurations;
using JT808.DotNetty.Handlers;
using JT808.DotNetty.Interfaces;
using JT808.DotNetty.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("JT808.DotNetty.Test")]
namespace JT808.DotNetty
{
    public static class JT808DotnettyExtensions
    {
        public static IHostBuilder UseJT808Host(this IHostBuilder builder)
        {
            return builder.ConfigureServices((hostContext, services) =>
            {
                services.Configure<JT808Configuration>(hostContext.Configuration.GetSection("JT808Configuration"));
                services.TryAddSingleton<JT808SessionManager>();
                services.TryAddSingleton<JT808AtomicCounterService>();
                services.TryAddSingleton<JT808MsgIdHandlerBase,JT808MsgIdDefaultHandler>();
                services.TryAddSingleton<IJT808SourcePackageDispatcher, JT808SourcePackageDispatcherDefaultImpl>();
                services.TryAddScoped<JT808ConnectionHandler>();
                services.TryAddScoped<JT808Decoder>();
                services.TryAddScoped<JT808ServerHandler>();
                services.TryAddSingleton<IJT808SessionService, JT808SessionServiceDefaultImpl>();
                services.TryAddSingleton<IJT808UnificationSendService, JT808UnificationSendServiceDefaultImpl>();
                services.TryAddSingleton<JT808WebAPIService>();
                services.TryAddScoped<JT808WebAPIServerHandler>();
                services.AddHostedService<JT808ServerHost>();
                services.AddHostedService<JT808WebAPIServerHost>();
            });
        }
    }
}