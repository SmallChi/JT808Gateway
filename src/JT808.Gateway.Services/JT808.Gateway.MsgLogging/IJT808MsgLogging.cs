using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JT808.Gateway.MsgLogging
{
    /// <summary>
    /// 808数据上下行日志接口
    /// </summary>
    public interface IJT808MsgLogging
    {
        void Processor((string TerminalNo, byte[] Data) parameter, JT808MsgLoggingType jT808MsgLoggingType);
    }
}
