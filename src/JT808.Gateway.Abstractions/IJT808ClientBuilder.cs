using JT808.Protocol;
using JT808.Protocol.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.Abstractions
{
    public interface IJT808ClientBuilder
    {
        IJT808DIBuilder JT808Builder { get; }
        IJT808DIBuilder Builder();
    }
}
