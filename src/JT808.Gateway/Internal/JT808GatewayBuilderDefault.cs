using JT808.Gateway.Abstractions;
using JT808.Protocol;
using JT808.Protocol.DependencyInjection;

namespace JT808.Gateway.Internal
{
    public class JT808GatewayBuilderDefault : IJT808GatewayBuilder
    {
        public IJT808DIBuilder JT808Builder { get; }

        public JT808GatewayBuilderDefault(IJT808DIBuilder builder)
        {
            JT808Builder = builder;
        }

        public IJT808DIBuilder Builder()
        {
            return JT808Builder;
        }
    }
}