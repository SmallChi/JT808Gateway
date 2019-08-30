using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JT808.DotNetty.Core.Impls
{
    internal class JT808MsgProducerDefaultImpl : IJT808MsgProducer
    {
        private readonly JT808MsgService JT808MsgService;
        public string TopicName => JT808NettyConstants.MsgTopic;
        public JT808MsgProducerDefaultImpl(JT808MsgService jT808MsgService)
        {
            JT808MsgService = jT808MsgService;
        }
        public void Dispose()
        {
            
        }

        public Task ProduceAsync(string terminalNo, byte[] data)
        {
            JT808MsgService.MsgQueue.Add((terminalNo, data));
            return Task.CompletedTask;
        }
    }
}
