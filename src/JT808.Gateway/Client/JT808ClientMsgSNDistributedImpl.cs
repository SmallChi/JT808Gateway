using JT808.Protocol;
using JT808.Protocol.Interfaces;
using System.Threading;

namespace JT808.Gateway.Client
{
    internal class JT808ClientMsgSNDistributedImpl : IJT808MsgSNDistributed
    {
        int _counter = 0;

        public ushort Increment()
        {
            return (ushort)Interlocked.Increment(ref _counter);
        }
    }
}
