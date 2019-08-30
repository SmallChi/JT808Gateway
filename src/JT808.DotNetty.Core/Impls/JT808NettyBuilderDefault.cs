using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Core.Interfaces;
using JT808.Protocol;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Core.Impls
{
    public class JT808NettyBuilderDefault : IJT808NettyBuilder
    {
        public IJT808Builder JT808Builder { get; }

        public JT808NettyBuilderDefault(IJT808Builder builder)
        {
            JT808Builder = builder;
        }

        public IJT808Builder Builder()
        {
            return JT808Builder;
        }

        public IJT808NettyBuilder ReplaceSessionPublishing<T>() where T : IJT808SessionPublishing
        {
            JT808Builder.Services.Replace(new ServiceDescriptor(typeof(IJT808SessionPublishing), typeof(T), ServiceLifetime.Singleton));
            return this;
        }

        public IJT808NettyBuilder ReplaceMsgProducer<T>() where T : IJT808MsgProducer
        {
            JT808Builder.Services.Replace(new ServiceDescriptor(typeof(IJT808MsgProducer), typeof(T), ServiceLifetime.Singleton));
            return this;
        }

        public IJT808NettyBuilder ReplaceMsgReplyConsumer<T>() where T : IJT808MsgReplyConsumer
        {
            JT808Builder.Services.Replace(new ServiceDescriptor(typeof(IJT808MsgReplyConsumer), typeof(T), ServiceLifetime.Singleton));
            return this;
        }
    }
}