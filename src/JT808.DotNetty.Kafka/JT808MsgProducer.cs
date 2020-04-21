using Confluent.Kafka;
using JT808.DotNetty.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JT808.DotNetty.Kafka
{
    public sealed class JT808MsgProducer : IJT808MsgProducer
    {
        private bool disposed = false;
        public string TopicName { get; }

        private readonly IProducer<string, byte[]> producer;
        public JT808MsgProducer(
          IOptions<JT808MsgProducerConfig> producerConfigAccessor)
        {
            producer = new ProducerBuilder<string, byte[]>(producerConfigAccessor.Value).Build();
            TopicName = producerConfigAccessor.Value.TopicName;
        }

        public async Task ProduceAsync(string terminalNo, byte[] data)
        {
            if (disposed) return;
            await producer.ProduceAsync(TopicName, new Message<string, byte[]>
            {
                Key = terminalNo,
                Value = data
            });
        }
        private void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                producer.Dispose();
            }
            disposed = true;
        }
        ~JT808MsgProducer()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            //必须为true
            Dispose(true);
            //通知垃圾回收机制不再调用终结器（析构器）
            GC.SuppressFinalize(this);
        }
    }
}
