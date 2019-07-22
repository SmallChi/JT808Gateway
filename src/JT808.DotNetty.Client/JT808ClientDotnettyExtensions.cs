using JT808.DotNetty.Client.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Client
{
    public static  class JT808ClientDotnettyExtensions
    {
        public static IServiceCollection AddJT808Client(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddSingleton<JT808SendAtomicCounterService>();
            serviceDescriptors.AddSingleton<JT808ReceiveAtomicCounterService>();
            serviceDescriptors.AddSingleton<IJT808TcpClientFactory, JT808TcpClientFactory>();
            serviceDescriptors.AddSingleton<JT808ReportService>();
            serviceDescriptors.AddHostedService<JT808ReportHostedService>();
            return serviceDescriptors;
        }
    }
}
