using JT808.Gateway.Abstractions;
using JT808.Gateway.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.InMemoryMQ
{
    public class JT808MsgReplyConsumerFactory : IJT808MsgReplyConsumerFactory
    {
        private readonly Func<JT808ConsumerType, IJT808MsgReplyConsumer> factory;

        public JT808MsgReplyConsumerFactory(Func<JT808ConsumerType, IJT808MsgReplyConsumer> accesor)
        {
            factory = accesor;
        }

        public IJT808MsgReplyConsumer Create(JT808ConsumerType consumerType)
        {
            return factory(consumerType);
        }
    }
}
