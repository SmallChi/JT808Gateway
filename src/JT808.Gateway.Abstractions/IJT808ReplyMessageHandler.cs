using JT808.Protocol;
using JT808.Protocol.Enums;
using JT808.Protocol.Extensions;
using JT808.Protocol.MessageBody;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.Abstractions
{
    public  interface IJT808ReplyMessageHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TerminalNo">请求数据</param>
        /// <param name="Data">当前会话</param>
        /// <returns>应答消息数据</returns>
        public byte[] Processor(string TerminalNo, byte[] Data);
    }
}
