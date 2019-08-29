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
    }
}
