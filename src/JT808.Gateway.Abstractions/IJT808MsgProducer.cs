using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JT808.Gateway.Abstractions
{
    public interface IJT808MsgProducer : IJT808PubSub, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="terminalNo">设备终端号</param>
        /// <param name="data">808 hex data</param>
        void ProduceAsync(string terminalNo, byte[] data);
    }
}
