using JT808.DotNetty.Core.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Core.Interfaces
{
    public interface IJT808TcpNettyBuilder
    {
        IJT808NettyBuilder Instance { get; }
        IJT808NettyBuilder Builder();
    }
}
