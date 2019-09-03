using Confluent.Kafka;
using JT808.DotNetty.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.DotNetty.Kafka
{
    public class JT808MsgConsumer : IJT808MsgConsumer
    {
        public CancellationTokenSource Cts => new CancellationTokenSource();

        private readonly IConsumer<string, byte[]> consumer;

        private readonly ILogger logger;

        public string TopicName { get; }

        public JT808MsgConsumer(
            IOptions<JT808MsgConsumerConfig> consumerConfigAccessor,
            ILoggerFactory loggerFactory)
        {
            consumer = new ConsumerBuilder<string, byte[]>(consumerConfigAccessor.Value).Build();
            TopicName = consumerConfigAccessor.Value.TopicName;
            logger = loggerFactory.CreateLogger("JT808MsgConsumer");
        }

        public void OnMessage(Action<(string TerminalNo, byte[] Data)> callback)
        {
            Task.Run(() =>
            {
                while (!Cts.IsCancellationRequested)
                {
                    try
                    {
                        //如果不指定分区，根据kafka的机制会从多个分区中拉取数据
                        //如果指定分区，根据kafka的机制会从相应的分区中拉取数据
                        var data = consumer.Consume(Cts.Token);
                        if (logger.IsEnabled(LogLevel.Debug))
                        {
                            logger.LogDebug($"Topic: {data.Topic} Key: {data.Key} Partition: {data.Partition} Offset: {data.Offset} TopicPartitionOffset:{data.TopicPartitionOffset}");
                        }
                        callback((data.Key, data.Value));
                    }
                    catch (ConsumeException ex)
                    {
                        logger.LogError(ex, TopicName);
                    }
                    catch (OperationCanceledException ex)
                    {
                        logger.LogError(ex, TopicName);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, TopicName);
                    }
                }
            }, Cts.Token);
        }

        public void Subscribe()
        {
            consumer.Subscribe(TopicName);
        }

        public void Unsubscribe()
        {
            consumer.Unsubscribe();
        }

        public void Dispose()
        {
            consumer.Close();
            consumer.Dispose();
        }
    }
}
