using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Dtos
{
    /// <summary>
    /// 包计数器服务
    /// </summary>
    public class JT808AtomicCounterDto
    {
        /// <summary>
        /// 成功数
        /// </summary>
        public long MsgSuccessCount { get; set; }
        /// <summary>
        /// 失败数
        /// </summary>
        public long MsgFailCount { get; set; }
    }
}
