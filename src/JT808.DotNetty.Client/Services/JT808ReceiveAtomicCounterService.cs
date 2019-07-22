using JT808.DotNetty.Client.Metadata;

namespace JT808.DotNetty.Client.Services
{
    /// <summary>
    /// 接收计数包服务
    /// </summary>
    public class JT808ReceiveAtomicCounterService
    {
        private readonly JT808AtomicCounter MsgSuccessCounter;

        public JT808ReceiveAtomicCounterService()
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
