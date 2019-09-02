using Confluent.Kafka;
using JT808.DotNetty.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JT808.DotNetty.Kafka
{
    public class JT808MsgProducer : IJT808MsgProducer
    {
        public string TopicName { get; }

        private readonly IProducer<string, byte[]> producer;
        public JT808MsgProducer(
          IOptions<JT808MsgProducerConfig> producerConfigAccessor)
        {
            producer = new ProducerBuilder<string, byte[]>(producerConfigAccessor.Value).Build();
            TopicName = producerConfigAccessor.Value.TopicName;
        }

        public void Dispose()
        {
            producer.Dispose();
        }

        public async Task ProduceAsync(string terminalNo, byte[] data)
        {
            await producer.ProduceAsync(TopicName, new Message<string, byte[]>
            {
                Key = terminalNo,
                Value = data
            });
        }
    }
}
