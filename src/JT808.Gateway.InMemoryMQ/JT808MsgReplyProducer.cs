using JT808.Gateway.Abstractions;
using JT808.Gateway.InMemoryMQ.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JT808.Gateway.InMemoryMQ
{
    public class JT808MsgReplyProducer : IJT808MsgReplyProducer
    {
        public string TopicName => JT808GatewayConstants.MsgReplyTopic;
        private readonly JT808ReplyMsgService JT808ReplyMsgService;
        public JT808MsgReplyProducer(JT808ReplyMsgService jT808ReplyMsgService)
        {
            JT808ReplyMsgService = jT808ReplyMsgService;
        }
        public async ValueTask ProduceAsync(string terminalNo, byte[] data)
        {
            await JT808ReplyMsgService.WriteAsync(terminalNo, data);
        }
        public void Dispose()
        {

        }
    }
}
