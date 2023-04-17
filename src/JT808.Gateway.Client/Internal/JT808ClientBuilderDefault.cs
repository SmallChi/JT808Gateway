using JT808.Protocol;
using JT808.Protocol.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.Client
{
    internal class JT808ClientBuilderDefault : IJT808ClientBuilder
    {
        public IJT808DIBuilder JT808Builder { get; }

        public JT808ClientBuilderDefault(IJT808DIBuilder builder)
        {
            JT808Builder = builder;
        }

        public IJT808DIBuilder Builder()
        {
            return JT808Builder;
        }
    }
}