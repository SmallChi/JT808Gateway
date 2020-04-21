using Confluent.Kafka;
using JT808.Gateway.Configs.Kafka;
using JT808.Gateway.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JJT808.Gateway.Kafka
{
    public sealed class JT808MsgReplyProducer : IJT808MsgReplyProducer
    {
        private bool disposed = false;
        public string TopicName { get;}

        private IProducer<string, byte[]> producer;
        public JT808MsgReplyProducer(
          IOptions<JT808MsgReplyProducerConfig> producerConfigAccessor)
        {
            producer = new ProducerBuilder<string, byte[]>(producerConfigAccessor.Value).Build();
            TopicName = producerConfigAccessor.Value.TopicName;
        }

        public async ValueTask ProduceAsync(string terminalNo, byte[] data)
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
        ~JT808MsgReplyProducer()
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
