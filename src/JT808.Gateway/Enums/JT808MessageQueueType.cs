using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.Enums
{
    public enum JT808MessageQueueType:byte
    {
        /// <summary>
        /// 使用内存队列
        /// </summary>
        InMemory=1,
        /// <summary>
        /// 使用第三方队列
        /// </summary>
        InPlug=2
    }
}
