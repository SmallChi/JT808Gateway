using JT808.DotNetty.Client.Metadata;

namespace JT808.DotNetty.Client.Services
{
    /// <summary>
    /// 发送计数包服务
    /// </summary>
    public class JT808SendAtomicCounterService
    {
        private readonly JT808AtomicCounter MsgSuccessCounter;

        public JT808SendAtomicCounterService()
        {
            MsgSuccessCounter=new JT808AtomicCounter();
        }

        public void Reset()
        {
            MsgSuccessCounter.Reset();
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
    }
}
