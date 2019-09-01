using EasyNetQ;
using EasyNetQ.Topology;
using JT808.DotNetty.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JT808.DotNetty.RabbitMQ
{
    public class JT808MsgProducer : IJT808MsgProducer
    {
        public string TopicName { get; }

        private readonly IBus bus;
        public JT808MsgProducer(IOptions<JT808ProducerConfig> producerConfigAccessor)
        {
            bus = RabbitHutch.CreateBus(producerConfigAccessor.Value.ConnectionString);
            TopicName = producerConfigAccessor.Value.TopicName;
        }

        public void Dispose()
        {
            bus.Dispose();
        }

        public Task ProduceAsync(string terminalNo, byte[] data)
        {
            var exchange = bus.Advanced.ExchangeDeclare(TopicName, ExchangeType.Fanout);
            bus.Advanced.Publish(exchange, "", false, new Message<byte[]>(data));
            return Task.CompletedTask;
        }
    }
}
