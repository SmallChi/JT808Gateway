using JT808.DotNetty.Core.Metadata;

namespace JT808.DotNetty.Core.Services
{
    /// <summary>
    /// 计数包服务
    /// </summary>
    public class JT808AtomicCounterService
    {
        private readonly JT808AtomicCounter MsgSuccessCounter;

        private readonly JT808AtomicCounter MsgFailCounter;

        public JT808AtomicCounterService()
        {
            MsgSuccessCounter=new JT808AtomicCounter();
            MsgFailCounter = new JT808AtomicCounter();
        }

        public void Reset()
        {
            MsgSuccessCounter.Reset();
            MsgFailCounter.Reset();
        }

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
