using JT808.DotNetty.Client.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using JT808.Protocol;

namespace JT808.DotNetty.Client
{
    public static  class JT808ClientDotnettyExtensions
    {
        public static IJT808Builder AddJT808Client(this IJT808Builder  jT808Builder)
        {
            jT808Builder.Services.AddSingleton<JT808SendAtomicCounterService>();
            jT808Builder.Services.AddSingleton<JT808ReceiveAtomicCounterService>();
            jT808Builder.Services.AddSingleton<IJT808TcpClientFactory, JT808TcpClientFactory>();
            jT808Builder.Services.AddSingleton<JT808ReportService>();
            jT808Builder.Services.AddHostedService<JT808ReportHostedService>();
            return jT808Builder;
        }
    }
}
