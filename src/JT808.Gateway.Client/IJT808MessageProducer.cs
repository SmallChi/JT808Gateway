using JT808.Protocol;
using System;
using System.Threading.Tasks;

namespace JT808.Gateway.Client
{
    /// <summary>
    /// 消息数据包
    /// </summary>
    public interface IJT808MessageProducer : IDisposable
    {
        ValueTask ProduceAsync(JT808Package package);
    }

    internal class JT808MessageProducerEmpty : IJT808MessageProducer
    {
        public void Dispose()
        {
            
        }

        public ValueTask ProduceAsync(JT808Package package)
        {
            return default;
        }
    }
}
