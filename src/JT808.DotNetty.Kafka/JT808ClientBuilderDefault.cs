using JT808.DotNetty.Abstractions;
using JT808.Protocol;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Kafka
{
    internal class JT808ClientBuilderDefault : IJT808ClientBuilder
    {
        public IJT808Builder JT808Builder { get; }

        public JT808ClientBuilderDefault(IJT808Builder builder)
        {
            JT808Builder = builder;
        }

        public IJT808Builder Builder()
        {
            return JT808Builder;
        }
    }
}