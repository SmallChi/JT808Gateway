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
    public sealed class JT808SessionProducer : IJT808SessionProducer
    {
        private bool disposed = false;
        public string TopicName { get; }

        private readonly IProducer<string, string> producer;

        private readonly ILogger logger;

        public JT808SessionProducer(
          ILoggerFactory loggerFactory,
          IOptions<JT808SessionProducerConfig> producerConfigAccessor)
        {
            producer = new ProducerBuilder<string, string>(producerConfigAccessor.Value).Build();
            TopicName = producerConfigAccessor.Value.TopicName;
            logger = loggerFactory.CreateLogger<JT808SessionProducer>();
        }

        public async void ProduceAsync(string notice,string terminalNo)
        {
            if (disposed) return;
            try
            {
                await producer.ProduceAsync(TopicName, new Message<string, string>
                {
                    Key = notice,
                    Value = terminalNo
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
        ~JT808SessionProducer()
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
