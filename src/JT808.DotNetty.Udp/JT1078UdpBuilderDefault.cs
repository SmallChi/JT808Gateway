using JT808.DotNetty.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
    }
}