using JT808.Gateway.Abstractions;
using JT808.Gateway.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JT808.Gateway.Internal
{
    internal class JT808MsgProducerDefault : IJT808MsgProducer
    {
        private readonly JT808MsgService JT808MsgService;
        public string TopicName => JT808GatewayConstants.MsgTopic;
        public JT808MsgProducerDefault(JT808MsgService jT808MsgService)
        {
            JT808MsgService = jT808MsgService;
        }
        public void Dispose()
        {
            
        }
        public ValueTask ProduceAsync(string terminalNo, byte[] data)
        {
            JT808MsgService.MsgQueue.Add((terminalNo, data));
            return default;
        }
    }
}
