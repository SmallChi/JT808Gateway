using JT808.DotNetty.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Xunit;

namespace JT808.DotNetty.Kafka.Test
{
    public class JT808SessionConsumerTest : JT808BaseTest
    {

        public JT808SessionConsumerConfig JT808ConsumerConfig = new JT808SessionConsumerConfig
        {
            GroupId= "JT808Session.test",
            TopicName = "JT808Session",
            BootstrapServers = BootstrapServers
        };
        [Fact]
        public void Test1()
        {
            using (IJT808SessionConsumer JT808MsgConsumer = new JT808SessionConsumer(JT808ConsumerConfig, new LoggerFactory()))
            {
                JT808MsgConsumer.Subscribe();
                JT808MsgConsumer.OnMessage(item => 
                {
                    Debug.WriteLine($"{item.TerminalNo}-{item.Notice}");
                });
                Thread.Sleep(30000);
                JT808MsgConsumer.Unsubscribe();
            }
        }
    }
}
