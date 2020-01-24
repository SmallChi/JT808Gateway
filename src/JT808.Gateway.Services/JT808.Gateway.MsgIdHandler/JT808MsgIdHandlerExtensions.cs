using JT808.Gateway.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.MsgIdHandler
{
    public static class JT808MsgIdHandlerExtensions
    {
        public static IJT808ClientBuilder AddJT808MsgIdHandler<TJT808MsgIdHandler>(this IJT808ClientBuilder jT808ClientBuilder)
            where TJT808MsgIdHandler: IJT808MsgIdHandler
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton(typeof(IJT808MsgIdHandler),typeof(TJT808MsgIdHandler));
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808MsgIdHandlerHostedService>();
            return jT808ClientBuilder;
        }
    }
}
