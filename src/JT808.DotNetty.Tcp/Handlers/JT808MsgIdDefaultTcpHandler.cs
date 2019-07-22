using JT808.DotNetty.Core;
using JT808.DotNetty.Core.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Tcp.Handlers
{
    /// <summary>
    /// 默认消息处理业务实现
    /// </summary>
    internal class JT808MsgIdDefaultTcpHandler : JT808MsgIdTcpHandlerBase
    {
        public JT808MsgIdDefaultTcpHandler(JT808TcpSessionManager sessionManager) : base(sessionManager)
        {
        }
    }
}
