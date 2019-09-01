using JT808.DotNetty.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace JT808.DotNetty.Kafka.Test
{
    public class JT808MsgProducerTest
    {
        [Fact]
        public void Test1()
        {
            using (IJT808MsgProducer jT808MsgProducer = new JT808MsgProducer(new JT808ProducerConfig { BootstrapServers = "192.168.3.11:9092" }))
            {
                jT808MsgProducer.ProduceAsync("123456", new byte[] { 0x7E, 0, 0x7E });
            }
        }
    }
}
