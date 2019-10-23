
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using JT808.Protocol;
using JT808.Gateway.Services;

namespace JT808.Gateway.Client
{
    public static  class JT808ClientDotnettyExtensions
    {
        public static IJT808Builder AddJT808Client(this IJT808Builder  jT808Builder)
        {
            jT808Builder.Services.AddSingleton<JT808ClientSendAtomicCounterService>();
            jT808Builder.Services.AddSingleton<JT808ClientReceiveAtomicCounterService>();
            jT808Builder.Services.AddSingleton<IJT808TcpClientFactory, JT808TcpClientFactory>();
            jT808Builder.Services.AddSingleton<JT808ClientReportService>();
            jT808Builder.Services.AddHostedService<JT808ClientReportHostedService>();
            return jT808Builder;
        }
    }
}
