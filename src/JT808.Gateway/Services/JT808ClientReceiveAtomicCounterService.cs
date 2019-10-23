using JT808.Gateway.Metadata;

namespace JT808.Gateway.Services
{
    /// <summary>
    /// 接收计数包服务
    /// </summary>
    public class JT808ClientReceiveAtomicCounterService
    {
        private readonly JT808AtomicCounter MsgSuccessCounter;

        public JT808ClientReceiveAtomicCounterService()
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
