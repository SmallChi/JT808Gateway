using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using JT808.Protocol;
using Microsoft.Extensions.Configuration;
using JT808.Gateway.Client.Services;

namespace JT808.Gateway.Client
{
    public static  class JT808ClientExtensions
    {
        public static IJT808Builder AddClient(this IJT808Builder jT808Builder)
        {
            jT808Builder.Services.AddSingleton<JT808SendAtomicCounterService>();
            jT808Builder.Services.AddSingleton<JT808ReceiveAtomicCounterService>();
            jT808Builder.Services.AddSingleton<IJT808TcpClientFactory, JT808TcpClientFactory>();
            jT808Builder.Services.Configure<JT808ReportOptions>((options)=> { });
            jT808Builder.Services.AddHostedService<JT808ReportHostedService>();
            return jT808Builder;
        }
        public static IJT808Builder AddClient(this IJT808Builder  jT808Builder, IConfiguration Configuration)
        {
            jT808Builder.Services.AddSingleton<JT808SendAtomicCounterService>();
            jT808Builder.Services.AddSingleton<JT808ReceiveAtomicCounterService>();
            jT808Builder.Services.AddSingleton<IJT808TcpClientFactory, JT808TcpClientFactory>();
            jT808Builder.Services.Configure<JT808ReportOptions>(Configuration.GetSection("JT808ReportOptions"));
            jT808Builder.Services.AddHostedService<JT808ReportHostedService>();
            return jT808Builder;
        }

        public static IJT808Builder AddClient(this IJT808Builder jT808Builder, Action<JT808ReportOptions> reportOptions)
        {
            jT808Builder.Services.AddSingleton<JT808SendAtomicCounterService>();
            jT808Builder.Services.AddSingleton<JT808ReceiveAtomicCounterService>();
            jT808Builder.Services.AddSingleton<IJT808TcpClientFactory, JT808TcpClientFactory>();
            jT808Builder.Services.Configure(reportOptions);
            jT808Builder.Services.AddHostedService<JT808ReportHostedService>();
            return jT808Builder;
        }
    }
}
