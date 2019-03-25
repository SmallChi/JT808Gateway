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
            serviceDescriptors.AddSingleton<IJT808TcpClientFactory, JT808TcpClientFactory>();
            return serviceDescriptors;
        }
    }
}
