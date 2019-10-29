using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Core.Handlers;
using JT808.DotNetty.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.WebApi
{
    public class JT808WebApiBuilderDefault : IJT808WebApiNettyBuilder
    {
        public JT808WebApiBuilderDefault(IJT808NettyBuilder jT808NettyBuilder) {
            Instance = jT808NettyBuilder;
        }

        public IJT808NettyBuilder Instance { get; }

        public IJT808NettyBuilder Builder()
        {
            return Instance;
        }

        public IJT808WebApiNettyBuilder ReplaceAuthorization<T>() where T : IJT808WebApiAuthorization
        {
            Instance.JT808Builder.Services.Replace(new ServiceDescriptor(typeof(IJT808WebApiAuthorization), typeof(T), ServiceLifetime.Singleton));
            return this;
        }

        public IJT808WebApiNettyBuilder ReplaceMsgIdHandler<T>() where T : JT808MsgIdHttpHandlerBase
        {
            Instance.JT808Builder.Services.Replace(new ServiceDescriptor(typeof(JT808MsgIdHttpHandlerBase), typeof(T), ServiceLifetime.Singleton));
            return this;
        }
    }
}
