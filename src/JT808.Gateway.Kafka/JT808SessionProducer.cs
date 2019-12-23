using Confluent.Kafka;
using JT808.Gateway.Configs.Kafka;
using JT808.Gateway.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JT808.Gateway.Kafka
{
    public class JT808SessionProducer : IJT808SessionProducer
    {
        public string TopicName { get; }

        private readonly IProducer<string, string> producer;
        public JT808SessionProducer(
          IOptions<JT808SessionProducerConfig> producerConfigAccessor)
        {
            producer = new ProducerBuilder<string, string>(producerConfigAccessor.Value).Build();
            TopicName = producerConfigAccessor.Value.TopicName;
        }

        public void Dispose()
        {
            producer.Dispose();
        }

        public async ValueTask ProduceAsync(string notice,string terminalNo)
        {
            await producer.ProduceAsync(TopicName, new Message<string, string>
            {
                Key = notice,
                Value = terminalNo
            });
        }
    }
}
