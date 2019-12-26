using JT808.Gateway.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.MsgLogging
{
    public static class JT808MsgLoggingExtensions
    {
        public static IJT808ClientBuilder AddJT808MsgLogging<TJT808MsgLogging>(this IJT808ClientBuilder jT808ClientBuilder)
            where TJT808MsgLogging: IJT808MsgLogging
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton(typeof(IJT808MsgLogging),typeof(TJT808MsgLogging));
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808MsgDownLoggingHostedService>();
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808MsgUpLoggingHostedService>();
            return jT808ClientBuilder;
        }
    }
}
