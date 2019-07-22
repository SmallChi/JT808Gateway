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
        IJT808TcpNettyBuilder ReplaceCustomMsgIdHandler<T>() where T : IJT808TcpCustomMsgIdHandler;
        IJT808TcpNettyBuilder ReplaceMsgIdHandler<T>() where T : JT808MsgIdTcpHandlerBase;
        IJT808TcpNettyBuilder ReplaceSessionService<T>() where T : IJT808TcpSessionService;
        IJT808TcpNettyBuilder ReplaceUnificationSendService<T>() where T : IJT808UnificationTcpSendService;
    }
}
