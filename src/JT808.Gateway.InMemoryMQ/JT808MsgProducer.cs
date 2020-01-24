using JT808.Gateway.Abstractions;
using JT808.Gateway.InMemoryMQ.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JT808.Gateway.InMemoryMQ
{
    public class JT808MsgProducer : IJT808MsgProducer
    {
        private readonly JT808MsgService JT808MsgService;
        public string TopicName => JT808GatewayConstants.MsgTopic;
        public JT808MsgProducer(JT808MsgService jT808MsgService)
        {
            JT808MsgService = jT808MsgService;
        }
        public async ValueTask ProduceAsync(string terminalNo, byte[] data)
        {
            await JT808MsgService.WriteAsync(terminalNo, data);
        }
        public void Dispose()
        {

        }
    }
}
