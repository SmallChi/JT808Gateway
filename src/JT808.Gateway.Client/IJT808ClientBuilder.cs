using JT808.Protocol;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.Client
{
    public interface IJT808ClientBuilder
    {
        IJT808Builder JT808Builder { get; }
        IJT808Builder Builder();
    }
}
