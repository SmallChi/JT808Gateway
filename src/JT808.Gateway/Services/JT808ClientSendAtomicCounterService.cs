using JT808.Gateway.Metadata;

namespace JT808.Gateway.Services
{
    /// <summary>
    /// 发送计数包服务
    /// </summary>
    public class JT808ClientSendAtomicCounterService
    {
        private readonly JT808AtomicCounter MsgSuccessCounter;

        public JT808ClientSendAtomicCounterService()
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
