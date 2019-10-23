using JT808.Gateway;
using JT808.Protocol;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.Impls
{
    internal class JT808GatewayBuilderDefault : IJT808GatewayBuilder
    {
        public IJT808Builder JT808Builder { get; }

        public JT808GatewayBuilderDefault(IJT808Builder builder)
        {
            JT808Builder = builder;
        }

        public IJT808Builder Builder()
        {
            return JT808Builder;
        }
    }
}