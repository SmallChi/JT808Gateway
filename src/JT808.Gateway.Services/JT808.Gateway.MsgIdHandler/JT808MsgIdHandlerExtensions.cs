using JT808.Gateway.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.MsgIdHandler
{
    public static class JT808MsgIdHandlerExtensions
    {
        public static IJT808ClientBuilder AddMsgIdHandler<TJT808UpMessageHandler>(this IJT808ClientBuilder jT808ClientBuilder)
            where TJT808UpMessageHandler : IJT808UpMessageHandler
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton(typeof(IJT808UpMessageHandler),typeof(TJT808UpMessageHandler));
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808MsgIdHandlerHostedService>();
            return jT808ClientBuilder;
        }
    }
}
