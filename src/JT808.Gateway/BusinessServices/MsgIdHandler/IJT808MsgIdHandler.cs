using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.BusinessServices.MsgIdHandler
{
    /// <summary>
    /// JT808消息Id处理程序
    /// </summary>
    public interface IJT808MsgIdHandler
    {
        void Processor((string TerminalNo, byte[] Data) parameter);
    }
}
