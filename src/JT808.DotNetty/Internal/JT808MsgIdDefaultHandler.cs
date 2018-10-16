using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Internal
{
    /// <summary>
    /// 默认消息处理业务实现
    /// </summary>
    internal class JT808MsgIdDefaultHandler : JT808MsgIdHandlerBase
    {
        public JT808MsgIdDefaultHandler(JT808SessionManager sessionManager) : base(sessionManager)
        {
        }
    }
}
