using JT808.DotNetty.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.MsgIdHandler
{
    public static class JT808DotNettyMsgIdHandlerExtensions
    {
        public static IJT808ClientBuilder AddJT808MsgIdHandler<TJT808DotNettyMsgIdHandler>(this IJT808ClientBuilder jT808ClientBuilder)
            where TJT808DotNettyMsgIdHandler: IJT808DotNettyMsgIdHandler
        {
            jT808ClientBuilder.JT808Builder.Services.AddSingleton(typeof(IJT808DotNettyMsgIdHandler),typeof(TJT808DotNettyMsgIdHandler));
            jT808ClientBuilder.JT808Builder.Services.AddHostedService<JT808DotNettyMsgIdHandlerHostedService>();
            return jT808ClientBuilder;
        }
    }
}
