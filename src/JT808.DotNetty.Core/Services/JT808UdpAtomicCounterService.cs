using JT808.DotNetty.Core.Metadata;

namespace JT808.DotNetty.Core.Services
{
    /// <summary>
    /// Udp计数包服务
    /// </summary>
    public class JT808UdpAtomicCounterService
    {
        private readonly JT808AtomicCounter MsgSuccessCounter = new JT808AtomicCounter();

        private readonly JT808AtomicCounter MsgFailCounter = new JT808AtomicCounter();

        public JT808UdpAtomicCounterService()
        {

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
