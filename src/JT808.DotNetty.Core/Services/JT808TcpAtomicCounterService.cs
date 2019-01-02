using JT808.DotNetty.Core.Metadata;

namespace JT808.DotNetty.Core.Services
{
    /// <summary>
    /// Tcp计数包服务
    /// </summary>
    public class JT808TcpAtomicCounterService
    {
        private readonly JT808AtomicCounter MsgSuccessCounter = new JT808AtomicCounter();

        private readonly JT808AtomicCounter MsgFailCounter = new JT808AtomicCounter();

        public JT808TcpAtomicCounterService()
        {

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
