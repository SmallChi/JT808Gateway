using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using JT808.Protocol;
using Microsoft.Extensions.Configuration;
using JT808.Gateway.Client.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using JT808.Gateway.Client.Internal;

namespace JT808.Gateway.Client
{
    public static  class JT808ClientExtensions
    {
        public static IJT808ClientBuilder AddClient(this IJT808Builder jT808Builder)
        {
            JT808ClientBuilderDefault jT808ClientBuilderDefault = new JT808ClientBuilderDefault(jT808Builder);
            jT808ClientBuilderDefault.JT808Builder.Services.AddSingleton<JT808RetryBlockingCollection>();
            jT808ClientBuilderDefault.JT808Builder.Services.AddSingleton<JT808SendAtomicCounterService>();
            jT808ClientBuilderDefault.JT808Builder.Services.AddSingleton<JT808ReceiveAtomicCounterService>();
            jT808ClientBuilderDefault.JT808Builder.Services.AddSingleton<IJT808TcpClientFactory, JT808TcpClientFactory>();
            jT808ClientBuilderDefault.JT808Builder.Services.AddSingleton<IJT808MessageProducer, JT808MessageProducerEmpty>();
            jT808ClientBuilderDefault.JT808Builder.Services.AddHostedService<JT808RetryClientHostedService>();
            return jT808ClientBuilderDefault;
        }

        public static IJT808ClientBuilder AddMessageProducer<TJT808MessageProducer>(this IJT808ClientBuilder jT808ClientBuilder)
            where TJT808MessageProducer: IJT808MessageProducer
        {
            jT808ClientBuilder.JT808Builder.Services.Replace(new ServiceDescriptor(typeof(IJT808MessageProducer), typeof(TJT808MessageProducer), ServiceLifetime.Singleton));
            return jT808ClientBuilder;
        }
        
        public static IJT808ClientBuilder AddClientReport(this IJT808ClientBuilder jT808ClientBuilder)
        {
            jT808ClientBuilder.JT808Builder.Services.Configure<JT808ReportOptions>((options) => { });
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808ReportHostedService>();
            return jT808ClientBuilder;
        }

        public static IJT808ClientBuilder AddClientReport(this IJT808ClientBuilder jT808ClientBuilder, IConfiguration Configuration)
        {
            jT808ClientBuilder.JT808Builder.Services.Configure<JT808ReportOptions>(Configuration.GetSection("JT808ReportOptions"));
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808ReportHostedService>();
            return jT808ClientBuilder;
        }

        public static IJT808ClientBuilder AddClientReport(this IJT808ClientBuilder jT808ClientBuilder, Action<JT808ReportOptions> reportOptions)
        {
            jT808ClientBuilder.JT808Builder.Services.Configure(reportOptions);
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808ReportHostedService>();
            return jT808ClientBuilder;
        }
    }
}
