using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Kafka;
using Microsoft.Extensions.Logging;

namespace JT808.DotNetty.SessionNotice.Test
{
    public class JT808DotNettySessionNoticeServiceInherited : JT808DotNettySessionNoticeService
    {
        public JT808DotNettySessionNoticeServiceInherited(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            Task.Run(()=> {
                while (true)
                {
                    Thread.Sleep(5000);
                    JT808SessionProducerConfig JT808ProducerConfig = new JT808SessionProducerConfig
                    {
                        TopicName = "JT808Session",
                        BootstrapServers = "127.0.0.1:9092"
                    };
                    using (IJT808SessionProducer jT808MsgProducer = new JT808SessionProducer(JT808ProducerConfig))
                    {
                        jT808MsgProducer.ProduceAsync("online", "123456").Wait();
                        jT808MsgProducer.ProduceAsync("offline", "123457").Wait();
                    }
                }         
            });
        }

        public override void Processor((string Notice, string TerminalNo) parameter)
        {
            base.Processor(parameter);
        }
    }
}
