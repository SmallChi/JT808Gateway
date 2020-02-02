using JT808.Gateway.Abstractions;
using JT808.Gateway.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace JT808.Gateway.InMemoryMQ
{
    public class JT808MsgConsumerFactory : IJT808MsgConsumerFactory
    {
        private readonly Func<JT808ConsumerType, IJT808MsgConsumer> factory;

        public JT808MsgConsumerFactory(Func<JT808ConsumerType, IJT808MsgConsumer> accesor)
        {
            factory = accesor;
        }

        public IJT808MsgConsumer Create(JT808ConsumerType consumerType)
        {
            return factory(consumerType);
        }
    }
}
