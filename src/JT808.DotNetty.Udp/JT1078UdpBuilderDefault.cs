using JT808.DotNetty.Core.Handlers;
using JT808.DotNetty.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Udp
{
    class JT1078UdpBuilderDefault : IJT808UdpNettyBuilder
    {
        public IJT808NettyBuilder Instance { get; }

        public JT1078UdpBuilderDefault(IJT808NettyBuilder builder)
        {
            Instance = builder;
        }

        public IJT808NettyBuilder Builder()
        {
            return Instance;
        }

        public IJT808UdpNettyBuilder ReplaceCustomMsgIdHandler<T>() where T : IJT808UdpCustomMsgIdHandler
        {
            Instance.JT808Builder.Services.Replace(new ServiceDescriptor(typeof(IJT808UdpCustomMsgIdHandler), typeof(T), ServiceLifetime.Singleton));
            return this;
        }

        public IJT808UdpNettyBuilder ReplaceMsgIdHandler<T>() where T : JT808MsgIdUdpHandlerBase
        {
            Instance.JT808Builder.Services.Replace(new ServiceDescriptor(typeof(JT808MsgIdUdpHandlerBase), typeof(T), ServiceLifetime.Singleton));
            return this;
        }

        public IJT808UdpNettyBuilder ReplaceSessionService<T>() where T : IJT808UdpSessionService
        {
            Instance.JT808Builder.Services.Replace(new ServiceDescriptor(typeof(IJT808UdpSessionService), typeof(T), ServiceLifetime.Singleton));
            return this;
        }

        public IJT808UdpNettyBuilder ReplaceUnificationSendService<T>() where T : IJT808UnificationUdpSendService
        {
            Instance.JT808Builder.Services.Replace(new ServiceDescriptor(typeof(IJT808UnificationUdpSendService), typeof(T), ServiceLifetime.Singleton));
            return this;
        }
    }
}