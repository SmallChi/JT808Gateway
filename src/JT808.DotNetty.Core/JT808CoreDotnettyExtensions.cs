using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Core.Configurations;
using JT808.DotNetty.Core.Impls;
using JT808.DotNetty.Core.Interfaces;
using JT808.DotNetty.Core.Services;
using JT808.DotNetty.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("JT808.DotNetty.Core.Test")]
[assembly: InternalsVisibleTo("JT808.DotNetty.Tcp.Test")]
[assembly: InternalsVisibleTo("JT808.DotNetty.Udp.Test")]
[assembly: InternalsVisibleTo("JT808.DotNetty.WebApi.Test")]
namespace JT808.DotNetty.Core
{
    public static class JT808CoreDotnettyExtensions
    {
        public static IServiceCollection AddJT808Core(this IServiceCollection  serviceDescriptors, IConfiguration configuration)
        {
            serviceDescriptors.Configure<JT808Configuration>(configuration.GetSection("JT808Configuration"));
            serviceDescriptors.TryAddSingleton<IJT808SessionPublishing, JT808SessionPublishingEmptyImpl>();
            serviceDescriptors.TryAddSingleton<IJT808SourcePackageDispatcher, JT808SourcePackageDispatcherEmptyImpl>();
            serviceDescriptors.TryAddSingleton<IJT808UnificationTcpSendService, JT808UnificationTcpSendService>();
            serviceDescriptors.TryAddSingleton<IJT808UnificationUdpSendService, JT808UnificationUdpSendService>();
            serviceDescriptors.TryAddSingleton<IJT808TcpSessionService, JT808TcpSessionService>();
            serviceDescriptors.TryAddSingleton<IJT808UdpSessionService, JT808UdpSessionService>();
            return serviceDescriptors;
        }
    }
}