using System;
using System.Threading.Tasks;

namespace JT808.Gateway.Abstractions
{
    /// <summary>
    /// 会话通知（在线/离线）
    /// </summary>
    public interface IJT808SessionProducer : IJT808PubSub, IDisposable
    {
        void ProduceAsync(string notice,string terminalNo);
    }
}
