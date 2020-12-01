using Confluent.Kafka;
using JT808.Gateway.Configs.Kafka;
using JT808.Gateway.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace JT808.Gateway.Kafka
{
    public sealed class JT808MsgReplyLoggingProducer : IJT808MsgReplyLoggingProducer
    {
        private bool disposed = false;
        public string TopicName { get;}

        private readonly IProducer<string, byte[]> producer;

        private readonly ILogger logger;

        public JT808MsgReplyLoggingProducer(
            ILoggerFactory loggerFactory,
            IOptions<JT808MsgReplyLoggingProducerConfig> producerConfigAccessor)
        {
            producer = new ProducerBuilder<string, byte[]>(producerConfigAccessor.Value).Build();
            TopicName = producerConfigAccessor.Value.TopicName;
            logger = loggerFactory.CreateLogger<JT808MsgReplyLoggingProducer>();
        }

        public async void ProduceAsync(string terminalNo, byte[] data)
        {
            if (disposed) return;
            try
            {
                await producer.ProduceAsync(TopicName, new Message<string, byte[]>
                {
                    Key = terminalNo,
                    Value = data
                });
            }
            catch (AggregateException ex)
            {
                logger.LogError(ex, "kafka error");
            }
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
        ~JT808MsgReplyLoggingProducer()
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
