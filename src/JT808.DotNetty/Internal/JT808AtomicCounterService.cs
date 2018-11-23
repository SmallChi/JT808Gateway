using JT808.DotNetty.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.DotNetty.Internal
{
    /// <summary>
    /// 计数包服务
    /// </summary>
    internal class JT808AtomicCounterService
    {
        private static readonly JT808AtomicCounter MsgSuccessCounter = new JT808AtomicCounter();

        private static readonly JT808AtomicCounter MsgFailCounter = new JT808AtomicCounter();

        public long MsgSuccessIncrement()
        {
            return MsgSuccessCounter.Increment();
        }

        public long MsgSuccessCount
        {
            get
            {
                return MsgSuccessCounter.Count;
            }
        }

        public long MsgFailIncrement()
        {
            return MsgFailCounter.Increment();
        }

        public long MsgFailCount
        {
            get
            {
                return MsgFailCounter.Count;
            }
        }
    }
}
