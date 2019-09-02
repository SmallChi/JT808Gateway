using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using JT808.Protocol;
using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Transmit;
using Microsoft.Extensions.Configuration;
using JT808.DotNetty.Transmit.Configs;

namespace JT808.DotNetty.Client
{
    public static  class JT808DotNettyTransmitExtensions
    {
        public static IJT808ClientBuilder AddJT808Transmit(this IJT808ClientBuilder jT808ClientBuilder,IConfiguration configuration)
        {
            jT808ClientBuilder.JT808Builder.Services.Configure<RemoteServerOptions>(configuration.GetSection("RemoteServerOptions"));
            jT808ClientBuilder.JT808Builder.Services.AddSingleton<JT808DotNettyTransmitService>();
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808DotNettyTransmitHostedService>();
            return jT808ClientBuilder;
        }
    }
}
