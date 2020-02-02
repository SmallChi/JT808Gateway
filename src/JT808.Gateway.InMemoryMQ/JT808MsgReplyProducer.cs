using JT808.Gateway.Abstractions;
using JT808.Gateway.Abstractions.Enums;
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

        //JT808ServerInMemoryMQExtensions
        private readonly JT808ReplyMsgService JT808ReplyMsgService;

        private readonly Func<JT808ConsumerType, JT808MsgServiceBase> func;
        public JT808MsgReplyProducer(
            Func<JT808ConsumerType, JT808MsgServiceBase> func,
            JT808ReplyMsgService jT808ReplyMsgService)
        {
            this.func = func;
            JT808ReplyMsgService = jT808ReplyMsgService;
        }
        public async ValueTask ProduceAsync(string terminalNo, byte[] data)
        {
            await JT808ReplyMsgService.WriteAsync(terminalNo, data);
            if (JT808ServerInMemoryMQExtensions.ReplyMessageLoggingConsumer.HasValue)
            {
                var method = func(JT808ConsumerType.ReplyMessageLoggingConsumer);
                if (method != null)
                {
                    await method.WriteAsync(terminalNo, data);
                }
            }
        }
        public void Dispose()
        {

        }
    }
}
