using JT808.Gateway.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.MsgIdHandler
{
    public static class JT808MsgIdHandlerExtensions
    {
        public static IJT808ClientBuilder AddMsgIdHandler<TJT808MsgIdHandler>(this IJT808ClientBuilder jT808ClientBuilder)
            where TJT808MsgIdHandler: IJT808MsgIdHandler
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton(typeof(IJT808MsgIdHandler),typeof(TJT808MsgIdHandler));
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808MsgIdHandlerHostedService>();
            return jT808ClientBuilder;
        }

        public static IJT808GatewayBuilder AddInMemoryMsgIdHandler<TJT808MsgIdHandler>(this IJT808GatewayBuilder jT808GatewayBuilder)
            where TJT808MsgIdHandler : IJT808MsgIdHandler
        {
            jT808GatewayBuilder.JT808Builder.Services.AddSingleton(typeof(IJT808MsgIdHandler), typeof(TJT808MsgIdHandler));
            jT808GatewayBuilder.JT808Builder.Services.AddHostedService<JT808MsgIdHandlerInMemoryHostedService>();
            return jT808GatewayBuilder;
        }
    }
}
