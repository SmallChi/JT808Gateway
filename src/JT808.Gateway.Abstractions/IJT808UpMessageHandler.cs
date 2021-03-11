using JT808.Protocol;
using JT808.Protocol.Enums;
using JT808.Protocol.Extensions;
using JT808.Protocol.MessageBody;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.Abstractions
{
    /// <summary>
    /// 上行消息处理接口
    /// </summary>
    public  interface IJT808UpMessageHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TerminalNo">sim</param>
        /// <param name="Data">808 hex</param>
        public void Processor(string TerminalNo, byte[] Data);
    }
}
