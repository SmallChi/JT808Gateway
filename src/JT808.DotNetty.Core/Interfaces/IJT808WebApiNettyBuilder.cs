using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Core.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Core.Interfaces
{
    public interface IJT808WebApiNettyBuilder
    {
        IJT808NettyBuilder Instance { get; }
        IJT808NettyBuilder Builder();
        IJT808WebApiNettyBuilder ReplaceMsgIdHandler<T>() where T : JT808MsgIdHttpHandlerBase;
        IJT808WebApiNettyBuilder ReplaceAuthorization<T>() where T : IJT808WebApiAuthorization;
    }
}
