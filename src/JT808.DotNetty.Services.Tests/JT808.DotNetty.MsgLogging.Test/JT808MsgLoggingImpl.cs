using JT808.DotNetty.Abstractions;
using JT808.DotNetty.Kafka;
using JT808.Protocol.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JT808.DotNetty.MsgLogging.Test
{
    public class JT808MsgLoggingImpl : IJT808MsgLogging
    {
        public readonly ILogger<JT808MsgLoggingImpl> logger;
        public JT808MsgLoggingImpl(ILoggerFactory loggerFactory) {
            logger = loggerFactory.CreateLogger<JT808MsgLoggingImpl>();
            Task.Run(() => {
                while (true)
                {
                    Thread.Sleep(5000);
                    using (IJT808MsgProducer jT808MsgProducer = new JT808MsgProducer(new JT808MsgProducerConfig
                    {
                        BootstrapServers = "127.0.0.1:9092",
                        TopicName = "JT808Msg"
                    }))
                    {
                        jT808MsgProducer.ProduceAsync("123456", new byte[] { 0x7E, 0,0,0,0, 0x7E }).Wait();
                    }

                    JT808MsgReplyProducerConfig JT808MsgProducerConfig = new JT808MsgReplyProducerConfig
                    {
                        TopicName = "JT808MsgReply",
                        BootstrapServers = "127.0.0.1:9092",
                    };
                    using (IJT808MsgReplyProducer jT808MsgProducer = new JT808MsgReplyProducer(JT808MsgProducerConfig))
                    {
                        jT808MsgProducer.ProduceAsync("123456", new byte[] { 0x7E,1,1,1,1, 0x7E }).Wait();
                    }
                }
            });
        }

        public void Processor((string TerminalNo, byte[] Data) parameter, JT808MsgLoggingType jT808MsgLoggingType)
        {
            logger.LogDebug($"{parameter.TerminalNo}:{parameter.Data.ToHexString()},方向:{jT808MsgLoggingType.ToString()}");
        }
    }
}
