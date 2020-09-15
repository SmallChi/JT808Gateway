using JT808.Gateway.Abstractions;
using JT808.Gateway.NormalHosting.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JT808.Gateway.NormalHosting.Impl
{
    public class JT808MsgReplyProducer : IJT808MsgReplyProducer
    {
        public string TopicName { get; } = JT808GatewayConstants.MsgReplyTopic;

        private readonly JT808MsgReplyDataService MsgReplyDataService;

        public JT808MsgReplyProducer(JT808MsgReplyDataService msgReplyDataService)
        {
            MsgReplyDataService = msgReplyDataService;
        }

        public async ValueTask ProduceAsync(string terminalNo, byte[] data)
        {
            await MsgReplyDataService.WriteAsync(terminalNo, data);
        }

        public void Dispose()
        {
        }
    }
}
