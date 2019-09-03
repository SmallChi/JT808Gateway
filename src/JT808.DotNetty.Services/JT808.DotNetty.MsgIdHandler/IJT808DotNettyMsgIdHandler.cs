using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.MsgIdHandler
{
    /// <summary>
    /// JT808消息Id处理程序
    /// </summary>
    public interface IJT808DotNettyMsgIdHandler
    {
        void Processor((string TerminalNo, byte[] Data) parameter);
    }
}
