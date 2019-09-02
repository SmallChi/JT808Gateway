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
    public class JT808MsgReplyConsumerTest: JT808BaseTest
    {

        public JT808MsgReplyConsumerConfig JT808ConsumerConfig = new JT808MsgReplyConsumerConfig
        {
            GroupId= "jt808.MsgReply.test",
            TopicName = "JT808MsgReply",
            BootstrapServers = BootstrapServers
        };
        [Fact]
        public void Test1()
        {
            using (IJT808MsgReplyConsumer JT808MsgConsumer = new JT808MsgReplyConsumer(JT808ConsumerConfig, new LoggerFactory()))
            {
                JT808MsgConsumer.Subscribe();
                JT808MsgConsumer.OnMessage(item => 
                {
                    Debug.WriteLine($"{item.TerminalNo}-{item.Data.Length}");
                });
                Thread.Sleep(30000);
                JT808MsgConsumer.Unsubscribe();
            }
        }
    }
}
