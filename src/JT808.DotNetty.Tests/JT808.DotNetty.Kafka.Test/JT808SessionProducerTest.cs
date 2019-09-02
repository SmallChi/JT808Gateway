using Confluent.Kafka;
using Confluent.Kafka.Admin;
using JT808.DotNetty.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xunit;

namespace JT808.DotNetty.Kafka.Test
{
    public class JT808SessionProducerTest: JT808BaseTest
    {
        public JT808SessionProducerConfig JT808ProducerConfig = new JT808SessionProducerConfig
        {
            TopicName = "JT808Session",
            BootstrapServers = BootstrapServers
        };

        public JT808SessionProducerTest()
        {
            using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = BootstrapServers }).Build())
            {
                try
                {
                    adminClient.DeleteTopicsAsync(new List<string>() { JT808ProducerConfig.TopicName }).Wait();
                }
                catch (AggregateException e)
                {
                    //Debug.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
                }
                catch (CreateTopicsException e)
                {
                    Debug.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
                }
            }
        }

        [Fact]
        public void Test1()
        {
            using (IJT808SessionProducer jT808MsgProducer = new JT808SessionProducer(JT808ProducerConfig))
            {
                jT808MsgProducer.ProduceAsync("online","123456").Wait();
                jT808MsgProducer.ProduceAsync("offline", "123457").Wait();
            }
        }
    }
}
