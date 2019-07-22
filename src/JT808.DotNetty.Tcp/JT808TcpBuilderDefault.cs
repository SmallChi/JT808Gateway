using JT808.DotNetty.Core.Handlers;
using JT808.DotNetty.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Tcp
{
    public class JT808TcpBuilderDefault : IJT808TcpNettyBuilder
    {
        public IJT808NettyBuilder Instance { get; }

        public JT808TcpBuilderDefault(IJT808NettyBuilder builder)
        {
            Instance = builder;
        }

        public IJT808NettyBuilder Builder()
        {
            return Instance;
        }

        public IJT808TcpNettyBuilder ReplaceCustomMsgIdHandler<T>() where T : IJT808TcpCustomMsgIdHandler
        {
            Instance.JT808Builder.Services.Replace(new ServiceDescriptor(typeof(IJT808TcpCustomMsgIdHandler), typeof(T), ServiceLifetime.Singleton));
            return this;
        }

        public IJT808TcpNettyBuilder ReplaceMsgIdHandler<T>() where T : JT808MsgIdTcpHandlerBase
        {
            Instance.JT808Builder.Services.Replace(new ServiceDescriptor(typeof(JT808MsgIdTcpHandlerBase), typeof(T), ServiceLifetime.Singleton));
            return this;
        }

        public IJT808TcpNettyBuilder ReplaceSessionService<T>() where T : IJT808TcpSessionService
        {
            Instance.JT808Builder.Services.Replace(new ServiceDescriptor(typeof(IJT808TcpSessionService), typeof(T), ServiceLifetime.Singleton));
            return this;
        }

        public IJT808TcpNettyBuilder ReplaceUnificationSendService<T>() where T : IJT808UnificationTcpSendService
        {
            Instance.JT808Builder.Services.Replace(new ServiceDescriptor(typeof(IJT808UnificationTcpSendService), typeof(T), ServiceLifetime.Singleton));
            return this;
        }
    }
}
