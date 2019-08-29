using JT808.DotNetty.Core.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Core.Interfaces
{
    public interface IJT808UdpNettyBuilder
    {
        IJT808NettyBuilder Instance { get; }
        IJT808NettyBuilder Builder();
        IJT808UdpNettyBuilder ReplaceSessionService<T>() where T : IJT808UdpSessionService;
        IJT808UdpNettyBuilder ReplaceUnificationSendService<T>() where T : IJT808UnificationUdpSendService;
    }
}
