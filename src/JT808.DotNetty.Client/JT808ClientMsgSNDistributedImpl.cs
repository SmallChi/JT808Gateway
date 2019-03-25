using JT808.Protocol;
using System.Threading;

namespace JT808.DotNetty.Client
{
    internal class JT808ClientMsgSNDistributedImpl : IMsgSNDistributed
    {
        int _counter = 0;

        public ushort Increment()
        {
            return (ushort)Interlocked.Increment(ref _counter);
        }
    }
}
