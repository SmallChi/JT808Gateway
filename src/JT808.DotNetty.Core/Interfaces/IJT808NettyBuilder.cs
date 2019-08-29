using JT808.DotNetty.Abstractions;
using JT808.Protocol;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Core.Interfaces
{
    public interface IJT808NettyBuilder
    {
        IJT808Builder JT808Builder { get; }
        IJT808NettyBuilder ReplaceSessionPublishing<T>() where T : IJT808SessionPublishing;
        IJT808Builder Builder();
    }
}
